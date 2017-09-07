using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Common.DataTransferObjects
{
    [Serializable]
    public class BelastungskategorieDTO:DataTransferObject
    {
        public string Typ { get; set; }
        public string ColorCode { get; set; }
        public decimal? DefaultBreiteFahrbahn { get; set; }
        public decimal? DefaultBreiteTrottoirRechts { get; set; }
        public decimal? DefaultBreiteTrottoirLinks { get; set; }
    }
}
