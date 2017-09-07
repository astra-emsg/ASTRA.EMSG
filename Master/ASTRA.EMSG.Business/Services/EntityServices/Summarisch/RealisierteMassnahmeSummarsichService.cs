using System;
using System.Collections.Generic;
using System.Linq;
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
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Services.EntityServices.Summarisch
{
    public interface IRealisierteMassnahmeSummarsichService : IService
    {
        void DeleteEntity(Guid id);
        RealisierteMassnahmeSummarsichModel GetById(Guid id);
        RealisierteMassnahmeSummarsichModel CreateEntity(RealisierteMassnahmeSummarsichModel realisierteMassnahmeSummarsichModel);
        RealisierteMassnahmeSummarsichModel UpdateEntity(RealisierteMassnahmeSummarsichModel realisierteMassnahmeSummarsichModel);
        IQueryable<RealisierteMassnahmeSummarsich> GetEntitiesBy(ErfassungsPeriod getErfassungsPeriod, Mandant mandant);
        IQueryable<RealisierteMassnahmeSummarsich> GetEntitiesBy(ErfassungsPeriod getErfassungsPeriod);
    }

    public class RealisierteMassnahmeSummarsichService : MandantAndErfassungsPeriodDependentEntityServiceBase<RealisierteMassnahmeSummarsich, RealisierteMassnahmeSummarsichModel>, IRealisierteMassnahmeSummarsichService
    {
        private readonly ILocalizationService localizationService;

        public RealisierteMassnahmeSummarsichService(
            ITransactionScopeProvider transactionScopeProvider,
            IEntityServiceMappingEngine entityServiceMappingEngine,
            IHistorizationService historizationService,
            ILocalizationService localizationService,
            ISecurityService securityService)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.localizationService = localizationService;
        }

        public override List<RealisierteMassnahmeSummarsichModel> GetCurrentModels()
        {
            return FilteredEntities.Fetch(rms => rms.Belastungskategorie).Select(CreateModel).ToList();
        }

        protected override void OnModelCreated(RealisierteMassnahmeSummarsich entity, RealisierteMassnahmeSummarsichModel model)
        {
            model.BelastungskategorieBezeichnung = entity.Belastungskategorie == null ? null : localizationService.GetLocalizedBelastungskategorieTyp(entity.BelastungskategorieTyp);
        }

        protected override Expression<Func<RealisierteMassnahmeSummarsich, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return rms => rms.ErfassungsPeriod; } }
        protected override Expression<Func<RealisierteMassnahmeSummarsich, Mandant>> MandantExpression { get { return rms => rms.Mandant; } }
    }
}
