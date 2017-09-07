namespace ASTRA.EMSG.Business.Infrastructure.Reporting
{
    public interface IReportDataSource
    {
    }

    public class ServerReportDataSource : IReportDataSource
    {
        public Microsoft.Reporting.WebForms.ReportDataSource ReportDataSource { get; private set; }

        public ServerReportDataSource(Microsoft.Reporting.WebForms.ReportDataSource reportDataSource)
        {
            ReportDataSource = reportDataSource;
        }
    }
}
