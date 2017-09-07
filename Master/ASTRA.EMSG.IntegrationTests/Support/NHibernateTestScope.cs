using System;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using NHibernate;

namespace ASTRA.EMSG.IntegrationTests.Support
{
    public class NHibernateTestScope : IDisposable
    {
        public NHibernateReadWriteTransactionScope Scope { get; private set; }
        public ISession Session { get { return Scope.Session; } }

        public NHibernateTestScope()
        {
            Scope = NHibernateDb.GetNewScope();
        }

        public void Dispose()
        {
            Scope.Commit();
            Scope.Dispose();
        }
    }
}