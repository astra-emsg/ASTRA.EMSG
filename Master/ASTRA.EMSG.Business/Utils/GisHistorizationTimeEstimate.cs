using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Business.Utils
{
    public static class GisHistorizationTimeEstimate
    {
        private static int segmentCountApproxValue = 3700;
        private static double minutesApproxValue = 30;

        public static int GetApproxHistorizationTime(int achssegmentCount)
        {
            double minutes = minutesApproxValue * achssegmentCount / segmentCountApproxValue;
            if (minutes > 5)
            {
                return (int)Math.Ceiling(minutes / 5) * 5;
            }
            else
            {
                return (int)Math.Ceiling(minutes);
            }
        }
    }
}
