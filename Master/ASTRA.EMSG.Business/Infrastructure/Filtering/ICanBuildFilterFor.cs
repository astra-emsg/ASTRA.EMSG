using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using NHibernate;

namespace ASTRA.EMSG.Business.Infrastructure.Filtering
{
    public interface ICanBuildFilterFor<TEntity>
        where TEntity : IEntity
    {
        IQueryable<TEntity> BuildFilter(IQueryable<TEntity> source);
        bool ShouldFilter { get; }
    }

    public interface ICanBuildQueryOver<TEntity> : ICanBuildFilterFor<TEntity> where TEntity : IEntity
    {
        IQueryOver<TEntity, TEntity> BuildFilter(IQueryOver<TEntity, TEntity> source);
    }
}