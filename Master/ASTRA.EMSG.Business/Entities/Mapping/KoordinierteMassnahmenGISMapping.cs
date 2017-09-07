using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using NHibernate.Spatial.Type;
using ASTRA.EMSG.Business.Utils;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class KoordinierteMassnahmenGISMapping : ClassMapBase<KoordinierteMassnahmeGIS>
    {
        public KoordinierteMassnahmenGISMapping()
        {
            MapTo(e => e.Projektname);
            MapTo(s => s.BezeichnungVon, "BezVon");
            MapTo(s => s.BezeichnungBis, "BezBis");
            MapTo(e => e.Laenge);
            MapTo(e => e.BreiteFahrbahn, "BreiteFb");
            MapTo(s => s.BreiteTrottoirLinks, "BreiteTrL");
            MapTo(s => s.BreiteTrottoirRechts, "BreiteTrR");

            ReferencesTo(za => za.MassnahmenvorschlagFahrbahn);
            HasMany(e => e.BeteiligteSysteme).Cascade.All().Table("ADD_BETSYS_MSG").KeyColumn("BST_BST_KMG_NOR_ID").Element("BST_TST_VL").AsBag().Not.LazyLoad();

            MapTo(e => e.KostenGesamtprojekt, "KostGesamt");
            MapTo(e => e.KostenFahrbahn, "KostenFb");
            MapTo(e => e.KostenTrottoirLinks, "KostenTrL");
            MapTo(e => e.KostenTrottoirRechts, "KostenTrR");

            MapToLongText(e => e.Beschreibung);
            MapTo(e => e.AusfuehrungsAnfang, "AusfAnf");
            MapTo(e => e.AusfuehrungsEnde, "AusfEnde");
            MapTo(e => e.LeitendeOrganisation);
            MapTo(e => e.Status);

            MapTo(e => e.Shape);
            ReferencesTo(e => e.ReferenzGruppe).Cascade.All();
            ReferencesTo(e => e.Mandant);
        }

        protected override string TableName
        {
            get { return "KoorMassGis"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}