using System.Web.Mvc;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Infrastructure.Filters
{
    public class AvailabilityFilter : FilterAttribute, IAuthorizationFilter
    {
        private readonly IAvailabilityService availabilityService;

        public AvailabilityFilter(IAvailabilityService availabilityService)
        {
            this.availabilityService = availabilityService;
            Order = 10;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (availabilityService.CheckAvailability() != Availability.Available)
            {
                filterContext.Result = new HttpNotFoundResult();
            }
        }

        
    }
}