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
using Kendo.Mvc.UI.Fluent;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class EditorTableBuilder<TModel> : IHtmlString
    {
        private readonly FluentTagBuilder fluentTagBuilder = new FluentTagBuilder("table");
        private readonly HtmlHelper<TModel> htmlHelper;
        private readonly int? fieldLabelWidth;
        private readonly int? fieldEditorWidth;
        private readonly bool createErrorDialogs;
        private const string HandleDecimalSeparator = "handleDecimalSeparator(event)";
        private string styleString = "width: 198px;";
        private string kendoClasses = "k-numeric-wrap k-input";

        public EditorTableBuilder(HtmlHelper<TModel> htmlHelper, int? fieldLabelWidth, int? fieldEditorWidth, bool createErrorDialogs = true)
        {
            this.htmlHelper = htmlHelper;
            this.fieldLabelWidth = fieldLabelWidth;
            this.fieldEditorWidth = fieldEditorWidth;
            this.createErrorDialogs = createErrorDialogs;
        }

        public string ToHtmlString()
        {
            if (!createErrorDialogs)
                return fluentTagBuilder.ToString();

            var addToInnerHtml = new FluentTagBuilder("script").AddAttribute("type", "text/javascript").AddToInnerHtml("common.createErrorDialogs()");
            return fluentTagBuilder + addToInnerHtml.ToString();
        }

        public EditorTableBuilder<TModel> WithTextBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, bool requiredField = true)
        {
            var editorHtml = htmlHelper.TextBoxFor(expression, new { @class = kendoClasses, style = styleString});
            BuildEditor(expression, requiredField, editorHtml);

            return this;
        }

        public EditorTableBuilder<TModel> WithTextAreaFor<TValue>(Expression<Func<TModel, TValue>> expression, bool requiredField = true)
        {
            var editorHtml = htmlHelper.TextAreaFor(expression, new { @class = kendoClasses, style = styleString });
            BuildEditor(expression, requiredField, editorHtml);

            return this;
        }

        public EditorTableBuilder<TModel> WithHiddenFor<TValue>(Expression<Func<TModel, TValue>> expression, bool requiredField = true)
        {
            var editorHtml = htmlHelper.HiddenFor(expression);
            var fieldEditorTd = new FluentTagBuilder("td")
                .AddToInnerHtml(editorHtml.ToHtmlString());

            var editorRow = new FluentTagBuilder("tr")
                .AddAttribute("style", "display: none")
                .AddToInnerHtml(fieldEditorTd);

            fluentTagBuilder.AddToInnerHtml(editorRow);
            return this;
        }

        public EditorTableBuilder<TModel> WithIntegerEditorFor(Expression<Func<TModel, int>> expression, int digitCount = 9, bool requiredField = true, bool visible = true)
        {
            var editorHtml = htmlHelper.IntegerTextBoxFor(expression, digitCount).HtmlAttributes(new { style = styleString, maxLength = digitCount }).ToHtmlString();
            BuildEditor(expression, requiredField, editorHtml, GetValidationMessage(expression), null, visible);

            return this;
        }

        public EditorTableBuilder<TModel> WithIntegerEditorFor(Expression<Func<TModel, int?>> expression, int digitCount = 9, bool requiredField = true, bool visible = true, int width = 200, bool isYear = false)
        {
            var editorHtml = htmlHelper.IntegerTextBoxFor(expression, digitCount).HtmlAttributes(new { style = string.Format("width: {0}px", width), maxLength = digitCount });
            if (isYear)
                editorHtml.Format("{0:yyyy}");
            BuildEditor(expression, requiredField, editorHtml.ToHtmlString(), GetValidationMessage(expression), null, visible);

            return this;
        }

        public EditorTableBuilder<TModel> WithDecimalEditorFor(Expression<Func<TModel, decimal>> expression, int decimalPlaces = 2, bool requiredField = true, string editorPostfix = null, bool visible = true, string customLabel = null)
        {
            var numericTextBoxBuilder = htmlHelper.DecimalTextBoxFor(expression, decimalPlaces).HtmlAttributes(new { style = styleString });
            var editorHtml = numericTextBoxBuilder.ToHtmlString();
            BuildEditor(expression, requiredField, editorHtml, GetValidationMessage(expression), editorPostfix, visible, customLabel);

            return this;
        }

        public EditorTableBuilder<TModel> WithDecimalEditorFor(Expression<Func<TModel, decimal?>> expression, int decimalPlaces = 2, bool requiredField = true, string editorPostfix = null, bool visible = true, string customLabel = null, int width = -1)
        {
            var numericTextBoxBuilder = htmlHelper.DecimalTextBoxFor(expression, decimalPlaces).HtmlAttributes(new { style = width < 0 ? styleString : string.Format("width: {0}px", width) });
            var editorHtml = numericTextBoxBuilder.ToHtmlString();
            BuildEditor(expression, requiredField, editorHtml, GetValidationMessage(expression), editorPostfix, visible, customLabel);

            return this;
        }

        private string GetValidationMessage<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            var validationMessage = htmlHelper.ValidationMessageFor(expression);
            return validationMessage == null ? null : validationMessage.ToHtmlString();
        }

        public EditorTableBuilder<TModel> WithEnumEditorFor<TValue>(Expression<Func<TModel, TValue>> expression, Expression<Func<TValue>> enumPropertyValueExpression, Func<DropDownListBuilder, DropDownListBuilder> customize)
        {
            return WithEnumEditorFor(expression, enumPropertyValueExpression, null, true, customize);
        }

        public EditorTableBuilder<TModel> WithEnumEditorFor<TValue>(Expression<Func<TModel, TValue>> expression, Expression<Func<TValue>> enumPropertyValueExpression, string emptyText = null, bool requiredField = true, Func<DropDownListBuilder, DropDownListBuilder> customize = null)
        {
            Func<DropDownListBuilder, DropDownListBuilder> func = ddlb => customize == null ? ddlb.Width(200) : customize(ddlb.Width(200));
            var editorHtml = htmlHelper.EnumDropDownListFor(expression, enumPropertyValueExpression, emptyText, null, func);
            BuildEditor(expression, requiredField, editorHtml.ToHtmlString(), GetValidationMessage(expression), isDropDown: true);

            return this;
        }

        public EditorTableBuilder<TModel> WithDateTimeEditorFor(Expression<Func<TModel, DateTime?>> expression, bool requiredField = true)
        {
            BuildEditor(expression, requiredField, new MvcHtmlString(htmlHelper.Kendo().DatePickerFor(expression).Min(Defaults.MinDateTime).Max(Defaults.MaxDateTime).HtmlAttributes(new { style = styleString }).ToHtmlString()));
            return this;
        }

        public EditorTableBuilder<TModel> WithDateTimeEditorFor(Expression<Func<TModel, DateTime>> expression, bool requiredField = true)
        {
            BuildEditor(expression, requiredField, new MvcHtmlString(htmlHelper.Kendo().DatePickerFor(expression).Min(Defaults.MinDateTime).Max(Defaults.MaxDateTime).HtmlAttributes(new { style = styleString }).ToHtmlString()));
            return this;
        }

        public EditorTableBuilder<TModel> WithCustomEditorFor<TValue>(Expression<Func<TModel, TValue>> expression, MvcHtmlString editorMvcHtmlString, bool requiredField = true, bool isDropDown = true, string customLabel = null)
        {
            BuildEditor(expression, requiredField, editorMvcHtmlString.ToHtmlString(), GetValidationMessage(expression), isDropDown: isDropDown, customLabel: customLabel);
            return this;
        }

        public EditorTableBuilder<TModel> WithLookupEditorFor<TValue>(Expression<Func<TModel, TValue>> expression, IEnumerable<DropDownListItem> dropDownItems, int width = 200, bool requiredField = true, Func<DropDownListBuilder, DropDownListBuilder> customize = null)
        {
            var dropDownListBuilder = htmlHelper
                .Kendo()
                .DropDownListFor(expression)
                .BindTo(dropDownItems);
            if (customize != null)
                dropDownListBuilder = customize(dropDownListBuilder);

            dropDownListBuilder.Width(width);
            var editorString = dropDownListBuilder.ToHtmlString();

            BuildEditor(expression, requiredField, editorString, GetValidationMessage(expression), isDropDown: true);
            return this;
        }

        public EditorTableBuilder<TModel> WithReadOnlyFieldFor<TValue>(Expression<Func<TModel, TValue>> expression, bool requiredField = true)
        {
            BuildEditor(expression, requiredField, htmlHelper.ReadonlyTextBoxFor(expression).ToHtmlString(), GetValidationMessage(expression));
            return this;
        }

        public EditorTableBuilder<TModel> WithDisplayFor<TValue>(Expression<Func<TModel, TValue>> expression, bool requiredField = true, string editorPostfix = null, bool visible = true)
        {
            if (!visible)
                return this;

            FluentTagBuilder span = new FluentTagBuilder("span")
                .AddAttribute("id", htmlHelper.ClientIdFor(expression).ToHtmlString())
                .AddToInnerHtml(htmlHelper.DisplayFor(expression).ToHtmlString());

            BuildEditor(expression, requiredField, span.ToString(), null, editorPostfix);
            return this;
        }

        public EditorTableBuilder<TModel> WithSeparator()
        {
            FluentTagBuilder tdBuilder = new FluentTagBuilder("td").AddAttribute("colspan", "2").AddToInnerHtml(new FluentTagBuilder("hr"));
            fluentTagBuilder.AddToInnerHtml(new FluentTagBuilder("tr").AddToInnerHtml(tdBuilder));
            return this;
        }

        private void BuildEditor<TValue>(Expression<Func<TModel, TValue>> expression, bool requiredField, IHtmlString editorIHtmlString)
        {
            var validationMessageFor = GetValidationMessage(expression);
            BuildEditor(expression, requiredField, editorIHtmlString.ToHtmlString(), validationMessageFor);
        }

        private void BuildEditor<TValue>(Expression<Func<TModel, TValue>> expression, bool requiredField, IHtmlString editorIHtmlString, IHtmlString validatorMessageIHtmlString)
        {
            BuildEditor(expression, requiredField, editorIHtmlString.ToHtmlString(), validatorMessageIHtmlString == null ? null : validatorMessageIHtmlString.ToHtmlString());
        }

        private void BuildEditor<TValue>(Expression<Func<TModel, TValue>> expression, bool requiredField, string editorString, string validationMessageHtml, string editorPostfix = null, bool visible = true, string customLabel = null, bool isDropDown = false)
        {
            int editorRightPadding = isDropDown ? 0 : 5;

            if (editorString.Contains("kendo"))
                editorString = editorString.Replace("198px;", "200px;");
            
            var fieldLabelTd = new FluentTagBuilder("td")
                .AddToInnerHtml(customLabel.HasText() ? customLabel : htmlHelper.LocalizedLabelFor(expression).ToString())
                .AddCssClass("field-label");

            if (fieldLabelWidth.HasValue)
                fieldLabelTd.AddAttribute("style", string.Format("min-width: {0}px; max-width: {0}px", fieldLabelWidth));

            if (requiredField)
                fieldLabelTd.AddCssClass("field-required");

            var fieldEditorTd = new FluentTagBuilder("td")
                .AddToInnerHtml(editorString)
                .AddCssClass("field-input");

            if (fieldEditorWidth.HasValue)
                fieldEditorTd.AddAttribute("style", string.Format("width: {0}px; padding-right: {1}px", fieldEditorWidth, editorRightPadding));
            else
                fieldEditorTd.AddAttribute("style", string.Format("padding-right: {0}px", editorRightPadding));

            if (editorPostfix != null)
                fieldEditorTd.AddToInnerHtml(editorPostfix);

            string modelName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            ModelState modelState = htmlHelper.ViewData.ModelState[modelName];
            ModelErrorCollection modelErrors = (modelState == null) ? null : modelState.Errors;

            var errorTd = new FluentTagBuilder("td").AddAttribute("style", "width: 24px");

            if (modelErrors != null && modelErrors.Count > 0)
            {
                errorTd.AddToInnerHtml(new FluentTagBuilder("img")
                    .AddAttribute("src", htmlHelper.ToUrlHelper().Content("~/Content/Images/infoerror.gif"))
                    .AddAttribute("data-property", ExpressionHelper.GetExpressionText(expression))
                    .AddCssClass("error-image")
                    .ToString(TagRenderMode.SelfClosing));

                errorTd
                .AddToInnerHtml(
                new FluentTagBuilder("div")
                    .AddCssClass("error-message-div")
                    .AddAttribute("data-property", ExpressionHelper.GetExpressionText(expression))
                    .AddAttribute("data-title", (string)HtmlHelperExtensions.HtmlHelperExtensions.Localize(expression))
                    .AddToInnerHtml(validationMessageHtml));
            }

            var editorRow = new FluentTagBuilder("tr")
                .AddToInnerHtml(fieldLabelTd)
                .AddToInnerHtml(fieldEditorTd)
                .AddToInnerHtml(errorTd);

            if (!visible)
                editorRow.AddAttribute("style", "display: none");
            
            fluentTagBuilder.AddToInnerHtml(editorRow);
        }
    }
}