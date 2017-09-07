using System.Web.Routing;
using JetBrains.Annotations;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class ActionInfo
    {
        public ActionInfo() { }

        public ActionInfo([AspMvcArea] string area, [AspMvcController] string controller, [AspMvcAction] string action)
        {
            Area = area;
            Controller = controller;
            Action = action;
        }

        public ActionInfo(RouteData routeData)
        {
            Area = routeData.GetArea();
            Controller = routeData.GetController();
            Action = routeData.GetAction();
        }

        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}