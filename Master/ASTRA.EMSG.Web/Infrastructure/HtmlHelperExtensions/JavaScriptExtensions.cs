using System.Web.Mvc;
using JetBrains.Annotations;
using System.Linq;

namespace ASTRA.EMSG.Web.Infrastructure.HtmlHelperExtensions
{
    public static partial class HtmlHelperExtensions
    {
        public static MvcHtmlString PostAndReplace(this HtmlHelper htmlHelper, [AspMvcAction] string actionName, string targetElementId)
        {
            return PostAndReplace(htmlHelper, actionName, null, targetElementId);
        }

        public static MvcHtmlString PostAndReplace(this HtmlHelper htmlHelper, [AspMvcAction] string actionName, [AspMvcController] string controllerName, string targetElementId, string idName = null)
        {
            var action = htmlHelper.ToUrlHelper().Action(actionName, controllerName);

            string url = string.Format("'{0}'", action);
            if (!string.IsNullOrEmpty(idName))
                url += string.Format(" + '/' + {0}", idName);

            string ajaxPost =
                string.Format(
                    @"
            $.ajax({{
                url: {0},
                type: 'POST',
                success: function (data) {{
                    $('#{1}').empty().append(data);
                }}
            }});"
                    , url, targetElementId);

            return new MvcHtmlString(ajaxPost);
        }

        public static MvcHtmlString PostAndReplaceFunction(this HtmlHelper htmlHelper, string functionName, [AspMvcAction] string actionName, string targetElementId, string[] parameters = null)
        {
            return PostAndReplaceFunction(functionName, targetElementId, parameters, htmlHelper.ToUrlHelper().Action(actionName));
        }

        public static MvcHtmlString PostAndReplaceFunction(this HtmlHelper htmlHelper, string functionName, [AspMvcAction] string actionName, [AspMvcController] string controllerName, string targetElementId, string[] parameters = null)
        {
            return PostAndReplaceFunction(functionName, targetElementId, parameters, htmlHelper.ToUrlHelper().Action(actionName, controllerName));
        }

        private static MvcHtmlString PostAndReplaceFunction(string functionName, string targetElementId, string[] parameters, string action)
        {
            var parameterStrings = parameters ?? new string[0];

            string functionParameters = string.Join(", ", parameterStrings);
            string dataParameters = string.Join(", ", parameterStrings.Select(p => string.Format("{0}: {0}", p)));

            string function = string.Format(
                @"function {0}({1}){{
                        $.ajax({{
                            url: '{2}',
                            type: 'POST',
                            data: {{ {3} }},
                            success: function (data) {{
                                $('#{4}').empty().append(data);
                            }}
                        }});
                    }}"
                , functionName, functionParameters, action, dataParameters, targetElementId);

            return new MvcHtmlString(function);
        }
    }
}