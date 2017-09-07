using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Master;
using Kendo.Mvc.UI;
using ExpressionHelper = ASTRA.EMSG.Common.ExpressionHelper;

namespace ASTRA.EMSG.Web.Infrastructure.HtmlHelperExtensions
{
    public static partial class HtmlHelperExtensions
    {
        public static MvcHtmlString DropDownList<TValue>(this HtmlHelper helper, Expression<Func<TValue>> expression, List<TextValuePair<TValue>> textValuePairs)
        {
            return DropDownList(helper, expression, textValuePairs, null);
        }

        public static MvcHtmlString DropDownList<TValue>(this HtmlHelper helper, Expression<Func<TValue>> expression, List<TextValuePair<TValue>> textValuePairs, string emptyText)
        {
            return DropDownList(helper, expression, textValuePairs, emptyText, null);
        }

        public static MvcHtmlString DropDownList<TValue>(this HtmlHelper helper, Expression<Func<TValue>> expression, List<TextValuePair<TValue>> textValuePairs, string emptyText, object htmlAttributes)
        {
            TValue selectedValue = expression.Compile()();
            var dropDownItemList = textValuePairs
                .ToDropDownItemList(
                item => item.Text,
                item => Equals(item.Value, null) ? "" : item.Value.ToString(),
                selectedValue, emptyText);
            return MvcHtmlString.Create(helper.Kendo().ComboBox().Name(ExpressionHelper.GetPropertyName(expression)).BindTo(dropDownItemList).HtmlAttributes(htmlAttributes).ToHtmlString());
        }
    }
}