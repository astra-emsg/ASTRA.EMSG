using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Infrastructure.Caching;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;

namespace ASTRA.EMSG.Business.Services.EntityServices.Katalogs
{
    public interface IWiederbeschaffungswertKatalogService : IService
    {
        WiederbeschaffungswertKatalogModel GetWiederbeschaffungswertKatalogModel(Belastungskategorie belastungskategorie);
        WiederbeschaffungswertKatalogModel GetWiederbeschaffungswertKatalogModel(Belastungskategorie belastungskategorie, ErfassungsPeriod erfassungsPeriod);
    }

    public class WiederbeschaffungswertKatalogService : MandantAndErfassungsPeriodDependentEntityServiceBase<WiederbeschaffungswertKatalog, WiederbeschaffungswertKatalogModel>, IWiederbeschaffungswertKatalogService
    {
        private readonly IHttpRequestCacheService httpRequestCacheService;

        public WiederbeschaffungswertKatalogService(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine,
            IHttpRequestCacheService httpRequestCacheService, ISecurityService securityService, IHistorizationService historizationService)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.httpRequestCacheService = httpRequestCacheService;
        }

        public WiederbeschaffungswertKatalogModel GetWiederbeschaffungswertKatalogModel(Belastungskategorie belastungskategorie)
        {
            return GetWiederbeschaffungswertKatalogModelInternal(belastungskategorie, historizationService.GetCurrentErfassungsperiod());
        }

        public WiederbeschaffungswertKatalogModel GetWiederbeschaffungswertKatalogModel(Belastungskategorie belastungskategorie, ErfassungsPeriod erfassungsPeriod)
        {
            return GetWiederbeschaffungswertKatalogModelInternal(belastungskategorie, erfassungsPeriod);
        }

        private WiederbeschaffungswertKatalogModel GetWiederbeschaffungswertKatalogModelInternal(Belastungskategorie belastungskategorie, ErfassungsPeriod erfassungsPeriod)
        {
            var wiederbeschaffungswertKatalogModels = GetWiederbeschaffungswertKatalogModelDictionary();

            Dictionary<Guid, WiederbeschaffungswertKatalogModel> wieder;
            if (!wiederbeschaffungswertKatalogModels.TryGetValue(erfassungsPeriod.Id, out wieder))
            {
                wieder = GetEntitiesBy(erfassungsPeriod, erfassungsPeriod.Mandant).ToDictionary(wk => wk.Belastungskategorie.Id, CreateModel);
                wiederbeschaffungswertKatalogModels.Add(erfassungsPeriod.Id, wieder);
            }

            return wieder[belastungskategorie.Id];
        }

        private Dictionary<Guid, Dictionary<Guid, WiederbeschaffungswertKatalogModel>> GetWiederbeschaffungswertKatalogModelDictionary()
        {
            var wiederbeschaffungswertKatalogModels = (Dictionary<Guid, Dictionary<Guid, WiederbeschaffungswertKatalogModel>>)httpRequestCacheService[CacheKey.WiederbeschaffungswertKatalogModels];
            if (wiederbeschaffungswertKatalogModels == null)
            {
                wiederbeschaffungswertKatalogModels = new Dictionary<Guid, Dictionary<Guid, WiederbeschaffungswertKatalogModel>>();
                httpRequestCacheService[CacheKey.WiederbeschaffungswertKatalogModels] = wiederbeschaffungswertKatalogModels;
            }

            return wiederbeschaffungswertKatalogModels;
        }

        protected override Expression<Func<WiederbeschaffungswertKatalog, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return wbk => wbk.ErfassungsPeriod; } }
        protected override Expression<Func<WiederbeschaffungswertKatalog, Mandant>> MandantExpression { get { return wbk => wbk.Mandant; } }
    }
}