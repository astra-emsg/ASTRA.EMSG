using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.GIS;

namespace ASTRA.EMSG.Business.AchsenUpdate
{
    public interface IReferenceUpdater
    {
        void BeforeDeleteAchse(Achse targetAchse);

        void BeforeUpdateAchse(Achse targetAchse, KopieAchse kopieAchse);

        void BeforeDeleteSegment(AchsenSegment segment);

        void BeforeUpdateSegment(AchsenSegment targetSegment, KopieAchse kopieAchse, KopieAchsenSegment kopieSegment);

        void AfterCreateSegment(AchsenSegment segment);

        void PostWork();

        void CreateValidFromDict(IEnumerable<Achse> achsen);

        ReferenceUpdaterStatistics Statistics { get; }
    }
}
