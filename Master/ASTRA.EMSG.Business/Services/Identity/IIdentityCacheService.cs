using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Infrastructure.Caching;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;

namespace ASTRA.EMSG.Business.Services.Identity
{
    public interface IIdentityCacheService
    {
        bool IsUserExists(string username);
        List<UserRole> GetRoleMandator(string mandantname, string username);
        List<IdentityMandatorShort> GetMandatorShort(string username);
        List<UserRole> GetRole(string username);
    }


    public class IdentityCacheService : IIdentityCacheService
    {
        public const string ApplicationName = "EMSG";

        private readonly ISessionCacheService sessionCacheService;
        private readonly IServerConfigurationProvider configurationProvider;
        private readonly IIdentityService identityService;

        public IdentityCacheService(ISessionCacheService sessionCacheService, IServerConfigurationProvider configurationProvider, IIdentityService identityService)
        {
            this.sessionCacheService = sessionCacheService;
            this.configurationProvider = configurationProvider;
            this.identityService = identityService;
        }

        public List<UserRole> GetRoleMandator(string mandantname, string username)
        {
            return GetCached(string.Format("GetRoleMandator_{0}_{1}", mandantname, username), () => identityService.GetRoleMandator(mandantname, username, ApplicationName));
        }

        public List<IdentityMandatorShort> GetMandatorShort(string username)
        {
            return GetCached(string.Format("GetMandatorShort_{0}", username), () => identityService.GetMandatorShort(username, ApplicationName));
        }

        public List<UserRole> GetRole(string username)
        {
            return GetCached(string.Format("GetRole_{0}", username), () => identityService.GetRole(username, ApplicationName));
        }

        public bool IsUserExists(string username)
        {
            return GetCached(string.Format("IsUserExists_{0}", username), () => identityService.IsUserExists(username));
        }

        private T GetCached<T>(string key, Func<T> defaultValue)
        {
            object value = sessionCacheService[key];
            if (value == null)
            {
                T defaultVal = defaultValue();
                sessionCacheService[key, configurationProvider.SecurityCacheTimeout] = defaultVal;
                return defaultVal;
            }
            return (T)value;
        }
    }
}