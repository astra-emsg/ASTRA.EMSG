using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Business.Interlis.AxisImport
{
    public class AxisImportException : Exception
    {
        public AxisImportException(string message) : base(message)
        {
        }
    }
}
