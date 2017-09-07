using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Infrastructure.TelerikExtensions
{
    [Serializable]
    public class SerializableGridModel<TModel> //: IGridModel
    {
        //IEnumerable IGridModel.Data { get { return Data; } }

        public IEnumerable<TModel> Data { get; set; }
        public int Total { get { return Data == null ? 0 : Data.Count(); } }
        public object Aggregates { get; set; }

        public SerializableGridModel()
        {
            Data = new List<TModel>();
        }

        public SerializableGridModel(IEnumerable<TModel> data)
        {
            Data = data;
        }
    }

    [Serializable]
    public class SerializableDataSourceResult
    {
        public IEnumerable Data { get; set; }

        public int Total { get; set; }

        public IEnumerable<AggregateResult> AggregateResults { get; set; }

        public object Errors { get; set; }

        public SerializableDataSourceResult(DataSourceResult dataSourceResult)
        {
            Data = dataSourceResult.Data;
            Total = dataSourceResult.Total;
            AggregateResults = dataSourceResult.AggregateResults;
            Errors = Errors;
        }
    }
}