using System;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.Common
{
    [Serializable]
    public class KenngroessenFruehererJahreDetailModel : Model
    {
        public decimal? MittlererZustand { get; set; }

        public decimal? Fahrbahnlaenge { get; set; }
        public int? Fahrbahnflaeche { get; set; }
        
        public Guid Belastungskategorie { get; set; }
        public string BelastungskategorieTyp { get; set; }
        public string BelastungskategorieBezeichnung { get; set; }
    }
}