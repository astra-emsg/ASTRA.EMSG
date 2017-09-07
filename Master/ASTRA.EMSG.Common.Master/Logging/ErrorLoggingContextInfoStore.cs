using System.Web;

namespace ASTRA.EMSG.Common.Master.Logging
{
    public static class ErrorLoggingContextInfoStore
    {
        public static ErrorLoggingContextInfo CurrentErrorLoggingContextInfo
        {
            get
            {
                if (HttpContext.Current == null)
                    return new ErrorLoggingContextInfo();

                if (!HttpContext.Current.Items.Contains("ErrorLoggingContextInfo"))
                    HttpContext.Current.Items.Add("ErrorLoggingContextInfo", new ErrorLoggingContextInfo());

                return (ErrorLoggingContextInfo)(HttpContext.Current.Items["ErrorLoggingContextInfo"]);
            }
        }
    }
}