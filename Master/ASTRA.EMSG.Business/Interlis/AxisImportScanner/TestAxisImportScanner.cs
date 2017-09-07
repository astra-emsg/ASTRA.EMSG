using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;

namespace ASTRA.EMSG.Business.Interlis.AxisImportScanner
{
    public class TestAxisImportScanner : AxisImportScanner
    {
        public TestAxisImportScanner(string baseDir, int sleepMS = 60000)
            : base(baseDir, sleepMS)
        {
        }

        protected override AxisImport.AxisImport GetImportInstance(TransactionScopeFactory transactionScopeFactory)
        {
            return new AxisImport.TestAxisImport(transactionScopeFactory, this);
        }
    }
}
