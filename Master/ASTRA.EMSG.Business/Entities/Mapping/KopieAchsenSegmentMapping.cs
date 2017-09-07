using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.GIS;
using NHibernate.Spatial.Type;
using ASTRA.EMSG.Business.Utils;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    class KopieAchsenSegmentMapping : ClassMapBase<KopieAchsenSegment>
    {
        public KopieAchsenSegmentMapping()
        {
            MapTo(rm => rm.ImpNr);
            MapTo(rm => rm.Name);
            MapTo(rm => rm.Operation);
            MapTo(rm => rm.Sequence);
            MapTo(rm => rm.Shape);
            MapTo(rm => rm.AchsenId).Column("KSG_KSG_KAC_NOR_ID");
        }

        protected override string TableName
        {
            get { return "KopAchsSeg"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.RaumbezugNutzdatentabelle; }
        }
    }


}
