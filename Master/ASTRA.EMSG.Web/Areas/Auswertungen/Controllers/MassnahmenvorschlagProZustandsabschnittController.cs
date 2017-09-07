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
    [AllowedModes(NetzErfassungsmodus.Tabellarisch)]
    [ReportInfo(AuswertungTyp.W3_6, NetzErfassungsmodus = NetzErfassungsmodus.Tabellarisch)]
    public class MassnahmenvorschlagProZustandsabschnittController : TabellarischReportControllerBase<MassnahmenvorschlagProZustandsabschnittParameter, MassnahmenvorschlagProZustandsabschnittPo, MassnahmenvorschlagProZustandsabschnittGridCommand>
    {
        public MassnahmenvorschlagProZustandsabschnittController(
            ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies
            ) : base(tabellarischeReportControllerBaseDependencies)
        {
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
            ViewBag.NetzErfassungsmodus = reportControllerService.GetNetzErfassungsmodus(erfassungsPeriodId);
        }


        protected override MassnahmenvorschlagProZustandsabschnittParameter GetReportParameter(MassnahmenvorschlagProZustandsabschnittGridCommand reportGridCommand)
        {
            var reportParameter = new MassnahmenvorschlagProZustandsabschnittParameter
            {
                Eigentuemer = reportGridCommand.EigentuemerTyp,
                Dringlichkeit = reportGridCommand.DringlichkeitTyp,
                Strassenname = reportGridCommand.Strassenname,
                ZustandsindexVon = reportGridCommand.ZustandsindexVon,
                ZustandsindexBis = reportGridCommand.ZustandsindexBis,
                Ortsbezeichnung = reportGridCommand.Ortsbezeichnung
            };

            return reportParameter;
        }

//        protected override void SetMapReportParameter(MassnahmenvorschlagProZustandsabschnittParameter mapParameter, MassnahmenvorschlagProZustandsabschnittGridCommand reportGridCommand)
//        {
//            mapParameter.Dringlichkeit = reportGridCommand.DringlichkeitTyp;
//            mapParameter.Eigentuemer = reportGridCommand.EigentuemerTyp;
//            mapParameter.Strassenname = reportGridCommand.Strassenname;
//            mapParameter.ZustandsindexVon = reportGridCommand.ZustandsindexVon;
//            mapParameter.ZustandsindexBis = reportGridCommand.ZustandsindexBis;
//
//        }
    }
}
