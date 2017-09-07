using System;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.BusinessExceptions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Exceptions;
using NHibernate;
using NHibernate.Criterion;

namespace ASTRA.EMSG.Business.Services.EntityServices.Common
{
    public abstract class MandantAndErfassungsPeriodDependentEntityServiceBase<TEntity, TModel> : MandantDependentEntityServiceBase<TEntity, TModel>
        where TEntity : class, IMandantDependentEntity, IErfassungsPeriodDependentEntity, new()
        where TModel : IModel, IIdHolder, new()
    {
        protected readonly IHistorizationService historizationService;

        protected MandantAndErfassungsPeriodDependentEntityServiceBase(ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine, ISecurityService securityService, IHistorizationService historizationService)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService)
        {
            this.historizationService = historizationService;
        }

        protected override TEntity GetEntityById(Guid id)
        {
            var entity = base.GetEntityById<TEntity>(id);

            if (entity == null)
                return null;

            if (entity.ErfassungsPeriod != CurrentErfassungsPeriod)
                throw new EmsgException(EmsgExceptionType.UnauthorizedErfassungperiodAccess);
            return entity;
        }

        protected override void OnEntityCreating(TEntity entity)
        {
            base.OnEntityCreating(entity);
            entity.ErfassungsPeriod = CurrentErfassungsPeriod;
        }

        protected abstract Expression<Func<TEntity, ErfassungsPeriod>> ErfassungsPeriodExpression { get; }

        protected override IQueryable<TEntity> FilterEntities(IQueryable<TEntity> entities)
        {
            return base.FilterEntities(entities).Where(GetComparisonPredicate(ErfassungsPeriodExpression, () => historizationService.GetCurrentErfassungsperiod()));
        }

        protected override ICriteria FilterEntities(ICriteria entities)
        {
            return base.FilterEntities(entities).Add(Restrictions.Eq("ErfassungsPeriod", historizationService.GetCurrentErfassungsperiod()));
        }

        public IQueryable<TEntity> GetEntitiesBy(ErfassungsPeriod erfassungsPeriod)
        {
            return base.FilterEntities(Queryable).Where(GetComparisonPredicate(ErfassungsPeriodExpression, () => erfassungsPeriod));
        }

        public IQueryable<TEntity> GetEntitiesBy(ErfassungsPeriod erfassungsPeriod, Mandant mandant)
        {
            return base.GetEntitiesBy(mandant).Where(GetComparisonPredicate(ErfassungsPeriodExpression, () => erfassungsPeriod));
        }

        protected ErfassungsPeriod CurrentErfassungsPeriod { get { return historizationService.GetCurrentErfassungsperiod(); } }
    }
}