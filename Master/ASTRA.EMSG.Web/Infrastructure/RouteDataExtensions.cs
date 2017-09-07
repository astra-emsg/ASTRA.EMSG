using System.Web.Routing;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public static class RouteDataExtensions
    {
        public static string GetArea(this RouteData routeData)
        {
            return (string)routeData.DataTokens["area"] ?? string.Empty;
        }

        public static string GetController(this RouteData routeData)
        {
            return (string)routeData.Values["controller"] ?? string.Empty;
        }

        public static string GetAction(this RouteData routeData)
        {
            return (string)routeData.Values["action"] ?? string.Empty;
        }
    }
}