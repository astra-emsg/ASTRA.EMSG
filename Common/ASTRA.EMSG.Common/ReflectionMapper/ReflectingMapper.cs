using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ASTRA.EMSG.Common.ReflectionMapper.Exceptions;
using ASTRA.EMSG.Common.ReflectionMapper.Interfaces;

namespace ASTRA.EMSG.Common.ReflectionMapper
{
    public class ReflectingMapper<TSource, TDestination> : IMapper
    {
        public ReflectingMapper()
            : this(false, Activator.CreateInstance<TDestination>)
        {
        }

        public ReflectingMapper(bool ignoreNonMatchingProperties)
            : this(ignoreNonMatchingProperties, Activator.CreateInstance<TDestination>)
        {
        }

        public ReflectingMapper(bool ignoreNonMatchingProperties, Func<TDestination> createInstance)
        {
            if (createInstance == null)
                throw new ArgumentNullException("createInstance");

            CreateInstance = createInstance;
            IgnoreNonMatchingProperties = ignoreNonMatchingProperties;

            Initialize();
        }

        protected Func<TDestination> CreateInstance { get; private set; }

        private readonly Dictionary<PropertyInfo, Action<TSource, TDestination, IMappingEngine>> _destinationPropertyActions
            = new Dictionary<PropertyInfo, Action<TSource, TDestination, IMappingEngine>>();

        protected Dictionary<PropertyInfo, Action<TSource, TDestination, IMappingEngine>> DestinationPropertyActions
        {
            get { return _destinationPropertyActions; }
        }

        protected bool IgnoreNonMatchingProperties { get; private set; }

        object IMapper.Translate(object source, IMappingEngine engine)
        {
            return Translate((TSource)source, engine);
        }

        Type IMapper.SourceType
        {
            get { return typeof(TSource); }
        }

        Type IMapper.DestinationType
        {
            get { return typeof(TDestination); }
        }

        void IMapper.Translate(object source, object destination, IMappingEngine engine)
        {
            Translate((TSource)source, (TDestination)destination, engine);
        }

        private void Initialize()
        {
            var sourceProperties = typeof(TSource).GetProperties();

            foreach (var property in typeof(TDestination).GetProperties())
            {
                var destinationProperty = property;

                var destinationPropertySetter = destinationProperty.GetSetMethod();

                if (destinationPropertySetter == null || !destinationPropertySetter.IsPublic)
                    continue;

                var sourceProperty = sourceProperties.Where(
                    sp =>
                    sp.Name == destinationProperty.Name).SingleOrDefault();

                Action<TSource, TDestination, IMappingEngine> action;
                if (sourceProperty == null)
                {
                    action = GetActionForNonMatchingProperty(destinationProperty);
                }
                else
                {
                    var translate = GetTranslateFunction(sourceProperty.PropertyType, destinationProperty.PropertyType, null);
                    action = (source, destination, engine) =>
                             SetFromSource(source, sourceProperty, destination, destinationProperty,
                                           sourceValue => translate(sourceValue, engine));

                }

                DestinationPropertyActions.Add(destinationProperty, action);
            }
        }

        private Action<TSource, TDestination, IMappingEngine> GetActionForNonMatchingProperty(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            if (IgnoreNonMatchingProperties)
                return (source, destination, engine) => { };

            return
                (source, destination, engine) =>
                { throw new PropertyMappingNotConfiguredException(property); };
        }

        protected static Func<object, IMappingEngine, object> GetTranslateFunction(Type sourcePropertyType, Type destinationPropertyType, Func<object, IMappingEngine, object> defaultTranslate)
        {
            Func<object, IMappingEngine, object> translate;

            if (destinationPropertyType.IsAssignableFrom(sourcePropertyType))
            {
                translate = (sourceValue, engine) => sourceValue;
            }
            else if (ImplementsIEnumerableT(sourcePropertyType) && ImplementsIEnumerableT(destinationPropertyType))
            {
                translate = (sourceValue, engine) =>
                            engine.TranslateEnumerable((IEnumerable)sourceValue, destinationPropertyType);
            }
            else
            {
                translate = defaultTranslate ?? ((sourceValue, engine) =>
                                                 engine.Translate(sourcePropertyType, destinationPropertyType, sourceValue));
            }

            return translate;
        }

        private static bool ImplementsIEnumerableT(Type type)
        {
            if (!type.IsGenericType) return type.IsArray;
            if (type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) return true;

            return type.GetInterface(typeof(IEnumerable<>).Name) != null;
        }

        private static void SetFromSource(TSource source, PropertyInfo sourceProperty, TDestination destination, PropertyInfo destinationProperty, Func<object, object> translate)
        {
            var sourceValue = sourceProperty.GetValue(source, null);
            var destinationValue = translate(sourceValue);
            destinationProperty.SetValue(destination, destinationValue, null);
        }

        public TDestination Translate(TSource source, IMappingEngine engine)
        {
            var destination = CreateInstance();

            Translate(source, destination, engine);

            return destination;
        }

        public void Translate(TSource source, TDestination destination, IMappingEngine engine)
        {
            if (!typeof(TDestination).IsValueType && Equals(destination, default(TDestination)))
                throw new ArgumentNullException("destination");

            foreach (var destinationPropertyAction in DestinationPropertyActions)
            {
                try
                {
                    destinationPropertyAction.Value(source, destination, engine);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(
                        string.Format("Exception while translating to property {0}.{1} from type {2}.",
                                      destinationPropertyAction.Key.DeclaringType.FullName,
                                      destinationPropertyAction.Key.Name,
                                      typeof(TSource).FullName),
                        ex);
                }
            }
        }

        public void Ignore(Expression<Func<TDestination, object>> destinationPropertyExpression)
        {
            var destinationProperty = ExtractPropertyFromExpression(destinationPropertyExpression);

            DestinationPropertyActions[destinationProperty] = (source, destination, engine) => { };
        }

        protected static PropertyInfo ExtractPropertyFromExpression(Expression<Func<TDestination, object>> propertyExpression)
        {
            var expression = propertyExpression.Body;

            if (expression.NodeType == ExpressionType.Convert)
            {
                expression = ((UnaryExpression)expression).Operand;
            }

            var memberExpression = expression as MemberExpression;

            if (memberExpression == null)
                throw new ArgumentException("Could not extract property from expression.");

            var propertyInfo = memberExpression.Member as PropertyInfo;

            if (propertyInfo == null)
                throw new ArgumentException("Could not extract property from expression.");

            return typeof(TDestination).GetProperty(propertyInfo.Name);
        }

        public void SetValueFrom(Expression<Func<TDestination, object>> destinationPropertyExpression, Func<TSource, object> getValue)
        {
            var destinationProperty = ExtractPropertyFromExpression(destinationPropertyExpression);

            DestinationPropertyActions[destinationProperty] =
                (source, destination, engine) =>
                {
                    var sourceValue = getValue(source);

                    Func<object, IMappingEngine, object> translate;
                    if (sourceValue == null)
                    {
                        translate = (s, e) => s;
                    }
                    else
                    {
                        translate = GetTranslateFunction(sourceValue.GetType(), destinationProperty.PropertyType,
                                                         (o, translationEngine) =>
                                                         translationEngine.Translate(
                                                             destinationProperty.PropertyType, o));
                    }

                    destinationProperty.SetValue(destination, translate(sourceValue, engine), null);
                };
        }
    }
}