using System;
using System.Collections.Generic;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Katalogs
{
    [Serializable]
    public class BelastungskategorieModel : Model
    {
        public string Typ { get; set; }
        public string ColorCode { get; set; }
        public decimal? DefaultBreiteFahrbahn { get; set; }
        public decimal? DefaultBreiteTrottoirRechts { get; set; }
        public decimal? DefaultBreiteTrottoirLinks { get; set; }

        public int Reihenfolge { get; set; }

        public List<BelagsTyp> AllowedBelagList { get; set; }
    }
}