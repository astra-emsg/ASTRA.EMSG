using Common.Logging;

namespace ASTRA.EMSG.Common.Master.Logging
{
    public static class Loggers
    {
        public static ILog ApplicationLogger = LogManager.GetLogger("ApplicationLogger");
        public static ILog WMSRedirectLogger = LogManager.GetLogger("WMSRedirectLogger");
        public static ILog IntegrationTestLogger = LogManager.GetLogger("IntegrationTestLogger");
        public static ILog ReportingLogger = LogManager.GetLogger("ReportingLogger");
        public static ILog PeformanceLogger = LogManager.GetLogger("PeformanceLogger");
        public static ILog HelpLogger = LogManager.GetLogger("HelpLogger");
        public static ILog TechLogger = LogManager.GetLogger("TechLogger");
    }
}