using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using ASTRA.EMSG.Business.Reporting;

namespace ASTRA.EMSG.Business.Infrastructure.Reporting
{
    public class EmbeddedResourceReportResourceLocator : IReportResourceLocator
    {
        public Stream GetReportDefinitionStream(string reportDefinitionResourceName)
        {
            Assembly currentAssembly = GetType().Assembly;
            return currentAssembly.GetManifestResourceStream(reportDefinitionResourceName);
        }

        public Stream GetHelperStream(string helperResourceName)
        {
            Assembly currentAssembly = GetType().Assembly;
            return currentAssembly.GetManifestResourceStream(helperResourceName);
        }

        public ReportSizeCollection GetReportSizeCollection<T>() where T : IEmsgPoProviderBase
        {
            var nameSpace = typeof(T).Namespace;
            var typeName = typeof(T).Name;

            var rdlcResourceName = string.Format("{0}.{1}Sizes.xml", nameSpace, typeName.Replace("PoProvider", ""));

            Assembly currentAssembly = GetType().Assembly;
            var xmlSerializer = new XmlSerializer(typeof(ReportSizeCollection));
            using (var stream = currentAssembly.GetManifestResourceStream(rdlcResourceName))
            {
                return (ReportSizeCollection)xmlSerializer.Deserialize(stream);
            }
        }
    }
}