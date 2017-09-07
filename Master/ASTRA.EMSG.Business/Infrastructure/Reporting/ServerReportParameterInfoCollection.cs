using Microsoft.Reporting.WebForms;

namespace ASTRA.EMSG.Business.Infrastructure.Reporting
{
    public class ServerReportParameterInfoCollection
    {
        public ServerReportParameterInfoCollection(ReportParameterInfoCollection reportParameterInfoCollection)
        {
            ReportParameterInfoCollection = reportParameterInfoCollection;
        }

        public ReportParameterInfoCollection ReportParameterInfoCollection { get; private set; }
        public ServerReportParameterInfo this[string index] { get { return new ServerReportParameterInfo(ReportParameterInfoCollection[index]); } }
    }
}