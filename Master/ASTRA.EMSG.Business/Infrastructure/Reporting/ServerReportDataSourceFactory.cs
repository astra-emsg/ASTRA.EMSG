namespace ASTRA.EMSG.Business.Infrastructure.Reporting
{
    public interface IReportDataSourceFactory
    {
        IReportDataSource GetReportDataSource(string name, object value);
    }

    public class ServerReportDataSourceFactory : IReportDataSourceFactory
    {
        public IReportDataSource GetReportDataSource(string name, object value)
        {
            return new ServerReportDataSource(new Microsoft.Reporting.WebForms.ReportDataSource(name, value));
        }
    }
}
