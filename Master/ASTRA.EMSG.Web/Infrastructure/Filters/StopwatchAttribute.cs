using System.Configuration;
using System.Diagnostics;
using System.Web.Mvc;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.Common.Master.Logging;

namespace ASTRA.EMSG.Web.Infrastructure.Filters
{
    public class StopwatchAttribute : ActionFilterAttribute
    {
        private readonly Stopwatch stopwatch;
        private readonly ServerConfigurationProvider config;

        public StopwatchAttribute()
        {
            stopwatch = new Stopwatch();
            config = new ServerConfigurationProvider();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (config.Environment == ApplicationEnvironment.SpecFlow)
                return;

            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            if (filterContext.IsChildAction)
                return;
            
            Debug.Assert(!string.IsNullOrWhiteSpace(controllerName));
            
            stopwatch.Restart();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (config.Environment == ApplicationEnvironment.SpecFlow)
                return;

            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            
            if (filterContext.IsChildAction)
                return;

            stopwatch.Stop();

            Loggers.PeformanceLogger.DebugFormat("Action {0}.{1} executed in {2}", controllerName, actionName, stopwatch.Elapsed);
        }
    }
}