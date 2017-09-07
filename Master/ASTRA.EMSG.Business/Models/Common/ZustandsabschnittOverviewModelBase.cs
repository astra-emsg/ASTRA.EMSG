using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.Common
{
    [Serializable]
    public class ZustandsabschnittOverviewModelBase : Model
    {
        public string Strassenname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public string Ortsbezeichnung { get; set; }

        public int? Abschnittsnummer { get; set; }

        public int? Sreassenabschnittsnummer { get; set; }

        public string StrasseBezeichnungVon { get; set; }

        public string StrasseBezeichnungBis { get; set; }

        public string StrasseOrtsbezeichnung { get; set; }

        public decimal? StrasseLaenge { get; set; }

        public bool HasTrottoir { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.ShortDecimalFormat, NullDisplayText = "")]
        public decimal? Zustandsindex { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.ShortDecimalFormat, NullDisplayText = "")]
        public decimal? Laenge { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal FlaecheFahrbahn { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaceheTrottoirLinks { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaceheTrottoirRechts { get; set; }

        public bool IsAufnahmedatumInAktuellenErfassungsperiod { get; set; }

        public string Bemerkung { get; set; }

        public string BemerkungShort { get; set; }
    }
}