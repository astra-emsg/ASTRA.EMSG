using ASTRA.EMSG.Business.Reports.BenchmarkauswertungZustandskennwerten;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Benchmarking.Controllers
{
    public class BenchmarkauswertungZustandskennwertenController : BenchmarkauswertungControllerBase<BenchmarkauswertungZustandskennwertenParameter, BenchmarkauswertungZustandskennwertenPo>
    {
        public BenchmarkauswertungZustandskennwertenController(IGrafischeReportControllerBaseDependencies grafischeReportControllerBaseDependencies, IErfassungsPeriodService erfassungsPeriodService, IKenngroessenFruehererJahreOverviewService kenngroessenFruehererJahreOverviewService, ILocalizationService localizationService) : base(grafischeReportControllerBaseDependencies, erfassungsPeriodService, kenngroessenFruehererJahreOverviewService, localizationService)
        {
        }
    }
}
