using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using Iesi.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Mapping;
using NUnit.Framework;
using System.Linq;

namespace ASTRA.EMSG.Tests.MappingConfigurationTests
{
    [TestFixture]
    public class MappingConfigurationTests
    {
        [Test]
        public void AllTableNameShouldBeUnique()
        {
            Configuration configuration = new MsSQLNHibernateConfigurationProvider(new ServerConfigurationProvider()).Configuration;

            var tableNameSet = new HashSet<string>();
            
            foreach (PersistentClass persistentClass in configuration.ClassMappings)
            {
                string tableName = persistentClass.Table.Name;
                Assert.IsFalse(tableNameSet.Contains(tableName), string.Format("Duplicated Table Name: {0}", tableName));

                tableNameSet.Add(tableName);
            }
        }

        [Test]
        public void AllColumnNameInAllTableShouldBeUnique()
        {
            Configuration configuration = new MsSQLNHibernateConfigurationProvider(new ServerConfigurationProvider()).Configuration;
            
            foreach (PersistentClass persistentClass in configuration.ClassMappings)
            {
                string tableName = persistentClass.Table.Name;

                var columnNameSet = new HashSet<string>();

                foreach (Property property in persistentClass.PropertyIterator)
                {
                    foreach (var columnName in property.ColumnIterator.Select(c => c.Text))
                    {
                        Assert.IsFalse(columnNameSet.Contains(columnName), string.Format("Duplicated Column Name in table ({0}): {1}", tableName, columnName));
                        columnNameSet.Add(columnName);
                    }
                }
            }
        }

        [Test]
        public void AllTableShortNameShouldBeunique()
        {
            Assembly assembly = typeof(Entity).Assembly;
            IEnumerable<Type> enityTypes = assembly.GetTypes()
                .Where(t => typeof(Entity).IsAssignableFrom(t))
                .Where(t => TypeDescriptor.GetAttributes(t).OfType<TableShortNameAttribute>().FirstOrDefault() != null);

            var tableShortNameSet = new HashSet<string>();

            foreach (var entityType in enityTypes)
            {
                var tableShortNameAttribute = TypeDescriptor.GetAttributes(entityType).OfType<TableShortNameAttribute>().FirstOrDefault();
                Assert.IsNotNull(tableShortNameAttribute);

                string tableShortName = tableShortNameAttribute.TableShortName;
                Assert.IsFalse(tableShortNameSet.Contains(tableShortName), string.Format("Duplicated Table Short Name: {0}", tableShortName));

                tableShortNameSet.Add(tableShortName);
            }
        }
    }
}
