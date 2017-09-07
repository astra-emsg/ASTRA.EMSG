using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using ASTRA.EMSG.Common.ReflectionMapper.Exceptions;
using ASTRA.EMSG.Common.ReflectionMapper.Interfaces;

namespace ASTRA.EMSG.Common.ReflectionMapper
{
    public class MappingEngine : IMappingEngine
    {
        private readonly IMappingConfiguration configuration;

        public MappingEngine(IMappingConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public TDestination Translate<TSource, TDestination>(TSource source)
        {
            var translator = GetTranslator(typeof(TSource), typeof(TDestination), source.GetType());
            return (TDestination)translator.Translate(source, this);
        }

        public IEnumerable<TDestination> TranslateEnumerable<TSource, TDestination>(IEnumerable<TSource> sourceItems)
        {
            return (IEnumerable<TDestination>)TranslateEnumerable(sourceItems, typeof(IEnumerable<TDestination>));
        }

        public List<TDestination> TranslateList<TSource, TDestination>(List<TSource> sourceItems)
        {
            return (List<TDestination>)TranslateEnumerable(sourceItems, typeof(List<TDestination>))
            ;
        }

        public object Translate(Type sourceType, Type destinationType, object source)
        {
            return TranslateSource(sourceType, destinationType, source);
        }

        private object TranslateSource(Type sourceType, Type destinationType, object source)
        {
            if (source == null)
                return null;

            IMapper mapper = GetTranslator(sourceType, destinationType, source.GetType());

            return mapper.Translate(source, this);
        }

        private IMapper GetTranslator(Type sourceType, Type destinationType, Type sourceDynamicType)
        {
            Type currentSourceType = sourceDynamicType;
            IMapper mapper;

            do
            {
                mapper = configuration.GetTranslator(currentSourceType, destinationType);

                currentSourceType = currentSourceType.BaseType;

            } while (mapper == null && sourceType.IsAssignableFrom(currentSourceType));

            if (mapper == null)
            {
                throw new MapperNotFoundException(sourceType, destinationType);
            }
            return mapper;
        }

        public IEnumerable TranslateEnumerable(IEnumerable sourceItems, Type destinationCollectionType)
        {
            if (sourceItems == null)
                return null;

            var sourceItemType = GetCollectionItemType(sourceItems.GetType());
            var destinationItemType = GetCollectionItemType(destinationCollectionType);

            var destinationListType = typeof(List<>).MakeGenericType(destinationItemType);
            var destinationList = (IList)Activator.CreateInstance(destinationListType);

            foreach (var sourceItem in sourceItems)
            {
                destinationList.Add(TranslateSource(sourceItemType, destinationItemType, sourceItem));
            }

            if (destinationCollectionType.IsArray)
            {
                return (IEnumerable)destinationListType.GetMethod("ToArray").Invoke(destinationList, null);
            }

            return destinationList;
        }

        private static Type GetCollectionItemType(Type collectionType)
        {
            Type sourceIEnumerable;
            if (collectionType.IsGenericType && collectionType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                sourceIEnumerable = collectionType;
            }
            else
            {
                sourceIEnumerable = collectionType.GetInterface(typeof(IEnumerable<>).Name);
            }

            return sourceIEnumerable.GetGenericArguments().First();
        }

        public TDestination Translate<TSource, TDestination>(TSource source, TDestination destination)
        {
            var translator = GetTranslator(typeof(TSource), typeof(TDestination), source.GetType());
            translator.Translate(source, destination, this);

            return destination;
        }

        public object Translate(Type destinationType, object source)
        {
            var translator = GetTranslator(source.GetType(), destinationType);
            return translator.Translate(source, this);
        }

        private IMapper GetTranslator(Type sourceType, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");

            if (sourceType == null)
                return null;

            var translator = configuration.GetTranslator(sourceType, destinationType);

            if (translator != null)
                return translator;

            return GetTranslator(sourceType.BaseType, destinationType);
        }
    }
}