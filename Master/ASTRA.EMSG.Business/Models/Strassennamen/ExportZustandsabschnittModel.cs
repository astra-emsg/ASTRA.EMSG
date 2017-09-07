using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Strassennamen
{
    [Serializable]
    public class ExportZustandsabschnittModel
    {
        public string Strassenname { get; set; }
        public string StrassennameBezeichnungVon { get; set; }
        public string StrassennameBezeichnungBis { get; set; }
        public string ExternalId { get; set; }
        public int? Abschnittsnummer { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public decimal? Laenge { get; set; }
        public decimal Zustandsindex { get; set; }
        public ZustandsindexTyp ZustandsindexTrottoirLinks { get; set; }
        public ZustandsindexTyp ZustandsindexTrottoirRechts { get; set; }
        public DateTime? Aufnahmedatum { get; set; }
        public string Aufnahmeteam { get; set; }
        public WetterTyp Wetter { get; set; }
        public string Bemerkung { get; set; }
        public string MassnahmenvorschlagKatalogTypFahrbahn { get; set; }
        public DringlichkeitTyp DringlichkeitFahrbahn { get; set; }
        public string MassnahmenvorschlagKatalogTypTrottoirLinks { get; set; }
        public DringlichkeitTyp DringlichkeitTrottoirLinks { get; set; }
        public string MassnahmenvorschlagKatalogTypTrottoirRechts { get; set; }
        public DringlichkeitTyp DringlichkeitTrottoirRechts { get; set; }
    }
}
