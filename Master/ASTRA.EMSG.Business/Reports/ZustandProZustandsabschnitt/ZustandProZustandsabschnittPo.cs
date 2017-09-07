using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.ZustandProZustandsabschnitt
{
    [Serializable]
    public class ZustandProZustandsabschnittPo
    {
        public Guid Id { get; set; }

        public string Strassenname { get; set; }
        public int? Strassenabschnittsnummer { get; set; }
        public string StrasseBezeichnungVon { get; set; }
        public string StrasseBezeichnungBis { get; set; }

        public int? Abschnittsnummer { get; set; }

        public string Bemerkung { get; set; }
        public string BemerkungShort { get; set; }

        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public string ExternalId { get; set; }

        public string BelastungskategorieTyp { get; set; }
        public string BelastungskategorieBezeichnung { get; set; }

        public BelagsTyp Belag { get; set; }
        public string BelagBezeichnung { get; set; }
        
        public decimal? FlaecheFahrbahn { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.ShortDecimalFormat, NullDisplayText = "")]
        public decimal? Laenge { get; set; }
        public decimal? Zustandsindex { get; set; }

        public decimal? FlaceheTrottoirLinks { get; set; }
        public ZustandsindexTyp ZustandsindexTrottoirLinks { get; set; }
        public string ZustandsindexTrottoirLinksBezeichnung { get; set; }

        public decimal? FlaceheTrottoirRechts { get; set; }
        public ZustandsindexTyp ZustandsindexTrottoirRechts { get; set; }
        public string ZustandsindexTrottoirRechtsBezeichnung { get; set; }
        
        public EigentuemerTyp Strasseneigentuemer { get; set; }
        public string StrasseneigentuemerBezeichnung { get; set; }

        public string Ortsbezeichnung { get; set; }
        public DateTime Aufnahmedatum { get; set; }
    }
}
