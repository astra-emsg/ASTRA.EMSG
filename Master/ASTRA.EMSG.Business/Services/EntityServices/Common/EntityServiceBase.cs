using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Utils;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Master.Logging;
using GeoAPI.Geometries;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Spatial.Criterion;

namespace ASTRA.EMSG.Business.Services.EntityServices.Common
{
    public abstract class EntityServiceBase<TEntity, TModel>
        where TEntity : class, IEntity, new()
        where TModel : IModel, IIdHolder, new()
    {
        private readonly ITransactionScopeProvider transactionScopeProvider;
        protected readonly IEntityServiceMappingEngine entityServiceMappingEngine;

        protected EntityServiceBase(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine)
        {
            this.transactionScopeProvider = transactionScopeProvider;
            this.entityServiceMappingEngine = entityServiceMappingEngine;
        }

        protected ISession CurrentSession { get { return transactionScopeProvider.CurrentTransactionScope.Session; } }

        protected IQueryable<T> Query<T>() { return CurrentSession.Query<T>(); }

        protected IQueryable<TEntity> Queryable { get { return CurrentSession.Query<TEntity>(); } }
        protected IQueryOver<TEntity> QueryOver { get { return CurrentSession.QueryOver<TEntity>(); } }

        protected virtual TEntity GetEntityById(Guid id) { return CurrentSession.Get<TEntity>(id); }
        protected virtual T GetEntityById<T>(Guid id) { return CurrentSession.Get<T>(id); }

        public TModel GetById(Guid id) { return CreateModel(GetEntityById(id)); }

        protected void Create(TEntity entity) { CurrentSession.Save(entity); }
        protected void Create<T>(T entity) { CurrentSession.Save(entity); }

        protected virtual TEntity CreateEntity(TEntity entity)
        {
            OnEntityCreating(entity);
            Create(entity);
            return entity;
        }
        
        public TModel CreateEntity(TModel model)
        {
            var entity = new TEntity();
            model.Id = entity.Id;
            OnEntityCreating(model, entity);
            UpdateEntityFieldsInternal(model, entity);
            CreateEntity(entity);

            return CreateModel(entity);
        }

        protected void Update(TEntity entity) { CurrentSession.Update(entity); }
        protected void Update<T>(T entity) { CurrentSession.Update(entity); }

        protected void UpdateEntity(TEntity entity)
        {
            OnEntityUpdating(entity);
            Update(entity);
        }
        
        public virtual TModel UpdateEntity(TModel model)
        {
            var entity = GetEntityById(model.Id);
            OnEntityUpdating(model, entity);
            UpdateEntityFieldsInternal(model, entity);
            UpdateEntity(entity);
            return CreateModel(entity);
        }

        protected virtual void UpdateEntityFieldsInternal(TModel model, TEntity entity)
        {
            entityServiceMappingEngine.Translate(model, entity);
        }

        protected void CreateOrUpdateEntity(TEntity entity) { CurrentSession.SaveOrUpdate(entity); }

        public virtual void CreateOrUpdate(TModel model)
        {
            var entity = GetEntityById(model.Id);

            if (entity == null)
            {
                CreateEntity(model);
            }
            else
            {
                UpdateEntityFieldsInternal(model, entity);
                UpdateEntity(entity);
            }
        }

        protected void Delete(TEntity entity) { CurrentSession.Delete(entity); }
        protected void Delete<T>(T entity) { CurrentSession.Delete(entity); }

        public void DeleteEntity(Guid id)
        {
            TEntity entityById = GetEntityById(id);
            OnEntityDeleting(entityById);
            Delete(entityById);
        }

        protected IQueryable<TEntity> FilteredEntities { get { return FilterEntities(Queryable); } }
        public virtual List<TModel> GetCurrentModels() { return FilteredEntities.Select(CreateModel).ToList(); }
        public virtual IQueryable<TEntity> GetCurrentEntities() { return FilteredEntities; }

        protected virtual TModel CreateModel(TEntity entity)
        {
            var model = new TModel();
            entityServiceMappingEngine.Translate(entity, model);
            OnModelCreated(entity, model);
            return model;
        }

        protected virtual void OnModelCreated(TEntity entity, TModel model) { }
        protected virtual void OnEntityCreating(TEntity entity) { ErrorLoggingContextInfoService.SetEntityInfo(entity); }
        protected virtual void OnEntityCreating(TModel model, TEntity entity) { }
        protected virtual void OnEntityUpdating(TEntity entity) { ErrorLoggingContextInfoService.SetEntityInfo(entity); }
        protected virtual void OnEntityUpdating(TModel model, TEntity entity) { }
        protected virtual void OnEntityDeleting(TEntity entity) { ErrorLoggingContextInfoService.SetEntityInfo(entity); }

        protected virtual IQueryable<TEntity> FilterEntities(IQueryable<TEntity> entities) { return entities; }
        protected virtual ICriteria FilterEntities(ICriteria entities) { return entities; }

        protected Expression<Func<TParameter, bool>> GetComparisonPredicate<TParameter, TCompare>(Expression<Func<TParameter, TCompare>> parametrizedExpression, Expression<Func<TCompare>> getCurrentExpression)
        {
            var predicate =
                Expression.Lambda<Func<TParameter, bool>>(
                    Expression.Equal(parametrizedExpression.Body, getCurrentExpression.Body),
                    new[] { parametrizedExpression.Parameters[0] });

            return predicate;
        }

        public IList<TEntity> GetEntityListBySpatialFilter(IGeometry filterGeom)
        {
            return CreateCriteriaWithSpatial(filterGeom).List<TEntity>().Where(e => ((IShapeHolder)e).Shape.Intersects(filterGeom)).ToList();
        }

        private ICriteria CreateCriteriaWithSpatial(IGeometry filterGeom)
        {
            ICriteria cr = CurrentSession.CreateCriteria<TEntity>();

            cr.Add(SpatialRestrictions.Filter(ExpressionHelper.GetPropertyName<IAbschnittGISBase, IGeometry>(e => e.Shape), filterGeom));
            return cr;
        }

        public IList<TEntity> GetCurrentEntityListBySpatialFilter(IGeometry filterGeom)
        {
            return FilterEntities(CreateCriteriaWithSpatial(filterGeom)).List<TEntity>().Where(e => ((IShapeHolder)e).Shape.Intersects(filterGeom)).ToList();
        }

        protected TViewModel CreateModelFromEntity<TViewModel, TEntityType>(TEntityType entity) where TViewModel : new()
        {
            var viewModel = new TViewModel();
            entityServiceMappingEngine.Translate(entity, viewModel);
            return viewModel;
        }

        protected TViewModel CreateModelFromEntity<TViewModel>(TEntity entity) where TViewModel : new()
        {
            var viewModel = new TViewModel();
            entityServiceMappingEngine.Translate(entity, viewModel);
            return viewModel;
        }
    }
}
