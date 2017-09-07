using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TechTalk.SpecFlow;

namespace ASTRA.EMSG.IntegrationTests.Support.ObjectReader
{
    public class ObjectReader
    {
        private readonly IObjectReaderConfiguration objectReaderConfiguration;

        public ObjectReader(IObjectReaderConfiguration objectReaderConfiguration)
        {
            this.objectReaderConfiguration = objectReaderConfiguration;
        }

        public TObject GetObject<TObject>(Table table) where TObject : new()
        {
            return GetObject(table, new TObject());
        }

        public TObject GetObject<TObject>(Table table, TObject obj) where TObject : new()
        {
            return SetProperties(obj, table, table.Rows[0]);
        }

        public List<TObject> GetObjectList<TObject>(Table table) where TObject : new()
        {
            return table.Rows.Select(tableRow => SetProperties(new TObject(), table, tableRow)).ToList();
        }

        public List<Tuple<TableRow,TObject>> GetObjectListWithRow<TObject>(Table table) where TObject : new()
        {
            return table.Rows.Select(tableRow => new Tuple<TableRow,TObject>(tableRow, SetProperties(new TObject(), table, tableRow))).ToList();
        }

        public bool AreObjectListWithTableEqual<TObject>(List<TObject> objects, Table table) where TObject : new()
        {
            if(objects.Count != table.RowCount)
                return false;

            foreach (TObject o in objects)
            {
                if (!IsInTable(o, table))
                    return false;
            }

            return true;
        }

        public bool ARe<TObject>(List<TObject> objects, Table table) where TObject : new()
        {
            foreach (var tableRow in table.Rows)
            {
                bool any = false;
                foreach (TObject o in objects)
                {
                    bool all = true;
                    foreach (string h in table.Header)
                    {
                        if (!AreEquals(o, tableRow, h))
                        {
                            all = false;
                            break;
                        }
                    }
                    if (all)
                    {
                        any = true;
                        break;
                    }
                }
                if (!any)
                    return false;
            }
            return true;
        }

        private bool IsInTable<TObject>(TObject obj, Table table) where TObject : new()
        {
            return table.Rows.Select(tableRow => table.Header.All(header => AreEquals(obj, tableRow, header))).Any(all => all);
        }

        private bool AreEquals<TObject>(TObject obj, TableRow tableRow, string header) where TObject : new()
        {
            return Equals(GetExpectedValue<TObject>(tableRow, header), GetCurrentValue(obj, header));
        }

        private object GetCurrentValue<TObject>(TObject obj, string header) where TObject : new()
        {
            return GetPropertyInfo<TObject>(header).GetValue(obj, null);
        }

        private object GetExpectedValue<TObject>(TableRow tableRow, string header) where TObject : new()
        {
            return GetValue<TObject>(tableRow, header, GetPropertyInfo<TObject>(header));
        }

        private TObject SetProperties<TObject>(TObject obj, Table table, TableRow tableRow) where TObject : new()
        {
            foreach (var header in table.Header)
            {
                var customAction = objectReaderConfiguration.GetColumnAction<TObject>(header);
                if (customAction != null)
                {
                    customAction(tableRow,obj);
                    continue;
                }
                var propertyInfo = GetPropertyInfo<TObject>(header);
                var value = GetValue<TObject>(tableRow, header, propertyInfo);

                propertyInfo.SetValue(obj, value, null);
            }

            return obj;
        }

        private object GetValue<TObject>(TableRow tableRow, string header, PropertyInfo propertyInfo) where TObject : new()
        {
            var converter = objectReaderConfiguration.GetConverter<TObject>(propertyInfo);
            var value = converter(tableRow[header], propertyInfo);
            return value;
        }

        private PropertyInfo GetPropertyInfo<TObject>(string header) where TObject : new()
        {
            var objType = typeof(TObject);

            var propertyName = objectReaderConfiguration.GetPropertyNameOfType<TObject>(header);

            var propertyInfo = objType.GetProperty(propertyName);
            if (propertyInfo == null)
                throw new Exception(string.Format("Property '{0}' not found on '{1}'", header, objType.Name));
            return propertyInfo;
        }
    }
}