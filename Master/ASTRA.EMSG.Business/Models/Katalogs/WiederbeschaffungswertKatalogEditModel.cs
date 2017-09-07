using System;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.Katalogs
{
    [Serializable]
    public class WiederbeschaffungswertKatalogEditModel : Model
    {
        public decimal GesamtflaecheFahrbahn { get; set; }
        public decimal FlaecheFahrbahn { get; set; }
        public decimal FlaecheTrottoir { get; set; }

        public decimal AlterungsbeiwertI { get; set; }
        public decimal AlterungsbeiwertII { get; set; }
               
        public Guid? Belastungskategorie { get; set; }
        public string BelastungskategorieBezeichnung { get; set; }
        public int BelastungskategorieReihenfolge { get; set; }
    }          
}