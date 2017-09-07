using System;
using System.Web;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Controllers;

namespace ASTRA.EMSG.Web.Infrastructure.Security
{
    public class TestPermissionService : PermissionService
    {
        public TestPermissionService(ISecurityService securityService) : base(securityService)
        {
        }

        public override Access CheckAccess()
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request.RequestContext.RouteData.GetArea()))
                return Access.Granted;

            return base.CheckAccess();
        }

        public override Access CheckAccess(Type controllerType, string actionName)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request.RequestContext.RouteData.GetArea()))
                return Access.Granted;

            return base.CheckAccess(controllerType, actionName);
        }
    }
}