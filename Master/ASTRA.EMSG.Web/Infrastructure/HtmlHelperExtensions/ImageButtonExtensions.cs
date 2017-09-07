using System;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.Logging;
using ASTRA.EMSG.Web.Infrastructure.Help;
using JetBrains.Annotations;
using Resources;

namespace ASTRA.EMSG.Web.Infrastructure.HtmlHelperExtensions
{
    public static partial class HtmlHelperExtensions
    {
        public static MvcHtmlString EditButton(this HtmlHelper html, string tooltipOverride = null, string onClickAction = null)
        {
            tooltipOverride = tooltipOverride ?? ButtonLocalization.Edit;
            return ImageButton(html, new UrlHelper(html.ViewContext.RequestContext).Content("~/Content/images/edit.jpg"),
                               tooltipOverride, onClickAction, additionalClasses: "k-grid-edit");
        }

        public static MvcHtmlString ImageAndLabelButton(this HtmlHelper html, string imageUrl, string text, string additionalClasses = "", Action<FluentTagBuilder> customizeButtonAnchor = null)
        {
            return UnobtrusiveImageButton(html, imageUrl, text, true, additionalClasses, customizeButtonAnchor);
        }

        public static MvcHtmlString ImageButton(this HtmlHelper html, string imageUrl, string text, 
            string onClickAction = null, 
            bool showTextAfterImage = false, 
            string additionalClasses = "",
            string defaultButtonClasses = "t-emsg-image-button k-button k-button-icon",
            string defaultImageClasses = "k-icon-image"
            )
        {
            var builder = new FluentTagBuilder("a");
            builder.AddCssClass(defaultButtonClasses);

            foreach(var addClass in (additionalClasses ?? string.Empty).Split(new [] { " "}, StringSplitOptions.RemoveEmptyEntries))
            {
                builder.AddCssClass(addClass);
            }
            builder.AddAttribute("title", text);
            builder.AddAttribute("tabindex", "0");
            if (!string.IsNullOrEmpty(onClickAction))
                builder.AddAttribute("onclick", onClickAction);
            builder.AddToInnerHtml(new FluentTagBuilder("img")
                .AddAttribute("alt", text)
                .AddAttribute("src", imageUrl)
                .AddCssClass(defaultImageClasses)
                .ToString(TagRenderMode.SelfClosing));

            if (showTextAfterImage)
                builder.AddToInnerHtml(new FluentTagBuilder("span").AddCssClass("imageButtonText").AddToInnerHtml(text));

            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString HelpButtonFor(this HtmlHelper htmlHelper, [AspMvcAction] string action, [AspMvcController] string controller = null, [AspMvcArea] string area = null)
        {
            var routeData = htmlHelper.ViewContext.RouteData;
            return GetHelpButtonFor(htmlHelper, action, area ?? routeData.GetArea(), controller ?? routeData.GetController());
        }

        public static MvcHtmlString HelpButton(this HtmlHelper htmlHelper)
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var area = routeData.GetArea();
            var controller = routeData.GetController();
            var action = routeData.GetAction();
            
            return GetHelpButtonFor(htmlHelper, action, area, controller);
        }

        private static MvcHtmlString GetHelpButtonFor(HtmlHelper htmlHelper, object action, object area, object controller)
        {
            string helpFileUrl;
            var urlHelper = htmlHelper.ToUrlHelper();
            if (action == "Index" && controller == "Home")
            {
                var url  = urlHelper.Action("Index", "HilfeSuche", new {are = ""});
                helpFileUrl = string.Format("window.open('{0}')", url);    
            }
            else
            {
                var url = urlHelper.Action("Hilfe", "HilfeSuche", new { actionName = action, areaName = area, controllerName = controller, area = "" });
                helpFileUrl = string.Format("window.open('{0}&Tick={1}')", url, DateTime.Now.Ticks);    
            }
            return ImageButton(htmlHelper, urlHelper.Content("~/Content/images/help.gif"), ButtonLocalization.Help, onClickAction: helpFileUrl, defaultButtonClasses: "t-emsg-help-button", defaultImageClasses: "t-emsg-help-icon");
        }

        private static string GetHelpFilePath(string cultureCode, object action, object controller, object area)
        {
            string helpFile;
            if (string.IsNullOrEmpty(area as string))
                helpFile = string.Format("~/Help/{0}/{1}_{2}.html", cultureCode, controller, action);
            else
                helpFile = string.Format("~/Help/{0}/{1}/{2}_{3}.html", cultureCode, area, controller, action);
            return helpFile;
        }

        private static string GetHelpFileKey(object action, object controller, object area)
        {
            string helpFileKey;
            if (string.IsNullOrEmpty(area as string))
                helpFileKey = string.Format("{0}_{1}", controller, action);
            else
                helpFileKey = string.Format("{0}_{1}_{2}", area, controller, action);
            return helpFileKey;
        }
        
        public static MvcHtmlString Button(this HtmlHelper helper, string text, string onClickFunction = null, string additionalCssClasses = null)
        {
            var builder = new FluentTagBuilder("a");
            builder.AddCssClass("t-emsg-button k-button");
            builder.AddAttribute("type", "button");
            builder.AddAttribute("title", text);

            foreach (var addClass in (additionalCssClasses ?? string.Empty).Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                builder.AddCssClass(addClass);

            builder.AddToInnerHtml(new FluentTagBuilder("span").AddToInnerHtml(text));

            if (!string.IsNullOrEmpty(onClickFunction))
                builder.AddAttribute("onclick", onClickFunction);

            return new MvcHtmlString(builder.ToString());
        }  
      
        public static MvcHtmlString ActionLinkButton(this HtmlHelper helper, string text, [AspMvcAction] string action)
        {
            var builder = new FluentTagBuilder("a");
            builder.AddCssClass("t-emsg-button k-button");
            builder.AddToInnerHtml(new FluentTagBuilder("span").AddToInnerHtml(text));
            builder.AddAttribute("href", helper.ToUrlHelper().Action(action));

            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString BackToOverviewButton<TModel>(this HtmlHelper<TModel> helper, string onClickFunction = null)
        {
            return Button(helper, ButtonLocalization.BackToOverview, onClickFunction, "backToOverviewButton");
        }
        
        public static MvcHtmlString CustomSubmitButton(this HtmlHelper helper, string text, string action)
        {
            return ImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/Submit.png"), text, action, true);
        }

        public static MvcHtmlString FilterButton(this HtmlHelper helper, string onClickFunction = null, string filterButtonlabel = null, string additionalClasses = null)
        {
            var cssClasses = "filterButton" + (string.IsNullOrEmpty(additionalClasses) ? string.Empty : " " + additionalClasses);
            return ImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/find.png"), string.IsNullOrEmpty(filterButtonlabel) ? ButtonLocalization.Filter : filterButtonlabel, onClickFunction, true, cssClasses);
        }

        public static MvcHtmlString CancelButton(this HtmlHelper helper, string onClickFunction = null)
        {
            return ImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/Cancel.png"), ButtonLocalization.Cancel, onClickFunction, true, "cancelButton");
        }

        public static MvcHtmlString ApplyButton(this HtmlHelper helper, [AspMvcAction] string applyAction, string editFormDiv)
        {
            string apply = string.Format("$.post('{0}', $(this).closest('form').serialize(), function (d) {{ common.destroyErrorDialogs(); $('#{1}').empty().append(d); }});", helper.ToUrlHelper().Action(applyAction), editFormDiv);
            return ImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/speichern.png"), ButtonLocalization.Apply, apply, true);
        }

        public static MvcHtmlString DeleteButton(this HtmlHelper helper, string onClickFunction, bool showTextAfterImage = true)
        {
            return ImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/formDelete.png"), ButtonLocalization.Delete, onClickFunction, showTextAfterImage);
        }

        public static MvcHtmlString EditButton<TModel>(this HtmlHelper<TModel> helper, string onClickFunction = null, string imagePath = null, string additionalClasses = "")
        {
            return ImageButton(helper, helper.ToUrlHelper().Content(imagePath ?? "~/Content/images/edit.png"), ButtonLocalization.Edit, string.Format("javascript:{0}", onClickFunction), false, additionalClasses);
        }
        
        public static MvcHtmlString GridDeleteButton<TModel>(this HtmlHelper<TModel> helper)
        {
            return new MvcHtmlString(string.Format("<a title=\"{0}\" class='k-button k-grid-delete k-button-icon' href='\\\\#'><span class='k-icon k-delete'></span></a>", GridLocalization.Delete));
        }

        public static MvcHtmlString UpButton<TModel>(this HtmlHelper<TModel> helper, string onClickFunction = null)
        {
            return ImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/sort_up.gif"), ButtonLocalization.Up, string.Format("javascript:{0}", onClickFunction));
        }

        public static MvcHtmlString DownButton<TModel>(this HtmlHelper<TModel> helper, string onClickFunction = null)
        {
            return ImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/sort_down.gif"), ButtonLocalization.Down, string.Format("javascript:{0}", onClickFunction));
        }
        
        public static MvcHtmlString StrassenabschnittDetailButton<TModel>(this HtmlHelper<TModel> helper, string onClickFunction = null, string text = null)
        {
            return ImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/EMSG-Lupe.png"), text ?? ButtonLocalization.StrassenabschnittDetail, string.Format("javascript:{0}", onClickFunction));
        }

        public static MvcHtmlString ZustandFahrbahnButton<TModel>(this HtmlHelper<TModel> helper, string onClickFunction = null, bool showTextAfterImage = false)
        {
            return ImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/EMSG-Strasse.png"), ButtonLocalization.Fahrbahn, string.Format("javascript:{0}", onClickFunction), showTextAfterImage, additionalClasses: "t-emsg-nopadding-image-button");
        }

        public static MvcHtmlString ZustandTrottoirButton<TModel>(this HtmlHelper<TModel> helper, string onClickFunction = null, bool showTextAfterImage = false)
        {
            return ImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/EMSG-Trottoir.png"), ButtonLocalization.Trottoir, string.Format("javascript:{0}", onClickFunction), showTextAfterImage);
        }

        public static MvcHtmlString SplitButton<TModel>(this HtmlHelper<TModel> helper, string onClickFunction = null)
        {
            return ImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/EMSG-Strasse-teilen.png"), ButtonLocalization.Split, string.Format("javascript:{0}", onClickFunction));
        }

        public static MvcHtmlString ExcelReportButton(this HtmlHelper helper, string onClickFunction = null)
        {
            return ImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/page_excel.png"), ButtonLocalization.DownloadReportExcel, string.Format("javascript:{0}", onClickFunction), true);
        }

        public static MvcHtmlString PdfReportButton(this HtmlHelper helper, string onClickFunction = null)
        {
            return ImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/page_white_acrobat.png"), ButtonLocalization.DownloadReportPdf, string.Format("javascript:{0}", onClickFunction), true);
        }

        public static MvcHtmlString FlagButton(this HtmlHelper helper, EmsgLanguage emsgLanguage, bool isSelected = false)
        {
            //string onClickAction = string.Format("javascript:window.location='{0}'", helper.ToUrlHelper().Action("SetLanguage", "Header", new {emsgLanguage}));
            //return ImageButton(helper, helper.ToUrlHelper().Content(string.Format("~/Content/flags/{0}.png", emsgLanguage)), ButtonLocalization.ResourceManager.GetString(emsgLanguage.ToString()), onClickAction, false, isSelected ? "t-emsg-selected-image-button" : null);

            string onClickAction = string.Format("javascript:window.location='{0}'", helper.ToUrlHelper().Action("SetLanguage", "Header", new { emsgLanguage }));
            return Button(helper, ButtonLocalization.ResourceManager.GetString(emsgLanguage.ToString()), onClickAction, isSelected ? "t-emsg-selected-image-button" : null);
        }
    }
}