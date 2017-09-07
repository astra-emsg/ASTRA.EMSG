using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NLog;

namespace ASTRA.EMSG.AxisImportService
{
    public static class Loggers
    {
        public static Logger IntegrationTestLogger = LogManager.GetLogger("IntegrationTestLogger");
        public static Logger ReportingLogger = LogManager.GetLogger("ReportingLogger");
        public static Logger PeformanceLogger = LogManager.GetLogger("PeformanceLogger");
        public static Logger HelpLogger = LogManager.GetLogger("HelpLogger");
        public static Logger TechLogger = LogManager.GetLogger("TechLogger");
    }
}
