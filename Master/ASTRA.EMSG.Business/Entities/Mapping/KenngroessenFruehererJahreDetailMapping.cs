using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class KenngroessenFruehererJahreDetailMapping : ClassMapBase<KenngroessenFruehererJahreDetail>
    {
        public KenngroessenFruehererJahreDetailMapping()
        {
            MapTo(nsd => nsd.MittlererZustand, "MitZst");
            MapTo(nsd => nsd.Fahrbahnlaenge, "FbLaenge");
            MapTo(nsd => nsd.Fahrbahnflaeche, "FbFlaeche");

            ReferencesTo(nsd => nsd.Belastungskategorie);
            ReferencesTo(nsd => nsd.KenngroessenFruehererJahre);
        }

        protected override string TableName
        {
            get { return "KengrFJDet"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}