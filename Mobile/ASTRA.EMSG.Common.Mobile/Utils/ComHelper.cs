using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ASTRA.EMSG.Common.Mobile.Utils
{
    public static class ComHelper
    {
        public static void DisposeComObject(object o)
        {
            int refsLeft = 0;
            do
            {
                refsLeft = Marshal.ReleaseComObject(o);
            }
            while (refsLeft > 0);
        }
    }
}
