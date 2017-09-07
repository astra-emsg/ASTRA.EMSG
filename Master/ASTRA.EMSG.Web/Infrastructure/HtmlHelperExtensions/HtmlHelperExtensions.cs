using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Master;
using Resources;
using ExpressionHelper = System.Web.Mvc.ExpressionHelper;
using FluentValidation.Internal;
using Kendo.Mvc;

namespace ASTRA.EMSG.Web.Infrastructure.HtmlHelperExtensions
{
    public static partial class HtmlHelperExtensions
    {
        public static HtmlHelper<TModel> ForType<TModel>(this HtmlHelper html)
        {
            return new HtmlHelper<TModel>(html.ViewContext, html.ViewDataContainer);
        }

        public static MvcHtmlString DisplayDecimal<TModel>(this HtmlHelper<TModel> html, decimal? d)
        {
            if (!d.HasValue)
                return new MvcHtmlString(string.Empty);
            return new MvcHtmlString(string.Format(FormatStrings.ShortDecimalFormat, d));
        }

        public static MvcHtmlString LocalizedLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var label = Localize(expression);
            return label != null ? html.LabelFor(expression, label.ToString()) : html.LabelFor(expression);
        }

        public static MvcHtmlString LocalizedLabelFor<TModel, TValue>(this HtmlHelper html, Expression<Func<TModel, TValue>> expression)
        {
            var label = Localize(expression);
            return label != null ? html.Label(ExpressionHelper.GetExpressionText(expression), label.ToString()) : html.Label(ExpressionHelper.GetExpressionText(expression));
        }

        public static MvcHtmlString LocalizeLookup<TModel>(this HtmlHelper<TModel> html, string lookupKey)
        {
            return new MvcHtmlString(LookupLocalization.ResourceManager.GetString(lookupKey));
        }

        public static MvcHtmlString DisplayLocalizedTextFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var label = Localize(expression);
            return label != null ? new MvcHtmlString(label.ToString()) : html.DisplayTextFor(expression);
        }

        public static object Localize<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
        {
            string resourceKey = GetResourceKey(expression);
            string result = ModelLocalization.ResourceManager.GetString(resourceKey);
            if (result == null)
                result = ModelLocalization.ResourceManager.GetString(Common.ExpressionHelper.GetPropertyName(expression));
            return result;
        }

        private static string GetResourceKey(Expression expression)
        {
            if (expression is LambdaExpression)
            {
                expression = ((LambdaExpression)expression).Body;
            }

            if (expression is MemberExpression)
            {
                var memberExpression = (MemberExpression)expression;
                if (memberExpression.Expression is MemberExpression)
                {
                    var memberExp = (MemberExpression) memberExpression.Expression;
                    if (memberExp.Expression is MemberExpression)
                        throw new NotSupportedException("There is no localization support for property expressions like: m => m.Prop1.Prop2.Prop3");
                    return string.Format("{0}_{1}", memberExp.Expression.Type.Name, memberExp.Member.Name);
                }
                    
                return string.Format("{0}_{1}", memberExpression.Expression.Type.Name, memberExpression.Member.Name);
            }

            throw new InvalidOperationException(string.Format("Unknown expressin type {0}", expression.NodeType));
        }

        public static MvcHtmlString LocalizedEnum(this HtmlHelper htmlHelper, Enum value)
        {
            var label = GetLabel(value);

            return new MvcHtmlString(label);
        }

        public static MvcHtmlString ClientIdFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
        {
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var sanitizedId = TagBuilder.CreateSanitizedId(fullName) ?? fullName;
            return new MvcHtmlString(sanitizedId.Replace(".", HtmlHelper.IdAttributeDotReplacement));
        }

        public static MvcHtmlString CollectionItemClientIdFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
        {
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var sanitizedId = TagBuilder.CreateSanitizedId(fullName) ?? fullName;
            return new MvcHtmlString(
                sanitizedId
                    .Replace(".", HtmlHelper.IdAttributeDotReplacement)
                    .Replace("[", HtmlHelper.IdAttributeDotReplacement)
                    .Replace("]", HtmlHelper.IdAttributeDotReplacement)
                );
        }

        public static string EscapeAsSelector(this string selector)
        {
            return Regex.Replace(selector, @"([ #;&,.+*~\':""!^$[\]()=>|\/@])", "\\\\$1");
        }

        public static string EscapeJavaScriptString(this string selector)
        {
            return Regex.Replace(selector, @"(['""])", "\\$1");
        }

        public static MvcHtmlString ReadonlyTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            
            MvcHtmlString displayFor = html.DisplayFor(expression);
            MvcHtmlString hiddenFor = html.HiddenFor(expression);
            TagBuilder tagBuilder = new TagBuilder("div");
            tagBuilder.InnerHtml = displayFor.ToHtmlString();
            tagBuilder.GenerateId(ClientIdFor(html, expression).ToHtmlString()+"Div");
            return MvcHtmlString.Create(tagBuilder.ToString() + "\n" + hiddenFor.ToHtmlString());
        }

        private static string GetLabel(Enum value)
        {
            string resourceKey = string.Format("{0}_{1}", value.GetType().Name, value);
            var label = EnumLocalization.ResourceManager.GetString(resourceKey) ?? value.ToString();
            return label;
        }

        public static IEnumerable<object> LocalizedEnumValues(this HtmlHelper htmlHelper, Type enumType)
        {
            return Enum.GetValues(enumType).Cast<Enum>().Select(value => new { Key = GetLabel(value), Value = value });
        }

        public static MvcHtmlString ValueIfNull(this HtmlHelper html, object value, string defaultValue)
        {
            if (value == null)
                return new MvcHtmlString(defaultValue);
            return new MvcHtmlString(value.ToString());
        }

        public static MvcHtmlString HidenCollection(this HtmlHelper htmlHelper, params string[] hiddenNames)
        {
            var sb = new StringBuilder();
            foreach (var hiddenName in hiddenNames)
            {
                sb.Append(htmlHelper.Hidden(hiddenName).ToString());
            }
            return MvcHtmlString.Create(sb.ToString());
        }

        public static EditorTableBuilder<TModel> EditorTable<TModel>(this HtmlHelper<TModel> htmlHelper, int? fieldLabelWidth = null, int? fieldEditorWidth = null, bool createErrorDialogs =true)
        {
            return new EditorTableBuilder<TModel>(htmlHelper, fieldLabelWidth, fieldEditorWidth, createErrorDialogs);
        }

        public static UrlHelper ToUrlHelper(this HtmlHelper htmlHelper)
        {
            return new UrlHelper(htmlHelper.ViewContext.RequestContext);
        }

        public static UrlHelper ToUrlHelper<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new UrlHelper(htmlHelper.ViewContext.RequestContext);
        }

        public static MvcHtmlString RequiredFieldLegend<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new MvcHtmlString(string.Format("<div class='requiredFieldLegend'>{0}</div>", TextLocalization.RequiredFieldLegend));
        }

        public static GridFilterBuilder<TModel> ToGridFilterBuilder<TModel>(this HtmlHelper htmlHelper, int? fieldLabelWidth = null, int? fieldEditorWidth = null)
        {
            return new GridFilterBuilder<TModel>(htmlHelper, fieldLabelWidth, fieldEditorWidth);
        }
    }
}