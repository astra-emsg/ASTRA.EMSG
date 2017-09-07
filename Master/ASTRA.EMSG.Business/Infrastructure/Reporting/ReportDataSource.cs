using System;
using System.Collections;
using System.Collections.Generic;

namespace ASTRA.EMSG.Business.Infrastructure.Reporting
{
    public class ReportDataSource
    {
        private const string PoPostfix = "Po";

        public string Name { get; private set; }
        public object Value { get; private set; }

        public IReportDataSourceFactory ReportDataSourceFactory { get; private set; }

        public ReportDataSource(IEnumerable dataSource, IReportDataSourceFactory reportDataSourceFactory)
            : this(GetDataSourceCollectionIdentifier(dataSource.GetType()), dataSource, reportDataSourceFactory)
        { }

        public ReportDataSource(object dataSource, IReportDataSourceFactory reportDataSourceFactory)
            : this(GetDataSourceIdentifier(dataSource.GetType()), new ArrayList { dataSource }, reportDataSourceFactory)
        { }

        public ReportDataSource(string name, object dataSource, IReportDataSourceFactory reportDataSourceFactory)
            : this(name, new ArrayList { dataSource }, reportDataSourceFactory)
        { }

        public ReportDataSource(string name, IEnumerable dataSource, IReportDataSourceFactory reportDataSourceFactory)
        {
            Name = name;
            Value = dataSource;
            ReportDataSourceFactory = reportDataSourceFactory;
        }

        public IReportDataSource ToReportDataSource()
        {
            return ReportDataSourceFactory.GetReportDataSource(Name, Value);
        }

        public static string GetDataSourceCollectionIdentifier(Type dataSourceCollectionType)
        {
            return GetDataSourceIdentifier(dataSourceCollectionType.GetGenericArguments()[0]);
        }

        private static string GetDataSourceIdentifier(Type dataSourceType)
        {
            var name = dataSourceType.Name;
            string dataSourceIdentifier;
            if (name.EndsWith(PoPostfix)) 
                dataSourceIdentifier = name.Substring(0, name.Length - 2);
            else 
                throw new NotSupportedException(string.Format("This datasource name hasn't a postfix '{1}' {0}!", name, PoPostfix));

            return dataSourceIdentifier;
        }

        public static void SetDataSources(IReportDataSourceCollection reportDataSourceCollection, IEnumerable<ReportDataSource> reportDataSources)
        {
            foreach (var reportDataSource in reportDataSources)
                reportDataSourceCollection.Add(reportDataSource.ToReportDataSource());
        }
    }
}