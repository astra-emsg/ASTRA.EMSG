using System.Web.Mvc;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungSummarisch
{
    public class NetzverwaltungSummarischAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "NetzverwaltungSummarisch";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "NetzverwaltungSummarisch_default",
                "NetzverwaltungSummarisch/{controller}/{action}/{id}",
                new { controller = "NetzverwaltungSummarisch", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
