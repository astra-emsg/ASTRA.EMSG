using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Common.DataTransferObjects
{
    [Serializable]
    public class SchadengruppeDTO:DataTransferObject
    {
        public Guid ZustandsabschnittId { get; set; }
        
        public SchadenschwereTyp SchadenschwereTyp { get; set; }
        public SchadenausmassTyp SchadenausmassTyp { get; set; }

        public int Gewicht { get; set; }

        public SchadengruppeTyp SchadengruppeTyp { get; set; }
    }
}
