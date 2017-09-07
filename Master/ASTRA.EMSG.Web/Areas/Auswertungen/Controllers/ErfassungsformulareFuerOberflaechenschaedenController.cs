using System.Web.Mvc;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.AusgefuellteErfassungsformulareFuerOberflaechenschaeden;
using ASTRA.EMSG.Business.Reports.ErfassungsformulareFuerOberflaechenschaeden;
using ASTRA.EMSG.Business.Services;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Common.ControllerServices;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Summarisch, NetzErfassungsmodus.Tabellarisch, NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W3_1)]
    public class ErfassungsformulareFuerOberflaechenschaedenController : Controller
    {
        private readonly IReportControllerService reportControllerService;
        private readonly IServerReportGenerator serverReportGenerator;
        private readonly IEmsgPoProviderService emsgPoProviderService;
        private readonly ISessionService sessionService;

        public ErfassungsformulareFuerOberflaechenschaedenController(
            ISessionService sessionService,
            IEmsgPoProviderService emsgPoProviderService,
            IServerReportGenerator serverReportGenerator, 
            IReportControllerService reportControllerService)
        {
            this.sessionService = sessionService;
            this.emsgPoProviderService = emsgPoProviderService;
            this.serverReportGenerator = serverReportGenerator;
            this.reportControllerService = reportControllerService;
            this.sessionService = sessionService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerateReport(OutputFormat outputFormat, BelagsTyp belagsTyp)
        {
            var parameter = new ErfassungsformulareFuerOberflaechenschaedenParameter { OutputFormat = outputFormat, BelagsTyp = belagsTyp };
            var emsgPoProvider = emsgPoProviderService.CreateEmsgPoProvider(parameter, new PresentationObjectProcessor<AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo>(this));

            sessionService.LastGeneratedReport = serverReportGenerator.GenerateReport(emsgPoProvider);

            return new EmsgEmptyResult();
        }

        public ActionResult GetLastGeneratedReport()
        {
            var lastGeneratedReport = sessionService.LastGeneratedReport;
            sessionService.LastGeneratedReport = null;
            return reportControllerService.GetReportFileResult(lastGeneratedReport);
        }
    }
}