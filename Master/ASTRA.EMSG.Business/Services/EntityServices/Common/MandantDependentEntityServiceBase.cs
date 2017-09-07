using System;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.BusinessExceptions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Business.Utils;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Exceptions;
using NHibernate;
using NHibernate.Criterion;

namespace ASTRA.EMSG.Business.Services.EntityServices.Common
{
    public abstract class MandantDependentEntityServiceBase<TEntity, TModel> : EntityServiceBase<TEntity, TModel>
        where TEntity : class, IMandantDependentEntity, new()
        where TModel : IModel, IIdHolder, new()
    {
        protected readonly ISecurityService securityService;

        protected MandantDependentEntityServiceBase(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine,
                                                          ISecurityService securityService)
            : base(transactionScopeProvider, entityServiceMappingEngine)
        {
            this.securityService = securityService;
        }

        protected override TEntity GetEntityById(Guid id)
        {
            var entity = base.GetEntityById<TEntity>(id);

            if (entity == null)
                return null;

            if (entity.Mandant != CurrentMandant)
                throw new EmsgException(EmsgExceptionType.UnauthorizedMandantAccess);
            return entity;
        }

        protected Mandant CurrentMandant { get { return securityService.GetCurrentMandant(); } }

        protected override void OnEntityCreating(TEntity entity)
        {
            base.OnEntityCreating(entity);
            entity.Mandant = CurrentMandant;
            ErrorLoggingContextInfoService.SetMandantInfo(entity);
        }

        protected override void OnEntityUpdating(TEntity entity)
        {
            base.OnEntityUpdating(entity);
            ErrorLoggingContextInfoService.SetMandantInfo(entity);
        }

        protected override void OnEntityDeleting(TEntity entity)
        {
            base.OnEntityDeleting(entity);
            ErrorLoggingContextInfoService.SetMandantInfo(entity);
        }

        protected override IQueryable<TEntity> FilterEntities(IQueryable<TEntity> entities)
        {
            return base.FilterEntities(entities).Where(GetComparisonPredicate(MandantExpression, () => CurrentMandant));
        }

        protected override ICriteria FilterEntities(ICriteria entities)
        {
            return base.FilterEntities(entities).Add(Restrictions.Eq("Mandant", CurrentMandant));
        }

        public IQueryable<TEntity> GetEntitiesBy(Mandant mandant)
        {
            return base.FilterEntities(Queryable).Where(GetComparisonPredicate(MandantExpression, () => mandant));
        }

        protected abstract Expression<Func<TEntity, Mandant>> MandantExpression { get; }
    }
}