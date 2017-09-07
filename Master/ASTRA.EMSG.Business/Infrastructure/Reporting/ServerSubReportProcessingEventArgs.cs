using Microsoft.Reporting.WebForms;

namespace ASTRA.EMSG.Business.Infrastructure.Reporting
{
    public class ServerSubReportProcessingEventArgs
    {
        public ServerSubReportProcessingEventArgs(SubreportProcessingEventArgs subreportProcessingEventArgs)
        {
            SubreportProcessingEventArgs = subreportProcessingEventArgs;
        }

        public SubreportProcessingEventArgs SubreportProcessingEventArgs { get; private set; }
        public IReportDataSourceCollection DataSources { get { return new ServerReportDataSourceCollection(SubreportProcessingEventArgs.DataSources); } }
        public string ReportPath { get { return SubreportProcessingEventArgs.ReportPath; } }
        public ServerReportParameterInfoCollection Parameters { get { return new ServerReportParameterInfoCollection(SubreportProcessingEventArgs.Parameters); } }
    }
}