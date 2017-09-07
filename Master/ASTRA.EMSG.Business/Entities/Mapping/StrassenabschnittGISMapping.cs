using ASTRA.EMSG.Business.Entities.GIS;
using NHibernate.Spatial.Type;
using ASTRA.EMSG.Business.Utils;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class StrassenabschnittGISMapping : ClassMapBase<StrassenabschnittGIS>
    {
        public StrassenabschnittGISMapping()
        {
            MapTo(s => s.Strassenname);
            MapTo(s => s.BezeichnungVon, "BezVon");
            MapTo(s => s.BezeichnungBis, "BezBis");
            MapTo(s => s.Belag);
            MapTo(s => s.Laenge);
            MapTo(s => s.BreiteFahrbahn, "BreiteFb");
            MapTo(s => s.Trottoir);
            MapTo(s => s.BreiteTrottoirLinks, "BreiteTrL");
            MapTo(s => s.BreiteTrottoirRechts, "BreiteTrR");
            MapTo(s => s.Strasseneigentuemer, "Eigentuemer");
            MapTo(s => s.Ortsbezeichnung, "Ortsbez");
            MapTo(s => s.Abschnittsnummer, "AbschnittsNr");
            MapTo(s => s.Shape);
            MapTo(s => s.IsLocked);

            ReferencesTo(s => s.Belastungskategorie);
            ReferencesTo(me => me.Mandant);
            ReferencesTo(me => me.ErfassungsPeriod);
            ReferencesTo(me => me.ReferenzGruppe).Cascade.All();
            ReferencesTo(me => me.CopiedFrom);

            HasMany(za => za.Zustandsabschnitten).KeyColumn("ZSG_ZSG_STG_NOR_ID").Cascade.All();
            HasMany(s => s.InspektionsRtStrAbschnitte).KeyColumn("IRS_IRS_STG_NOR_ID");

            //HasManyToMany(ir => ir.InspektionsRoute).Cascade.All().Inverse().Table("INSPEKTIONSRTSTRABSCHNITTE");
        }

        protected override string TableName
        {
            get { return "StraGIS"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}
