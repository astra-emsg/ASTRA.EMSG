using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Web.Areas.Common.ControllerServices;

namespace ASTRA.EMSG.Web.Areas.Common.DependencyPackages
{
    public interface IGrafischeReportControllerBaseDependencies : ISingletonDependencyPackage
    {
        IReportControllerService ReportControllerService { get; }
        IEmsgPoProviderService EmsgPoProviderService { get; }
        IServerReportGenerator ServerReportGenerator { get; }
        ISessionService SessionService { get; }
    }

    public class GrafischeReportControllerBaseDependencies : IGrafischeReportControllerBaseDependencies
    {
        public IReportControllerService ReportControllerService { get; private set; }
        public IEmsgPoProviderService EmsgPoProviderService { get; private set; }
        public IServerReportGenerator ServerReportGenerator { get; private set; }
        public ISessionService SessionService { get; private set; }

        public GrafischeReportControllerBaseDependencies(IServerReportGenerator serverReportGenerator, IReportControllerService reportControllerService, IEmsgPoProviderService emsgPoProviderService, ISessionService sessionService)
        {
            ServerReportGenerator = serverReportGenerator;
            ReportControllerService = reportControllerService;
            EmsgPoProviderService = emsgPoProviderService;
            SessionService = sessionService;
        }
    }
}