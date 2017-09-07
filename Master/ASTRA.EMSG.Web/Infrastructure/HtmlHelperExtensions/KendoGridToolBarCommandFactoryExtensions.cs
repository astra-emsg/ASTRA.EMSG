using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using JetBrains.Annotations;
using Resources;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace ASTRA.EMSG.Web.Infrastructure.HtmlHelperExtensions
{
    public static class KendoGridToolBarCommandFactoryExtensions
    {
        public static GridToolBarCustomCommandBuilder<TModel> AddButton<TModel>(this GridToolBarCommandFactory<TModel> commands, string javascriptMethod, string buttonLabel) where TModel : class
        {
            return GridToolbarButton("~/Content/images/neuanlegen.png", buttonLabel, javascriptMethod, commands);
        }
        
        public static GridToolBarCustomCommandBuilder<TModel> ExportInspektionsrouteButton<TModel>(this GridToolBarCommandFactory<TModel> commands, string javascriptMethod) where TModel : class
        {
            return GridToolbarButton("~/Content/images/export.png", ButtonLocalization.ExportInspektionsroute, javascriptMethod, commands);
        }
        
        public static GridToolBarCustomCommandBuilder<TModel> ImportInspektionsrouteButton<TModel>(this GridToolBarCommandFactory<TModel> commands, string javascriptMethod) where TModel : class
        {
            return GridToolbarButton("~/Content/images/import.png", ButtonLocalization.ImportInspektionsroute, javascriptMethod, commands);
        }

        private static GridToolBarCustomCommandBuilder<TModel> GridToolbarButton<TModel>([PathReference] string icon, string lable, string javascriptMethod, GridToolBarCommandFactory<TModel> commands) where TModel : class
        {
            return commands.Custom()
                //.ButtonType(GridButtonType.ImageAndText)
                .Text(lable)
                .HtmlAttributes(
                    new
                        {
                            title = lable,
                            style = string.Format("background: url('{0}'); height: 18px; width: 18px; margin-top: 0px", new UrlHelper(HttpContext.Current.Request.RequestContext).Content(icon))
                        })
                .Url(string.Format("javascript:{0}", javascriptMethod));
        }
    }

    public class EmsgToolbarBuilder
    {
        List<string> buttons = new List<string>();
        public EmsgToolbarBuilder()
        {

        }

        public EmsgToolbarBuilder AddButton(string javascriptMethod, string buttonLabel)
        {
            return BuildStandardButton(javascriptMethod, buttonLabel, "~/Content/images/neuanlegen.png");
        }

        private EmsgToolbarBuilder BuildStandardButton(string javascriptMethod, string buttonLabel, string icon)
        {
            return CustomButton(buttonLabel, "javascript:" + javascriptMethod,
                string.Format("background: url('{0}'); height: 18px; width: 18px; margin-top: 0px",
                    new UrlHelper(HttpContext.Current.Request.RequestContext).Content(icon)));
        }

        public EmsgToolbarBuilder ExportButton(string javascriptMethod, string buttonLabel)
        {
            return BuildStandardButton(javascriptMethod, buttonLabel, "~/Content/images/export.png");
        }

        public EmsgToolbarBuilder ImportButton(string javascriptMethod, string buttonLabel)
        {
            return BuildStandardButton(javascriptMethod, buttonLabel, "~/Content/images/import.png");
        }

        public EmsgToolbarBuilder CustomButton(string title, string href, string style = null)
        {
            var builder = new TagBuilder("a");
            builder.AddCssClass("k-button k-button-icontext");
            builder.MergeAttribute("href", href);
            builder.MergeAttribute("title", title);
            builder.InnerHtml = string.Format("<span class=\"k-icon\" style=\"{0}\"></span>{1}", style, title);
            buttons.Add(builder.ToString());
            return this;
        }

        public string ToTemplate()
        {
            return String.Join(" ", buttons);
        }
    }
}