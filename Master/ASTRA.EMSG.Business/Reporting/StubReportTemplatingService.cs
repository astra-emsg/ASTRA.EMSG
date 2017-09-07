using System.Xml;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Models.Administration;

namespace ASTRA.EMSG.Business.Reporting
{
    public class StubReportTemplatingService : ReportTemplatingService
    {
        private const string PageHeaderXPath = "//nm:PageHeader";
        private const string PageFooterXPath = "//nm:PageFooter";
        private const string EmbeddedImagesXPath = "//nm:EmbeddedImages";

        public StubReportTemplatingService(IReportResourceLocator reportResourceLocator) : base(reportResourceLocator)
        {
        }

        public override void ReplaceHeaderFooter(XmlDocument doc, XmlNamespaceManager xmlNsmgr, PaperType paperType, MandantLogoModel mandantLogoModel, OutputFormat outputFormat)
        {
            XmlNode docPageNode = doc.SelectSingleNode("//nm:Page", xmlNsmgr);
            XmlNode docReportNode = doc.SelectSingleNode("//nm:Report", xmlNsmgr);

            RemoveXmlNode(PageHeaderXPath, doc, docPageNode, xmlNsmgr);
            RemoveXmlNode(PageFooterXPath, doc, docPageNode, xmlNsmgr);
            RemoveXmlNode(EmbeddedImagesXPath, doc, docReportNode, xmlNsmgr);

            var reportParametersNode = doc.SelectSingleNode("//nm:ReportParameters", xmlNsmgr);
            reportParametersNode.InnerXml += "<ReportParameter Name=\"ReportTitle\"><DataType>String</DataType><Prompt>ReportParameter1</Prompt></ReportParameter>";
            reportParametersNode.InnerXml += "<ReportParameter Name=\"FooterText\"><DataType>String</DataType><Nullable>true</Nullable><AllowBlank>true</AllowBlank><Prompt>ReportParameter1</Prompt></ReportParameter>";
            reportParametersNode.InnerXml += "<ReportParameter Name=\"IsExcelOutput\"><DataType>Boolean</DataType><Prompt>ReportParameter1</Prompt></ReportParameter>";
        }
    }
}