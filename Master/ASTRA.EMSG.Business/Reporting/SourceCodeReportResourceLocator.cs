using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using ASTRA.EMSG.Business.Infrastructure.Reporting;

namespace ASTRA.EMSG.Business.Reporting
{
    public class SourceCodeReportResourceLocator : IReportResourceLocator
    {
        public Stream GetReportDefinitionStream(string reportDefinitionResourceName)
        {
            var path = GetPathFromresouceName(reportDefinitionResourceName, "rdlc");
            return File.OpenRead(path);
        }

        public Stream GetHelperStream(string helperResourceName)
        {
            var path = GetPathFromresouceName(helperResourceName, "helper");
            return File.OpenRead(path);
        }

        public ReportSizeCollection GetReportSizeCollection<T>() where T : IEmsgPoProviderBase
        {
            var nameSpace = typeof(T).Namespace;
            var typeName = typeof(T).Name;

            var rdlcResourceName = string.Format("{0}.{1}Sizes.xml", nameSpace, typeName.Replace("PoProvider", ""));

            var path = GetPathFromresouceName(rdlcResourceName, "xml");

            var xmlSerializer = new XmlSerializer(typeof(ReportSizeCollection));
            
            using (var fileStream = File.OpenRead(path))
            {
                return (ReportSizeCollection)xmlSerializer.Deserialize(fileStream);
            }
        }
        
        private static string GetPathFromresouceName(string rdlcResourceName, string extension)
        {
            var assemblyPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            string rdlcPath = @"..\..\ASTRA.EMSG.Business\" +
                              rdlcResourceName.Replace("ASTRA.EMSG.Business.", "").Replace('.', '\\').
                                  Replace("\\" + extension, "." + extension);
            var path = Path.GetFullPath(Path.Combine(assemblyPath, rdlcPath));
            return path;
        }
    }
}
