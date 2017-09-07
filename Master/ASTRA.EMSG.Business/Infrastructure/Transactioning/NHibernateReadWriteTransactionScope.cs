using System.Data;

namespace ASTRA.EMSG.Business.Infrastructure.Transactioning
{
    public class NHibernateReadWriteTransactionScope : NHibernateTransactionScope
    {
        public NHibernateReadWriteTransactionScope(IsolationLevel isolationLevel, INHibernateConfigurationProvider nHibernateConfigurationProvider)
            : base(isolationLevel, nHibernateConfigurationProvider, true) { }

        public override bool IsActive { get { return transaction.IsActive; } }
        public override void Commit() { transaction.Commit(); }
        public override void Rollback() { transaction.Rollback(); }
        public override void Flush() { Session.Flush(); }
    }
}