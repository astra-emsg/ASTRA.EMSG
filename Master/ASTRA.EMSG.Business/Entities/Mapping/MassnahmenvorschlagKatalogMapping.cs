using ASTRA.EMSG.Business.Entities.Katalogs;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class MassnahmenvorschlagKatalogMapping : ClassMapBase<MassnahmenvorschlagKatalog>
    {
        public MassnahmenvorschlagKatalogMapping()
        {
            MapTo(m => m.DefaultKosten, "DefKosten");
            MapTo(m => m.IsCustomized);

            ReferencesTo(m => m.Parent);
            ReferencesTo(m => m.Belastungskategorie);
            ReferencesTo(m => m.Mandant);
            ReferencesTo(m => m.ErfassungsPeriod);
        }

        protected override string TableName
        {
            get { return "MassVorsch"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.EigenschaftenTextkatalog; }
        }
    }

    public class MassnahmentypKatalogMapping : ClassMapBase<MassnahmentypKatalog>
    {
        public MassnahmentypKatalogMapping()
        {
            MapTo(m => m.Typ);
            MapTo(m => m.KatalogTyp);
            MapTo(m => m.LegendNumber);
        }

        protected override string TableName
        {
            get { return "Masstyp"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.EigenschaftenTextkatalog; }
        }
    }
}