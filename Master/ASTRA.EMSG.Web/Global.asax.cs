using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.Common.Master.Logging;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Container;
using ASTRA.EMSG.Web.Infrastructure.Filters;
using ASTRA.EMSG.Web.Infrastructure.Security;
using Autofac;
using Autofac.Integration.Mvc;
using FluentValidation;
using FluentValidation.Mvc;
using ASTRA.EMSG.Common.Utils;
using NHibernate.Logging.CommonLogging;
using StackExchange.Profiling;

namespace ASTRA.EMSG.Master
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        private static ITransactionScopeProvider transactionScopeProvider;
        private static ICookieService cookieService;
        private static IServerConfigurationProvider serverConfigurationProvider;
        private static IHttpRequestService httpRequestService;
        private static IEreignisLogService ereignisLogService;
        private static ISecurityService securityService;

        protected void Application_Start()
        {
            var dateTime = DateTime.Now;
            SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));
            //Autofac Container Setup
            var container = new ServerContainerSetup().BuildContainer();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();

            //Register Filters
            GlobalFilters.Filters.Add(new AuthorizationFilter(container.Resolve<IPermissionService>(), container.Resolve<ISecurityService>(), container.Resolve<ISessionService>(), container.Resolve<IEreignisLogService>()));
            GlobalFilters.Filters.Add(new AvailabilityFilter(container.Resolve<IAvailabilityService>()));
            GlobalFilters.Filters.Add(new StopwatchAttribute(), 3);
            // to force httpRuntime executionTimeout="10" locally
            //GlobalFilters.Filters.Add(new ExecutionTimeoutToGetAroundBugInMVCAttribute());

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            FluentValidationModelValidatorProvider.Configure(provider => provider.ValidatorFactory = container.Resolve<IValidatorFactory>());
            ValidatorOptions.DisplayNameResolver += DisplayNameResolver;
            ValidatorOptions.ResourceProviderType = typeof(ValidationErrorLocalizationWrapper);

            transactionScopeProvider = container.Resolve<ITransactionScopeProvider>();
            cookieService = container.Resolve<ICookieService>();
            serverConfigurationProvider = container.Resolve<IServerConfigurationProvider>();
            ereignisLogService = container.Resolve<IEreignisLogService>();
            httpRequestService = container.Resolve<IHttpRequestService>();
            securityService = container.Resolve<ISecurityService>();

            Loggers.ApplicationLogger.DebugFormat("Initialization duration: {0} second", (DateTime.Now - dateTime).TotalSeconds);

            if (serverConfigurationProvider.EnableMiniProfiler)
            {
                MiniProfiler.Settings.SqlFormatter = new Web.Infrastructure.MiniProfiler.OracleFormatter();
                MiniProfiler.Settings.IgnoredPaths = new[] { "/WMS", "/content/", "/scripts/", "/favicon.ico", "/OpenLayers-2.10/" };
                MiniProfiler.Settings.StackMaxLength = 720;
                MiniProfiler.Settings.MaxJsonResponseSize = 10000000;
            }
        }

        protected void Application_BeginRequest()
        {
            if (serverConfigurationProvider.EnableMiniProfiler)
                MiniProfiler.Start();

            if (!cookieService.EmsgLanguage.HasValue)
                cookieService.EmsgLanguage = EmsgLanguage.Ch;

            CultureInfo cultureInfo = cookieService.EmsgLanguage.ToCultureInfo();

            //Note: SpecFlow feature files are in de-AT
            if (serverConfigurationProvider.Environment == ApplicationEnvironment.SpecFlow)
                cultureInfo = CultureInfo.CreateSpecificCulture("de-at");

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        protected void Application_EndRequest()
        {
            if (transactionScopeProvider.HasRequestTransaction)
            {
                try
                {
                    if (HttpContext.Current.Server.GetLastError() != null || httpRequestService.LastException != null)
                        transactionScopeProvider.CurrentTransactionScope.Rollback();
                    else
                        transactionScopeProvider.CurrentTransactionScope.Commit();
                }
                catch (Exception ex)
                {
                    Loggers.ApplicationLogger.Error(ex.Message, ex);
                }
                finally
                {
                    transactionScopeProvider.ResetCurrentTransactionScope();
                }
            }

            if (serverConfigurationProvider.EnableMiniProfiler)
                MiniProfiler.Stop();
        }

        private string DisplayNameResolver(Type type, MemberInfo memberInfo, LambdaExpression lambdaExpression)
        {
            if (memberInfo == null)
                return null;

            string resourceKey = string.Format("{0}_{1}", type.Name, memberInfo.Name);
            var localizedPropertyName = Resources.ModelLocalization.ResourceManager.GetString(resourceKey);
            if (!localizedPropertyName.HasText())
                localizedPropertyName = Resources.ModelLocalization.ResourceManager.GetString(memberInfo.Name);

            return localizedPropertyName;
        }

        //Note: Not called when customerrors = "On" http://stackoverflow.com/questions/6508415/application-error-not-firing-when-customerrors-on
        protected void Application_Error(object sender, EventArgs args)
        {
            Exception lastError = HttpContext.Current.Server.GetLastError() ?? httpRequestService.LastException;

            Guid trackId = Guid.NewGuid();
            httpRequestService.LastErrorTrackId = trackId;
            var userinfo = securityService.GetCurrentUserInfo();
            string errorText = "";

            if (userinfo != null)
                errorText = String.Format("CurrentUser: {0}, CurrentMandant: {1}", userinfo.CurrentUserName, userinfo.CurrentMandantName) + Environment.NewLine;

            Loggers.ApplicationLogger.Error(errorText + string.Format("[TrackingId: {0}][{1}]{2}", trackId, ErrorLoggingContextInfoStore.CurrentErrorLoggingContextInfo, lastError.Message), lastError);

            if (HttpContext.Current.Session != null)
                ereignisLogService.LogEreignis(EreignisTyp.Systemfehler, new Dictionary<string, object> { { "fehler", lastError.Message } });
        }
    }
}