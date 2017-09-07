using System;
using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.Business.Infrastructure.Caching
{
    public class CacheServiceBase
    {
        private const string ExpirePostFix = "_Expire";
        private const string CachePostFix = "_Cache";

        private readonly IStoreService storeService;

        public CacheServiceBase(IStoreService storeService)
        {
            this.storeService = storeService;
        }

        public virtual object this[string key]
        {
            get
            {
                if (DateTime.Now > ((DateTime?)storeService[key + ExpirePostFix] ?? DateTime.MaxValue))
                {
                    storeService[key + ExpirePostFix] = null;
                    storeService[key + CachePostFix] = null;
                }
                return storeService[key + CachePostFix];
            }
            set { storeService[key + CachePostFix] = value; }
        }

        public virtual object this[string key, int timeoutInSeconds]
        {
            set
            {
                if (storeService[key + ExpirePostFix] == null)
                    storeService[key + ExpirePostFix] = DateTime.Now.AddSeconds(timeoutInSeconds);

                storeService[key + CachePostFix] = value;
            }
        }

        public void Reset(string key)
        {
            storeService[key + ExpirePostFix] = null;
            storeService[key + CachePostFix] = null;
        }
    }
}