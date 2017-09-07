using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.EineListeVonRealisiertenMassnahmenGeordnetNachJahren;
using ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProStrassenabschnitt;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Tabellarisch, NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W2_2)]
    public class WiederbeschaffungswertUndWertverlustProStrassenabschnittController : TabellarischReportControllerBase<WiederbeschaffungswertUndWertverlustProStrassenabschnittParameter, WiederbeschaffungswertUndWertverlustProStrassenabschnittPo, WiederbeschaffungswertUndWertverlustProStrassenabschnittGridCommand>
    {
        public WiederbeschaffungswertUndWertverlustProStrassenabschnittController(ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies)
            : base(tabellarischeReportControllerBaseDependencies)
        {
        }

        protected override WiederbeschaffungswertUndWertverlustProStrassenabschnittParameter GetReportParameter(WiederbeschaffungswertUndWertverlustProStrassenabschnittGridCommand reportGridCommand)
        {
            return new WiederbeschaffungswertUndWertverlustProStrassenabschnittParameter
                       {
                           Strassenname = reportGridCommand.Strassenname,
                           Ortsbezeichnung = reportGridCommand.Ortsbezeichnung,
                           Eigentuemer = reportGridCommand.Eigentuemer
                       };
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
        }
    }
}
