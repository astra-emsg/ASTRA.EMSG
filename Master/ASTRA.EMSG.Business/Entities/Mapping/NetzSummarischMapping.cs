using ASTRA.EMSG.Business.Entities.Summarisch;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class NetzSummarischMapping : ClassMapBase<NetzSummarisch>
    {
        public NetzSummarischMapping()
        {
            MapTo(ns => ns.MittleresErhebungsJahr, "MitErhJahr");

            ReferencesTo(ns => ns.Mandant);
            ReferencesTo(ns => ns.ErfassungsPeriod);

            HasMany(ns => ns.NetzSummarischDetails).KeyColumn("NSD_NSD_NSU_NOR_ID").Cascade.All();
        }

        protected override string TableName
        {
            get { return "NetzSum"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}
