using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("ACH")]
    public class Achse : Entity, IMandantDependentEntity, IErfassungsPeriodDependentEntity
    {
        public Achse()
        {
            AchsenSegmente = new List<AchsenSegment>();
        }

        public virtual Guid BsId { get; set; }
        public virtual DateTime VersionValidFrom { get; set; }
        public virtual String Name { get; set; }
        public virtual int Operation { get; set; }
        public virtual int ImpNr { get; set; }

        public virtual Mandant Mandant { get; set; }
        public virtual ErfassungsPeriod ErfassungsPeriod { get; set; }

        public virtual IList<AchsenSegment> AchsenSegmente { get; set; }
        public virtual Achse CopiedFrom { get; set; }
    }
}
