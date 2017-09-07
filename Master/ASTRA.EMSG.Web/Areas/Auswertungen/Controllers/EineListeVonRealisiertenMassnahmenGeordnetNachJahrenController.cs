using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.EineListeVonRealisiertenMassnahmenGeordnetNachJahren;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Tabellarisch)]
    [ReportInfo(AuswertungTyp.W5_1)]
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenController : TabellarischReportControllerBase<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenParameter, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPo, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGridCommand>
    {
        public EineListeVonRealisiertenMassnahmenGeordnetNachJahrenController(ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies)
            : base(tabellarischeReportControllerBaseDependencies)
        {
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
        }

        protected override EineListeVonRealisiertenMassnahmenGeordnetNachJahrenParameter GetReportParameter(EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGridCommand reportGridCommand)
        {
            return new EineListeVonRealisiertenMassnahmenGeordnetNachJahrenParameter
                       {
                           ErfassungsPeriodIdBis = reportGridCommand.ErfassungsPeriodIdBis.Value,
                           ErfassungsPeriodIdVon = reportGridCommand.ErfassungsPeriodIdVon.Value,
                           Projektname = reportGridCommand.Projektname
                       };
        }
    }
}