using System;
using ASTRA.EMSG.Business.Entities.GIS;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    class InspektionsRtStrAbschnitteMapping : ClassMapBase<InspektionsRtStrAbschnitte>
    {
        public InspektionsRtStrAbschnitteMapping()
        {
            ReferencesTo(sa => sa.StrassenabschnittGIS);
            ReferencesTo(ir => ir.InspektionsRouteGIS);
            MapTo(r => r.Reihenfolge);
                
        }

        protected override string TableName
        {
            get { return "InspekStra"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}
