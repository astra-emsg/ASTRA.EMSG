using System;
using System.Security;
using ASTRA.EMSG.Business.Infrastructure.Reporting;

namespace ASTRA.EMSG.Business.Reporting
{
    [Serializable]
    public class EmsgReportParameter : ReportParameter
    {
        public Guid? ErfassungsPeriodId { get; set; }

        public bool IsPreview { get; set; }
    }
}