using System;
using System.Collections.Generic;
using ASTRA.EMSG.Common;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Business.Models.GIS
{
    [Serializable]
    public class InspektionsRouteGISModel : Model
    {
        public InspektionsRouteGISModel()
        {
            InspektionsRtStrAbschnitteModelList = new List<InspektionsRtStrAbschnitteModel>();
        }

        public string FeatureGeoJSONString { get; set; }
        public IGeometry Shape { get; set; }
        
        public string Bezeichnung { get; set; }
        public string Beschreibung { get; set; }
        public string Bemerkungen { get; set; }
        public string InInspektionBei { get; set; }
        public DateTime? InInspektionBis { get; set; }
        public bool IsLocked { get; set; }

        public IList<InspektionsRtStrAbschnitteModel> InspektionsRtStrAbschnitteModelList { get; set; }
    }
}
