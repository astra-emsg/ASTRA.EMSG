using ASTRA.EMSG.Business.Entities.Summarisch;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class NetzSummarischDetailMapping : ClassMapBase<NetzSummarischDetail>
    {
        public NetzSummarischDetailMapping()
        {
            MapTo(nsd => nsd.MittlererZustand, "MitZst");
            MapTo(nsd => nsd.Fahrbahnlaenge, "FbLaenge");
            MapTo(nsd => nsd.Fahrbahnflaeche, "FbFlaeche");

            ReferencesTo(nsd => nsd.Belastungskategorie);
            ReferencesTo(nsd => nsd.NetzSummarisch);
        }

        protected override string TableName
        {
            get { return "NetzSumDet"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}