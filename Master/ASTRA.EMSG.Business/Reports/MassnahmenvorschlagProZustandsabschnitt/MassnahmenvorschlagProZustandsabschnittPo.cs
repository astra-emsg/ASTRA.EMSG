using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.MassnahmenvorschlagProZustandsabschnitt
{
    [Serializable]
    public class MassnahmenvorschlagProZustandsabschnittPo
    {
        public Guid Id { get; set; }

        public int? Strassenabschnittsnummer { get; set; }
        public string Strassenname { get; set; }
        public string Ortsbezeichnung { get; set; }
        public int? Abschnittsnummer { get; set; }
        public string StrasseBezeichnungVon { get; set; }
        public string StrasseBezeichnungBis { get; set; }

        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }

        public string Bemerkung { get; set; }
        public string BemerkungShort { get; set; }

        public EigentuemerTyp Strasseneigentuemer { get; set; }
        public string StrasseneigentuemerBezeichnung { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.ShortDecimalFormat, NullDisplayText = "")]
        public decimal Laenge { get; set; }

        public decimal? Zustandsindex { get; set; }

        public ZustandsindexTyp ZustandsindexTrottoirLinks { get; set; }
        public string ZustandsindexTrottoirLinksBezeichnung { get; set; }

        public ZustandsindexTyp ZustandsindexTrottoirRechts { get; set; }
        public string ZustandsindexTrottoirRechtsBezeichnung { get; set; }

        public decimal KostenFahrbahn { get; set; }
        public decimal KostenTrottoirLinks { get; set; }
        public decimal KostenTrottoirRechts { get; set; }

        public DringlichkeitTyp DringlichkeitFahrbahn { get; set; }
        public string DringlichkeitFahrbahnBezeichnung { get; set; }

        public DringlichkeitTyp DringlichkeitTrottoirLinks { get; set; }
        public string DringlichkeitTrottoirLinksBezeichnung { get; set; }

        public DringlichkeitTyp DringlichkeitTrottoirRechts { get; set; }
        public string DringlichkeitTrottoirRechtsBezeichnung { get; set; }

        public string MassnahmenvorschlagKatalogTypFahrbahn { get; set; }
        public string MassnahmenvorschlagKatalogTypFahrbahnBezeichnung { get; set; }

        public string MassnahmenvorschlagKatalogTypTrottoirLinks { get; set; }
        public string MassnahmenvorschlagKatalogTypTrottoirLinksBezeichnung { get; set; }

        public string MassnahmenvorschlagKatalogTypTrottoirRechts { get; set; }
        public string MassnahmenvorschlagKatalogTypTrottoirRechtsBezeichnung { get; set; }
    }
}
