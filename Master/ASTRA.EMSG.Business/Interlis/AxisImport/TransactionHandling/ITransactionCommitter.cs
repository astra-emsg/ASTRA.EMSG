using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace ASTRA.EMSG.Business.Interlis.AxisImport.TransactionHandling
{
    public interface ITransactionCommitter
    {
        /// <summary>
        /// in multi-transaction mode: increment operation counter -
        /// if the limit is reached, will commit and start the next transaction.
        /// 
        /// </summary>
        /// <returns></returns>
        bool Next();

        /// <summary>
        /// in multi-transactions-mode: force commit and start the next transaction.
        /// </summary>
        void ForceNext();

        /// <summary>
        /// get the current session
        /// </summary>
        ISession Session { get; }

        /// <summary>
        /// in multi-transactions-mode: commit last transaction
        /// </summary>
        void Finish();
    }
}
