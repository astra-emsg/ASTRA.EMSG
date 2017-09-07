using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Common
{
    public static class HexStringFromGuid
    {
        public static string Convert(Guid guid)
        {
            string s = "";
            byte[] bytes = guid.ToByteArray();

            foreach (byte b in bytes)
            {
                s += string.Format("{0:X2}", b);
            }

            return s;
        }
    }
}
