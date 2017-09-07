using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.StrassenabschnitteListeOhneInspektionsroute;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W1_4)]
    public class StrassenabschnitteListeOhneInspektionsrouteController : TabellarischReportControllerBase<StrassenabschnitteListeOhneInspektionsrouteParameter, StrassenabschnitteListeOhneInspektionsroutePo, StrassenabschnitteListeOhneInspektionsrouteGridCommand>
    {
        public StrassenabschnitteListeOhneInspektionsrouteController(ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies) 
            : base(tabellarischeReportControllerBaseDependencies)
        {
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
            ViewBag.NetzErfassungsmodus = reportControllerService.GetNetzErfassungsmodus(erfassungsPeriodId);            
        }

        protected override StrassenabschnitteListeOhneInspektionsrouteParameter GetReportParameter(StrassenabschnitteListeOhneInspektionsrouteGridCommand reportGridCommand)
        {
            return new StrassenabschnitteListeOhneInspektionsrouteParameter
                       {
                           Eigentuemer = reportGridCommand.Eigentuemer,
                           Strassenname = reportGridCommand.Strassenname
                       };
        }
    }
}
