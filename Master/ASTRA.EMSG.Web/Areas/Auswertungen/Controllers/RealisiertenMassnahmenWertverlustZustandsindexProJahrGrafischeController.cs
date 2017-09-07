using System.Linq;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafische;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Security;
using Resources;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Summarisch, NetzErfassungsmodus.Tabellarisch, NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W5_4)]
    public class RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeController : GrafischeReportControllerBase<RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter, RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeDiagramPo>
    {
        private readonly IErfassungsPeriodService erfassungsPeriodService;
        private readonly IKenngroessenFruehererJahreOverviewService kenngroessenFruehererJahreOverviewService;
        private readonly ILocalizationService localizationService;

        public RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeController(
            IGrafischeReportControllerBaseDependencies grafischeReportControllerBaseDependencies,
            IErfassungsPeriodService erfassungsPeriodService,
            IKenngroessenFruehererJahreOverviewService kenngroessenFruehererJahreOverviewService,
            ILocalizationService localizationService)
            : base(grafischeReportControllerBaseDependencies)
        {
            this.erfassungsPeriodService = erfassungsPeriodService;
            this.kenngroessenFruehererJahreOverviewService = kenngroessenFruehererJahreOverviewService;
            this.localizationService = localizationService;
        }

        protected override RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter PrepareViewBagForIndex()
        {
            var erfassungsPeriodModels = erfassungsPeriodService
                .GetAllErfassungsPeriodModels(new[] { NetzErfassungsmodus.Summarisch, NetzErfassungsmodus.Tabellarisch, NetzErfassungsmodus.Gis });

            var erfassugnsPeriodList = erfassungsPeriodModels.ToDropDownItemList(ep =>
                                                                                 {
                                                                                     if (!ep.IsClosed)
                                                                                         return TextLocalization.Current;
                                                                                     return string.Format("{0} ({1})", ep.Erfassungsjahr.Year, localizationService.GetLocalizedEnum(ep.NetzErfassungsmodus));
                                                                                 }, ep => ep.Id, erfassungsPeriodModels.Single(ep => !ep.IsClosed));
            var kengrossen = kenngroessenFruehererJahreOverviewService.GetCurrentModels().OrderByDescending(s => s.Jahr).ToDropDownItemList(
                kg => string.Format("{0} ({1})", kg.Jahr, TextLocalization.FruehererJahre),
                kg => kg.Id);


            ViewBag.ErfassugnsPeriodList = erfassugnsPeriodList.Concat(kengrossen).ToList();
            ViewBag.CurrentErfassungsPeriodId = erfassungsPeriodService.GetCurrentErfassungsPeriod().Id;

            return new RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter();
        }
    }
}
