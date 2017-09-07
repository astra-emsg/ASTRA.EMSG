using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using Autofac.Features.Indexed;

namespace ASTRA.EMSG.Business.Infrastructure.Filtering
{
    public interface IFiltererFactory
    {
        IFilterer<TEntity> CreateFilterer<TEntity>(IFilterParameter filterParameter) where TEntity : IEntity;
    } 

    public class FiltererFactory : IFiltererFactory
    {
        private readonly IIndex<Type, IFilterBuilder> index;

        public FiltererFactory(IIndex<Type, IFilterBuilder> index)
        {
            this.index = index;
        }

        public IFilterer<TEntity> CreateFilterer<TEntity>(IFilterParameter filterParameter)
            where TEntity : IEntity
        {
            var filters = new List<ICanBuildFilterFor<TEntity>>();
            foreach (var parameterFilter in filterParameter.GetType().GetInterfaces())
            {
                if (parameterFilter != typeof(IFilterParameter) && typeof(IFilterParameter).IsAssignableFrom(parameterFilter))
                {
                    var filter = index[parameterFilter];
                    filter.Initialize(filterParameter);
                    if (filter is ICanBuildFilterFor<TEntity>)
                        filters.Add(filter as ICanBuildFilterFor<TEntity>);
                }
            }
            return new Filterer<TEntity>(filters);
        }
    }
}