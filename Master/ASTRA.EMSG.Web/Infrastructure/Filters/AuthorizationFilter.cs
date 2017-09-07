using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Caching;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Exceptions;
using ASTRA.EMSG.Common.Master.Logging;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Business.Models.Identity;

namespace ASTRA.EMSG.Web.Infrastructure.Filters
{
    public class AuthorizationFilter : FilterAttribute, IAuthorizationFilter
    {
        private readonly IPermissionService permissionService;
        private readonly ISecurityService securityService;
        private readonly ISessionService sessionService;
        private readonly IEreignisLogService ereignisLogService;

        public AuthorizationFilter(IPermissionService permissionService, ISecurityService securityService, ISessionService sessionService, IEreignisLogService ereignisLogService)
        {
            this.permissionService = permissionService;
            this.securityService = securityService;
            this.sessionService = sessionService;
            this.ereignisLogService = ereignisLogService;
        }

        private bool IsAuthorized
        {
            get
            {
                var dateTime = DateTime.Now;
                var isAuthorized = permissionService.CheckAccess() == Access.Granted;
                Loggers.PeformanceLogger.Debug(string.Format("==> SecurityCheck Time: {0}", (DateTime.Now - dateTime).TotalMilliseconds));
                return isAuthorized;
            }
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (filterContext.ParentActionViewContext != null && filterContext.ParentActionViewContext.ViewData is ViewDataDictionary<HandleErrorInfo>)
                return;

            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

            if (skipAuthorization)
                return;

            if (IsAuthorized)
            {
                HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                cache.SetProxyMaxAge(new TimeSpan(0L));
                cache.AddValidationCallback(CacheValidateHandler, null);
            }
            else
            {
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string actionName = filterContext.ActionDescriptor.ActionName;
                string userName = securityService.GetCurrentUserName();
                if (!String.IsNullOrEmpty(userName))
                    Loggers.ApplicationLogger.Warn(string.Format("User {0} is unauthorized for Action {1}.{2}! ", userName, controllerName, actionName));
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            if (!IsAuthorized)
                return HttpValidationStatus.IgnoreThisRequest;

            return HttpValidationStatus.Valid;
        }
    }
}