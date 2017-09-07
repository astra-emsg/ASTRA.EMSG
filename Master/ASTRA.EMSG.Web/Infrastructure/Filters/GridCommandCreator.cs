using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class GridCommandCreator : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public string ActionParameterName { get; set; }

        public string GridName { get; set; }

        public bool EnableCustomBinding { get; set; }

        public GridCommandCreator()
        {
            ActionParameterName = "command";
        }

        private string Prefix(string key)
        {
            if (StringExtensions.HasValue(this.GridName))
                return GridName + "-" + key;
            return key;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string key = ActionParameterName;
            IEnumerable<KeyValuePair<string, object>> source = filterContext.ActionParameters.Where(parameter => parameter.Value is GridCommand);
            if (source.Count() == 1)
                key = source.First().Key;

            if (!filterContext.ActionParameters.ContainsKey(key))
                return;

            GridCommand gridCommand = (GridCommand)filterContext.ActionParameters[key] ?? new GridCommand();
            gridCommand.Page = filterContext.Controller.ValueOf<int>(Prefix(GridUrlParameters.Page));
            gridCommand.PageSize = filterContext.Controller.ValueOf<int>(Prefix(GridUrlParameters.PageSize));

            string from1 = filterContext.Controller.ValueOf<string>(Prefix(GridUrlParameters.Sort));
            gridCommand.SortDescriptors.AddRange(GridDescriptorSerializer.Deserialize<SortDescriptor>(from1));

            string input = filterContext.Controller.ValueOf<string>(Prefix(GridUrlParameters.Filter));
            if (input != null)
                input = input.Replace('+', ' ');
            gridCommand.FilterDescriptors.AddRange(FilterDescriptorFactory.Create(input));

            string from2 = filterContext.Controller.ValueOf<string>(Prefix(GridUrlParameters.Group));
            gridCommand.GroupDescriptors.AddRange(GridDescriptorSerializer.Deserialize<GroupDescriptor>(from2));

            string from3 = filterContext.Controller.ValueOf<string>(Prefix(GridUrlParameters.Aggregates));
            gridCommand.Aggregates.AddRange(GridDescriptorSerializer.Deserialize<AggregateDescriptor>(from3));

            filterContext.ActionParameters[ActionParameterName] = gridCommand;
        }
    }
}