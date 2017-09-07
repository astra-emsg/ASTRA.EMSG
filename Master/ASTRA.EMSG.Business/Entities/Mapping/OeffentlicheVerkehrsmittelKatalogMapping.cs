using ASTRA.EMSG.Business.Entities.Katalogs;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class OeffentlicheVerkehrsmittelKatalogMapping: ClassMapBase<OeffentlicheVerkehrsmittelKatalog>
    {
        public OeffentlicheVerkehrsmittelKatalogMapping()
        {
            MapTo(ns => ns.Typ);
        }

        protected override string TableName
        {
            get { return "OeffVerKat"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.EigenschaftenTextkatalog; }
        }
    }
}
