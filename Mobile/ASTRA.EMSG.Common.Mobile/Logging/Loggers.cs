using NLog;

namespace ASTRA.EMSG.Common.Mobile.Logging
{
    public static class Loggers
    {
        public static Logger TechLogger = LogManager.GetLogger("TechLogger");
        public static Logger PerformanceLogger = LogManager.GetLogger("PerformanceLogger");
        public static Logger BusinessLogger = LogManager.GetLogger("BusinessLogger");
    }
}
