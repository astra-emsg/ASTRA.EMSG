using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.Summarisch
{
    [Serializable]
    public class NetzSummarischDetailModel : Model
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.ShortDecimalFormat, NullDisplayText = "")]
        public decimal? MittlererZustand { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.ShortDecimalFormat, NullDisplayText = "")]
        public decimal Fahrbahnlaenge { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public int Fahrbahnflaeche { get; set; }
        
        public Guid Belastungskategorie { get; set; }
        public string BelastungskategorieTyp { get; set; }
    }
}