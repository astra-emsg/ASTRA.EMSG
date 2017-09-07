using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.EineListeVonRealisiertenMassnahmenGeordnetNachJahren;
using ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProJahrGrafische;
using ASTRA.EMSG.Business.Reports.ZustandsspiegelProJahrGrafische;
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
    [ReportInfo(AuswertungTyp.W5_3)]
    public class WiederbeschaffungswertUndWertverlustProJahrGrafischeController : GrafischeReportControllerBase<WiederbeschaffungswertUndWertverlustProJahrGrafischeParameter, WiederbeschaffungswertUndWertverlustProJahrGrafischeDiagramPo>
    {
        private readonly IErfassungsPeriodService erfassungsPeriodService;
        private readonly IKenngroessenFruehererJahreOverviewService kenngroessenFruehererJahreOverviewService;
        private readonly ILocalizationService localizationService;

        public WiederbeschaffungswertUndWertverlustProJahrGrafischeController(
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

        protected override WiederbeschaffungswertUndWertverlustProJahrGrafischeParameter PrepareViewBagForIndex()
        {
            var erfassungsPeriodModels = erfassungsPeriodService
                .GetAllErfassungsPeriodModels(new[] {  NetzErfassungsmodus.Tabellarisch, NetzErfassungsmodus.Gis, NetzErfassungsmodus.Summarisch });

            var erfassugnsPeriodList = erfassungsPeriodModels.ToDropDownItemList(ep =>
                                                                                 {
                                                                                     if (!ep.IsClosed)
                                                                                         return TextLocalization.Current;
                                                                                     return string.Format("{0} ({1})", ep.Erfassungsjahr.Year, localizationService.GetLocalizedEnum(ep.NetzErfassungsmodus));
                                                                                 }, ep => ep.Id, erfassungsPeriodModels.SingleOrDefault(ep => !ep.IsClosed));
            var kengrossen = kenngroessenFruehererJahreOverviewService.GetCurrentModels().OrderByDescending(s => s.Jahr).ToDropDownItemList(
                kg => string.Format("{0} ({1})", kg.Jahr, TextLocalization.FruehererJahre),
                kg => kg.Id);


            ViewBag.ErfassugnsPeriodList = erfassugnsPeriodList.Concat(kengrossen).ToList();
            ViewBag.CurrentErfassungsPeriodId = erfassungsPeriodService.GetCurrentErfassungsPeriod().Id;

            return new WiederbeschaffungswertUndWertverlustProJahrGrafischeParameter();
        }
    }
}
