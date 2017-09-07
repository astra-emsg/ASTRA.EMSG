using System;

namespace ASTRA.EMSG.IntegrationTests.Support
{
    public static class StringExtensions
    {
        public static Guid? ParseNullableGuid(this string value) { return value.IsNull() ? (Guid?) null : value.ParseGuid(); }
        public static Guid ParseGuid(this string value) { return new Guid(value); }

        public static TResult As<TResult>(this string value)
        {
            return (TResult)Convert.ChangeType(value, typeof(TResult));
        }

        public static TResult? AsNullable<TResult>(this string value) where TResult : struct
        {
            if (value.IsNull())
                return null;
            return value.As<TResult>();
        }

        public static TEnum ParseEnum<TEnum>(this string value) where TEnum : struct
        {
            if (value.IsNull())
                return default(TEnum);
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        public static bool IsNull(this string value)
        {
            return object.Equals(value, null)
                || string.Equals("-", value, StringComparison.InvariantCultureIgnoreCase);
        }

        public static decimal? ParseNullableDecimal(this string value) { return value.IsNull() ? (decimal?) null : value.ParseDecimal(); }
        public static decimal ParseDecimal(this string value) { return decimal.Parse(value); }

        public static DateTime? ParseNullableDateTime(this string value) { return value.IsNull() ? (DateTime?)null : value.ParseDateTime(); }
        public static DateTime ParseDateTime(this string value) { return DateTime.Parse(value); }

        public static int? ParseNullableInt(this string value) { return value.IsNull() ? (int?) null : value.ParseInt(); }
        public static int ParseInt(this string value) { return int.Parse(value); }

        public static bool ParseBool(this string value)
        {
            switch (value.ToLower())
            {
                case "ja":
                    return true;
                case "nein":
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(value);
            }
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static string ConvertNull(this string value)
        {
            return value.IsNull() ? null : value;
        }
    }
}
