using ASTRA.EMSG.Business.Entities.Katalogs;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class WiederbeschaffungswertKatalogMapping : ClassMapBase<WiederbeschaffungswertKatalog>
    {
        public WiederbeschaffungswertKatalogMapping()
        {
            MapTo(w => w.GesamtflaecheFahrbahn, "FlaeGesFb");
            MapTo(w => w.FlaecheFahrbahn, "FlaecheFb");
            MapTo(w => w.FlaecheTrottoir, "FlaecheTr");

            MapTo(b => b.AlterungsbeiwertI, "AlterungI");
            MapTo(b => b.AlterungsbeiwertII, "AlterungII");

            MapTo(b => b.IsCustomized);

            ReferencesTo(w => w.Belastungskategorie);
            ReferencesTo(w => w.Mandant);
            ReferencesTo(w => w.ErfassungsPeriod);
        }

        protected override string TableName
        {
            get { return "WbbKat"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.EigenschaftenTextkatalog; }
        }
    }
}