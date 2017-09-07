using Microsoft.Reporting.WebForms;

namespace ASTRA.EMSG.Business.Infrastructure.Reporting
{
    public interface IReportDataSourceCollection
    {
        void Add(IReportDataSource reportDataSource);
    }

    public class ServerReportDataSourceCollection : IReportDataSourceCollection
    {
        public ReportDataSourceCollection ReportDataSourceCollection { get; private set; }

        public ServerReportDataSourceCollection(ReportDataSourceCollection reportDataSourceCollection)
        {
            ReportDataSourceCollection = reportDataSourceCollection;
        }

        public void Add(IReportDataSource reportDataSource)
        {
            ReportDataSourceCollection.Add(((ServerReportDataSource)reportDataSource).ReportDataSource);
        }
    }
}
