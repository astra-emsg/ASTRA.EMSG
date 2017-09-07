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
    [AllowedModes(NetzErfassungsmodus.Summarisch)]
    [ReportInfo(AuswertungTyp.W5_1)]
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischController : TabellarischReportControllerBase<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischParameter, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPo, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischGridCommand>
    {
        public EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischController(ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies) : base(tabellarischeReportControllerBaseDependencies)
        {
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
        }

        protected override EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischParameter GetReportParameter(EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischGridCommand reportGridCommand)
        {
            return new EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischParameter
            {
                ErfassungsPeriodIdBis = reportGridCommand.ErfassungsPeriodIdBis.Value,
                ErfassungsPeriodIdVon = reportGridCommand.ErfassungsPeriodIdVon.Value,
                Projektname = reportGridCommand.Projektname
            };
        }
    }
}