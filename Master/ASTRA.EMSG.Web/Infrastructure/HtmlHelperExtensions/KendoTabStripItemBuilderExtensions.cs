using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI.Fluent;

namespace ASTRA.EMSG.Web.Infrastructure.HtmlHelperExtensions
{
    public static class KendoTabStripItemBuilderExtensions
    {
        public static TabStripItemBuilder AddErrorIcon(this TabStripItemBuilder builder, ModelStateDictionary modelStateDictionary, UrlHelper url, string key)
        {
            if (modelStateDictionary.Any(k => k.Key.StartsWith(key) && k.Value.Errors.Count != 0))
                builder.ImageUrl(url.Content("~/Content/images/infoerror.gif"));
            return builder;
        }

    }
}