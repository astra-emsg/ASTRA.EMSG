using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Security;
using NHibernate.Linq;
using System.Linq;

namespace ASTRA.EMSG.Business.Services.EntityServices.Common
{
    public interface IKenngroessenFruehererJahreOverviewService : IService
    {
        List<KenngroessenFruehererJahreOverviewModel> GetCurrentModels();
    }
    
    public class KenngroessenFruehererJahreOverviewService : MandantDependentEntityServiceBase<KenngroessenFruehererJahre, KenngroessenFruehererJahreOverviewModel>, IKenngroessenFruehererJahreOverviewService
    {
        public KenngroessenFruehererJahreOverviewService(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine, ISecurityService securityService) : base(transactionScopeProvider, entityServiceMappingEngine, securityService)
        {
        }

        public override List<KenngroessenFruehererJahreOverviewModel> GetCurrentModels()
        {
            return FilteredEntities.FetchMany(e => e.KenngroesseFruehereJahrDetails).Select(CreateModel).ToList();
        }

        protected override void OnModelCreated(KenngroessenFruehererJahre entity, KenngroessenFruehererJahreOverviewModel model)
        {
            base.OnModelCreated(entity, model);
            model.KenngroesseFruehereJahrDetailOverviewModels = 
                entity.KenngroesseFruehereJahrDetails
                .OrderBy(kjd => kjd.Belastungskategorie.Reihenfolge)
                .Select(CreateModelFromEntity<KenngroessenFruehererJahreDetailOverviewModel, KenngroessenFruehererJahreDetail>)
                .ToList();
        }

        protected override Expression<Func<KenngroessenFruehererJahre, Mandant>> MandantExpression { get { return k => k.Mandant; } }
    }
}