﻿using ASTRA.EMSG.Business.Entities.Mapping;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Entities.Common
{
    [TableShortName("SCD")]
    public class Schadendetail : Entity
    {
        public virtual Zustandsabschnitt Zustandsabschnitt { get; set; }

        public virtual SchadendetailTyp SchadendetailTyp { get; set; }

        public virtual SchadenschwereTyp SchadenschwereTyp { get; set; }
        public virtual SchadenausmassTyp SchadenausmassTyp { get; set; }
    }
}
