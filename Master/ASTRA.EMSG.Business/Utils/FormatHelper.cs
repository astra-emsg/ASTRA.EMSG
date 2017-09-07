using System;
using System.Globalization;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Utils
{
    public static class FormatHelper
    {
        public static string ToReportDateTimeString(decimal? decimalValue)
        {
            return ToReportDateTimeString(decimalValue, string.Empty);
        }

        public static string ToReportDateTimeString(decimal? decimalValue, string emptyString)
        {
            return decimalValue.HasValue
                       ? DateTime.FromBinary((long) decimalValue.Value).ToString(FormatStrings.ReportDateTimeFormat)
                       : string.Empty;
        }

        public static string ToReportDecimalString(decimal? decimalValue)
        {
            return decimalValue.HasValue
                       ? decimalValue.Value.ToString("0.##")
                       : string.Empty;
        }

        public static string ToReportNoDecimalPercentString(decimal? decimalValue)
        {
            return ToReportNoDecimalPercentString(decimalValue, string.Empty);
        }

        public static string ToReportNoDecimalPercentString(decimal? decimalValue, string emptyString)
        {
            return decimalValue.HasValue
                       ? decimalValue.Value.ToString(FormatStrings.ReportNoDecimalPercentFormat)
                       : emptyString;
        }

        public static string ToASCIIString(this string text)
        {
            var decomposed = text.Normalize(NormalizationForm.FormD);
            var filtered = decomposed.Where(r => char.GetUnicodeCategory(r) != UnicodeCategory.NonSpacingMark);
            return new string(filtered.ToArray()).Replace("’", "'");
        }
    }
}