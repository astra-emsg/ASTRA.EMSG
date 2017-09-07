using System;

namespace ASTRA.EMSG.Business.Models.Strassennamen
{
    public class ZustandsabschnittImportOverviewModel
    {
        public string Strassenname { get; set; }
        public string StrassennameBezeichnungVon { get; set; }
        public string StrassennameBezeichnungBis { get; set; }

        public int? Abschnittsnummer { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }

        public decimal Zustandsindex { get; set; }

        public DateTime Aufnahmedatum { get; set; }
    }
}