using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.ZustandProZustandsabschnitt;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W3_7, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class ZustandProZustandsabschnittGISController : GisReportControllerBase<ZustandProZustandsabschnittParameter, ZustandProZustandsabschnittMapParameter, ZustandProZustandsabschnittPo, ZustandProZustandsabschnittGridCommand>
    {
        public ZustandProZustandsabschnittGISController(
            ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies
            )
            : base(tabellarischeReportControllerBaseDependencies)
        {
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
            ViewBag.NetzErfassungsmodus = reportControllerService.GetNetzErfassungsmodus(erfassungsPeriodId);
        }

        protected override void SetMapReportParameter(ZustandProZustandsabschnittParameter parameter, ZustandProZustandsabschnittGridCommand reportGridCommand)
        {
            parameter.Eigentuemer = reportGridCommand.EigentuemerTyp;
            parameter.Strassenname = reportGridCommand.Strassenname;
            parameter.ZustandsindexVon = reportGridCommand.ZustandsindexVon;
            parameter.ZustandsindexBis = reportGridCommand.ZustandsindexBis;
            parameter.Ortsbezeichnung = reportGridCommand.Ortsbezeichnung;
        }
    }
}