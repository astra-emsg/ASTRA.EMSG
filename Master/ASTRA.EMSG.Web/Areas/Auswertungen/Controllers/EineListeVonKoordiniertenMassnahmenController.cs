using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.EineListeVonKoordiniertenMassnahmen;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W4_3)]
    [ReportInfo(AuswertungTyp.W4_4)]
    public class EineListeVonKoordiniertenMassnahmenController : GisReportControllerBase<EineListeVonKoordiniertenMassnahmenParameter,EineListeVonKoordiniertenMassnahmenMapParameter, EineListeVonKoordiniertenMassnahmenPo, EineListeVonKoordiniertenMassnahmenGridCommand>
    {
        public EineListeVonKoordiniertenMassnahmenController(ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies) 
            : base(tabellarischeReportControllerBaseDependencies)
        {
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
            ViewBag.NetzErfassungsmodus = reportControllerService.GetNetzErfassungsmodus(erfassungsPeriodId);
        }

        protected override void SetMapReportParameter(EineListeVonKoordiniertenMassnahmenParameter mapParameter, EineListeVonKoordiniertenMassnahmenGridCommand reportGridCommand)
        {
            mapParameter.Status = reportGridCommand.StatusTyp;
            mapParameter.Projektname = reportGridCommand.Projektname;
            mapParameter.AusfuehrungsanfangVon = reportGridCommand.AusfuehrungsanfangVon;
            mapParameter.AusfuehrungsanfangBis = reportGridCommand.AusfuehrungsanfangBis;
        }
    }
}
