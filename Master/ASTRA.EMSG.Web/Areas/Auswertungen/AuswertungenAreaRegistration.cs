using System.Web.Mvc;

namespace ASTRA.EMSG.Web.Areas.Auswertungen
{
    public class AuswertungenAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Auswertungen";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "InspektionsrouteLegendLabel",                                          
               "Auswertungen/ListeDerInspektionsrouten/GetInspektionsRouteReportLegendImage/{*query}",                          
                new { controller = "ListeDerInspektionsrouten", action = "GetInspektionsRouteReportLegendImage" }
           );
            context.MapRoute(
                "Auswertungen_default",
                "Auswertungen/{controller}/{action}/{id}",
                new { controller = "Auswertungen", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
