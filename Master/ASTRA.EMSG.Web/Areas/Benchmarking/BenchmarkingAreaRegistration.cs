using System.Web.Mvc;

namespace ASTRA.EMSG.Web.Areas.Benchmarking
{
    public class BenchmarkingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Benchmarking";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Benchmarking_default",
                "Benchmarking/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
