using ASTRA.EMSG.Business.Reports.BenchmarkauswertungKennwertenRealisiertenMassnahmen;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Benchmarking.Controllers
{
    public class BenchmarkauswertungKennwertenRealisiertenMassnahmenController : BenchmarkauswertungControllerBase<BenchmarkauswertungKennwertenRealisiertenMassnahmenParameter, BenchmarkauswertungKennwertenRealisiertenMassnahmenPo>
    {
        public BenchmarkauswertungKennwertenRealisiertenMassnahmenController(IGrafischeReportControllerBaseDependencies grafischeReportControllerBaseDependencies, IErfassungsPeriodService erfassungsPeriodService, IKenngroessenFruehererJahreOverviewService kenngroessenFruehererJahreOverviewService, ILocalizationService localizationService) : base(grafischeReportControllerBaseDependencies, erfassungsPeriodService, kenngroessenFruehererJahreOverviewService, localizationService)
        {
        }
    }
}
