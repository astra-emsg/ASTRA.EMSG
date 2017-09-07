using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.EineListeVonMassnahmenGegliedertNachTeilsystemen;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W4_1)]
    [ReportInfo(AuswertungTyp.W4_2)]
    public class EineListeVonMassnahmenGegliedertNachTeilsystemenController : GisReportControllerBase<EineListeVonMassnahmenGegliedertNachTeilsystemenParameter, EineListeVonMassnahmenGegliedertNachTeilsystemenMapParameter, EineListeVonMassnahmenGegliedertNachTeilsystemenPo, EineListeVonMassnahmenGegliedertNachTeilsystemenGridCommand>
    {
        public EineListeVonMassnahmenGegliedertNachTeilsystemenController(ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies)
            : base(tabellarischeReportControllerBaseDependencies)
        {
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
            ViewBag.NetzErfassungsmodus = reportControllerService.GetNetzErfassungsmodus(erfassungsPeriodId);
        }

        protected override void SetMapReportParameter(EineListeVonMassnahmenGegliedertNachTeilsystemenParameter mapParameter, EineListeVonMassnahmenGegliedertNachTeilsystemenGridCommand reportGridCommand)
        {
            mapParameter.Projektname = reportGridCommand.Projektname;
            mapParameter.Status = reportGridCommand.StatusTyp;
            mapParameter.Dringlichkeit = reportGridCommand.DringlichkeitTyp;
            mapParameter.Teilsystem = reportGridCommand.TeilsystemTyp;
        }
    }
}
