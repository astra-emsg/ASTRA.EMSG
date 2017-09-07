using System;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("AUL")]
    public class AchsenUpdateLog : Entity, IErfassungsPeriodDependentEntity
    {
        public virtual int ImpNr { get; set; }
        public virtual Mandant Mandant { get; set; }
        public virtual ErfassungsPeriod ErfassungsPeriod { get; set; }
        public virtual string Statistics { get; set; }
        public virtual DateTime Timestamp { get; set; }

        public virtual int AchsInserts { get; set; }
        public virtual int AchsUpdates { get; set; }
        public virtual int AchsDeletes { get; set; }

        public virtual int SegmInserts { get; set; }
        public virtual int SegmUpdates { get; set; }
        public virtual int SegmDeletes { get; set; }

        public virtual int SektInserts { get; set; }
        public virtual int SektUpdates { get; set; }
        public virtual int SektDeletes { get; set; }

        public virtual int UpdatedReferences { get; set; }
        public virtual int DeletedReferences { get; set; }

        public virtual int UpdatedStrassenabschnitts { get; set; }
        public virtual int DeletedStrassenabschnitts { get; set; }

        public virtual int UpdatedZustandsabschnitts { get; set; }
        public virtual int DeletedZustandsabschnitts { get; set; }

        public virtual int UpdatedKoordinierteMassnahmen { get; set; }
        public virtual int DeletedKoordinierteMassnahmen { get; set; }

        public virtual int UpdatedMassnahmenvorschlagTeilsysteme { get; set; }
        public virtual int DeletedMassnahmenvorschlagTeilsysteme { get; set; }
    }
}
