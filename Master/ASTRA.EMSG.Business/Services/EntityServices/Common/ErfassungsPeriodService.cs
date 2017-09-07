using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Security;
using System.Linq;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Services.EntityServices.Common
{
    public interface IErfassungsPeriodService : IService
    {
        ErfassungsPeriod GetCurrentErfassungsPeriod();
        ErfassungsPeriodModel GetCurrentErfassungsPeriodModel();
        List<ErfassungsPeriodModel> GetClosedErfassungsPeriodModels();
        ErfassungsPeriod GetEntityById(Guid id);
        ErfassungsPeriod GetCurrentErfassungsPeriod(Mandant mandant);
        ErfassungsPeriod GetOldestClosedErfassungsperiod();
        ErfassungsPeriod GetNewestClosedErfassungsPeriod();
        bool WasThereJahresabschluss();
        List<ErfassungsPeriod> GetErfassungsPeriods(Guid erfassungsPeriodIdVon, Guid erfassungsPeriodIdBis);
        IQueryable<ErfassungsPeriod> GetEntitiesBy(Mandant mandant);
        List<ErfassungsPeriodModel> GetAllErfassungsPeriodModels(NetzErfassungsmodus[] netzErfassungsmodusList);
        List<ErfassungsPeriod> GetErfassungsPeriods(int jahrVon, int jahrBis);
        void InvalidateCurrentErfassungsPeriodCache();
    }

    public class ErfassungsPeriodService : MandantDependentEntityServiceBase<ErfassungsPeriod, ErfassungsPeriodModel>, IErfassungsPeriodService
    {
        private readonly IHttpRequestService cache;

        public ErfassungsPeriodService(ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine, 
            IHttpRequestService httpRequestService,
            ISecurityService securityService) 
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService)
        {
            cache = httpRequestService;
        }

        public ErfassungsPeriod GetCurrentErfassungsPeriod()
        {
            return cache.GetOrUpdateCache("GetCurrentErfassungsPeriod", () => FilteredEntities.Single(ep => !ep.IsClosed));
        }

        public ErfassungsPeriodModel GetCurrentErfassungsPeriodModel()
        {
            return cache.GetOrUpdateCache("GetCurrentErfassungsPeriodModel", () => CreateModel(GetCurrentErfassungsPeriod()));
        }

        public void InvalidateCurrentErfassungsPeriodCache()
        {
            cache.Invalidate("GetCurrentErfassungsPeriod");
            cache.Invalidate("GetCurrentErfassungsPeriodModel");
        }

        public ErfassungsPeriod GetCurrentErfassungsPeriod(Mandant mandant)
        {
            return Queryable.Where(ep => ep.Mandant == mandant).Single(ep => !ep.IsClosed);
        }

        protected override Expression<Func<ErfassungsPeriod, Mandant>> MandantExpression { get { return ef => ef.Mandant; } }

        public List<ErfassungsPeriodModel> GetClosedErfassungsPeriodModels()
        {
            return FilteredEntities.Where(ep => ep.IsClosed).OrderBy(ep => ep.Erfassungsjahr).Select(CreateModel).ToList();
        }

        ErfassungsPeriod IErfassungsPeriodService.GetEntityById(Guid id)
        {
            //var key = string.Format("GetCurrentErfassungsPeriodModel-{0}", id);
            //return cache.GetOrUpdateCache(key, () => GetEntityById(id));
            return GetEntityById(id);
        }

        public List<ErfassungsPeriod> GetErfassungsPeriods(Guid erfassungsPeriodIdVon, Guid erfassungsPeriodIdBis)
        {
            var erfassungsPeriodVon = GetEntityById(erfassungsPeriodIdVon);
            var erfassungsPeriodBis = GetEntityById(erfassungsPeriodIdBis);

            return GetErfassungsPeriods(erfassungsPeriodVon.Erfassungsjahr.Year, erfassungsPeriodBis.Erfassungsjahr.Year);
        }
        
        public List<ErfassungsPeriod> GetErfassungsPeriods(int jahrVon, int jahrBis)
        {
            return FilteredEntities
                .Where(ep => jahrVon <= ep.Erfassungsjahr.Year)
                .Where(ep => ep.Erfassungsjahr.Year <= jahrBis)
                .OrderBy(ep => ep.Erfassungsjahr)
                .ToList();
        }

        public List<ErfassungsPeriodModel> GetAllErfassungsPeriodModels(NetzErfassungsmodus[] netzErfassungsmodusList)
        {
            return FilteredEntities
                .Where(ep => netzErfassungsmodusList.Contains(ep.NetzErfassungsmodus))
                .Select(CreateModel)
                .OrderByDescending(ep => ep.Erfassungsjahr)
                .ToList();
        }

        public bool WasThereJahresabschluss()
        {
            return FilteredEntities.Any(ep => ep.IsClosed);
        }

        public ErfassungsPeriod GetOldestClosedErfassungsperiod()
        {
            return FilteredEntities.Where(ep => ep.IsClosed).OrderBy(ep => ep.Erfassungsjahr).FirstOrDefault();
        }

        public ErfassungsPeriod GetNewestClosedErfassungsPeriod()
        {
            return FilteredEntities.Where(ep => ep.IsClosed).OrderByDescending(ep => ep.Erfassungsjahr).FirstOrDefault();
        }
    }
}