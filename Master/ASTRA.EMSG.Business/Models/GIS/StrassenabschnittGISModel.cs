using System;
using ASTRA.EMSG.Business.Models.Common;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Business.Models.GIS
{
    [Serializable]
    public class StrassenabschnittGISModel : StrassenabschnittModelBase, IAbschnittGISModelBase
    {
        public string FeatureGeoJSONString { get; set; }
        [NonSerialized]
        private IGeometry shape;
        public IGeometry Shape { get { return shape; } set { shape = value; } }
        public ReferenzGruppeModel ReferenzGruppeModel { get; set; }

        public bool BelongsToInspektionsroute { get; set; }

        public bool IsLocked { get; set; }

        public int? Abschnittsnummer { get; set; }
    }
}