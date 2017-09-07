using Kendo.Mvc.UI.Fluent;

namespace ASTRA.EMSG.Web.Infrastructure.HtmlHelperExtensions
{
    public static class KendoGridActionCommandBuilderExtensions
    {
        public static GridCustomActionCommandBuilder<TModel> ImageUrl<TModel>(this GridCustomActionCommandBuilder<TModel> builder, string imageUrl) where TModel : class
        {
            return builder.HtmlAttributes(new { style = GetImageBackgoundStyle(imageUrl) });
        }

        private static string GetImageBackgoundStyle(string imageUrl)
        {
            return "background-size: 100% auto; background-position: 0 0; background-image: url('" + imageUrl + "');";
        }
    }
}