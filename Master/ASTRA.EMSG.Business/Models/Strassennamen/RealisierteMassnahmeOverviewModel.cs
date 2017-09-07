using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Strassennamen
{
    [Serializable]
    public class RealisierteMassnahmeOverviewModel : Model
    {
        public string Projektname { get; set; }

        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.ShortDecimalFormat, NullDisplayText = "")]
        public decimal? Laenge { get; set; }

        public decimal? BreiteFahrbahn { get; set; }
        public decimal? BreiteTrottoirLinks { get; set; }
        public decimal? BreiteTrottoirRechts { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheFahrbahn { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheTrottoirRechts { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheTrottoirLinks { get; set; }

        public string Beschreibung { get; set; }

        public decimal? KostenFahrbahn { get; set; }
        public decimal? KostenTrottoirLinks { get; set; }
        public decimal? KostenTrottoirRechts { get; set; }

        public Guid? RealisierteMassnahmenvorschlagKatalog { get; set; }
        public string RealisierteMassnahmenvorschlagKatalogBezeichnung { get; set; }

        public Guid? Belastungskategorie { get; set; }

        public EigentuemerTyp Strasseneigentuemer { get; set; }
    }
}