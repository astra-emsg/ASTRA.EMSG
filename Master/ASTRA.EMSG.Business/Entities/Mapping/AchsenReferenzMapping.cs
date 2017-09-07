using ASTRA.EMSG.Business.Entities.GIS;
using NHibernate.Spatial.Type;
using ASTRA.EMSG.Business.Utils;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class AchsenReferenzMapping : ClassMapBase<AchsenReferenz>
    {
        public AchsenReferenzMapping()
        {
            MapTo(ar => ar.Strassenname);
            MapTo(ar => ar.Version);
            MapTo(ar => ar.VonRBBS);
            MapTo(ar => ar.NachRBBS);
            MapTo(ar => ar.Shape);

            ReferencesTo(ar => ar.CopiedFrom);
            ReferencesTo(a => a.AchsenSegment).Cascade.None();
            ReferencesTo(rg => rg.ReferenzGruppe);
        }

        protected override string TableName
        {
            get { return "AchsRef"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}
