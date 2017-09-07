using ASTRA.EMSG.Business.Entities.GIS;
using NHibernate.Spatial.Type;
using ASTRA.EMSG.Business.Utils;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    class InspektionsRouteGISMapping : ClassMapBase<InspektionsRouteGIS>
    {
        public InspektionsRouteGISMapping()
        {
            MapTo(ir => ir.Shape);
            MapTo(ir => ir.Bezeichnung);
            MapToLongText(ir => ir.Bemerkungen);
            MapToLongText(ir => ir.Beschreibung);
            MapTo(ir => ir.InInspektionBei, "InInspBei");
            MapTo(ir => ir.InInspektionBis, "InInspBis");
            MapTo(ir => ir.LegendNumber);

            HasMany(ir => ir.CheckOutsGISList).KeyColumn("COG_COG_IRG_NOR_ID").Cascade.AllDeleteOrphan();

            HasMany(ir => ir.InspektionsRtStrAbschnitteList).KeyColumn("IRS_IRS_IRG_NOR_ID").Cascade.AllDeleteOrphan()/*.Inverse()*/;
            HasMany(ir => ir.StatusverlaufList).KeyColumn("IRV_IRV_IRG_NOR_ID").Cascade.All()/*.Inverse()*/;
            //HasManyToMany(ir => ir.StrassenabschnittGISList).ParentKeyColumn("IRS_IRS_IRG_NOR_ID").ChildKeyColumn("IRS_IRS_STG_NOR_ID").Inverse().Table("PPV_INSPSTRABS_MFN");
            
            ReferencesTo(ir => ir.Mandant);
            ReferencesTo(ir => ir.ErfassungsPeriod);
        }

        protected override string TableName
        {
            get { return "InspekRoute"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}
