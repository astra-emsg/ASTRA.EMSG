using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.GIS
{
    [Serializable]
    public class RealisierteMassnahmeGISOverviewModel : Model
    {
        public string Projektname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }

        public string Beschreibung { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheFahrbahn { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheTrottoirRechts { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheTrottoirLinks { get; set; }


        public decimal? KostenFahrbahn { get; set; }
        public decimal? KostenTrottoirLinks { get; set; }
        public decimal? KostenTrottoirRechts { get; set; }
    }
}