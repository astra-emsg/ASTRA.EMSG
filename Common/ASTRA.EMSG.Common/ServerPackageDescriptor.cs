using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Common;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using ASTRA.EMSG.Common.Utils;
using ASTRA.EMSG.Common.EMSGBruTile;


namespace ASTRA.EMSG.Common
{
    [Serializable]
    public class ServerPackageDescriptor
    {
        public ServerPackageDescriptor()
        {
            Inspektionsrouten = new List<XMLKeyValuePair<Guid, string>>();
            FileCount = new List<XMLKeyValuePair<string, int>>();
            CheckOutsGISInspektionsroutenList = new List<XMLKeyValuePair<Guid, Guid>>();
            LayerConfig = new List<TiledLayerInfo>();
        }
        public string Version { get; set; }
        public string CurrentCulture { get; set; }
        //public Guid CheckOutGIS { get; set; }
        public ObservableCollection<XMLKeyValuePair<Guid, string>> GetObservableCollection()
        {
            ObservableCollection<XMLKeyValuePair<Guid, string>> collection = new ObservableCollection<XMLKeyValuePair<Guid, string>>();
            var orderedInspektionsrouten = Inspektionsrouten.OrderBy(kvp => kvp.Value);

            orderedInspektionsrouten.ToList().ForEach(kvp => collection.Add(kvp));
          
            return collection;
        }
       
        public List<XMLKeyValuePair<Guid, string>> Inspektionsrouten { get; set; }
        public List<XMLKeyValuePair<Guid, Guid>> CheckOutsGISInspektionsroutenList { get; set; }
        
        
        public List<XMLKeyValuePair<string, int>> FileCount { get; set; }
        public List<TiledLayerInfo> LayerConfig { get; set; }
    }
}
