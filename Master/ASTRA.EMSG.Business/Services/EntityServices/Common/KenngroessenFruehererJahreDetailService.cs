using System;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Security;

namespace ASTRA.EMSG.Business.Services.EntityServices.Common
{
    public class KenngroessenFruehererJahreDetailService : MandantDependentEntityServiceBase<KenngroessenFruehererJahreDetail, KenngroessenFruehererJahreDetailModel>
    {
        public KenngroessenFruehererJahreDetailService(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine, ISecurityService securityService) : base(transactionScopeProvider, entityServiceMappingEngine, securityService)
        {
        }

        protected override Expression<Func<KenngroessenFruehererJahreDetail, Mandant>> MandantExpression { get { return k => k.KenngroessenFruehererJahre.Mandant; } }
    }
}