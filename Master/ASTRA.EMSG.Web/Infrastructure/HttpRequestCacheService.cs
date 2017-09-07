using System.Collections.Generic;
using ASTRA.EMSG.Business.Infrastructure.Caching;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Services;
using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class HttpRequestCacheService : CacheServiceBase, IHttpRequestCacheService
    {
        private const string MenuItemModelsKey = "MenuItemModels";

        public HttpRequestCacheService(IHttpRequestService httpRequestService) : base(httpRequestService)
        {
        }

        public List<MenuItemModel> MenuItemModels { get { return (List<MenuItemModel>) this[MenuItemModelsKey]; } set { this[MenuItemModelsKey] = value; } }
    }
}