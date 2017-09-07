using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using NHibernate;

namespace ASTRA.EMSG.Business.Infrastructure.Filtering
{
    public interface IFilterer<TEntity>
        where TEntity : IEntity
    {
        IQueryable<TEntity> Filter(IQueryable<TEntity> source);
        IQueryOver<TEntity, TEntity> Filter(IQueryOver<TEntity, TEntity> source);
    }

    public class Filterer<TEntity> : IFilterer<TEntity>
        where TEntity : IEntity
    {
        private readonly IEnumerable<ICanBuildFilterFor<TEntity>> filterBuilders;

        public Filterer(IEnumerable<ICanBuildFilterFor<TEntity>> filterBuilders)
        {
            this.filterBuilders = filterBuilders;
        }

        public IQueryable<TEntity> Filter(IQueryable<TEntity> source)
        {
            IQueryable<TEntity> result = source;
            foreach (ICanBuildFilterFor<TEntity> builder in filterBuilders)
            {
                if (builder.ShouldFilter)
                    result = builder.BuildFilter(result);
            }
            return result;
        }

        public IQueryOver<TEntity, TEntity> Filter(IQueryOver<TEntity, TEntity> source)
        {
            IQueryOver<TEntity, TEntity> result = source;
            foreach (var builder in filterBuilders.OfType<ICanBuildQueryOver<TEntity>>())
            {
                if (builder.ShouldFilter)
                    result = builder.BuildFilter(result);
            }
            return result;
        }
    }
}