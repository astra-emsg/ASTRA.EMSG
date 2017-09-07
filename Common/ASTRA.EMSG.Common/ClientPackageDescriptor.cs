using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Common
{
    public class ClientPackageDescriptor
    {
        public ClientPackageDescriptor()
        {
            Inspektionsrouten = new List<XMLKeyValuePair<Guid, string>>();
            FileCount = new List<XMLKeyValuePair<string, int>>();
            CheckOutsGISInspektionsroutenList = new List<XMLKeyValuePair<Guid, Guid>>();
        }
        //public Guid CheckOutGIS { get; set; }
        public string Version { get; set; }
        public string CurrentCulture { get; set; }
        public List<XMLKeyValuePair<Guid, string>> Inspektionsrouten { get; set; }
        public List<XMLKeyValuePair<string, int>> FileCount { get; set; }
        public List<XMLKeyValuePair<Guid, Guid>> CheckOutsGISInspektionsroutenList { get; set; }
    }
}
