using ASTRA.EMSG.Business.Entities.GIS;
using NHibernate.Spatial.Type;
using ASTRA.EMSG.Business.Utils;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class MassnahmenvorschlagTeilsystemeGISMapping : ClassMapBase<MassnahmenvorschlagTeilsystemeGIS>
    {
        public MassnahmenvorschlagTeilsystemeGISMapping()
        {
            MapTo(e => e.Projektname);
            MapTo(s => s.BezeichnungVon, "BezVon");
            MapTo(s => s.BezeichnungBis, "BezBis");
            MapToLongText(e => e.Beschreibung);
            MapTo(e => e.ZustaendigeOrganisation, "ZustaendOrg");
            MapTo(e => e.Teilsystem);
            MapTo(e => e.Dringlichkeit);
            MapTo(e => e.Status);
            MapTo(e => e.Kosten);
            MapTo(e => e.Laenge);

            MapTo(e => e.Shape);

            ReferencesTo(e => e.ReferenzGruppe).Cascade.All();
            ReferencesTo(e => e.Mandant);
        }

        protected override string TableName
        {
            get { return "MassTeilGis"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}
