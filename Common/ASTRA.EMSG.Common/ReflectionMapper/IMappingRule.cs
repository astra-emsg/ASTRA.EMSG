using System.Reflection;

namespace ASTRA.EMSG.Common.ReflectionMapper
{
    public interface IMappingRule
    {
        bool CanApply(PropertyInfo sourceProperty, PropertyInfo destinationProperty);
        void Apply<TSource, TDestination>(PropertyInfo sourceProperty, PropertyInfo destinationProperty, RuleDrivenReflectingMapper<TSource, TDestination> ruleDrivenReflectingMapper);
    }
}