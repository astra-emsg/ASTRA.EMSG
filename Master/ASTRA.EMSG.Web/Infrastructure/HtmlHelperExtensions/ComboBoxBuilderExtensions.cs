using Kendo.Mvc.UI.Fluent;

namespace ASTRA.EMSG.Web.Infrastructure.HtmlHelperExtensions
{
    public static class ComboBoxBuilderExtensions
    {
        public static ComboBoxBuilder Width(this ComboBoxBuilder builder, int widthPixel)
        {
            return builder.HtmlAttributes(new { style = string.Format("width: {0}px", widthPixel) });
        }
    }
}