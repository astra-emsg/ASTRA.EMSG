using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace ASTRA.EMSG.Business.Infrastructure.Transactioning
{
    public static class NHibernateConfigurationExtensions
    {
        private static readonly PropertyInfo TableMappingsProperty =
            typeof(Configuration).GetProperty("TableMappings", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        public static void CreateIndexesForForeignKeys(this Configuration configuration)
        {
            configuration.BuildMappings();
            var tables = (ICollection<Table>)TableMappingsProperty.GetValue(configuration, null);
            foreach (var table in tables)
            {
                foreach (var foreignKey in table.ForeignKeyIterator)
                {
                    var idx = new Index();
                    idx.AddColumns(foreignKey.ColumnIterator);
                    idx.Name = "IDX_" + foreignKey.Name;
                    idx.Table = table;
                    table.AddIndex(idx);
                }
            }
        }
    }
}
