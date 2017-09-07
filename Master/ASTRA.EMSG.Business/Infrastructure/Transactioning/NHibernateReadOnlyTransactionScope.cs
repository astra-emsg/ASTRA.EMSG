using System;
using System.Data;

namespace ASTRA.EMSG.Business.Infrastructure.Transactioning
{
    public class NHibernateReadOnlyTransactionScope : NHibernateTransactionScope
    {
        internal NHibernateReadOnlyTransactionScope(IsolationLevel isolationLevel, INHibernateConfigurationProvider nHibernateConfigurationProvider)
            : base(isolationLevel, nHibernateConfigurationProvider, true) { }

        public override void Flush()
        {
            // NOP
        }

        public override void Commit()
        {
            // NOP
        }

        public override bool IsActive
        {
            get { return false; }
        }

        public override void Rollback()
        {
            //NOP
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!isAlreadyDisposed)
            {
                if (isDisposing)
                {
                    //ToDo: Remove IsDirty() check. Not performant!
                    if (Session.IsDirty())
                        throw new InvalidOperationException("Modifications on the NHibernate Session was detected. You should not modify entities in a read only scope.");

                    transaction.Commit();
                }
            }

            base.Dispose(isDisposing);
        }
    }
}