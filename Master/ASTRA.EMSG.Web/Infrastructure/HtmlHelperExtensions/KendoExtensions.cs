using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using Resources;

namespace ASTRA.EMSG.Web.Infrastructure.HtmlHelperExtensions
{
    public static partial class HtmlHelperExtensions
    {
        private static Kendo.Mvc.UI.Fluent.NumericTextBoxBuilder<TValue> NumericTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, int digitCount) where TValue : struct
        {
            string addtionalClass = null;
            string key = ExpressionHelper.GetExpressionText(expression);
            if (helper.ViewData.ModelState.ContainsKey(key) && helper.ViewData.ModelState[key].Errors.Any())
                addtionalClass = "input-validation-error";

            var builder = helper.Kendo().NumericTextBoxFor(expression)
                .Spinners(false);

            if (addtionalClass != null)
                builder.HtmlAttributes(new { @class = addtionalClass, maxlength = digitCount });
            
            return builder;
        }

        private static Kendo.Mvc.UI.Fluent.NumericTextBoxBuilder<TValue> NumericTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue?>> expression, int digitCount) where TValue : struct
        {
            string addtionalClass = null;
            string key = ExpressionHelper.GetExpressionText(expression);
            if (helper.ViewData.ModelState.ContainsKey(key) && helper.ViewData.ModelState[key].Errors.Any())
                addtionalClass = "input-validation-error";

            var builder = helper.Kendo().NumericTextBoxFor(expression)
                .Spinners(false);

            if (addtionalClass != null)
                builder.HtmlAttributes(new { @class = addtionalClass, maxlength = digitCount });
            
            return builder;
        }

        public static Kendo.Mvc.UI.Fluent.NumericTextBoxBuilder<int> IntegerTextBoxFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, int>> expression, int digitCount = 9)
        {
            return helper.NumericTextBoxFor(expression, digitCount).Decimals(0).Format("n0");
        }

        public static Kendo.Mvc.UI.Fluent.NumericTextBoxBuilder<int> IntegerTextBoxFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, int?>> expression, int digitCount = 9)
        {
            return helper.NumericTextBoxFor(expression, digitCount).Decimals(0).Format("n0");
        }

        public static Kendo.Mvc.UI.Fluent.NumericTextBoxBuilder<decimal> DecimalTextBoxFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, decimal>> expression, int decimalDigits = 2)
        {
            return helper.NumericTextBoxFor(expression, 12).Decimals(decimalDigits);
        }

        public static Kendo.Mvc.UI.Fluent.NumericTextBoxBuilder<decimal> DecimalTextBoxFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, decimal?>> expression, int decimalDigits = 2)
        {
            return helper.NumericTextBoxFor(expression, 12).Decimals(decimalDigits);
        }

        public static WindowBuilder TelerikWindow<TModel>(this HtmlHelper<TModel> helper, string title = "Title", string windowName = "Window", string contentDivId = "contentDiv")
        {
            return helper.Kendo().Window()
               .Name(windowName)
               .Title(title)
               .Draggable(true)
               .Resizable()
               .Content(string.Format("<div id='{0}'></div>", contentDivId))
               .Scrollable(true)
               .Modal(true)
               .Actions(b => b.Maximize().Close())
               .Visible(false);
        }

        public static MvcHtmlString JavaScriptActionLink(this HtmlHelper htmlHelper, string function, string text)
        {
            return new MvcHtmlString(string.Format("<a class='k-button' href=javascript:{0}>{1}</a>", function, text));
        }

        public static string JavaScriptActionLinkString(this HtmlHelper htmlHelper, string function, string text)
        {
            return JavaScriptActionLink(htmlHelper, function, text).ToHtmlString();
        }

        public static GridBuilder<T> ApplyEmsgSettings<T>(this GridBuilder<T> gridBuilder, bool isHilfeSuche = false, bool pageable = true, bool filterable = true,
            bool sortable = true, bool editable = true, bool scrollable = false, bool refreshable = true) where T : class
        {
            string noRecords = isHilfeSuche
                ? string.Format("<div class=\"emsg-no-records-text\">{0}</div>", TextLocalization.EmptyHelpSearchResult)
                : string.Format("<div class=\"emsg-no-records-text\">{0}</div>", GridLocalization.NoRecords);

            return 
                gridBuilder
                .NoRecords(r => r.Template(noRecords))
                .Pageable(c => c.Enabled(pageable)
                                .Refresh(refreshable))
                .Filterable(c => c.Enabled(filterable))
                .Sortable(c => c.Enabled(sortable)
                                .SortMode(GridSortMode.MultipleColumn))
                .Editable(c => c.Enabled(editable)
                                .DisplayDeleteConfirmation(GridLocalization.DeleteConfirmation))
                .Scrollable(c => c.Enabled(scrollable));
        }
    }
}