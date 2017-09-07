using System;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.EineListeVonKoordiniertenMassnahmen
{
    [Serializable]
    public class EineListeVonKoordiniertenMassnahmenPo
    {
        public Guid Id { get; set; }

        public string Projektname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }

        public decimal Laenge { get; set; }

        public decimal FlaecheFahrbahn { get; set; }
        public decimal? FlaecheTrottoirLinks { get; set; }
        public decimal? FlaecheTrottoirRechts { get; set; }
        
        public string BeteiligteSystemeListe { get; set; }

        public decimal? KostenGesamtprojekt { get; set; }
        public decimal? KostenStrasse { get; set; }

        public string Beschreibung { get; set; }
        public DateTime? AusfuehrungsAnfang { get; set; }
        public DateTime? AusfuehrungsEnde { get; set; }

        public string LeitendeOrganisation { get; set; }

        public StatusTyp Status { get; set; }
        public string StatusBezeichnung { get; set; }
    }
}
