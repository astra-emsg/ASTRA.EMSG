using System.Web.Mvc;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public static class ControllerContextExtensions
    {
        public static string GetArea(this ControllerContext controllerContext)
        {
            return controllerContext.RouteData.GetArea();
        }

        public static string GetController(this ControllerContext controllerContext)
        {
            return controllerContext.RouteData.GetController();
        }

        public static string GetAction(this ControllerContext controllerContext)
        {
            return controllerContext.RouteData.GetAction();
        }
    }
}