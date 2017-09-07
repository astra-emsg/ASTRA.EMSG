using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.ZustandProZustandsabschnitt;
using ASTRA.EMSG.Business.Services.GIS.WMS;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Tabellarisch)]
    [ReportInfo(AuswertungTyp.W3_5, NetzErfassungsmodus = NetzErfassungsmodus.Tabellarisch)]
    public class ZustandProZustandsabschnittController : TabellarischReportControllerBase<ZustandProZustandsabschnittParameter, ZustandProZustandsabschnittPo, ZustandProZustandsabschnittGridCommand>
    {
        public ZustandProZustandsabschnittController(
            ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies
            )
            : base(tabellarischeReportControllerBaseDependencies)
        {
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
            ViewBag.NetzErfassungsmodus = reportControllerService.GetNetzErfassungsmodus(erfassungsPeriodId);
        }

        protected override ZustandProZustandsabschnittParameter GetReportParameter(ZustandProZustandsabschnittGridCommand reportGridCommand)
        {
            var parameter = new ZustandProZustandsabschnittParameter();
            parameter.Eigentuemer = reportGridCommand.EigentuemerTyp;
            parameter.Strassenname = reportGridCommand.Strassenname;
            parameter.ZustandsindexVon = reportGridCommand.ZustandsindexVon;
            parameter.ZustandsindexBis = reportGridCommand.ZustandsindexBis;
            parameter.Ortsbezeichnung = reportGridCommand.Ortsbezeichnung;
            parameter.Laenge = reportGridCommand.Laenge;
            return parameter;
        }
    }
}