using System;
using System.Linq.Expressions;
using System.Reflection;
using TechTalk.SpecFlow;

namespace ASTRA.EMSG.IntegrationTests.Support.ObjectReader
{
    public class ObjectReaderConfigurationForType<T> : IObjectReaderConfigurationForType<T>
    {
        private readonly ObjectReaderConfiguration objectReaderConfiguration;

        public ObjectReaderConfigurationForType(ObjectReaderConfiguration objectReaderConfiguration)
        {
            this.objectReaderConfiguration = objectReaderConfiguration;
        }

        public IObjectReaderConfigurationForType<T> ConverterFor<TProperty>(Expression<Func<T, TProperty>> propertyExpression, Func<string, PropertyInfo, TProperty> converter)
        {
            objectReaderConfiguration.ConverterFor(propertyExpression, (s, pi) => converter(s, pi));
            return this;
        }

        public IObjectReaderConfigurationForType<T> PropertyAliasFor<TProperty>(Expression<Func<T, TProperty>> propertyExpression, string propertyAlias)
        {
            objectReaderConfiguration.PropertyAliasFor(propertyExpression, propertyAlias);
            return this;
        }

        public IObjectReaderConfiguration ConverterFor(Type type, Func<string, PropertyInfo, object> converter)
        {
            return objectReaderConfiguration.ConverterFor(type, converter);
        }

        public IObjectReaderConfiguration ConverterFor<TObject, TProperty>(Expression<Func<TObject, TProperty>> propertyExpression, Func<string, PropertyInfo, object> converter)
        {
            return objectReaderConfiguration.ConverterFor(propertyExpression, converter);
        }

        public Func<string, PropertyInfo, object> GetConverter<TObject>(PropertyInfo propertyInfo)
        {
            return objectReaderConfiguration.GetConverter<TObject>(propertyInfo);
        }

        public IObjectReaderConfiguration PropertyAliasFor<TObject, TProperty>(Expression<Func<TObject, TProperty>> propertyExpression, string propertyAlias)
        {
            return objectReaderConfiguration.PropertyAliasFor(propertyExpression, propertyAlias);
        }

        public string GetPropertyNameOfType<TObject>(string propertyAlias)
        {
            return objectReaderConfiguration.GetPropertyNameOfType<TObject>(propertyAlias);
        }

        public string GetPropertyName(string propertyAlias)
        {
            return objectReaderConfiguration.GetPropertyName(propertyAlias);
        }

        public ObjectReader GetObjectReader()
        {
            return objectReaderConfiguration.GetObjectReader();
        }

        public IObjectReaderConfiguration SetGenericPropertyNameResolver(Func<string, string> propertyNameResolver)
        {
            return objectReaderConfiguration.SetGenericPropertyNameResolver(propertyNameResolver);
        }

        public Func<string, string> GetGenericPropertyNameResolver()
        {
            return objectReaderConfiguration.GetGenericPropertyNameResolver();
        }

        public IObjectReaderConfigurationForType<TObject> ConfigurationFor<TObject>()
        {
            return objectReaderConfiguration.ConfigurationFor<TObject>();
        }

        public IObjectReaderConfiguration CustomSetterFor<TObject>(string columnHeader, Action<TableRow, TObject> customSetter)
        {
            return objectReaderConfiguration.CustomSetterFor(columnHeader, customSetter);
        }

        public Action<TableRow, TObject> GetColumnAction<TObject>(string header)
        {
            return objectReaderConfiguration.GetColumnAction<TObject>(header);
        }
    }
}