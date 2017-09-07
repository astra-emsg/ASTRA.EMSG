using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Entities.GIS
{
     [TableShortName("ALK")]
    public class AchsenLock : Entity, IMandantDependentEntity
    {
        public virtual Mandant Mandant { get; set; }
        public virtual bool IsLocked { get; set; }

         //maybe reuse for historization
        public virtual LockingType LockType { get; set; }
        public virtual DateTime LockStart { get; set; }
        public virtual DateTime? LockEnd { get; set; }
    }
}
