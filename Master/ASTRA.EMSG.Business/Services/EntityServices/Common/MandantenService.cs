using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.Business.Services.EntityServices.Common
{
    public interface IMandantenService : IService
    {
        Mandant GetMandantById(Guid id);
        List<MandantModel> GetCurrentModels();
        List<Mandant> GetCurrentMandanten();
    }

    public class MandantenService : EntityServiceBase<Mandant, MandantModel>, IMandantenService
    {
        private readonly ISessionService cache;

        public MandantenService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine,
            ISessionService sessionService) 
            : base(transactionScopeProvider, entityServiceMappingEngine)
        {
            cache = sessionService;
        }

        public List<Mandant> GetCurrentMandanten()
        {
            const string key = "GetCurrentMandanten";
            return cache.GetOrUpdateCache(key, () => GetCurrentEntities().ToList());
        }

        public Mandant GetMandantById(Guid id)
        {
            var key = string.Format("GetMandantById-{0}", id);
            return cache.GetOrUpdateCache(key, () => GetEntityById(id));
        }
    }

    public static class StoreServiceExtensions
    {
        public static T GetOrUpdateCache<T>(this IStoreService storeService, string key, Func<T> getValue) where T : class
        {
            var value = storeService[key] as T;
            if (value == null)
            {
                value = getValue();
                storeService[key] = value;
            }
            return value;
        }

        public static void Invalidate(this IStoreService storeService, string key)
        {
            storeService[key] = null;
        }
    }
}