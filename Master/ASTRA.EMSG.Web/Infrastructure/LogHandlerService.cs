using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.Common.Master.Logging;
using Ionic.Zip;
using NLog;
using System.Linq;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class LogHandlerService : ILogHandlerService
    {
        private readonly string nlogConfig;
        private const string MinLevelAttributeName = "minlevel";
        private const string LoggerName = "ApplicationLogger";

        private readonly IServerConfigurationProvider serverConfigurationProvider;

        public LogHandlerService(
            IServerConfigurationProvider serverConfigurationProvider,
            IServerPathProvider serverPathProvider)
        {
            this.serverConfigurationProvider = serverConfigurationProvider;
            nlogConfig = serverPathProvider.MapPath(@"~/NLog.config");
        }

        public void SetLogLevel(string logLevel)
        {
            var doc = new XmlDocument();
            var xmlNodeList = GetXmlNodeList(doc);

            foreach (XmlNode node in xmlNodeList)
                node.Attributes[MinLevelAttributeName].InnerText = logLevel;

            doc.Save(nlogConfig);
        }

        public string GetLogLevel()
        {
            return GetXmlNodeList(new XmlDocument())[0].Attributes[MinLevelAttributeName].InnerText;
        }

        public List<string> GetAllLogLevel()
        {
            return new List<LogLevel>{LogLevel.Trace, LogLevel.Debug, LogLevel.Info, LogLevel.Warn, LogLevel.Error, LogLevel.Fatal}.Select(ll => ll.Name).ToList();
        }

        public bool IsLogLevelValid(string logLevel)
        {
            try
            {
                LogLevel.FromString(logLevel);
            }
            catch (Exception ex)
            {
                Loggers.ApplicationLogger.Error(ex);
                return false;
            }

            return true;
        }

        private XmlNodeList GetXmlNodeList(XmlDocument doc)
        {
            doc.Load(nlogConfig);

            // Create an XmlNamespaceManager to resolve the default namespace.
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("nl", "http://www.nlog-project.org/schemas/NLog.xsd");
            nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            return doc.DocumentElement.SelectNodes(string.Format("//nl:logger[@name='{0}']", LoggerName), nsmgr);
        }

        public Stream DownloadApplicationLog()
        {
            var memoryStream = new MemoryStream();
            using (ZipFile zipFile = new ZipFile())
            {
                foreach (string logFile in Directory.GetFiles(serverConfigurationProvider.LogFolderPath, "*.log", SearchOption.AllDirectories))
                {
                    using (var fs = new FileStream(logFile, FileMode.Open))
                    {
                        zipFile.AddEntry(Path.GetFileName(logFile), ReadFully(fs));
                    }
                }

                zipFile.Save(memoryStream);
            }

            memoryStream.Seek(0, 0);
            return memoryStream;
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    }
}
