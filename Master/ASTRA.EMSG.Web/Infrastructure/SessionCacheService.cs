using ASTRA.EMSG.Business.Infrastructure.Caching;
using ASTRA.EMSG.Business.Services;
using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class SessionCacheService : CacheServiceBase, ISessionCacheService
    {
        public SessionCacheService(ISessionService sessionService) : base(sessionService)
        {
        }
    }
}