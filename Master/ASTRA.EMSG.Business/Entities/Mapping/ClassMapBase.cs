using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Common;
using FluentNHibernate.Mapping;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public abstract class ClassMapBase<TEntity> : ClassMap<TEntity> where TEntity : IEntity
    {
        private const string SuffixTableName = "MSG";

        protected ClassMapBase()
        {
            Id(b => b.Id).GeneratedBy.Assigned().Column(string.Format("{0}_ID", GetShortNameFromType(typeof(TEntity)).ToUpper()));
            MapTo(s => s.CreatedAt);
            MapTo(s => s.CreatedBy);
            MapTo(s => s.UpdatedAt);
            MapTo(s => s.UpdatedBy);
            Table(string.Format("{0}_{1}_{2}", PrefixTableName, TableName, SuffixTableName).ToUpper());
        }

        protected abstract string TableName { get; }
        protected abstract string PrefixTableName { get; }
        
        protected PropertyPart MapTo(Expression<Func<TEntity, object>> propertyExpression, string columnName = null, string klass = null, string prefix = null, Type entityTypeOverride = null)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                var propertyName = string.Format("{0}{1}", prefix, ExpressionHelper.GetPropertyName(propertyExpression));
                if (propertyName.Length > 11)
                {
                    propertyName = propertyName.Substring(0, 11);
                }
                columnName = propertyName;
            }

            if (string.IsNullOrWhiteSpace(klass))
            {
                var propertyType = ExpressionHelper.GetPropertyType(propertyExpression);
                if (propertyType == typeof(DateTime))
                    klass = SpalteninhaltsKlasse.Date;
                else if (propertyType == typeof(decimal))
                    klass = SpalteninhaltsKlasse.Number;
                else
                    klass = SpalteninhaltsKlasse.Value;
                //TODO: implement futher convetions
            }

            var tableShortName = GetShortNameFromType(entityTypeOverride ?? typeof(TEntity));
            return Map(propertyExpression, string.Format("{0}_{1}_{2}", tableShortName, columnName, klass).ToUpper());
        }

        protected PropertyPart MapToLongText(Expression<Func<TEntity, object>> propertyExpression, string columnName = null, string klass = null, string prefix = null, Type entityTypeOverride = null)
        {
            //Note: 5000 means LongText (In Oracle 'NCLOB')
            return MapTo(propertyExpression, columnName, klass, prefix, entityTypeOverride).Length(5000);
        }

        protected ManyToOnePart<TProperty> ReferencesTo<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, string relation = null, Type entityTypeOverride = null)
        {
            var tableShortName = GetShortNameFromType(entityTypeOverride ?? typeof(TEntity));
            relation = relation ?? "NOR";
            var childShortName = GetShortNameFromType(ExpressionHelper.GetPropertyType(propertyExpression));
            var columnName = string.Format("{0}_{1}_{2}_{3}_ID", tableShortName, tableShortName, childShortName, relation).ToUpper();
            var foreignKeyConstraintName = string.Format("CFK_{0}_{1}_{2}", childShortName, tableShortName, relation).ToUpper();
            return References(propertyExpression, columnName).ForeignKey(foreignKeyConstraintName);
        }

        public static string GetShortNameFromType(Type type)
        {
            var tableShortNameAttribute = TypeDescriptor.GetAttributes(type).OfType<TableShortNameAttribute>().FirstOrDefault();
            if (tableShortNameAttribute != null)
                return tableShortNameAttribute.TableShortName;
            throw new InvalidOperationException(string.Format("Missing TableShortNameAttribute from type: {0}", type.Name));
        }
    }

    public static class MemberExtensions
    {
        
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TableShortNameAttribute : Attribute
    {
        public string TableShortName { get; private set; }

        public TableShortNameAttribute(string tableShortName)
        {
            TableShortName = tableShortName;
        }
    }

    public static class SpalteninhaltsKlasse
    {
        public static string Identifier = "ID";
        public static string Date = "DT";
        public static string Code = "CD";
        public static string Name = "NM";
        public static string Number = "NR";
        public static string Percentage = "PC";
        public static string Text = "TX";
        public static string Time = "TM";
        public static string Timestamp = "TS";
        public static string Value = "VL";
    }

    public static class PrefixTableNameKlasse
    {
        public static string InventarobjektNutzdatentabelle = "ADD";
        public static string EigenschaftenTextkatalog = "VAT";
        public static string AdministrationParameter = "ADP";
        public static string InventarobjektBeziehungstabelle = "ADR";
        public static string RaumbezugNutzdatentabelle = "SRD";
    }
}