using System;

namespace ASTRA.EMSG.Common
{
    public static class EnumExtensions
    {
        public static T? ParseAsEnum<T>(this string text)
            where T : struct 
        {
            T value;
            if (Enum.TryParse(text, out value))
                return value;

            return null;
        }
    }
}
