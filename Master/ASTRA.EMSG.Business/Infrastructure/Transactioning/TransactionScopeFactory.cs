using System.Data;

namespace ASTRA.EMSG.Business.Infrastructure.Transactioning
{
    public class TransactionScopeFactory : ITransactionScopeFactory
    {
        private readonly INHibernateConfigurationProvider nHibernateConfigurationProvider;

        public TransactionScopeFactory(INHibernateConfigurationProvider nHibernateConfigurationProvider)
        {
            this.nHibernateConfigurationProvider = nHibernateConfigurationProvider;
        }

        private static IsolationLevel DefaultIsolationLevel { get { return IsolationLevel.ReadCommitted; } }
        private static IsolationLevel HighIsolationLevel { get { return IsolationLevel.Serializable; } }

        public ITransactionScope CreateReadOnly()
        {
            return new NHibernateReadOnlyTransactionScope(DefaultIsolationLevel, nHibernateConfigurationProvider);
        }

        public ITransactionScope CreateReadWrite()
        {
            return new NHibernateReadWriteTransactionScope(DefaultIsolationLevel, nHibernateConfigurationProvider);
        }

        public ITransactionScope CreateIsolatedReadWrite()
        {
            return new NHibernateReadWriteTransactionScope(HighIsolationLevel, nHibernateConfigurationProvider);
        }
    }
}