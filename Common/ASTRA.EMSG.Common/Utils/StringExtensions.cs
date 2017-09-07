using System;

namespace ASTRA.EMSG.Common.Utils
{
    public static class StringExtensions
    {
        public static TEnum AsEnum<TEnum>(this string value) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        public static bool HasText(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}
