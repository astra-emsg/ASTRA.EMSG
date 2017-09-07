using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Infrastructure.Caching;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Identity;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;

namespace ASTRA.EMSG.Business.Services.Security
{
    public class CachedIdentityRoleService : IIdentityCacheService
    {
        public const string ApplicationName = "EMSG";

        private readonly ISessionCacheService sessionCacheService;
        private readonly IServerConfigurationProvider configurationProvider;
        private readonly IIdentityCacheService roleService;

        public CachedIdentityRoleService(ISessionCacheService sessionCacheService, IServerConfigurationProvider configurationProvider, IdentityRoleService roleService)
        {
            this.sessionCacheService = sessionCacheService;
            this.configurationProvider = configurationProvider;
            this.roleService = roleService;
        }
            
        public List<UserRole> GetRoleMandator(string mandantname, string username)
        {
            return GetCached(string.Format("GetRoleMandator_{0}_{1}", mandantname, username), () => roleService.GetRoleMandator(mandantname, username));
        }

        public List<IdentityMandatorShort> GetMandatorShort(string username)
        {
            return GetCached(string.Format("GetMandatorShort_{0}", username), () => roleService.GetMandatorShort(username));
        }

        public List<UserRole> GetRole(string username)
        {
            return GetCached(string.Format("GetRole_{0}", username), () => roleService.GetRole(username));
        }

        public bool IsUserExists(string username)
        {
            return GetCached(string.Format("IsUserExists_{0}", username), () => roleService.IsUserExists(username));
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