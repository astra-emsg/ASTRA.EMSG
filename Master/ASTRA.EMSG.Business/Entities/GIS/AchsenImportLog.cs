using System;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.GIS
{

    [TableShortName("AIL")]
    public class AchsenImportLog : Entity
    {
        public const int FAILED = 0;
        public const int INPROGRESS = 1;
        public const int SUCCESS = 2;

        public virtual int ImpNr { get; set; }
        public virtual string Path { get; set; }
        public virtual int Progress { get; set; }
        public virtual DateTime Timestamp { get; set; }
        public virtual DateTime SenderTimestamp { get; set; }

        public virtual int AchsInserts { get; set; }
        public virtual int AchsUpdates { get; set; }
        public virtual int AchsDeletes { get; set; }

        public virtual int SegmInserts { get; set; }
        public virtual int SegmUpdates { get; set; }
        public virtual int SegmDeletes { get; set; }

        public virtual int SektInserts { get; set; }
        public virtual int SektUpdates { get; set; }
        public virtual int SektDeletes { get; set; }

    }
}
