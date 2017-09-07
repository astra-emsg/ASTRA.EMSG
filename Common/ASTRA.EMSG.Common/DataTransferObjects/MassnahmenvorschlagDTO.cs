using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Common.DataTransferObjects
{
    [Serializable]
    public class MassnahmenvorschlagDTO:DataTransferObject
    {
        public Guid? Typ { get; set; }
        public DringlichkeitTyp Dringlichkeit { get; set; }
        public decimal? Kosten { get; set; }
    }
}
