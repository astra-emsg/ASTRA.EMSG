using System;
using System.Reflection;
using ASTRA.EMSG.Common.ReflectionMapper;

namespace ASTRA.EMSG.Business.Infrastructure.MappingRules
{
    public interface INullableToNonNullableMappingRule : IMappingRule
    {

    }

    public class NullableToNonNullableMappingRule : INullableToNonNullableMappingRule
    {
        public bool CanApply(PropertyInfo sourceProperty, PropertyInfo destinationProperty)
        {
            var isSourceNullable = sourceProperty.PropertyType.IsGenericType &&
                                   sourceProperty.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>);

            var isTargetNullable = destinationProperty.PropertyType.IsGenericType &&
                                   destinationProperty.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>);

            return isSourceNullable && !isTargetNullable;
        }

        public void Apply<TSource, TDestination>(PropertyInfo sourceProperty, PropertyInfo destinationProperty, RuleDrivenReflectingMapper<TSource, TDestination> ruleDrivenReflectingMapper)
        {
            
            ruleDrivenReflectingMapper.SetValueFrom(destinationProperty, source => sourceProperty.GetValue(source, null) ?? Activator.CreateInstance(destinationProperty.PropertyType)  );
        }
    }
}