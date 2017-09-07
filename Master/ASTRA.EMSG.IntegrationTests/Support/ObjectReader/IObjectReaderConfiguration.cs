using System;
using System.Linq.Expressions;
using System.Reflection;
using TechTalk.SpecFlow;

namespace ASTRA.EMSG.IntegrationTests.Support.ObjectReader
{
    public interface IObjectReaderConfiguration
    {
        IObjectReaderConfiguration ConverterFor(Type type, Func<string, PropertyInfo, object> converter);
        IObjectReaderConfiguration ConverterFor<TObject, TProperty>(Expression<Func<TObject, TProperty>> propertyExpression, Func<string, PropertyInfo, object> converter);
        Func<string, PropertyInfo, object> GetConverter<TObject>(PropertyInfo propertyInfo);

        IObjectReaderConfiguration PropertyAliasFor<TObject, TProperty>(Expression<Func<TObject, TProperty>> propertyExpression, string propertyAlias);
        string GetPropertyNameOfType<TObject>(string propertyAlias);
        ObjectReader GetObjectReader();
        IObjectReaderConfiguration SetGenericPropertyNameResolver(Func<string, string> propertyNameResolver);
        Func<string, string> GetGenericPropertyNameResolver();
        string GetPropertyName(string propertyAlias);
        IObjectReaderConfigurationForType<T> ConfigurationFor<T>();
        IObjectReaderConfiguration CustomSetterFor<TObject>(string columnHeader, Action<TableRow, TObject> customSetter);
        Action<TableRow, TObject> GetColumnAction<TObject>(string header);
    }
}