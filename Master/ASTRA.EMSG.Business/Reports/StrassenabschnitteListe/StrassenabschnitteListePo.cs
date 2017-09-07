using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.StrassenabschnitteListe
{
    [Serializable]
    public class StrassenabschnitteListePo
    {
        public Guid Id { get; set; }

        public string Strassenname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public string ExternalId { get; set; }

        public int? Abschnittsnummer { get; set; }
        public EigentuemerTyp Strasseneigentuemer { get; set; }
        public string Ortsbezeichnung { get; set; }
        public string StrasseneigentuemerBezeichnung { get; set; }
        public string BelastungskategorieTyp { get; set; }
        public string BelastungskategorieBezeichnung { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.ShortDecimalFormat, NullDisplayText = "")]
        public decimal? Laenge { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheFahrbahn { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheTrottoirLinks { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheTrottoirRechts { get; set; }

        public string TrottoirBezeichnung { get; set; }
    }
}