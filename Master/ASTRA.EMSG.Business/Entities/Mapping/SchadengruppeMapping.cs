using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class SchadengruppeMapping : ClassMapBase<Schadengruppe>
    {
        public SchadengruppeMapping()
        {
            References(s => s.Zustandsabschnitt).Column("SCG_SCG_ZST_ID");
            MapTo(s => s.SchadengruppeTyp, "GruppeTyp");
            MapTo(s => s.SchadenschwereTyp, "SchwereTyp");
            MapTo(s => s.SchadenausmassTyp, "AusmassTyp");
        }

        protected override string TableName
        {
            get { return "SchadGruppe"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}
