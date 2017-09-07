using System;
using ASTRA.EMSG.Business.Entities.Mapping;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Entities.Common
{
    [TableShortName("EPR")]
    public class ErfassungsPeriod : Entity, IMandantDependentEntity
    {
        public virtual string Name { get; set; }
        public virtual bool IsClosed { get; set; }
        public virtual NetzErfassungsmodus NetzErfassungsmodus { get; set; }
        public virtual DateTime Erfassungsjahr { get; set; }

        public virtual Mandant Mandant { get; set; }
    }
}