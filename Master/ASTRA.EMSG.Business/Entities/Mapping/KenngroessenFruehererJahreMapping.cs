using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class KenngroessenFruehererJahreMapping : ClassMapBase<KenngroessenFruehererJahre>
    {
        public KenngroessenFruehererJahreMapping()
        {
            MapTo(e => e.Jahr);
            MapTo(e => e.KostenFuerWerterhaltung);

            HasMany(e => e.KenngroesseFruehereJahrDetails).KeyColumn("KFD_KFD_KFJ_NOR_ID").Cascade.All(); ;

            ReferencesTo(e => e.Mandant);
        }

        protected override string TableName
        {
            get { return "KengrFJ"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}
