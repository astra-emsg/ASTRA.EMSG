using System.Web.Mvc;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungStrassennamen
{
    public class NetzverwaltungStrassennamenAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "NetzverwaltungStrassennamen";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "NetzverwaltungStrassennamen_default",
                "NetzverwaltungStrassennamen/{controller}/{action}/{id}",
                new { controller = "NetzverwaltungStrassennamen", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
