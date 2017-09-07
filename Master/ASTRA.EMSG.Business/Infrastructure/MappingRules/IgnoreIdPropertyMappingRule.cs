using System;
using System.Reflection;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.ReflectionMapper;

namespace ASTRA.EMSG.Business.Infrastructure.MappingRules
{
    public interface IIgnoreIdPropertyMappingRule : IMappingRule
    {
    }

    public class IgnoreIdPropertyMappingRule : IIgnoreIdPropertyMappingRule
    {
        public bool CanApply(PropertyInfo sourceProperty, PropertyInfo destinationProperty)
        {
            return sourceProperty.Name == ExpressionHelper.GetPropertyName<IEntity, Guid>(e => e.Id);
        }

        public void Apply<TSource, TDestination>(PropertyInfo sourceProperty, PropertyInfo destinationProperty, RuleDrivenReflectingMapper<TSource, TDestination> ruleDrivenReflectingMapper)
        {
            ruleDrivenReflectingMapper.Ignore(destinationProperty);
        }
    }
}