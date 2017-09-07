using System;
using System.Data;
using ASTRA.EMSG.Common.Master.Logging;
using NHibernate;

namespace ASTRA.EMSG.Business.Infrastructure.Transactioning
{
    public abstract class NHibernateTransactionScope : ITransactionScope
    {
        protected static readonly object syncRoot = new object();
        protected static ISessionFactory nHibernateSessionFactory;

        public static void ResetSessionFactory()
        {
            lock (syncRoot)
            {
                nHibernateSessionFactory = null;
            }
        }

        protected ISessionFactory GetSessionFactory(INHibernateConfigurationProvider nHibernateConfigurationProvider)
        {
            if (nHibernateSessionFactory == null)
            {
                lock (syncRoot)
                {
                    if (nHibernateSessionFactory == null)
                    {
                        nHibernateSessionFactory = nHibernateConfigurationProvider.Configuration.BuildSessionFactory();
                    }
                }
            }
            return nHibernateSessionFactory;
        }

        protected bool isAlreadyDisposed;
        protected readonly ITransaction transaction;
        protected readonly ISession nHibernateSession;
        public ISession Session
        {
            get
            {
                if (nHibernateSession != null)
                    return nHibernateSession;
                // no null check, there should be a session
                return previousScope.Session;
            }
        }

        [ThreadStatic]
        protected static NHibernateTransactionScope currentScope;
        protected readonly NHibernateTransactionScope previousScope;
        protected static NHibernateTransactionScope Current { get { return currentScope; } }

        protected NHibernateTransactionScope(IsolationLevel isolationLevel, INHibernateConfigurationProvider nHibernateConfigurationProvider, bool newSessionRequired)
        {
            isAlreadyDisposed = false;

            //Create session if no current scope exists or if explicitly requested)
            if (currentScope == null || newSessionRequired)
                nHibernateSession = GetSessionFactory(nHibernateConfigurationProvider).OpenSession();

            transaction = Session.BeginTransaction(isolationLevel);

            //Update current scope
            previousScope = currentScope;
            currentScope = this;
        }

        ~NHibernateTransactionScope()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!isAlreadyDisposed)
            {
                isAlreadyDisposed = true;

                if (isDisposing)
                {
                    try
                    {
                        if (nHibernateSession != null)
                            nHibernateSession.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Loggers.ApplicationLogger.Error(ex.Message, ex);
                        throw;
                    }
                    finally
                    {
                        currentScope = previousScope;
                        try
                        {
                            transaction.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Loggers.ApplicationLogger.Error(ex.Message, ex);
                            throw;
                        }
                    }
                }

                GC.SuppressFinalize(this);
            }
        }

        public abstract bool IsActive { get; }
        public abstract void Commit();
        public abstract void Rollback();
        public abstract void Flush();
    }
}