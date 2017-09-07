using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Reporting;
using Kendo.Mvc;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public static class GridCommandExtensions
    {
        public static List<GridFilterDescriptor> GetGridFilterDescriptors(this GridCommand command)
        {
            return command.FilterDescriptors.OfType<FilterDescriptor>().Select(CreateGridFilterDescriptor).ToList();
        }

        private static GridFilterDescriptor CreateGridFilterDescriptor(FilterDescriptor filterDescriptor)
        {
            return new GridFilterDescriptor
                       {
                           Member = filterDescriptor.Member,
                           MemberType = filterDescriptor.MemberType,
                           Operator = (GridFilterOperator)Enum.Parse(typeof(GridFilterOperator), filterDescriptor.Operator.ToString()),
                           Value = filterDescriptor.Value
                       };
        }
    }
}