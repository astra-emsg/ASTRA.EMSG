using ASTRA.EMSG.Business.Entities.GIS;
using NHibernate.Spatial.Type;
using ASTRA.EMSG.Business.Utils;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class RealisierteMassnahmenGISMapping : ClassMapBase<RealisierteMassnahmeGIS>
    {
        public RealisierteMassnahmenGISMapping()
        {
            MapTo(e => e.Projektname);
            MapTo(s => s.BezeichnungVon, "BezVon");
            MapTo(s => s.BezeichnungBis, "BezBis");
            MapTo(e => e.Laenge);
            MapTo(e => e.BreiteFahrbahn, "BreiteFb");
            MapTo(s => s.BreiteTrottoirLinks, "BreiteTrL");
            MapTo(s => s.BreiteTrottoirRechts, "BreiteTrR");

            ReferencesTo(za => za.MassnahmenvorschlagFahrbahn);
            ReferencesTo(za => za.MassnahmenvorschlagTrottoir, "Tr");
            ReferencesTo(za => za.Belastungskategorie);

            HasMany(e => e.BeteiligteSysteme).Cascade.All().Table("ADD_BETSYSRM_MSG").KeyColumn("BST_BST_RMG_NOR_ID").KeyNullable().Element("BST_TST_VL").AsBag();

            MapTo(e => e.KostenGesamtprojekt, "KostGesamt");
            MapTo(e => e.KostenFahrbahn, "KostenFb");
            MapTo(e => e.KostenTrottoirLinks, "KostenTrL");
            MapTo(e => e.KostenTrottoirRechts, "KostenTrR");
            MapTo(za => za.Strasseneigentuemer, "Eigentuemer");

            MapToLongText(e => e.Beschreibung);
            MapTo(e => e.LeitendeOrganisation);

            MapTo(e => e.Shape);
            ReferencesTo(e => e.ReferenzGruppe).Cascade.All();
            ReferencesTo(e => e.Mandant);
            ReferencesTo(za => za.ErfassungsPeriod);
        }

        protected override string TableName
        {
            get { return "RealMassGis"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}