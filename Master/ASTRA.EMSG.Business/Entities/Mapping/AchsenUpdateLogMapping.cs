using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.GIS;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class AchsenUpdateLogMapping : ClassMapBase<AchsenUpdateLog>
    {
        public AchsenUpdateLogMapping()
        {
            MapTo(ar => ar.ImpNr).UniqueKey("UK_AUL_IMPNRMAN");
            ReferencesTo(ar => ar.Mandant).UniqueKey("UK_AUL_IMPNRMAN");
            ReferencesTo(ar => ar.ErfassungsPeriod);
            MapTo(ar => ar.Statistics);
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

            MapTo(ar => ar.UpdatedReferences);
            MapTo(ar => ar.DeletedReferences);

            MapTo(ar => ar.UpdatedStrassenabschnitts);
            MapTo(ar => ar.DeletedStrassenabschnitts);

            MapTo(ar => ar.UpdatedZustandsabschnitts);
            MapTo(ar => ar.DeletedZustandsabschnitts);

            MapTo(ar => ar.UpdatedKoordinierteMassnahmen);
            MapTo(ar => ar.DeletedKoordinierteMassnahmen);

            MapTo(ar => ar.UpdatedMassnahmenvorschlagTeilsysteme);
            MapTo(ar => ar.DeletedMassnahmenvorschlagTeilsysteme);
        }

        protected override string TableName
        {
            get { return "AchsUpdLog"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}
