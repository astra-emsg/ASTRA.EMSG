using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

namespace ASTRA.EMSG.Business.Infrastructure.Reporting
{
    public class ServerReportParameterInfo
    {
        public ServerReportParameterInfo(ReportParameterInfo reportParameterInfo)
        {
            ReportParameterInfo = reportParameterInfo;
        }

        public ReportParameterInfo ReportParameterInfo { get; private set; }
        public IList<string> Values { get { return ReportParameterInfo.Values; } }
    }
}