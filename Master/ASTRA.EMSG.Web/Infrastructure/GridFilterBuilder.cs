using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Web.Infrastructure.HtmlHelperExtensions;
using Resources;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public interface ITextFilterBuilder<T>
    {
        ITextFilterBuilder<T> WithTextFilter<TValue>(Expression<Func<T, TValue>> expression, bool triggerOn3Char = false, bool notLastRow = false, int width = 198);
        IHtmlString WithFilterButton(bool inSeparateRow = false, string filterFunction = "RefreshGrid", bool useUnobtrusive = false, string filterButtonLabel = null);
        IHtmlString WithUnobstrusiveFilterButton(bool inSeparateRow = false, string filterButtonLabel = null);
        ITextFilterBuilder<T> WithDateFilter(Expression<Func<T, DateTime?>> expression, DateTime? defaultValue = null, int width = 200);
        ITextFilterBuilder<T> WithDecimalFilter(Expression<Func<T, decimal?>> expression, int decimalPlaces = 2, double? defaultValue = null);
        ITextFilterBuilder<T> WithDateTimeFilter(Expression<Func<T, DateTime?>> expression);
        IHtmlString WithoutFilterButton();
        IHtmlString WithGisReportFilterButton(string filterFunction = "RefreshGrid", bool useUnobtrusive = false, string filterButtonLabel = null);
        GridFilterBuilder<T> WithIntEnumFilter<TEnum>(Expression<Func<T, int?>> expression, int width = 200, string filterFunction = "RefreshGrid") where TEnum : struct;

        GridFilterBuilder<T> WithEnumFilter<TValue, TEnum>(Expression<Func<T, TValue>> expression, int width = 200, string filterFunction = "RefreshGrid")
            where TEnum : struct;
    }

    public class GridFilterBuilder<TModel> : IHtmlString, ITextFilterBuilder<TModel>
    {
        private const string TriggerFilterOnEnter = "triggerFilterOnEnter(event)";
        private const string TriggerFilterOn3Char = "triggerFilterOn3Char(event)";
        private readonly FluentTagBuilder fluentTagBuilder = new FluentTagBuilder("table");
        private readonly FluentTagBuilder emptyCellBuilder = new FluentTagBuilder("td").AddToInnerHtml("&nbsp;");

        private FluentTagBuilder lastRowBuilder;
        private readonly HtmlHelper htmlHelper;

        private readonly int? fieldLabelWidth;
        private readonly int? fieldEditorWidth;

        private string styleString = "width: 198px;";
        private string kendoClasses = "k-numeric-wrap k-input";

        public GridFilterBuilder(HtmlHelper htmlHelper, int? fieldLabelWidth, int? fieldEditorWidth)
        {
            this.htmlHelper = htmlHelper;
            this.fieldLabelWidth = fieldLabelWidth;
            this.fieldEditorWidth = fieldEditorWidth;
        }

        public string ToHtmlString()
        {
            return fluentTagBuilder.ToString();
        }

        private Dictionary<string, object> GetDropDownHtmlAttributes(int widthPixel)
        {
            return new Dictionary<string, object> { { "style", string.Format("z-index: 99; width: {0}px;", widthPixel)  } };
        }

        private Dictionary<string, object> GetDateFilterHtmlAttributes(int widthPixel)
        {            
            return new Dictionary<string, object> { { "style", string.Format("z-index: 99; width: {0}px;", widthPixel) }, { "onkeyup",  TriggerFilterOnEnter} };
        }


        public GridFilterBuilder<TModel> WithEnumFilter<TValue, TEnum>(Expression<Func<TModel, TValue>> expression, int width = 200, string filterFunction = "RefreshGrid")
            where TEnum : struct
        {
            var enumEditor = htmlHelper.EnumDropDownList<TEnum>(ExpressionHelper.GetExpressionText(expression),
                                                                emptyText: TextLocalization.All,
                                                                customize: b => b.HtmlAttributes(GetDropDownHtmlAttributes(width)).Events(events => events.Change(filterFunction)));
            var buildRowWithLabel = BuildRowWithLabel(expression, enumEditor.ToHtmlString(), true, true);
            buildRowWithLabel.AddToInnerHtml(emptyCellBuilder.ToString());
            fluentTagBuilder.AddToInnerHtml(buildRowWithLabel);
            return this;
        }
        
        public GridFilterBuilder<TModel> WithIntEnumFilter<TEnum>(Expression<Func<TModel, int?>> expression, int width = 200, string filterFunction = "RefreshGrid")
            where TEnum : struct
        {
            var enumEditor = htmlHelper.EnumDropDownList<TEnum>(ExpressionHelper.GetExpressionText(expression),
                                                                emptyText: TextLocalization.All,
                                                                customize: b => b.HtmlAttributes(GetDropDownHtmlAttributes(width)).Events(events => events.Change(filterFunction)), useIntValues: true);
            var buildRowWithLabel = BuildRowWithLabel(expression, enumEditor.ToHtmlString(), true, true);
            buildRowWithLabel.AddToInnerHtml(emptyCellBuilder.ToString());
            fluentTagBuilder.AddToInnerHtml(buildRowWithLabel);
            return this;
        }
        
        public GridFilterBuilder<TModel> WithCustomFilter<TValue>(Expression<Func<TModel, TValue>> expression, MvcHtmlString mvcHtmlString)
        {
            var buildRowWithLabel = BuildRowWithLabel(expression, mvcHtmlString.ToHtmlString());
            buildRowWithLabel.AddToInnerHtml(emptyCellBuilder.ToString());
            fluentTagBuilder.AddToInnerHtml(buildRowWithLabel);
            return this;
        }

        public GridFilterBuilder<TModel> WithLookupFilter<TValue>(Expression<Func<TModel, TValue>> expression, IEnumerable<DropDownListItem> dropDownItems, int width = 200, string filterFunction = "RefreshGrid")
        {
            var editorString = htmlHelper
                .Kendo()
                .DropDownList()
                .Name(ExpressionHelper.GetExpressionText(expression))
                .BindTo(dropDownItems)
                .Events(events => events.Change(filterFunction))
                .HtmlAttributes(GetDropDownHtmlAttributes(width))
                .ToHtmlString();

            var buildRowWithLabel = BuildRowWithLabel(expression, editorString, true, true);
            buildRowWithLabel.AddToInnerHtml(emptyCellBuilder.ToString());
            fluentTagBuilder.AddToInnerHtml(buildRowWithLabel);
            return this;
        }

        public ITextFilterBuilder<TModel> WithTextFilter<TValue>(Expression<Func<TModel, TValue>> expression, bool triggerOn3Char = false, bool notLastRow = false, int width = 198)
        {
            var textEditor = htmlHelper.TextBox(ExpressionHelper.GetExpressionText(expression), null, new {onkeyup = (triggerOn3Char? TriggerFilterOn3Char : TriggerFilterOnEnter), @class = kendoClasses, style = string.Format("width: {0}px", width) });

            if (lastRowBuilder != null)
                fluentTagBuilder.AddToInnerHtml(lastRowBuilder.AddToInnerHtml(emptyCellBuilder.ToString()));

            if (notLastRow)
            {
                fluentTagBuilder.AddToInnerHtml(BuildRowWithLabel(expression, textEditor.ToHtmlString()));
            }
            else
            {
                lastRowBuilder = BuildRowWithLabel(expression, textEditor.ToHtmlString());   
            }
            return this;
        }

        public ITextFilterBuilder<TModel> WithDateFilter(Expression<Func<TModel, DateTime?>> expression, DateTime? defaultValue = null, int width = 200)
        {
            var dateTimePicker = htmlHelper.Kendo()
                .DatePicker()
                .Name(ExpressionHelper.GetExpressionText(expression))
                .Min(Defaults.MinDateTime)
                .Max(Defaults.MaxDateTime)
                //.ButtonTitle(ButtonLocalization.Calendar) //NOT supported in Kendo
                .HtmlAttributes(new { style = string.Format("width: {0}px", width), onkeyup = TriggerFilterOnEnter})
                .Value(defaultValue);

            if (lastRowBuilder != null)
                fluentTagBuilder.AddToInnerHtml(lastRowBuilder.AddToInnerHtml(emptyCellBuilder.ToString()));

            lastRowBuilder = BuildRowWithLabel(expression, dateTimePicker.ToHtmlString());
            return this;
        }

        public ITextFilterBuilder<TModel> WithDateTimeFilter(Expression<Func<TModel, DateTime?>> expression)
        {
            var dateTimePicker = htmlHelper.Kendo()
                .DateTimePicker()
                .Name(ExpressionHelper.GetExpressionText(expression))
                .Min(Defaults.MinDateTime)
                .Max(Defaults.MaxDateTime)
                //.CalendarButtonTitle(ButtonLocalization.Calendar) //NOT supported in Kendo
                //.TimeButtonTitle(ButtonLocalization.Clock)        //NOT supported in Kendo
                .HtmlAttributes(new { onkeyup = TriggerFilterOnEnter })
                .Interval(30);

            if (lastRowBuilder != null)
                fluentTagBuilder.AddToInnerHtml(lastRowBuilder.AddToInnerHtml(emptyCellBuilder.ToString()));

            lastRowBuilder = BuildRowWithLabel(expression, dateTimePicker.ToHtmlString());
            return this;
        }

        public ITextFilterBuilder<TModel> WithDecimalFilter(Expression<Func<TModel, decimal?>> expression, int decimalPlaces = 2, double? defaultValue = null)
        {
            var numericTextBox = htmlHelper.Kendo().NumericTextBox()
                .Name(ExpressionHelper.GetExpressionText(expression))
                .Spinners(false).Placeholder(TextLocalization.EmptyMessage)
                .HtmlAttributes(new { style = styleString, maxlength = 12, onkeyup = TriggerFilterOnEnter })
                .Decimals(decimalPlaces)
                .Value(defaultValue);

            if (lastRowBuilder != null)
                fluentTagBuilder.AddToInnerHtml(lastRowBuilder.AddToInnerHtml(emptyCellBuilder.ToString()));

            lastRowBuilder = BuildRowWithLabel(expression, numericTextBox.ToHtmlString());
            return this;
        }

        public IHtmlString WithUnobstrusiveFilterButton(bool inSeparateRow = false, string filterButtonLabel = null)
        {
            return WithFilterButton(inSeparateRow, useUnobtrusive: true, filterButtonLabel: filterButtonLabel);
        }

        public IHtmlString WithFilterButton(bool inSeparateRow = false, string filterFunction = "RefreshGrid", bool useUnobtrusive = false, string filterButtonLabel = null)
        {
            var filterButton = useUnobtrusive
                                   ? htmlHelper.UnobtrusiveFilterButton(filterButtonLabel).ToHtmlString()
                                   : htmlHelper.FilterButton(string.Format("{0}()", filterFunction), filterButtonLabel).ToHtmlString();
            var filterButtonBuilder = new FluentTagBuilder("td").AddToInnerHtml(filterButton);

            if (inSeparateRow)
            {
                if (lastRowBuilder != null)
                    fluentTagBuilder.AddToInnerHtml(lastRowBuilder.AddToInnerHtml(emptyCellBuilder.ToString()));
                var filterRow = new FluentTagBuilder("tr");
                filterRow
                    .AddToInnerHtml(emptyCellBuilder.ToString())
                    .AddToInnerHtml(filterButtonBuilder.AddAttribute("style", "text-align:right").ToString())
                    .AddToInnerHtml(emptyCellBuilder.ToString());
                fluentTagBuilder.AddToInnerHtml(filterRow);
                return this;
            }

            if (lastRowBuilder != null)
            {
                lastRowBuilder.AddToInnerHtml(filterButtonBuilder.ToString());
                fluentTagBuilder.AddToInnerHtml(lastRowBuilder);
                return this;
            }

            return this;
        }
        
        public IHtmlString WithGisReportFilterButton(string filterFunction = "RefreshGrid", bool useUnobtrusive = false, string filterButtonLabel = null)
        {
            var filterButton = useUnobtrusive
                                   ? htmlHelper.UnobtrusiveFilterButton(filterButtonLabel).ToHtmlString()
                                   : htmlHelper.FilterButton(string.Format("{0}()", filterFunction), filterButtonLabel, "emsg-gis-report-filter-button").ToHtmlString();
            var filterButtonBuilder = new FluentTagBuilder("td").AddToInnerHtml(filterButton);

                if (lastRowBuilder != null)
                    fluentTagBuilder.AddToInnerHtml(lastRowBuilder.AddToInnerHtml(emptyCellBuilder.ToString()));
                var filterRow = new FluentTagBuilder("tr");
            filterRow
                .AddToInnerHtml(filterButtonBuilder.AddAttribute("style", "text-align:left").ToString())
                .AddToInnerHtml(emptyCellBuilder.ToString())
                .AddToInnerHtml(emptyCellBuilder.ToString());
                fluentTagBuilder.AddToInnerHtml(filterRow);

                return this;
        }

        public IHtmlString WithoutFilterButton()
        {
            if (lastRowBuilder != null)
            {
                fluentTagBuilder.AddToInnerHtml(lastRowBuilder);
                return this;
            }

            return this;
        }

        private FluentTagBuilder BuildRowWithLabel<TValue>(Expression<Func<TModel, TValue>> expression, string editorString,  bool visible = true, bool isDropDown = false)
        {
            int editorRightPadding = isDropDown ? 0 : 4;

            if (editorString.Contains("kendo"))
                editorString = editorString.Replace("198px;", "200px;");

            var fieldLabelTd = new FluentTagBuilder("td")
                .AddToInnerHtml(htmlHelper.LocalizedLabelFor(expression).ToString())
                .AddCssClass("field-label");

            if (fieldLabelWidth.HasValue)
                fieldLabelTd.AddAttribute("style", string.Format("width: {0}px", fieldLabelWidth));

            var fieldEditorTd = new FluentTagBuilder("td")
                .AddToInnerHtml(editorString)
                .AddCssClass("field-input");

            if (fieldEditorWidth.HasValue)
                fieldEditorTd.AddAttribute("style", string.Format("width: {0}px; padding-right: {1}px", fieldEditorWidth, editorRightPadding));
            else
                fieldEditorTd.AddAttribute("style", string.Format("padding-right: {0}px", editorRightPadding));

            var editorRow = new FluentTagBuilder("tr")
                .AddToInnerHtml(fieldLabelTd)
                .AddToInnerHtml(fieldEditorTd);

            if (!visible)
                editorRow.AddAttribute("style", "display: none");

            return editorRow;
        }
    }
}