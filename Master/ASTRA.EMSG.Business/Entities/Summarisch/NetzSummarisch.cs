using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.Summarisch
{
    [TableShortName("NSU")]
    public class NetzSummarisch : Entity, IMandantDependentEntity, IErfassungsPeriodDependentEntity
    {
        public NetzSummarisch()
        {
            NetzSummarischDetails = new List<NetzSummarischDetail>();
        }

        public virtual IList<NetzSummarischDetail> NetzSummarischDetails { get; set; }

        public virtual DateTime? MittleresErhebungsJahr { get; set; }
        public virtual Mandant Mandant { get; set; }
        public virtual ErfassungsPeriod ErfassungsPeriod { get; set; }
    }
}