using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ASTRA.EMSG.Web.Infrastructure.Help
{
    [Serializable]
    public class HelpMappingItem
    {
        public string Key { get; set; }
        [XmlElement("FilePath")]
        public List<FilePath> FilePath { get; set; }
    }
}