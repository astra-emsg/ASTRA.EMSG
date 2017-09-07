using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Summarisch
{
    [Serializable]
    public class RealisierteMassnahmeSummarsichOverviewModel : Model
    {
        public string Projektname { get; set; }
        public string Beschreibung { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public int? KostenFahrbahn { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public int? Fahrbahnflaeche { get; set; }
        
        public Guid? Belastungskategorie { get; set; }
        public string BelastungskategorieTyp { get; set; }
        public string BelastungskategorieBezeichnung { get; set; }
        public EigentuemerTyp Strasseneigentuemer { get; set; }
    }
}