using System;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Common.Utils;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Business.Models.GIS
{
    [Serializable]
    public class ZustandsabschnittGISModel : ZustandsabschnittModelBase, IAbschnittGISModelBase
    {
        public Guid StrassenabschnittGIS { get; set; }
        public Guid ReferenzGruppe { get; set; }

        public bool IsLocked { get; set; }

        public string FeatureGeoJSONString { get; set; }
        [NonSerialized]
        private IGeometry shape;
        public IGeometry Shape { get {return shape ;} set {shape = value;} }
        private ReferenzGruppeModel referenzGruppeModel;
        public ReferenzGruppeModel ReferenzGruppeModel { get { return referenzGruppeModel; } set { ReferenzGruppe = value.Id; referenzGruppeModel = value; } }
        public override Guid StrassenabschnittBaseId { get { return StrassenabschnittGIS; } }
    }
}
