using System;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Common
{
    [Serializable]
    public class SchadendetailModel : Model
    {
        public Guid ZustandsabschnittId { get; set; }
        public SchadendetailTyp SchadendetailTyp { get; set; }

        public SchadenschwereTyp SchadenschwereTyp { get; set; }
        public SchadenausmassTyp SchadenausmassTyp { get; set; }

        public int Matrix { get { return (int)SchadenschwereTyp * (int)SchadenausmassTyp; } }
    }
}
