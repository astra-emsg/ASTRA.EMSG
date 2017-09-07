using System;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Common.DataTransferObjects
{
    [Serializable]
    public class SchadendetailDTO : DataTransferObject
    {
        public Guid ZustandsabschnittId { get; set; }
        public SchadendetailTyp SchadendetailTyp { get; set; }

        public SchadenschwereTyp SchadenschwereTyp { get; set; }
        public SchadenausmassTyp SchadenausmassTyp { get; set; }
    }
}
