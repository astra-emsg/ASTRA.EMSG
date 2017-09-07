using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Master;
using System.Linq;
using TechTalk.SpecFlow;

namespace ASTRA.EMSG.IntegrationTests.Support.ObjectReader
{
    public class ObjectReaderConfiguration : IObjectReaderConfiguration
    {
        private readonly Dictionary<Type, Func<string, PropertyInfo, object>> propertyTypeBasedConverters = new Dictionary<Type, Func<string, PropertyInfo, object>>();
        private readonly Dictionary<string, Func<string, PropertyInfo, object>> propertyNameBasedConverters = new Dictionary<string, Func<string, PropertyInfo, object>>();
        private readonly Dictionary<string, Delegate> columnActions = new Dictionary<string, Delegate>();

        private readonly Dictionary<string, string> propertyAliasMapping = new Dictionary<string, string>();

        private Func<string, string> resolvePropertyName = s => s;

        public IObjectReaderConfiguration ConverterFor(Type type, Func<string, PropertyInfo, object> converter)
        {
            if(propertyTypeBasedConverters.ContainsKey(type))
                propertyTypeBasedConverters[type] = converter;
            else
                propertyTypeBasedConverters.Add(type, converter);

            return this;
        }

        public IObjectReaderConfiguration CustomSetterFor<TObject>(string columnHeader, Action<TableRow, TObject> customSetter)
        {
            columnActions[columnHeader] = customSetter;
            return this;
        }

        public Action<TableRow,TObject> GetColumnAction<TObject>(string header)
        {
            if (columnActions.ContainsKey(header))
                return (Action<TableRow,TObject>)columnActions[header];
            return null;
        }

        public IObjectReaderConfiguration ConverterFor<TObject, TProperty>(Expression<Func<TObject, TProperty>> propertyExpression, Func<string, PropertyInfo, object> converter)
        {
            var propertyName = ExpressionHelper.GetPropertyName(propertyExpression);
            string key = string.Format("{0}.{1}", typeof (TObject).Name, propertyName);

            if(propertyNameBasedConverters.ContainsKey(key))
                propertyNameBasedConverters[key] = converter;
            else
                propertyNameBasedConverters.Add(key, converter); 
            
            return this;
        }

        public Func<string, PropertyInfo, object> GetConverter<TObject>(PropertyInfo propertyInfo)
        {
            var propertyTypeKey = propertyInfo.PropertyType;
            var type = typeof (TObject);
            string propertyNameKey = GetPropertyNameKey(propertyInfo, type);

            if(propertyNameBasedConverters.ContainsKey(propertyNameKey))
                return propertyNameBasedConverters[propertyNameKey];

            //Recursive search
            while (!propertyTypeBasedConverters.ContainsKey(propertyTypeKey) && propertyTypeKey.BaseType != null)
                propertyTypeKey = propertyTypeKey.BaseType;

            if(propertyTypeBasedConverters.ContainsKey(propertyTypeKey))
                return propertyTypeBasedConverters[propertyTypeKey];

            return (s, pi) => s;
        }

        private static string GetPropertyNameKey(PropertyInfo propertyInfo, Type type)
        {
            return string.Format("{0}.{1}", type.Name, propertyInfo.Name);
        }

        public IObjectReaderConfiguration PropertyAliasFor<TObject, TProperty>(Expression<Func<TObject, TProperty>> propertyExpression, string propertyAlias)
        {
            var key = string.Format("{0}.{1}", typeof (TObject).Name, propertyAlias);
            var value = ExpressionHelper.GetPropertyName(propertyExpression);

            if(propertyAliasMapping.ContainsKey(key))
                propertyAliasMapping[key] = value;
            else
                propertyAliasMapping.Add(key, value);

            return this;
        }

        public string GetPropertyNameOfType<TObject>(string propertyAlias)
        {
            var key = string.Format("{0}.{1}", typeof (TObject).Name, propertyAlias);
            if(propertyAliasMapping.ContainsKey(key))
                return propertyAliasMapping[key];

            return resolvePropertyName(propertyAlias);
        }

        public string GetPropertyName(string propertyAlias)
        {
            var key = propertyAliasMapping.Where(pair => pair.Key.Split('.')[1] == propertyAlias).Select(pair => pair.Key).FirstOrDefault();

            if(!string.IsNullOrEmpty(key))
                return propertyAliasMapping[key];

            return resolvePropertyName(propertyAlias);
        }

        public ObjectReader GetObjectReader()
        {
            return new ObjectReader(this);
        }

        public IObjectReaderConfiguration SetGenericPropertyNameResolver(Func<string, string> propertyNameResolver)
        {
            resolvePropertyName = propertyNameResolver;
            return this;
        }
        
        public Func<string, string> GetGenericPropertyNameResolver()
        {
            return resolvePropertyName;
        }

        public IObjectReaderConfigurationForType<T> ConfigurationFor<T>()
        {
            return new ObjectReaderConfigurationForType<T>(this); 
        }
    }
}