using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProBelastungskategorie
{
    [Serializable]
    public class WiederbeschaffungswertUndWertverlustProBelastungskategoriePo
    {
        public string BelastungskategorieTyp { get; set; }
        public string BelastungskategorieBezeichnung { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? GesamtFlaeche { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheFahrbahn { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheTrottoir { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal Wiederbeschaffungswert { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal AlterungsbeiwertI { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal AlterungsbeiwertII { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal WertlustI { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal WertlustII { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.ShortDecimalFormat, NullDisplayText = "")]
        public decimal? MittlererZustandindex { get; set; }
    }
}
