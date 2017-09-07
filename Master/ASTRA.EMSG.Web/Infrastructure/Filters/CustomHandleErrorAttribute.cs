using System.Web.Mvc;
using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.Web.Infrastructure.Filters
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            DependencyResolver.Current.GetService<IHttpRequestService>().LastException = filterContext.Exception;
        }
    }
}