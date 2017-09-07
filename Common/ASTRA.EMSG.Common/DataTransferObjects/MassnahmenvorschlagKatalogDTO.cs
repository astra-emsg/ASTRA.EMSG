using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Common.DataTransferObjects
{
    [Serializable]
    public class MassnahmenvorschlagKatalogDTO:DataTransferObject
    {
        public string Typ { get; set; }
        public string TypBezeichnung { get; set; }
        public decimal DefaultKosten { get; set; }
        public MassnahmenvorschlagKatalogTyp KatalogTyp { get; set; }
        public Guid? Belastungskategorie { get; set; }
        public Guid? Mandant { get; set; }
    }
}
