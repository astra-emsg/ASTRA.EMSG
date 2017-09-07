using ASTRA.EMSG.Business.Entities.Katalogs;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class GlobalWiederbeschaffungswertKatalogMapping : ClassMapBase<GlobalWiederbeschaffungswertKatalog>
    {
        public GlobalWiederbeschaffungswertKatalogMapping()
        {
            MapTo(w => w.GesamtflaecheFahrbahn, "FlaeGesFb");
            MapTo(w => w.FlaecheFahrbahn, "FlaecheFb");
            MapTo(w => w.FlaecheTrottoir, "FlaecheTr");

            MapTo(b => b.AlterungsbeiwertI, "AlterungI");
            MapTo(b => b.AlterungsbeiwertII, "AlterungII");

            ReferencesTo(w => w.Belastungskategorie);
        }

        protected override string TableName
        {
            get { return "GWbbKat"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.EigenschaftenTextkatalog; }
        }
    }
}