using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.GIS;
using NHibernate.Spatial.Type;
using ASTRA.EMSG.Business.Utils;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    class KopieSektorMapping : ClassMapBase<KopieSektor>
    {
        public KopieSektorMapping()
        {
            MapTo(rm => rm.ImpNr);
            MapTo(rm => rm.Name);
            MapTo(rm => rm.Operation);
            MapTo(rm => rm.Sequence);
            MapTo(rm => rm.MarkerGeom);
            MapTo(rm => rm.SegmentId).Column("KSK_KSK_KSG_NOR_ID");
            MapTo(rm => rm.Km);
            MapTo(rm => rm.SectorLength, "SektorLen");
        }

        protected override string TableName
        {
            get { return "KopSektor"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.RaumbezugNutzdatentabelle; }
        }
    }


}
