using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.NochNichtInspizierteStrassenabschnitte
{
    [Serializable]
    public class NochNichtInspizierteStrassenabschnittePo
    {
        public Guid Id { get; set; }

        public string Strassenname { get; set; }

        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        
        public EigentuemerTyp Strasseneigentuemer { get; set; }
        public string StrasseneigentuemerBezeichnung { get; set; }
        public string BelastungskategorieTyp { get; set; }
        public string BelastungskategorieBezeichnung { get; set; }
        
        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheFahrbahn { get; set; }
        
        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheTrottoirLinks { get; set; }
        
        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheTrottoirRechts { get; set; }
    }
}
