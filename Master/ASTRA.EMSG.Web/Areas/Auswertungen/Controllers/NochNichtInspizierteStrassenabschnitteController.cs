using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.NochNichtInspizierteStrassenabschnitte;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Tabellarisch, NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W3_4)]
    public class NochNichtInspizierteStrassenabschnitteController : TabellarischReportControllerBase<NochNichtInspizierteStrassenabschnitteParameter, NochNichtInspizierteStrassenabschnittePo, NochNichtInspizierteStrassenabschnitteGridCommand>
    {
        public NochNichtInspizierteStrassenabschnitteController(ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies)
            : base(tabellarischeReportControllerBaseDependencies)
        {
        }

        protected override NochNichtInspizierteStrassenabschnitteParameter GetReportParameter(NochNichtInspizierteStrassenabschnitteGridCommand reportGridCommand)
        {
            return new NochNichtInspizierteStrassenabschnitteParameter
                       {
                           Strassenname = reportGridCommand.Strassenname,
                           Eigentuemer = reportGridCommand.Eigentuemer
                       };
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
        }
    }
}
