using System;
using System.Linq.Expressions;
using System.Reflection;
using TechTalk.SpecFlow;

namespace ASTRA.EMSG.IntegrationTests.Support.ObjectReader
{
    public interface IObjectReaderConfigurationForType<T> : IObjectReaderConfiguration
    {
        IObjectReaderConfigurationForType<T> ConverterFor<TProperty>(Expression<Func<T, TProperty>> propertyExpression, Func<string, PropertyInfo, TProperty> converter);
        IObjectReaderConfigurationForType<T> PropertyAliasFor<TProperty>(Expression<Func<T, TProperty>> propertyExpression, string propertyAlias);
    }
}