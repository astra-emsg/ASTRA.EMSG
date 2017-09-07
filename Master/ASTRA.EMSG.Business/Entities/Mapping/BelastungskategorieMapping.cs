using ASTRA.EMSG.Business.Entities.Katalogs;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class BelastungskategorieMapping : ClassMapBase<Belastungskategorie>
    {
        public BelastungskategorieMapping()
        {
            MapTo(b => b.Typ);
            MapTo(b => b.Reihenfolge);
            MapTo(b => b.DefaultBreiteFahrbahn, "DefBrFb");
            MapTo(b => b.DefaultBreiteTrottoirRechts, "DefBrTrR");
            MapTo(b => b.DefaultBreiteTrottoirLinks, "DefBrTrL");

            HasMany(b => b.AllowedBelagList).Cascade.All().Table("ADD_ALBELI_MSG").KeyColumn("BET_BET_BLK_NOR_ID").Element("BET_BTY_VL").AsBag().Not.LazyLoad();

            MapTo(b => b.ColorCode, "Farbcode");
        }

        protected override string TableName
        {
            get { return "BelKat"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.EigenschaftenTextkatalog; }
        }
    }
}
