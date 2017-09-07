using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ASTRA.EMSG.Common.ReflectionMapper
{
    public class RuleDrivenReflectingMapper<TSource, TDestination> : ReflectingMapper<TSource, TDestination>
    {
        public RuleDrivenReflectingMapper(params IMappingRule[] mappingRules) : this(true, mappingRules)
        { }

        public RuleDrivenReflectingMapper(bool ignoreNonMatchingProperties, params IMappingRule[] mappingRules)
            : base(ignoreNonMatchingProperties, Activator.CreateInstance<TDestination>)
        {
            ApplyRules(mappingRules);
        }

        public RuleDrivenReflectingMapper(bool ignoreNonMatchingProperties, Func<TDestination> createInstance, params IMappingRule[] mappingRules)
            : base(ignoreNonMatchingProperties, createInstance)
        {
            ApplyRules(mappingRules);
        }


        private void ApplyRules(params IMappingRule[] mappingRules)
        {
            var destinationProperties = typeof(TDestination).GetProperties();
            var sourceProperties = typeof(TSource).GetProperties();

            foreach (var destinationProperty in destinationProperties)
            {
                var sourceProperty = sourceProperties.Where(dp => dp.Name == destinationProperty.Name).SingleOrDefault();

                if (sourceProperty == null)
                    continue;

                //bool hasSetter = sourceProperty.GetSetMethod(false) != null;

                var appliableRules = mappingRules.Where(mappingRule => mappingRule.CanApply(sourceProperty, destinationProperty)); 
                foreach (var mappingRule in appliableRules)
                    mappingRule.Apply(sourceProperty, destinationProperty, this);
            }
        }

        public void SetValueFrom(PropertyInfo destinationProperty, Func<TSource, object> getTranslatedValue)
        {
            DestinationPropertyActions[destinationProperty] =
                (source, destination, engine) =>
                {
                    var translatedValue = getTranslatedValue(source);
                    destinationProperty.SetValue(destination, translatedValue, null);
                };
        }

        public void SetValueFrom(PropertyInfo destinationProperty, Func<TSource, TDestination, object> getTranslatedValue)
        {
            DestinationPropertyActions[destinationProperty] =
                (source, destination, engine) =>
                {
                    var translatedValue = getTranslatedValue(source, destination);
                    destinationProperty.SetValue(destination, translatedValue, null);
                };
        }

        public void SetValueFrom(Expression<Func<TDestination, object>> destinationPropertyExpression, Func<TSource, TDestination, object> getTranslatedValue)
        {
            var destinationProperty = ExtractPropertyFromExpression(destinationPropertyExpression);

            DestinationPropertyActions[destinationProperty] =
                (source, destination, engine) =>
                {
                    var translatedValue = getTranslatedValue(source, destination);
                    destinationProperty.SetValue(destination, translatedValue, null);
                };
        }

        public void Ignore(PropertyInfo destinationProperty)
        {
            DestinationPropertyActions[destinationProperty] = (source, destination, engine) => { };
        }
    }
}
