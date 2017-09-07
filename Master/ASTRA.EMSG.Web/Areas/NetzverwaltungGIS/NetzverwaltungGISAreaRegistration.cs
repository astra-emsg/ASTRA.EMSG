using System.Web.Mvc;
using System.Web.Routing;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungGIS
{
    public class NetzverwaltungGISAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "NetzverwaltungGIS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "WMTS",                                           // Route name
               "NetzverwaltungGIS/WMS/GetWmts/{*query}",                          // URL with parameters
                new { controller = "WMS", action = "GetWmts" }
           );
            
            context.MapRoute(
                "NetzverwaltungGIS_default",
                "NetzverwaltungGIS/{controller}/{action}/{id}",
                new { controller = "NetzverwaltungGIS", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
