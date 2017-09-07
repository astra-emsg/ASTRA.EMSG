using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Common
{
    [Serializable]
    public abstract class StrassenabschnittModelBase : Model
    {
        public string Strassenname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public string ExternalId { get; set; }
        
        public Guid? Belastungskategorie { get; set; }
        public string BelastungskategorieTyp { get; set; }
        public BelagsTyp Belag { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.ShortDecimalFormat, NullDisplayText = "")]
        public decimal? Laenge { get; set; }

        public decimal? BreiteFahrbahn { get; set; }
        public TrottoirTyp Trottoir { get; set; }
        public decimal? BreiteTrottoirLinks { get; set; }
        public decimal? BreiteTrottoirRechts { get; set; }

        public bool ShouldCheckBelagChange { get; set; }

        public EigentuemerTyp Strasseneigentuemer { get; set; }
        public string Ortsbezeichnung { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? GesamtFlaeche { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheFahrbahn { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheTrottoir { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheTrottoirLinks { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheTrottoirRechts { get; set; }
    }
}
