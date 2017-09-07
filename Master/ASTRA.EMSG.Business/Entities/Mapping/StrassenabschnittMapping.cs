using ASTRA.EMSG.Business.Entities.Strassennamen;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class StrassenabschnittMapping : ClassMapBase<Strassenabschnitt>
    {
        public StrassenabschnittMapping()
        {
            MapTo(s => s.Strassenname);
            MapTo(s => s.BezeichnungVon, "BezVon");
            MapTo(s => s.BezeichnungBis, "BezBis");
            MapTo(s => s.ExternalId);
            MapTo(s => s.Belag);
            MapTo(s => s.Laenge);
            MapTo(s => s.BreiteFahrbahn, "BreiteFb");
            MapTo(s => s.Trottoir);
            MapTo(s => s.BreiteTrottoirLinks, "BreiteTrL");
            MapTo(s => s.BreiteTrottoirRechts, "BreiteTrR");
            MapTo(s => s.Strasseneigentuemer, "Eigentuemer");
            MapTo(s => s.Ortsbezeichnung, "Ortsbez");
            MapTo(s => s.Abschnittsnummer, "AbschnittsNr");

            ReferencesTo(s => s.Belastungskategorie);
            ReferencesTo(me => me.Mandant);
            ReferencesTo(me => me.ErfassungsPeriod);

            HasMany(za => za.Zustandsabschnitten).KeyColumn("ZST_ZST_STT_NOR_ID").Cascade.All();
        }

        protected override string TableName
        {
            get { return "StraTAB"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}
