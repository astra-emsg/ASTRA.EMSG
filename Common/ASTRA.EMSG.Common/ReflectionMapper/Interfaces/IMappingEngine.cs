using System;
using System.Collections;
using System.Collections.Generic;

namespace ASTRA.EMSG.Common.ReflectionMapper.Interfaces
{
    public interface IMappingEngine
    {
        TDestination Translate<TSource, TDestination>(TSource source);
        IEnumerable<TDestination> TranslateEnumerable<TSource, TDestination>(IEnumerable<TSource> source);
        List<TDestination> TranslateList<TSource, TDestination>(List<TSource> source);
        object Translate(Type sourceType, Type destinationType, object source);
        IEnumerable TranslateEnumerable(IEnumerable sourceItems, Type destinationCollectionType);
        TDestination Translate<TSource, TDestination>(TSource source, TDestination destination);
        object Translate(Type destinationType, object source);
    }
}