using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Summarisch;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using System.Linq;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Services.EntityServices.Summarisch
{
    public interface INetzSummarischDetailService : IService
    {
        IQueryable<NetzSummarischDetail> GetEntitiesBy(ErfassungsPeriod getErfassungsPeriod);
        IQueryable<NetzSummarischDetail> GetEntitiesBy(ErfassungsPeriod getErfassungsPeriod, Mandant mandant);
        NetzSummarischDetailModel GetById(Guid id);
        NetzSummarischDetailModel UpdateEntity(NetzSummarischDetailModel netzSummarischDetailModel);
        List<NetzSummarischDetailModel> GetCurrentModels();
        IQueryable<NetzSummarischDetail> GetEntites();
    }

    public class NetzSummarischDetailService : MandantAndErfassungsPeriodDependentEntityServiceBase<NetzSummarischDetail, NetzSummarischDetailModel>, INetzSummarischDetailService
    {
        private readonly ILocalizationService localizationService;

        public NetzSummarischDetailService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine,
            ISecurityService securityService, 
            IHistorizationService historizationService,
            ILocalizationService localizationService)
            :base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.localizationService = localizationService;
        }

        protected override Expression<Func<NetzSummarischDetail, Mandant>> MandantExpression { get { return nsd => nsd.NetzSummarisch.Mandant; } }
        protected override Expression<Func<NetzSummarischDetail, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return nsd => nsd.NetzSummarisch.ErfassungsPeriod; } }

        public override List<NetzSummarischDetailModel> GetCurrentModels()
        {
            return FilteredEntities.OrderBy(ns => ns.Belastungskategorie.Reihenfolge).Fetch(ns => ns.Belastungskategorie).Select(CreateModel).ToList();
        }

        public IQueryable<NetzSummarischDetail> GetEntites()
        {
            return FilteredEntities;
        }

        protected override void OnModelCreated(NetzSummarischDetail entity, NetzSummarischDetailModel model)
        {
            model.BelastungskategorieTyp = localizationService.GetLocalizedBelastungskategorieTyp(model.BelastungskategorieTyp);
        }
    }
}