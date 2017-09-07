using ASTRA.EMSG.Business.Entities.Katalogs;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class GemeindeKatalogMapping: ClassMapBase<GemeindeKatalog>
    {
        public GemeindeKatalogMapping()
        {
            MapTo(ns => ns.Typ);
        }

        protected override string TableName
        {
            get { return "GemKat"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.EigenschaftenTextkatalog; }
        }
    }
}