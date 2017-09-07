using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class SchadendetailMapping : ClassMapBase<Schadendetail>
    {
        public SchadendetailMapping()
        {
            References(s => s.Zustandsabschnitt).Column("SCD_SCD_ZST_ID");
            MapTo(s => s.SchadendetailTyp, "DetailTyp");
            MapTo(s => s.SchadenschwereTyp, "SchwereTyp");
            MapTo(s => s.SchadenausmassTyp, "AusmassTyp");
        }

        protected override string TableName
        {
            get { return "SchadDet"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}
