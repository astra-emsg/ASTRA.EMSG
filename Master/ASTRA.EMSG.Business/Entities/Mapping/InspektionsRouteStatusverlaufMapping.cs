using ASTRA.EMSG.Business.Entities.GIS;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    class InspektionsRouteStatusverlaufMapping : ClassMapBase<InspektionsRouteStatusverlauf>
    {
        public InspektionsRouteStatusverlaufMapping()
        {
            MapTo(ir => ir.Datum);
            MapTo(ir => ir.Status);
            ReferencesTo(za => za.InspektionsRouteGIS);
        }

        protected override string TableName
        {
            get { return "InspekStatu"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}