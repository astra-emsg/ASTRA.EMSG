using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ASTRA.EMSG.Web.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    internal sealed class ExecutionTimeoutToGetAroundBugInMVCAttribute : ActionFilterAttribute
    {
        public ExecutionTimeoutToGetAroundBugInMVCAttribute() { }
        private MethodInfo _beginMethod;
        private MethodInfo _endMethod;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var context = HttpContext.Current;

                if (_beginMethod == null) //thread locking is known to loose here since it only affects app start up.
                    _beginMethod = context.GetType().GetMethod("BeginCancellablePeriod", BindingFlags.NonPublic | BindingFlags.Instance);

                _beginMethod.Invoke(context, null);
            }
            catch (Exception ex)
            {
                //GroupCommerce.Logging.LoggerPlaceHolder.RealIfExistsOrFakeLogger.Error("Could not call BeginCancellablePeriod() on http context.", ex);
            }

        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                var context = HttpContext.Current;

                if (_endMethod == null) //thread locking is known to loose here since it only affects app start up.
                    _endMethod = context.GetType().GetMethod("EndCancellablePeriod", BindingFlags.NonPublic | BindingFlags.Instance);

                _endMethod.Invoke(context, null);
            }
            catch (Exception ex)
            {
                //GroupCommerce.Logging.LoggerPlaceHolder.RealIfExistsOrFakeLogger.Error("Could not call EndCancellablePeriod() on http context.", ex);
            }
        }
    }
}