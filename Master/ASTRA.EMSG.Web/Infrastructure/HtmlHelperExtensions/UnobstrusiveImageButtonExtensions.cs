using System;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Common.Enums;
using JetBrains.Annotations;
using Resources;

namespace ASTRA.EMSG.Web.Infrastructure.HtmlHelperExtensions
{
    public static partial class HtmlHelperExtensions
    {
        public static MvcHtmlString UnobtrusiveImageButton(this HtmlHelper html, string imageUrl, string text, bool showTextAfterImage = false, string additionalClasses = "", Action<FluentTagBuilder> customizeButtonAnchor = null)
        {
            var anchorBuilder = new FluentTagBuilder("a");
            anchorBuilder.AddCssClass("t-emsg-image-button k-button k-button-icon");

            foreach (var addClass in (additionalClasses ?? string.Empty).Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                anchorBuilder.AddCssClass(addClass);

            anchorBuilder.AddAttribute("title", text);
            anchorBuilder.AddAttribute("tabindex", "0");

            anchorBuilder.AddToInnerHtml(new FluentTagBuilder("img")
                .AddAttribute("alt", text)
                .AddAttribute("src", imageUrl)
                .AddCssClass("k-icon-image")
                .ToString(TagRenderMode.SelfClosing));

            if (showTextAfterImage)
                anchorBuilder.AddToInnerHtml(new FluentTagBuilder("span").AddCssClass("imageButtonText").AddToInnerHtml(text));

            if (customizeButtonAnchor != null)
                customizeButtonAnchor(anchorBuilder);

            return new MvcHtmlString(anchorBuilder.ToString());
        }

        public static MvcHtmlString UnobtrusiveDeleteButton(this HtmlHelper helper, [AspMvcAction] string deleteAction, bool showTextAfterImage = true, string comfirmationMessage = null)
        {
            Action<FluentTagBuilder> customizeButtonAnchor = b => b
                .AddAttribute("data-delete-action", helper.ToUrlHelper().Action(deleteAction))
                .AddAttribute("data-delete-comfirmation-message", comfirmationMessage);
            return UnobtrusiveImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/formDelete.png"), ButtonLocalization.Delete, showTextAfterImage, "emsg-delete-button", customizeButtonAnchor);
        }

        public static MvcHtmlString UnobtrusiveSubmitButton(this HtmlHelper helper, string comfirmationMessage = null)
        {
            return UnobtrusiveSubmitButton(helper, ButtonLocalization.Save, comfirmationMessage);
        }
        
        public static MvcHtmlString UnobtrusiveCommitImportButton(this HtmlHelper helper)
        {
            Action<FluentTagBuilder> customizeButtonAnchor = b => b.AddAttribute("data-commit-url", helper.ToUrlHelper().Action("CommitLastImportResult"));
            return UnobtrusiveImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/Submit.png"), ButtonLocalization.CommitImport, true, "emsg-commit-import-button", customizeButtonAnchor);
        }
        
        public static MvcHtmlString UnobtrusiveCancelImportButton(this HtmlHelper helper)
        {
            return UnobtrusiveImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/Cancel.png"), ButtonLocalization.CancelImport, true, "emsg-cancel-import-button");
        }

        public static MvcHtmlString UnobtrusiveSubmitButton(this HtmlHelper helper, string buttonText, string comfirmationMessage)
        {
            Action<FluentTagBuilder> customizeButtonAnchor = b => b.AddAttribute("data-submit-comfirmation-message", comfirmationMessage);
            return UnobtrusiveImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/Submit.png"), buttonText, true, "emsg-submit-button", customizeButtonAnchor);
        }

        public static MvcHtmlString UnobtrusiveSplitButton<TModel>(this HtmlHelper<TModel> helper, bool showTextAfterImage = false)
        {
            return UnobtrusiveImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/EMSG-Strasse-teilen.png"), ButtonLocalization.Split, showTextAfterImage, "emsg-split-button");
        }

        public static MvcHtmlString UnobtrusiveFilterButton(this HtmlHelper helper, string filterButtonLabel = null)
        {
            return UnobtrusiveImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/find.png"), string.IsNullOrEmpty(filterButtonLabel) ? ButtonLocalization.Filter : filterButtonLabel, true, "emsg-filter-button filterButton");
        }

        public static MvcHtmlString UnobstrusiveEditButton<TModel>(this HtmlHelper<TModel> helper, Guid? id = null)
        {
            return UnobtrusiveImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/edit.png"), ButtonLocalization.Edit, false, "emsg-edit-button", c => c.AddAttribute("data-id", id.ToString()));
        }

        public static MvcHtmlString UnobtrusiveCancelButton(this HtmlHelper helper, string buttonText = null)
        {
            buttonText = buttonText ?? ButtonLocalization.Cancel;
            return UnobtrusiveImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/Cancel.png"), buttonText, true, "cancelButton emsg-cancel-button");
        }

        public static MvcHtmlString UnobtrusiveApplyButton(this HtmlHelper helper, [AspMvcAction] string applyAction, string editFormDiv, string comfirmationMessage = null)
        {
            Action<FluentTagBuilder> customizeButtonAnchor = b => b
                .AddAttribute("data-apply-action", helper.ToUrlHelper().Action(applyAction))
                .AddAttribute("data-apply-comfirmation-message", comfirmationMessage);
            return UnobtrusiveImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/speichern.png"), ButtonLocalization.Apply, true, "emsg-apply-button", customizeButtonAnchor);
        }

        public static MvcHtmlString UnobtrusiveReCalculateButton(this HtmlHelper helper, [AspMvcAction] string recalculateAction)
        {
            Action<FluentTagBuilder> customizeButtonAnchor = b => b.AddAttribute("data-recalculate-action", helper.ToUrlHelper().Action(recalculateAction));
            return UnobtrusiveImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/Refresh.png"), ButtonLocalization.ReCalculate, true, "emsg-recalculate-button", customizeButtonAnchor);
        }

        public static MvcHtmlString UnobstrusiveShadenerfassungsformularExcelReportButton(this HtmlHelper helper, [AspMvcAction] string generateReportAction, [AspMvcAction] string getLastGeneratedReportAction, BelagsTyp belagsTyp)
        {
            return UnobstrusiveShadenerfassungsformularReportButton(helper, generateReportAction, getLastGeneratedReportAction, "~/Content/images/page_excel.png", ButtonLocalization.DownloadReportExcel, OutputFormat.Excel, "emsg-excel-report-button", belagsTyp);
        }

        public static MvcHtmlString UnobstrusiveShadenerfassungsformularPdfReportButton(this HtmlHelper helper, [AspMvcAction] string generateReportAction, [AspMvcAction] string getLastGeneratedReportAction, BelagsTyp belagsTyp)
        {
            return UnobstrusiveShadenerfassungsformularReportButton(helper, generateReportAction, getLastGeneratedReportAction, "~/Content/images/page_white_acrobat.png", ButtonLocalization.DownloadReportPdf, OutputFormat.Pdf, "emsg-pdf-report-button", belagsTyp);
        }

        private static MvcHtmlString UnobstrusiveShadenerfassungsformularReportButton(HtmlHelper helper, string generateReportAction, string getLastGeneratedReportAction, [PathReference] string image, string buttontext, OutputFormat outputFormat, string additionalClasses, BelagsTyp belagsTyp)
        {
            Action<FluentTagBuilder> customizeButtonAnchor = b => b.AddAttribute("data-generate-report-action", helper.ToUrlHelper().Action(generateReportAction))
                                                                 .AddAttribute("data-get-last-generated-report-action", helper.ToUrlHelper().Action(getLastGeneratedReportAction))
                                                                 .AddAttribute("data-belags-typ", belagsTyp.ToString())
                                                                 .AddAttribute("data-output-format", outputFormat.ToString());
            return UnobtrusiveImageButton(helper, helper.ToUrlHelper().Content(image), buttontext, true, additionalClasses, customizeButtonAnchor);
        }

        public static MvcHtmlString UnobstrusiveGisExcelReportButton(this HtmlHelper helper, string additionalClasses = null)
        {
            return UnobstrusiveGisReportButton(helper, "emsg-excel-report-button", ButtonLocalization.DownloadReportExcel, "~/Content/images/page_excel.png", OutputFormat.Excel, additionalClasses);
        }

        public static MvcHtmlString UnobstrusiveGisPdfReportButton(this HtmlHelper helper, string additionalClasses = null)
        {
            return UnobstrusiveGisReportButton(helper, "emsg-pdf-report-button", ButtonLocalization.DownloadReportPdf, "~/Content/images/page_white_acrobat.png", OutputFormat.Pdf, additionalClasses);
        }

        public static MvcHtmlString UnobstrusiveGisPdfPreviewButton(this HtmlHelper helper, string additionalClasses = null)
        {
            return UnobstrusiveGisReportButton(helper, "emsg-pdf-preview-button", ButtonLocalization.PreviewReportPdf, "~/Content/images/page_white_acrobat.png", OutputFormat.Pdf, additionalClasses);
        }
        
        public static MvcHtmlString UnobstrusiveGisMapPdfReportButton(this HtmlHelper helper, string additionalClasses = null)
        {
            Action<FluentTagBuilder> customizeButtonAnchor = b => b.AddAttribute("data-generate-report-action", helper.ToUrlHelper().Action("GenerateMapReport"))
                         .AddAttribute("data-get-last-generated-report-action", helper.ToUrlHelper().Action("GetLastGeneratedMapReport"))
                         .AddAttribute("data-output-format", OutputFormat.Pdf.ToString());

            return UnobtrusiveImageButton(helper, helper.ToUrlHelper().Content("~/Content/images/page_white_acrobat.png"), ButtonLocalization.DownloadReportPdf, true, string.Format("{0} {1}", "emsg-map-pdf-report-button", additionalClasses), customizeButtonAnchor);
        
        }

        private static MvcHtmlString UnobstrusiveGisReportButton(HtmlHelper helper, string buttonClass, string buttonLabel, string buttonImage, OutputFormat outputFormat, string additionalClasses)
        {
            Action<FluentTagBuilder> customizeButtonAnchor = b => b.AddAttribute("data-generate-report-action", helper.ToUrlHelper().Action("GenerateReport"))
                         .AddAttribute("data-get-last-generated-report-action", helper.ToUrlHelper().Action("GetLastGeneratedReport"))
                         .AddAttribute("data-output-format", outputFormat.ToString());

            return UnobtrusiveImageButton(helper, helper.ToUrlHelper().Content(buttonImage), buttonLabel, true, string.Format("{0} {1}", buttonClass, additionalClasses), customizeButtonAnchor);
        }

        public static MvcHtmlString StatusverlaufButton(this HtmlHelper helper, [AspMvcAction] string action, Guid id, string markerClass = "emsg-statusverlauf-button")
        {
            Action<FluentTagBuilder> customizeButtonAnchor = builder =>
            {
                builder.AddAttribute("data-url", helper.ToUrlHelper().Action(action));
                builder.AddAttribute("data-id", id.ToString());
            };

            return ImageAndLabelButton(helper, helper.ToUrlHelper().Content("~/Content/images/folders-stack.png"), ButtonLocalization.Statusverlauf, markerClass, customizeButtonAnchor);
        }

        public static MvcHtmlString ClosePopupButton(this HtmlHelper helper, string markerClass = "emsg-popup-close-button")
        {
            return ImageAndLabelButton(helper, helper.ToUrlHelper().Content("~/Content/images/cancel_on.gif"), ButtonLocalization.Close, markerClass);
        }

        public static MvcHtmlString UnobtrusiveOkCancelApplyPopupFormButtons<TModel>(this HtmlHelper<TModel> helper, [AspMvcAction] string applyAction, string editorDiv, string comfirmationMessage = null)
        {
            return MvcHtmlString.Create(
                string.Format("<div class='popupFormButtons'><div style='display: table-cell;'>{0}</div><div style='display: table-cell;'>{1}</div><div style='display: table-cell;'>{2}</div></div>",
                helper.UnobtrusiveSubmitButton(comfirmationMessage).ToHtmlString(),
                helper.UnobtrusiveApplyButton(applyAction, editorDiv, comfirmationMessage).ToHtmlString(),
                helper.UnobtrusiveCancelButton().ToHtmlString()));
        }

        public static MvcHtmlString UnobtrusiveOkCancelPopupFormButtons<TModel>(this HtmlHelper<TModel> helper)
        {
            return MvcHtmlString.Create(
                string.Format("<div class='popupFormButtons'><div style='display: table-cell;'>{0}</div><div style='display: table-cell;'>{1}</div></div>",
                helper.UnobtrusiveSubmitButton().ToHtmlString(),
                helper.UnobtrusiveCancelButton().ToHtmlString()));
        }

        public static MvcHtmlString UnobtrusiveOkCancelFormButtons<TModel>(this HtmlHelper<TModel> helper, string customLabel = null)
        {
            return MvcHtmlString.Create(
                string.Format("<div class='formButtons'>{0}{1}</div>",
                customLabel == null ? helper.UnobtrusiveSubmitButton().ToHtmlString() : helper.UnobtrusiveSubmitButton(customLabel, null).ToHtmlString(),
                helper.UnobtrusiveCancelButton().ToHtmlString()));
        }

        public static MvcHtmlString UnobtrusiveOkFormButton<TModel>(this HtmlHelper<TModel> helper, string customLabel, string submitComfirmationMessage = null)
        {
            return MvcHtmlString.Create(
                string.Format("<div class='formButtons'>{0}</div>",
                customLabel == null ? helper.UnobtrusiveSubmitButton(submitComfirmationMessage).ToHtmlString() : helper.UnobtrusiveSubmitButton(customLabel, submitComfirmationMessage).ToHtmlString()));
        }
    }
}