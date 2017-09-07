using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.MassnahmenvorschlagProZustandsabschnitt;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W3_8, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class MassnahmenvorschlagProZustandsabschnittGISController : GisReportControllerBase<MassnahmenvorschlagProZustandsabschnittParameter, MassnahmenvorschlagProZustandsabschnittMapParameter, MassnahmenvorschlagProZustandsabschnittPo, MassnahmenvorschlagProZustandsabschnittGridCommand>
    {
        public MassnahmenvorschlagProZustandsabschnittGISController(
            ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies
            ) : base(tabellarischeReportControllerBaseDependencies)
        {
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
            ViewBag.NetzErfassungsmodus = reportControllerService.GetNetzErfassungsmodus(erfassungsPeriodId);
        }

        protected override void SetMapReportParameter(MassnahmenvorschlagProZustandsabschnittParameter mapParameter, MassnahmenvorschlagProZustandsabschnittGridCommand reportGridCommand)
        {
            mapParameter.Dringlichkeit = reportGridCommand.DringlichkeitTyp;
            mapParameter.Eigentuemer = reportGridCommand.EigentuemerTyp;
            mapParameter.Strassenname = reportGridCommand.Strassenname;
            mapParameter.ZustandsindexVon = reportGridCommand.ZustandsindexVon;
            mapParameter.ZustandsindexBis = reportGridCommand.ZustandsindexBis;
            mapParameter.Ortsbezeichnung = reportGridCommand.Ortsbezeichnung;

        }
    }
}
