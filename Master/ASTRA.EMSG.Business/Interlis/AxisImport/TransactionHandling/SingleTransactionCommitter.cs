using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using NHibernate;

namespace ASTRA.EMSG.Business.Interlis.AxisImport.TransactionHandling
{
    public class SingleTransactionCommitter : ITransactionCommitter, IDisposable
    {
        private ITransactionScope CurrentTransactionScope;

        private int Counter = 0;

        public SingleTransactionCommitter(ITransactionScope CurrentTransactionScope)
        {
            this.CurrentTransactionScope = CurrentTransactionScope;
        }

        public bool Next()
        {
            Counter++;
            return false;
        }

        public ISession Session { get { return CurrentTransactionScope.Session; } }

        public void Finish()
        {
            // nothing
        }

        public void Dispose()
        {
            // nothing
        }

        public void ForceNext()
        {
            // nothing
        }
    }
}
