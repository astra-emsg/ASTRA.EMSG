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
    [AllowedModes(NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W5_1)]
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISController : TabellarischReportControllerBase<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISParameter, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPo, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISGridCommand>
    {
        public EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISController(ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies)
            : base(tabellarischeReportControllerBaseDependencies)
        {
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
        }

        protected override EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISParameter GetReportParameter(EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISGridCommand reportGridCommand)
        {
            return new EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISParameter
                       {
                           ErfassungsPeriodIdBis = reportGridCommand.ErfassungsPeriodIdBis.Value,
                           ErfassungsPeriodIdVon = reportGridCommand.ErfassungsPeriodIdVon.Value,
                           LeitendeOrganisation = reportGridCommand.LeitendeOrganisation,
                           Projektname = reportGridCommand.Projektname
                       };
        }
    }
}