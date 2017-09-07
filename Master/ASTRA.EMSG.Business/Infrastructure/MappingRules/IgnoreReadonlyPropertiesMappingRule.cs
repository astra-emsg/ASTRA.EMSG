using System.Reflection;
using ASTRA.EMSG.Common.ReflectionMapper;

namespace ASTRA.EMSG.Business.Infrastructure.MappingRules
{
    public interface IIgnoreReadonlyPropertiesMappingRule : IMappingRule
    {
    }

    public class IgnoreReadonlyPropertiesMappingRule : IIgnoreReadonlyPropertiesMappingRule
    {
        public bool CanApply(PropertyInfo sourceProperty, PropertyInfo destinationProperty)
        {
            return !destinationProperty.CanWrite;
        }

        public void Apply<TSource, TDestination>(PropertyInfo sourceProperty, PropertyInfo destinationProperty, RuleDrivenReflectingMapper<TSource, TDestination> ruleDrivenReflectingMapper)
        {
            ruleDrivenReflectingMapper.Ignore(destinationProperty);
        }
    }
}