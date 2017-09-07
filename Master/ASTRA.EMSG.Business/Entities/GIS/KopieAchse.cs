using System;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("KAC")]
    public class KopieAchse : Entity, IKopie
    {
        public virtual DateTime VersionValidFrom { get; set; }
        public virtual String Name { get; set; }
        public virtual String Owner { get; set; }
        public virtual int Operation { get; set; }
        public virtual int ImpNr { get; set; }
    }
}
