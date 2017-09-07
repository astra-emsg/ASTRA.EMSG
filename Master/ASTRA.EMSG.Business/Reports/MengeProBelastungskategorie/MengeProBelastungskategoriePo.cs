using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Reports.MengeProBelastungskategorie
{
    [Serializable]
    public class MengeProBelastungskategoriePo
    {
        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public int Fahrbahnflaeche { get; set; }
        
        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public int Trottoirflaeche { get; set; }

        public string BelastungskategorieTyp { get; set; }
        public string BelastungskategorieKurzBezeichnung { get; set; }

        public int BelastungskategorieReihenfolge { get; set; }
    }
}