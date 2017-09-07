using System;
using System.Xml.Serialization;

namespace ASTRA.EMSG.Web.Infrastructure.Help
{
    [Serializable]
    public class FilePath
    {
        [XmlText]
        public string Path { get; set; }

        [XmlAttribute("lang")]
        public string Language { get; set; }
    }
}