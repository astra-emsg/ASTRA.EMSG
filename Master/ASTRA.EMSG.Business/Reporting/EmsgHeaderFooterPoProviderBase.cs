using System.Xml;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using System.Linq;

namespace ASTRA.EMSG.Business.Reporting
{
    public abstract class EmsgHeaderFooterPoProviderBase<TReportParameter, TReportPo> : EmsgPoProviderBase<TReportParameter, TReportPo>
        where TReportParameter : EmsgReportParameter
        where TReportPo : new()
    {
        public IReportTemplatingService ReportTemplatingService { get; set; }

        protected override void CustomizeReport(XmlDocument doc, XmlNamespaceManager xmlNsmgr)
        {
            base.CustomizeReport(doc, xmlNsmgr);

            var supportedFormats = new[] { OutputFormat.Image, OutputFormat.Excel, OutputFormat.Pdf, OutputFormat.Xml };
            if (!supportedFormats.Contains(OutputFormat))
                return;

            var outputFormat = OutputFormat;

            if (outputFormat == OutputFormat.Image || outputFormat == OutputFormat.Xml)
                outputFormat = OutputFormat.Pdf;

            ReportTemplatingService.ReplaceHeaderFooter(doc, xmlNsmgr, PaperType, MandantLogoModel, outputFormat);
            ReportTemplatingService.ReplaceTextWrapper(doc, xmlNsmgr);
        }

        protected override void SetReportParameters(TReportParameter parameter)
        {
            base.SetReportParameters(parameter);

            SetReportTitle();
            AddReportParameter("FooterText", FooterText);
            AddReportParameter("IsExcelOutput", parameter.OutputFormat == OutputFormat.Excel);
        }

        protected virtual void SetReportTitle()
        {
            var resourceKey = string.Format("{0}ReportTitle", GetReportName(GetType()));
            var localizedTitle = LocalizationService.GetLocalizedTitle(resourceKey);
            AddReportParameter("ReportTitle", string.IsNullOrEmpty(localizedTitle) ? resourceKey : localizedTitle);
        }

        protected virtual string FooterText { get; private set; }

        protected abstract PaperType PaperType { get; }
    }
}