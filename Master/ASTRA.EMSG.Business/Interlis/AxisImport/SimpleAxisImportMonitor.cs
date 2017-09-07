using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Business.Interlis.AxisImport
{
    public class SimpleAxisImportMonitor : IAxisImportMonitor
    {
        public bool IsCancelled()
        {
            return false;
        }

        public void WriteLog(string text)
        {
            Console.WriteLine(text);
        }
    }
}
