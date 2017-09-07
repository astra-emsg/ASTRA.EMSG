using System;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.Katalogs
{
    [Serializable]
    public class WiederbeschaffungswertKatalogModel : Model
    {
        public decimal GesamtflaecheFahrbahn { get; set; }
        public decimal FlaecheFahrbahn { get; set; }
        public decimal FlaecheTrottoir { get; set; }

        public decimal AlterungsbeiwertI { get; set; }
        public decimal AlterungsbeiwertII { get; set; }

        public string BelastungskategorieTyp { get; set; }
    }
}