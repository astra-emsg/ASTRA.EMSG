using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Business.Interlis.AxisImport
{
    public class AxisImportCancelledException : Exception
    {
        public AxisImportCancelledException(string message)
            : base(message)
        {
        }
    }
}
