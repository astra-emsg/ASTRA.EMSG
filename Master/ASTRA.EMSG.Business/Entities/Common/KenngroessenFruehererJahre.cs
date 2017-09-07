using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.Common
{
    [TableShortName("KFJ")]
    public class KenngroessenFruehererJahre : Entity, IMandantDependentEntity
    {
        public KenngroessenFruehererJahre()
        {
            KenngroesseFruehereJahrDetails = new List<KenngroessenFruehererJahreDetail>();
        }

        public virtual int Jahr { get; set; }
        public virtual decimal KostenFuerWerterhaltung { get; set; }

        public virtual IList<KenngroessenFruehererJahreDetail> KenngroesseFruehereJahrDetails { get; set; }

        public virtual Mandant Mandant { get; set; }
    }
}
