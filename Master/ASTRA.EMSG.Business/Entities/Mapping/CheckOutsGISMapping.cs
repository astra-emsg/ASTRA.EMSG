using ASTRA.EMSG.Business.Entities.GIS;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class CheckOutsGISMapping : ClassMapBase<CheckOutsGIS>
    {
        public CheckOutsGISMapping()
        {
            ReferencesTo(co => co.Mandant);
            MapTo(co => co.CheckInDatum);
            MapTo(co => co.CheckOutDatum);
            //Not used 
            //MapTo(co => co.CheckedOutUntil);
            MapTo(co => co.InspectionBy);
            MapTo(co => co.Description);
            MapTo(co => co.Comments);

            ReferencesTo(co => co.InspektionsRouteGIS).Cascade.None();
        }

        protected override string TableName
        {
            get { return "Checkout"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}
