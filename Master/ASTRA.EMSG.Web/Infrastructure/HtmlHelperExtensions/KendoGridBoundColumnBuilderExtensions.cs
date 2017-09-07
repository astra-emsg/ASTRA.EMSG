using System;
using System.Linq.Expressions;
using ASTRA.EMSG.Common;
using Kendo.Mvc;
using Resources;
using Kendo.Mvc.UI.Fluent;

namespace ASTRA.EMSG.Web.Infrastructure.HtmlHelperExtensions
{
    public static class KendoGridBoundColumnBuilderExtensions
    {
        public static GridBoundColumnBuilder<TModel> Sum<TModel>(this GridBoundColumnBuilder<TModel> builder, string formatString = null) where TModel : class
        {
            string sum = string.Format("#= sum == 0 ? '0' : kendo.format('{0}', sum) #", formatString ?? FormatStrings.TelerikGridSumFormat);
            return builder
                .AlignRight()
                .AggregateForKendo("sum")
                .ClientFooterTemplate(string.Format("<div style='text-align: right;'>{0}</div>", sum));
        }

        public static GridBoundColumnBuilder<TModel> AggregateForKendo<TModel>(this GridBoundColumnBuilder<TModel> builder, string aggregateType) where TModel : class
        {
            return builder.ClientFooterTemplate("#= 'aggregateType' #");
        }

        public static GridBoundColumnBuilder<TModel> SumLabel<TModel>(this GridBoundColumnBuilder<TModel> builder) where TModel : class
        {
            return builder.ClientFooterTemplate(string.Format("<div'><b>{0}</b></div>", GridHeaderFooterLocalization.GridFooterSumme));
        }

        public static GridBoundColumnBuilder<TModel> AlignRight<TModel>(this GridBoundColumnBuilder<TModel> builder, string formatString = null) where TModel : class
        {
            return builder.HtmlAttributes(new {@class = "alignRight"});
        }

        public static GridBoundColumnBuilder<TModel> TruncatedTextTemplate<TModel>(this GridBoundColumnBuilder<TModel> builder, Expression<Func<TModel,string >> longText, Expression<Func<TModel,string >> shortText) where TModel : class
        {
            var longTextPopertyName = ExpressionHelper.GetPropertyName(longText);
            var shortPropertyName = ExpressionHelper.GetPropertyName(shortText);
            return builder.ClientTemplate(string.Format("<span title=\" #= {0} || '' # \"> #= {1} || '' #</span>", longTextPopertyName, shortPropertyName));
        }
    }
}