using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.GIS;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class AchsenImportLogMapping : ClassMapBase<AchsenImportLog>
    {
        public AchsenImportLogMapping()
        {
            MapTo(ar => ar.ImpNr).Unique();
            MapTo(ar => ar.Path);
            MapTo(ar => ar.Progress);
            MapTo(ar => ar.SenderTimestamp);
            MapTo(ar => ar.Timestamp);

            MapTo(ar => ar.AchsInserts);
            MapTo(ar => ar.AchsUpdates);
            MapTo(ar => ar.AchsDeletes);

            MapTo(ar => ar.SegmInserts);
            MapTo(ar => ar.SegmUpdates);
            MapTo(ar => ar.SegmDeletes);

            MapTo(ar => ar.SektInserts);
            MapTo(ar => ar.SektUpdates);
            MapTo(ar => ar.SektDeletes);
        }

        protected override string TableName
        {
            get { return "AchsImpLog"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}
