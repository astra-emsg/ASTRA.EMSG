using System.Reflection;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Common.ReflectionMapper;

namespace ASTRA.EMSG.Business.Infrastructure.MappingRules
{
    public interface IEntityToIdMappingRule : IMappingRule
    {
    }

    public class EntityToIdMappingRule : IEntityToIdMappingRule
    {
        public bool CanApply(PropertyInfo sourceProperty, PropertyInfo destinationProperty)
        {
            return typeof(IEntity).IsAssignableFrom(sourceProperty.PropertyType);
        }

        public void Apply<TSource, TDestination>(PropertyInfo sourceProperty, PropertyInfo destinationProperty, RuleDrivenReflectingMapper<TSource, TDestination> ruleDrivenReflectingMapper)
        {
            ruleDrivenReflectingMapper.SetValueFrom(destinationProperty, source => EntityToRefDtoTransformation(source, sourceProperty));
        }

        private object EntityToRefDtoTransformation<TSource>(TSource source, PropertyInfo sourceProperty)
        {
            var sourceEntityPropertyValue = (IEntity)sourceProperty.GetValue(source, new object[0]);
            if (sourceEntityPropertyValue == null)
                return null;

            return sourceEntityPropertyValue.Id;
        }
    }
}