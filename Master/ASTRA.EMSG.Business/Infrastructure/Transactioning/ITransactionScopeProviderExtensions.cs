using System;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using NHibernate;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Infrastructure.Transactioning
{
    public static class ITransactionScopeProviderExtensions
    {
        public static IQueryable<TEntity> Queryable<TEntity>(this ITransactionScopeProvider transactionScopeProvider) where TEntity : class, IEntity { return transactionScopeProvider.CurrentTransactionScope.Session.Query<TEntity>(); }
        public static IQueryOver<TEntity, TEntity> QueryOver<TEntity>(this ITransactionScopeProvider transactionScopeProvider) where TEntity : class, IEntity { return transactionScopeProvider.CurrentTransactionScope.Session.QueryOver<TEntity>(); }

        public static TEntity GetById<TEntity>(this ITransactionScopeProvider transactionScopeProvider, Guid id) where TEntity : class, IEntity { return transactionScopeProvider.CurrentTransactionScope.Session.Get<TEntity>(id); }
        public static void Create<TEntity>(this ITransactionScopeProvider transactionScopeProvider, TEntity entity) where TEntity : class, IEntity { transactionScopeProvider.CurrentTransactionScope.Session.Save(entity); }
        public static void Update<TEntity>(this ITransactionScopeProvider transactionScopeProvider, TEntity entity) where TEntity : class, IEntity { transactionScopeProvider.CurrentTransactionScope.Session.Update(entity); }
        public static void CreateOrUpdate<TEntity>(this ITransactionScopeProvider transactionScopeProvider, TEntity entity) where TEntity : class, IEntity { transactionScopeProvider.CurrentTransactionScope.Session.SaveOrUpdate(entity); }
        public static void Delete<TEntity>(this ITransactionScopeProvider transactionScopeProvider, Guid id) where TEntity : class, IEntity { transactionScopeProvider.CurrentTransactionScope.Session.Delete(GetById<TEntity>(transactionScopeProvider, id)); }
        public static void Delete<TEntity>(this ITransactionScopeProvider transactionScopeProvider, TEntity entity) where TEntity : class, IEntity { transactionScopeProvider.CurrentTransactionScope.Session.Delete(entity); }
    }
}