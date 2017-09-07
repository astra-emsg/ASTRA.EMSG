using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using NHibernate;

namespace ASTRA.EMSG.Business.Interlis.AxisImport.TransactionHandling
{
    public class MultiTransactionCommitter : ITransactionCommitter, IDisposable
    {
        private const int COMMIT_EVERY_N_OPERATIONS = 1000;

        TransactionScopeFactory transactionScopeFactory;

        private ITransactionScope CurrentTransactionScope;

        private int Counter = 0;

        public MultiTransactionCommitter(TransactionScopeFactory transactionScopeFactory)
        {
            this.transactionScopeFactory = transactionScopeFactory;
            CurrentTransactionScope = transactionScopeFactory.CreateReadWrite();
        }

        public bool Next()
        {
            Counter++;
            if (Counter % COMMIT_EVERY_N_OPERATIONS == 0)
            {
                ForceNext();
                return true;
            }
            return false;
        }

        public ISession Session { get { return CurrentTransactionScope.Session; } }

        public void Finish()
        {
            CurrentTransactionScope.Commit();
            CurrentTransactionScope.Dispose();
        }

        public void Dispose()
        {
            CurrentTransactionScope.Dispose();
        }

        public void ForceNext()
        {
            Counter = 0;
            CurrentTransactionScope.Commit();
            CurrentTransactionScope.Dispose();
            CurrentTransactionScope = transactionScopeFactory.CreateReadWrite();
        }
    }
}
