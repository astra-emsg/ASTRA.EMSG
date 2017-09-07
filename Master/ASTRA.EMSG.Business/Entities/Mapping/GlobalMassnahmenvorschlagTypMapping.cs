using ASTRA.EMSG.Business.Entities.Katalogs;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class GlobalMassnahmenvorschlagTypMapping : ClassMapBase<GlobalMassnahmenvorschlagKatalog>
    {
        public GlobalMassnahmenvorschlagTypMapping()
        {

            MapTo(m => m.DefaultKosten, "DefKosten");
            ReferencesTo(m => m.Belastungskategorie);
            ReferencesTo(m => m.Parent);
        }

        protected override string TableName
        {
            get { return "GMassVor"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.EigenschaftenTextkatalog; }
        }
    }
}