using System;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Utils;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Services.EntityServices.Common
{
    public abstract class ErfassungsPeriodDependentEntityServiceBase<TEntity, TModel> : EntityServiceBase<TEntity, TModel>
        where TEntity : class, IErfassungsPeriodDependentEntity, new()
        where TModel : IModel, IIdHolder, new()
    {
        protected readonly IHistorizationService historizationService;

        protected ErfassungsPeriodDependentEntityServiceBase(ITransactionScopeProvider transactionScopeProvider,
                                                                   IEntityServiceMappingEngine entityServiceMappingEngine, IHistorizationService historizationService)
            : base(transactionScopeProvider, entityServiceMappingEngine)
        {
            this.historizationService = historizationService;
        }

        protected override void OnEntityCreating(TEntity entity)
        {
            base.OnEntityCreating(entity);
            entity.ErfassungsPeriod = CurrentErfassungsPeriod;
            ErrorLoggingContextInfoService.SetErfassungsPeriodInfo(entity);
        }

        protected override void OnEntityUpdating(TEntity entity)
        {
            base.OnEntityUpdating(entity);
            ErrorLoggingContextInfoService.SetErfassungsPeriodInfo(entity);
        }

        protected override void OnEntityDeleting(TEntity entity)
        {
            base.OnEntityDeleting(entity);
            ErrorLoggingContextInfoService.SetErfassungsPeriodInfo(entity);
        }

        protected abstract Expression<Func<TEntity, ErfassungsPeriod>> ErfassungsPeriodExpression { get; }

        protected override IQueryable<TEntity> FilterEntities(IQueryable<TEntity> entities)
        {
            return base.FilterEntities(entities).Where(GetComparisonPredicate(ErfassungsPeriodExpression, () => historizationService.GetCurrentErfassungsperiod()));
        }

        public IQueryable<TEntity> GetEntitiesBy(ErfassungsPeriod erfassungsPeriod)
        {
            return base.FilterEntities(Queryable).Where(GetComparisonPredicate(ErfassungsPeriodExpression, () => erfassungsPeriod));
        }

        protected ErfassungsPeriod CurrentErfassungsPeriod { get { return historizationService.GetCurrentErfassungsperiod(); } }
    }
}