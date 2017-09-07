using System.Web.Mvc;
using ASTRA.EMSG.Business.Services;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers
{
    public class KenngroessenFruehererJahreGISController : KenngroessenFruehererJahreControllerBase
    {
        public KenngroessenFruehererJahreGISController(ILocalizationService localizationService, IBelastungskategorieService belastungskategorieService, IKenngroessenFruehererJahreOverviewService kenngroessenFruehererJahreOverviewService, IKenngroessenFruehererJahreService kenngroessenFruehererJahreService, IErfassungsPeriodService erfassungsPeriodService) : base(localizationService, belastungskategorieService, kenngroessenFruehererJahreOverviewService, kenngroessenFruehererJahreService, erfassungsPeriodService)
        {
        }
    }
}
