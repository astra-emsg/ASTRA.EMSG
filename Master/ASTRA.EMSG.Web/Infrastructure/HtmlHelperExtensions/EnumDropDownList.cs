using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Kendo.Mvc.UI.Fluent;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Infrastructure.HtmlHelperExtensions
{
    public static partial class HtmlHelperExtensions
    {
        public static MvcHtmlString EnumDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, Expression<Func<TValue>> enumPropertyExpression, string emptyText, object htmlAttributes, Func<DropDownListBuilder, DropDownListBuilder> customize = null)
        {
            var selected = enumPropertyExpression.Compile()();
            var enumType = typeof(TValue);

            if (!enumType.IsEnum)
                throw new ArgumentException("Type is not an enum.");

            if (selected != null && selected.GetType() != enumType)
                throw new ArgumentException("Selected object is not " + enumType);

            var dropDownItemList = Enum.GetValues(enumType).Cast<object>()
                .ToDropDownItemList(
                item => htmlHelper.LocalizedEnum((Enum) item).ToString(), 
                item => (int)item, 
                selected, emptyText);

            var comboBoxBuilder = htmlHelper.Kendo()
                .DropDownListFor(expression)
                .BindTo(dropDownItemList)
                .HtmlAttributes(htmlAttributes);

            if (customize != null)
                comboBoxBuilder = customize(comboBoxBuilder);

            return MvcHtmlString.Create(comboBoxBuilder.ToHtmlString());
        }

        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, string name, TEnum? selected = null, string emptyText = null, object htmlAttributes = null, Func<DropDownListBuilder, DropDownListBuilder> customize = null, Boolean useIntValues = false)
            where TEnum : struct 
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
                throw new ArgumentException("Type is not an enum.");

            if (selected != null && selected.GetType() != enumType)
                throw new ArgumentException("Selected object is not " + enumType);

            var dropDownItemList = Enum.GetValues(enumType).Cast<object>()
                .ToDropDownItemList(
                item => htmlHelper.LocalizedEnum((Enum)item).ToString(),
                item => item.ToString(),
                selected, emptyText);

            if (useIntValues)
            {
              dropDownItemList = Enum.GetValues(enumType).Cast<object>()
                .ToDropDownItemList(
                item => htmlHelper.LocalizedEnum((Enum)item).ToString(),
                item => (int)item,
                selected, emptyText);
            };

            var comboBoxBuilder = htmlHelper.Kendo().DropDownList().Name(name).BindTo(dropDownItemList).HtmlAttributes(htmlAttributes);
            if (customize != null)
                comboBoxBuilder = customize(comboBoxBuilder);

            return MvcHtmlString.Create(comboBoxBuilder.ToHtmlString());
        }
    }
}