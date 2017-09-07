using System.Collections.Generic;
using ASTRA.EMSG.Business.Models.Common;

namespace ASTRA.EMSG.Business.Infrastructure.Caching
{
    public interface IHttpRequestCacheService : ICacheService
    {
        List<MenuItemModel> MenuItemModels { get; set; }
    }
}