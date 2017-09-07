using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Xml;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Master.Logging;

namespace ASTRA.EMSG.Business.Infrastructure.Reporting
{
    public interface IPoProvider
    {
        string ReportFileName { get; }
        OutputFormat OutputFormat { get; }

        IEnumerable<ReportDataSource> DataSources { get; }

        string ReportDefinitionResourceName { get; }
        Dictionary<string, string> SubReportDefinitionResourceNames { get; }
        void SubReportProcessingEventHandler(object sender, ServerSubReportProcessingEventArgs args);

        List<IReportParameter> ReportParameters { get; }

        void SetBaseParameter(ReportParameter parameter);
        void LoadDataSources(object parameter);
        bool HasData { get; set; }
        Stream OpenReportDefinitionStreams();
        Dictionary<string, Stream> OpenSubReportDefinitionStreams();

        int? MaxImagePreviewPageHeight { get; }
        int? MaxImagePreviewPageWidth { get; }
    }

    public abstract class PoProviderBase<TParameter> : IPoProvider
    {
        private const string RdLocId = "rd:LocID";
        private const string PoProviderPostfix = "PoProvider";

        public ILocalizationService LocalizationService { get; set; }

        public IReportResourceLocator ReportResourceLocator { get; set; }

        protected PoProviderBase()
        {
            ReportParameters = new List<IReportParameter>();
        }

        public OutputFormat OutputFormat { get; private set; }

        public virtual string ReportDefinitionResourceName { get { return GetRdlcResourceName(GetType()); } }

        public Dictionary<string, string> SubReportDefinitionResourceNames
        {
            get
            {
                var reportNames = new Dictionary<string, string>();
                AddSubReportDefinitionResourceNames(reportNames);
                return reportNames;
            }
        }

        protected virtual void AddSubReportDefinitionResourceNames(Dictionary<string, string> subReportDefinitionResourceNames) { }

        public void SubReportProcessingEventHandler(object sender, ServerSubReportProcessingEventArgs args)
        {
            OnSubReportProcessing(sender, args);
        }

        protected virtual void OnSubReportProcessing(object sender, ServerSubReportProcessingEventArgs args){ }

        protected string GetRdlcResourceName(Type poProviderType)
        {
            var nameSpace = poProviderType.Namespace;
            var typeName = poProviderType.Name;

            var rdlcResourceName = string.Format("{0}.{1}.rdlc", nameSpace, GetReportName(poProviderType));

            var rdlcResource = poProviderType.Assembly.GetManifestResourceInfo(rdlcResourceName);
            if (!typeName.EndsWith(PoProviderPostfix) || rdlcResource == null)
                throw new MissingManifestResourceException(string.Format("Resource not found {0}", rdlcResourceName));

            return rdlcResourceName;
        }

        public void SetBaseParameter(ReportParameter parameter)
        {
            OutputFormat = parameter.OutputFormat;
        }

        public void LoadDataSources(object parameter)
        {
            LoadDataSources((TParameter)parameter);
        }

        public bool HasData { get; set; }

        public abstract void LoadDataSources(TParameter parameter);

        public string ReportFileName
        {
            get
            {
                var poProviderType = GetType();
                var typeName = poProviderType.Name;

                if (!typeName.EndsWith(PoProviderPostfix))
                    throw new NotSupportedException(string.Format("Report Model class Name doesn't contains '{1}' postfix. ({0})", typeName, PoProviderPostfix));

                return LocalizationService.GetLocalizedReportText(string.Format("{0}{1}ReportFileName", GetReportName(poProviderType), ReportFileNamePostfix)) ?? GetReportName(poProviderType);
            }
        }

        protected string ReportFileNamePostfix { get; set; }

        protected string GetReportName(Type poProviderType)
        {
            var typeName = poProviderType.Name;
            return typeName.Substring(0, typeName.Length - PoProviderPostfix.Length);
        }

        public abstract IEnumerable<ReportDataSource> DataSources { get; }

        public List<IReportParameter> ReportParameters { get; private set; }

        public Stream OpenReportDefinitionStreams()
        {
            using (var manifestResourceStream = ReportResourceLocator.GetReportDefinitionStream(ReportDefinitionResourceName))
            {
                return CustomizeReport(manifestResourceStream);
            }
        }

        public Dictionary<string, Stream> OpenSubReportDefinitionStreams()
        {
            var subReportDefinitionStreams = new Dictionary<string, Stream>();

            foreach (var subReportKeyValuePair in SubReportDefinitionResourceNames)
            {
                using (var manifestResourceStream = ReportResourceLocator.GetReportDefinitionStream(subReportKeyValuePair.Value))
                {
                    subReportDefinitionStreams.Add(subReportKeyValuePair.Key, CustomizeReport(manifestResourceStream, subReportKeyValuePair.Key));
                }
            }

            return subReportDefinitionStreams;
        }

        private Stream CustomizeReport(Stream rdlcXmlStream, string reportKey = null)
        {
            var doc = new XmlDocument();
            try
            {
                doc.Load(rdlcXmlStream);

                // Create an XmlNamespaceManager to resolve the default namespace.
                var nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("nm", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");
                nsmgr.AddNamespace("rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");

                LocalizeReport(doc, nsmgr);
                if(reportKey == null)
                    CustomizeReport(doc, nsmgr);
                else
                    CustomizeSubReport(doc, nsmgr, reportKey);

                var memoryStream = new MemoryStream();
                doc.Save(memoryStream);
                memoryStream.Seek(0, 0);

                return memoryStream;
            }
            catch (XmlException ex)
            {
                Loggers.ReportingLogger.WarnFormat("Error during CustomizeReport: {0}", ex, ex.Message);
                return rdlcXmlStream;
            }
        }

        protected virtual void CustomizeReport(XmlDocument doc, XmlNamespaceManager nsmgr)
        {
        }

        protected virtual void CustomizeSubReport(XmlDocument doc, XmlNamespaceManager nsmgr, string subReportKey)
        {
        }

        private void LocalizeReport(XmlDocument doc, XmlNamespaceManager nsmgr)
        {
            //Go through the nodes of XML document and localize the text of nodes Value, ToolTip, Label. 
            foreach (string nodeName in new[] {"Value", "ToolTip", "Label"})
            {
                foreach (
                    XmlNode node in doc.DocumentElement.SelectNodes(String.Format("//nm:{0}[@{1}]", nodeName, RdLocId), nsmgr))
                {
                    String nodeValue = node.InnerText;
                    if (String.IsNullOrEmpty(nodeValue) || !nodeValue.StartsWith("="))
                    {
                        var resourceIdentifier = node.Attributes[RdLocId].Value;
                        var resourceIdentifierParts = resourceIdentifier.Split('.');

                        string localizedValue;
                        if (resourceIdentifierParts.Length > 1)
                            localizedValue = LocalizationService.GetLocalizedValue(resourceIdentifierParts[0],
                                                                                   resourceIdentifierParts[1]);
                        else
                            localizedValue = LocalizationService.GetLocalizedText(resourceIdentifierParts[0]);

                        if (!String.IsNullOrEmpty(localizedValue))
                            node.InnerText = localizedValue;
                        else
                            Loggers.ReportingLogger.DebugFormat("REPORT - Localization not found for key: {0}", resourceIdentifier);
                    }
                }
            }
        }

        public virtual int? MaxImagePreviewPageHeight { get { return null; } }
        public virtual int? MaxImagePreviewPageWidth { get { return null; } }
    }
}