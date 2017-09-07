using System;
using System.Linq;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public static class ReportTypeService
    {
        public static string GetReportType(Type type, NetzErfassungsmodus netzErfassungsmodus)
        {
            return string.Join(", ",
                type.GetCustomAttributes(true)
                    .OfType<ReportInfoAttribute>()
                    .Where(r => r.NetzErfassungsmodus == (NetzErfassungsmodus)(-1) || r.NetzErfassungsmodus == netzErfassungsmodus)
                    .Select(r => r.ReportType.ToString().Replace('_', '.'))) + " ";
        }
    }
}