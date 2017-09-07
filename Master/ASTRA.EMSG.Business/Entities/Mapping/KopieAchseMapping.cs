using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.GIS;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    class KopieAchseMapping : ClassMapBase<KopieAchse>
    {
        public KopieAchseMapping()
        {
            MapTo(rm => rm.ImpNr);
            MapTo(rm => rm.Name);
            MapTo(rm => rm.Operation);
            MapTo(rm => rm.Owner);
            MapTo(rm => rm.VersionValidFrom, "ValidFrom");
        }

        protected override string TableName
        {
            get { return "KopAchse"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.RaumbezugNutzdatentabelle; }
        }
    }
}
