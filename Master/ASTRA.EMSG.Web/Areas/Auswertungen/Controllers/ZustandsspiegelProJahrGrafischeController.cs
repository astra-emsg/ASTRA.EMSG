using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.ZustandsspiegelProJahrGrafische;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Security;
using Resources;
using System.Linq;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Tabellarisch, NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W5_2)]
    public class ZustandsspiegelProJahrGrafischeController : GrafischeReportControllerBase<ZustandsspiegelProJahrGrafischeParameter, ZustandsspiegelProJahrGrafischeDiagramPo>
    {
        private readonly IErfassungsPeriodService erfassungsPeriodService;
        private readonly ILocalizationService localizationService;

        public ZustandsspiegelProJahrGrafischeController(
            IGrafischeReportControllerBaseDependencies grafischeReportControllerBaseDependencies,
            IErfassungsPeriodService erfassungsPeriodService,
            ILocalizationService localizationService)
            : base(grafischeReportControllerBaseDependencies)
        {
            this.erfassungsPeriodService = erfassungsPeriodService;
            this.localizationService = localizationService;
        }

        protected override ZustandsspiegelProJahrGrafischeParameter PrepareViewBagForIndex()
        {
            var erfassungsPeriodModels = erfassungsPeriodService
                .GetAllErfassungsPeriodModels(new[] { NetzErfassungsmodus.Tabellarisch, NetzErfassungsmodus.Gis });

            ViewBag.ErfassugnsPeriodList = erfassungsPeriodModels.ToDropDownItemList(
                ep =>
                    {
                        if (!ep.IsClosed)
                            return TextLocalization.Current;
                        return string.Format("{0} ({1})", ep.Erfassungsjahr.Year,
                                             localizationService.GetLocalizedEnum(ep.NetzErfassungsmodus));
                    }, ep => ep.Id,
                erfassungsPeriodModels.Single(ep => !ep.IsClosed));

            ViewBag.CurrentErfassungsPeriodId = erfassungsPeriodService.GetCurrentErfassungsPeriod().Id;

            return new ZustandsspiegelProJahrGrafischeParameter();
        }
    }
}
