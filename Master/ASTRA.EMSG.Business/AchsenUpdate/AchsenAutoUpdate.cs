using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using NHibernate;
using ASTRA.EMSG.Business.Entities;

namespace ASTRA.EMSG.Business.AchsenUpdate
{
    public class AchsenAutoUpdate : AchsenUpdate
    {
        public AchsenAutoUpdate(ISession session, Mandant mandant, ErfassungsPeriod erfassungsPeriod, string owner)
            : base(session,
            mandant,
            erfassungsPeriod,
            owner, 
            GetLastImpNr(session, mandant))
            
        {
        }

        public static int GetLastImpNr(ISession session, Mandant mandant)
        {
            var list = session.QueryOver<AchsenUpdateLog>()
                .Where(o => o.Mandant == mandant)
                .List();

            if (list.Count == 0)
            {
                return 0; // full import required
            }

            return list.Max(o => o.ImpNr);
        }

        public override void Start()
        {
            base.Start();

            if (base.MaxImpNr > base.LastImpNr)
            {
                string statisticsText = base.Statistics + "";
                if (this.ReferenceUpdater != null)
                {
                    statisticsText += this.ReferenceUpdater.Statistics + "";
                }

                AchsenUpdateLog updateLog = new AchsenUpdateLog();
                updateLog.Mandant = base.Mandant;
                updateLog.ErfassungsPeriod = base.ErfassungsPeriod;
                updateLog.ImpNr = base.MaxImpNr;
                updateLog.Statistics = statisticsText;
                updateLog.Timestamp = DateTime.Now;

                updateLog.AchsInserts = base.Statistics.NumAchsen.Inserts;
                updateLog.AchsUpdates = base.Statistics.NumAchsen.Updates;
                updateLog.AchsDeletes = base.Statistics.NumAchsen.Deletes;

                updateLog.SegmInserts = base.Statistics.NumSegment.Inserts;
                updateLog.SegmUpdates = base.Statistics.NumSegment.Updates;
                updateLog.SegmDeletes = base.Statistics.NumSegment.Deletes;

                updateLog.SektInserts = base.Statistics.NumSector.Inserts;
                updateLog.SektUpdates = base.Statistics.NumSector.Updates;
                updateLog.SektDeletes = base.Statistics.NumSector.Deletes;

                if (this.ReferenceUpdater != null)
                {
                    ReferenceUpdaterStatistics refUpdStatistics = this.ReferenceUpdater.Statistics;

                    updateLog.UpdatedReferences= refUpdStatistics.UpdatedReferences;
                    updateLog.DeletedReferences= refUpdStatistics.DeletedReferences;

                    updateLog.UpdatedStrassenabschnitts= refUpdStatistics.UpdatedStrassenabschnitts;
                    updateLog.DeletedStrassenabschnitts= refUpdStatistics.DeletedStrassenabschnitts;

                    updateLog.UpdatedZustandsabschnitts= refUpdStatistics.UpdatedZustandsabschnitts;
                    updateLog.DeletedZustandsabschnitts= refUpdStatistics.DeletedZustandsabschnitts;

                    updateLog.UpdatedKoordinierteMassnahmen= refUpdStatistics.UpdatedKoordinierteMassnahmen;
                    updateLog.DeletedKoordinierteMassnahmen= refUpdStatistics.DeletedKoordinierteMassnahmen;

                    updateLog.UpdatedMassnahmenvorschlagTeilsysteme= refUpdStatistics.UpdatedMassnahmenvorschlagTeilsysteme;
                    updateLog.DeletedMassnahmenvorschlagTeilsysteme = refUpdStatistics.DeletedMassnahmenvorschlagTeilsysteme;
                }

                session.Persist(updateLog);
            }
        }

    }
}
