using System;

namespace ASTRA.EMSG.Business.Entities.Common
{
    public class Defaults
    {
        public const int MaxDecimal = int.MaxValue;
        public const int MaxInt = int.MaxValue;
        public const int MaxStringLength = 150;
        public const int MaxTextBlockLength = 8000;
        public static readonly DateTime MinDateTime = new DateTime(1900, 01, 01);
        public static readonly DateTime MaxDateTime = new DateTime(9999, 12, 31);
    }
}
