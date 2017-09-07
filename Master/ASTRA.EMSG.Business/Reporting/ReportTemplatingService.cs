using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Models.Administration;
using NHibernate.Util;

namespace ASTRA.EMSG.Business.Reporting
{
    public interface IReportTemplatingService
    {
        void ReplaceHeaderFooter(XmlDocument doc, XmlNamespaceManager xmlNsmgr, PaperType paperType, MandantLogoModel mandantLogoModel, OutputFormat outputFormat);
        void ReplaceTextWrapper(XmlDocument doc, XmlNamespaceManager xmlNsmgr);
        void AdjustChartSizeForColumnCount<T>(XmlDocument doc, XmlNamespaceManager nsmgr, int diagrammColumnCount) where T : IEmsgPoProviderBase;
        void SetChartLeft(XmlDocument doc, XmlNamespaceManager nsmgr, decimal left, string chartName = "Chart");
        void SetChartInnerPlotPostionWidth(XmlDocument doc, XmlNamespaceManager nsmgr, decimal width);
        void SetChartInnerPlotPostionLeft(XmlDocument doc, XmlNamespaceManager nsmgr, decimal left);
        decimal ParseValueWithMeasure(string text);
        void SetChartWidth(XmlDocument doc, XmlNamespaceManager nsmgr, decimal width, string chartName = "Chart");
        decimal GetMatrixTableColumnWidth(XmlDocument doc, XmlNamespaceManager nsmgr);
        decimal GetChartWidth(XmlDocument doc, XmlNamespaceManager nsmgr, string chartName = "Chart");
        decimal GetChartInnerPlotPostionLeftPercent(XmlDocument doc, XmlNamespaceManager nsmgr);
    }

    public class ReportTemplatingService : IReportTemplatingService
    {
        private const string HeaderFooterSubreportRdlcName = "ASTRA.EMSG.Business.Reports.CommonSubReport.HeaderFooter";
        private const string PageHeaderXPath = "//nm:PageHeader";
        private const string PageFooterXPath = "//nm:PageFooter";
        private const string EmbeddedImagesXPath = "//nm:EmbeddedImages";

        private readonly IReportResourceLocator reportResourceLocator;

        public ReportTemplatingService(IReportResourceLocator reportResourceLocator)
        {
            this.reportResourceLocator = reportResourceLocator;
        }

        public virtual void ReplaceTextWrapper(XmlDocument doc, XmlNamespaceManager xmlNsmgr)
        {
            var leftNode = doc.SelectSingleNode("//nm:Code", xmlNsmgr);
            if (leftNode == null)
                return;

            var helper = reportResourceLocator.GetHelperStream("ASTRA.EMSG.Business.Reporting.TextWrapper.helper");
            using (var redaer = new StreamReader(helper))
            {
                var helperText = redaer.ReadToEnd();
                var regex = new Regex(@"'TEMPLATE-TextWrapper(.*?)'TEMPLATE-TextWrapper", RegexOptions.Multiline);
                var replacedText = regex.Replace(leftNode.InnerText.Replace("\r\n", ""), helperText);
                replacedText = replacedText.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
                leftNode.InnerXml = replacedText;
            }
        }

        public virtual void ReplaceHeaderFooter(XmlDocument doc, XmlNamespaceManager xmlNsmgr, PaperType paperType, MandantLogoModel mandantLogoModel, OutputFormat outputFormat)
        {
            XmlNode docPageNode = doc.SelectSingleNode("//nm:Page", xmlNsmgr);
            XmlNode docReportNode = doc.SelectSingleNode("//nm:Report", xmlNsmgr);

            RemoveXmlNode(PageHeaderXPath, doc, docPageNode, xmlNsmgr);
            RemoveXmlNode(PageFooterXPath, doc, docPageNode, xmlNsmgr);
            RemoveXmlNode(EmbeddedImagesXPath, doc, docReportNode, xmlNsmgr);

            Stream templateRdlcXmlStream = reportResourceLocator.GetReportDefinitionStream(string.Format("{0}{1}_{2}.rdlc", HeaderFooterSubreportRdlcName, paperType, outputFormat.ToFileExtension()));

            var templateDoc = new XmlDocument();
            templateDoc.Load(templateRdlcXmlStream);

            var templateDocHeader = templateDoc.SelectSingleNode(PageHeaderXPath, xmlNsmgr);
            PositionMandantLogo(templateDocHeader, xmlNsmgr, mandantLogoModel);

            docPageNode.InnerXml += templateDocHeader.OuterXml;
            docPageNode.InnerXml += templateDoc.SelectSingleNode(PageFooterXPath, xmlNsmgr).OuterXml;
            docReportNode.InnerXml += templateDoc.SelectSingleNode(EmbeddedImagesXPath, xmlNsmgr).OuterXml;

            //PositionMandantLogo(docPageNode, xmlNsmgr, mandantLogoModel);

            var reportParametersNode = doc.SelectSingleNode("//nm:ReportParameters", xmlNsmgr);
            reportParametersNode.InnerXml += "<ReportParameter Name=\"ReportTitle\"><DataType>String</DataType><Prompt>ReportParameter1</Prompt></ReportParameter>";
            reportParametersNode.InnerXml += "<ReportParameter Name=\"FooterText\"><DataType>String</DataType><Nullable>true</Nullable><AllowBlank>true</AllowBlank><Prompt>ReportParameter1</Prompt></ReportParameter>";
            reportParametersNode.InnerXml += "<ReportParameter Name=\"IsExcelOutput\"><DataType>Boolean</DataType><Prompt>ReportParameter1</Prompt></ReportParameter>";
        }

        private void PositionMandantLogo(XmlNode docPageNode, XmlNamespaceManager xmlNsmgr, MandantLogoModel mandantLogoModel)
        {
            var leftNode = docPageNode.SelectSingleNode("//nm:Image[@Name='HeaderMandantImage']/nm:Left", xmlNsmgr);
            var topNode = docPageNode.SelectSingleNode("//nm:Image[@Name='HeaderMandantImage']/nm:Top", xmlNsmgr);
            var widthNode = docPageNode.SelectSingleNode("//nm:Image[@Name='HeaderMandantImage']/nm:Width", xmlNsmgr);
            var heightNode = docPageNode.SelectSingleNode("//nm:Image[@Name='HeaderMandantImage']/nm:Height", xmlNsmgr);


            var width = decimal.Divide(mandantLogoModel.Width, mandantLogoModel.Height)*ParseValueWithMeasure(heightNode.InnerText);
            decimal shiftHorizontal = ParseValueWithMeasure(widthNode.InnerText) - width;

            if (shiftHorizontal < 0)
            {
                var height = decimal.Divide(mandantLogoModel.Height, mandantLogoModel.Width)*
                             ParseValueWithMeasure(widthNode.InnerText);
                decimal shiftVertical = ParseValueWithMeasure(heightNode.InnerText) - height;

                heightNode.InnerText = ToRdlcCm(Math.Round(height, 2));
                topNode.InnerText = ToRdlcCm(Math.Round(ParseValueWithMeasure(topNode.InnerText) + shiftVertical, 2));

            }
            else
            {
                widthNode.InnerText = ToRdlcCm(Math.Round(width, 2));
                leftNode.InnerText = ToRdlcCm(Math.Round(ParseValueWithMeasure(leftNode.InnerText) + shiftHorizontal, 2));
            }
        }

        protected void RemoveXmlNode(string xpath, XmlDocument doc, XmlNode removeTargetNode, XmlNamespaceManager xmlNsmgr)
        {
            var headerNode = doc.SelectSingleNode(xpath, xmlNsmgr);
            if (headerNode != null)
                removeTargetNode.RemoveChild(headerNode);
        }

        public void AdjustChartSizeForColumnCount<T>(XmlDocument doc, XmlNamespaceManager nsmgr, int diagrammColumnCount) where T : IEmsgPoProviderBase
        {
            ReportSizeCollection reportSizeCollection = reportResourceLocator.GetReportSizeCollection<T>();

            var reportSize = reportSizeCollection.ReportSizes.Single(r => r.ColumnCount == diagrammColumnCount);
            
            const decimal marginWidth = 3.1m;

            var chartWidth = (GetMatrixTableColumnWidth(doc, nsmgr) + reportSize.ColumnWidthCorrection) * (diagrammColumnCount + 1);
            var totalWidth = chartWidth + 2 * marginWidth;

            SetChartInnerPlotPostionLeft(doc, nsmgr, 3 / totalWidth * 100);
            SetChartInnerPlotPostionWidth(doc, nsmgr, chartWidth/totalWidth*100);

            SetChartWidth(doc, nsmgr, totalWidth);
            SetChartLeft(doc, nsmgr, reportSize.ChartLeft);
        }

        public void SetChartWidth(XmlDocument doc, XmlNamespaceManager nsmgr, decimal width, string chartName = "Chart")
        {
            var chartWidthXmlNode = doc.DocumentElement.SelectSingleNode(String.Format("//nm:Chart[@Name='{0}']/nm:Width", chartName), nsmgr);
            chartWidthXmlNode.InnerText = ToRdlcCm(width);
        }
        
        public void SetChartLeft(XmlDocument doc, XmlNamespaceManager nsmgr, decimal left, string chartName = "Chart")
        {
            var chartLeftXmlNode = doc.DocumentElement.SelectSingleNode(String.Format("//nm:Chart[@Name='{0}']/nm:Left", chartName), nsmgr);
            chartLeftXmlNode.InnerText = ToRdlcCm(left);
        }

        public void SetChartInnerPlotPostionWidth(XmlDocument doc, XmlNamespaceManager nsmgr, decimal width)
        {
            var chartInnerPlotPostionWidthNode = doc.DocumentElement.SelectNodes(String.Format("//nm:ChartArea/nm:ChartInnerPlotPosition/nm:Width"), nsmgr);
            foreach (XmlNode chartInnerPlotPostionWidth in chartInnerPlotPostionWidthNode)
                chartInnerPlotPostionWidth.InnerText = string.Format("{0}", Math.Round(width, 2).ToString(CultureInfo.InvariantCulture));
        }

        public void SetChartInnerPlotPostionLeft(XmlDocument doc, XmlNamespaceManager nsmgr, decimal left)
        {
            var chartInnerPlotPostionLeftNode = doc.DocumentElement.SelectNodes(String.Format("//nm:ChartArea/nm:ChartInnerPlotPosition/nm:Left"), nsmgr);
            foreach (XmlNode chartInnerPlotPostionLeft in chartInnerPlotPostionLeftNode)
                chartInnerPlotPostionLeft.InnerText = string.Format("{0}", Math.Round(left, 2).ToString(CultureInfo.InvariantCulture));
        }

        public decimal GetChartInnerPlotPostionLeftPercent(XmlDocument doc, XmlNamespaceManager nsmgr)
        {
            var chartInnerPlotPostionLeftNode = doc.DocumentElement.SelectNodes(String.Format("//nm:ChartArea/nm:ChartInnerPlotPosition/nm:Left"), nsmgr);
            return decimal.Parse((chartInnerPlotPostionLeftNode.First() as XmlNode).InnerText, CultureInfo.InvariantCulture);
        }

        public decimal ParseValueWithMeasure(string text)
        {
            var valueWithMeasure = decimal.Parse(text.Substring(0, text.Length - 2), CultureInfo.InvariantCulture);

            if(text.Substring(text.Length - 2, 2).ToLower() == "in")
                return valueWithMeasure * 2.54m;

            return valueWithMeasure;
        }

        public decimal GetMatrixTableColumnWidth(XmlDocument doc, XmlNamespaceManager nsmgr)
        {
            var tableColumnWidthXmlNode = doc.DocumentElement.SelectSingleNode(String.Format("//nm:TablixBody/nm:TablixColumns/nm:TablixColumn/nm:Width"), nsmgr);
            return ParseValueWithMeasure(tableColumnWidthXmlNode.InnerText);
        }

        public decimal GetChartWidth(XmlDocument doc, XmlNamespaceManager nsmgr, string chartName = "Chart")
        {
            var chartWidthXmlNode = doc.DocumentElement.SelectSingleNode(String.Format("//nm:Chart[@Name='{0}']/nm:Width", chartName), nsmgr);
            return ParseValueWithMeasure(chartWidthXmlNode.InnerText);
        }

        private static string ToRdlcCm(decimal width)
        {
            return string.Format("{0}cm", width.ToString(CultureInfo.InvariantCulture));
        }
    }
}
