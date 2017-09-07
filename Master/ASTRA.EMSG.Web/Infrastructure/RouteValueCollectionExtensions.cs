using System.Web;
using System.Web.Routing;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public static class RouteValueCollectionExtensions
    {
        public static RouteValueDictionary MergeFromRequest(this object routeValues, HttpRequestBase requestBase, params string[] valuesToCopy)
        {
            var result = new RouteValueDictionary(routeValues);
            foreach (var key in valuesToCopy)
            {
                result.Add(key, requestBase.Params.Get(key));
            }
            return result;
        }

        public static RouteValueDictionary MergeFromRequest(this RouteValueDictionary routeValues, HttpRequestBase requestBase, params string[] valuesToCopy)
        {
            var result = new RouteValueDictionary(routeValues);
            foreach (var key in valuesToCopy)
            {
                result.Add(key, requestBase.Params.Get(key));
            }
            return result;
        }
    }
}