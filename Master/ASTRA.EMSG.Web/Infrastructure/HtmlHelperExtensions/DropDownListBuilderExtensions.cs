using System.Reflection;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace ASTRA.EMSG.Web.Infrastructure.HtmlHelperExtensions
{
    public static class DropDownListBuilderExtensions
    {
        public static DropDownListBuilder Width(this DropDownListBuilder builder, int widthPixel)
        {
            var widgetBase =
                (WidgetBase)builder.GetType()
                    .GetProperty("Component", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(builder);
            DictionaryExtensions.Merge(widgetBase.HtmlAttributes,
                new {style = string.Format("width: {0}px;", widthPixel)});
            return builder;
        }
    }
}