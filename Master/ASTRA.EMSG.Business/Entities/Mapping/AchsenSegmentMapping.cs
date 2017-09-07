using ASTRA.EMSG.Business.Entities.GIS;
using NHibernate.Spatial.Type;
using ASTRA.EMSG.Business.Utils;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class AchsenSegmentMapping : ClassMapBase<AchsenSegment>
    {
        public AchsenSegmentMapping()
        {
            MapTo(s => s.BsId).Index("IDX_SEGMENT_BSID"); ;
            MapTo(s => s.Operation).Default("-1"); 
            MapTo(s => s.Name);
            MapTo(s => s.Sequence).Default("-1");
            MapTo(s => s.ImpNr).Default("-1");

            MapTo(s => s.Shape);
            //MapTo(s => s.Shape4d);
            MapTo(s => s.Version);  // obsolet -> ImpNr
            MapTo(s => s.AchsenId);  // obsolet
            MapTo(s => s.IsInverted);

            ReferencesTo(me => me.Achse).Index("IDX_ACHSSEG_ACHSE");
            ReferencesTo(me => me.ErfassungsPeriod).Index("IDX_ACHSSEG_ERFPERIOD");
            ReferencesTo(me => me.Mandant).Index("IDX_ACHSSEG_MANDANT"); ;
            ReferencesTo(a => a.CopiedFrom);

            HasMany(ar => ar.Sektoren).KeyColumn("SEK_SEK_ACS_NOR_ID").Cascade.All();
            HasMany(ar => ar.AchsenReferenzen).KeyColumn("ACR_ACR_ACS_NOR_ID").Cascade.All();
        }

        protected override string TableName
        {
            get { return "AchsSeg"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.RaumbezugNutzdatentabelle; }
        }
    }
}
