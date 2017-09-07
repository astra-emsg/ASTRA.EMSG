using System.IO;
using ASTRA.EMSG.Business.Reporting;

namespace ASTRA.EMSG.Business.Infrastructure.Reporting
{
    public interface IReportResourceLocator
    {
        Stream GetReportDefinitionStream(string reportDefinitionResourceName);

        Stream GetHelperStream(string helperResourceName);

        ReportSizeCollection GetReportSizeCollection<T>() where T : IEmsgPoProviderBase;
    }
}