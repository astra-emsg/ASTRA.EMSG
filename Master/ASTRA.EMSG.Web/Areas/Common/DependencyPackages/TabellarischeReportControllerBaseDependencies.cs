using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Web.Areas.Common.ControllerServices;

namespace ASTRA.EMSG.Web.Areas.Common.DependencyPackages
{
    public interface ITabellarischeReportControllerBaseDependencies : ISingletonDependencyPackage
    {
        IReportControllerService ReportControllerService { get; }
        IEmsgPoProviderService EmsgPoProviderService { get; }
        IServerReportGenerator ServerReportGenerator { get; }
        ISessionService SessionService { get; }
        IErfassungsPeriodService ErfassungsPeriodService { get; }
    }

    public class TabellarischeReportControllerBaseDependencies : ITabellarischeReportControllerBaseDependencies
    {
        public IReportControllerService ReportControllerService { get; private set; }
        public IEmsgPoProviderService EmsgPoProviderService { get; private set; }
        public IServerReportGenerator ServerReportGenerator { get; private set; }
        public ISessionService SessionService { get; private set; }
        public IErfassungsPeriodService ErfassungsPeriodService { get; private set; }

        public TabellarischeReportControllerBaseDependencies(
            IServerReportGenerator serverReportGenerator,
            IReportControllerService reportControllerService, 
            IEmsgPoProviderService emsgPoProviderService, 
            ISessionService sessionService,
            IErfassungsPeriodService erfassungsPeriodService)
        {
            ServerReportGenerator = serverReportGenerator;
            ReportControllerService = reportControllerService;
            EmsgPoProviderService = emsgPoProviderService;
            SessionService = sessionService;
            ErfassungsPeriodService = erfassungsPeriodService;
        }
    }
}