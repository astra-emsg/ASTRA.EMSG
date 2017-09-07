using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.EMSGBruTile;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Map.Services
{
    public class DataChangedEventArgs: EventArgs
    {
        public DataChangedEventArgs()
        {
            this.MobileLocalization = new Dictionary<string, string>();
        }
        public string AchsenGeoJson { get; set; }
        public string StrabsGeoJson { get; set; }
        public string ZabsGeoJson { get; set; }
        public Guid ActiveInspectionRouteId { get; set; }
        public List<TiledLayerInfo> LayerInfo { get; set; }
        public Dictionary<string, string> MobileLocalization { get; set; }
        public string[] Slds { get; set; }
    }
}
