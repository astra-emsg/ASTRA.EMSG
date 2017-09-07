using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ASTRA.EMSG.Common.ReflectionMapper;

namespace ASTRA.EMSG.Business.Infrastructure.MappingRules
{
    public interface IIgnoreIdMappingRule : IMappingRule
    { }
    public class IgnoreIdMappingRule : IIgnoreIdMappingRule
    {
        public bool CanApply(PropertyInfo sourceProperty, PropertyInfo destinationProperty)
        {
            return (destinationProperty.Name =="Id");
        }

        public void Apply<TSource, TDestination>(PropertyInfo sourceProperty, PropertyInfo destinationProperty, RuleDrivenReflectingMapper<TSource, TDestination> ruleDrivenReflectingMapper)
        {
            ruleDrivenReflectingMapper.Ignore(destinationProperty);
        }
    }
}
