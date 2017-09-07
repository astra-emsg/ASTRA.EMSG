namespace ASTRA.EMSG.Business.Utils
{
    public static class LaengeTextTrimmer
    {
        public static string TrimToMaxLength(this string text, int maxLength)
        {
            if(string.IsNullOrEmpty(text))
                return text;

            if (text.Length < maxLength)
                return text;

            return string.Format("{0}...", text.Substring(0, maxLength));
        }
    }
}