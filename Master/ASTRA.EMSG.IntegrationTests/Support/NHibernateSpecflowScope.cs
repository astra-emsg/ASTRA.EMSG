using System;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using NHibernate;

namespace ASTRA.EMSG.IntegrationTests.Support
{
    public class NHibernateSpecflowScope : IDisposable
    {
        public NHibernateReadWriteTransactionScope Scope { get; private set; }
        public ISession Session { get { return Scope.Session; } }

        public NHibernateSpecflowScope()
        {
            Scope = NHibernateDb.GetNewScope();
            ScenarioContextWrapper.CurrentScope = Scope;
        }

        public void Dispose()
        {
            Scope.Commit();
            ScenarioContextWrapper.CurrentScope = null;
            Scope.Dispose();
        }
    }
}