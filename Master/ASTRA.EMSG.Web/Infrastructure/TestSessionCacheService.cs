using ASTRA.EMSG.Business.Services;
using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class TestSessionCacheService : SessionCacheService
    {
        private readonly ISessionService sessionService;

        public TestSessionCacheService(ISessionService sessionService) : base(sessionService)
        {
            this.sessionService = sessionService;
        }

        public override object this[string key]
        {
            get { return sessionService[key]; }
            set { sessionService[key] = value; }
        }

        public override object this[string key, int timeoutInSeconds]
        {
            set { sessionService[key] = value; }
        }
    }
}