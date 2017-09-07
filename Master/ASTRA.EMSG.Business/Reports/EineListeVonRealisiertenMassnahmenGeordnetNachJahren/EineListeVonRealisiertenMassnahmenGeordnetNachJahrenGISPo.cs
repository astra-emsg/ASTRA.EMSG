using System;

namespace ASTRA.EMSG.Business.Reports.EineListeVonRealisiertenMassnahmenGeordnetNachJahren
{
    [Serializable]
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPo : EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPoBase
    {
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }

        public string MassnahmenbeschreibungFahrbahnTyp { get; set; }

        public decimal FlaecheFahrbahn { get; set; }
        public decimal? FlaecheTrottoirLinks { get; set; }
        public decimal? FlaecheTrottoirRechts { get; set; }

        public string BeteiligteSystemeListe { get; set; }

        public decimal? KostenGesamtprojekt { get; set; }
        public decimal? KostenFahrbahn { get; set; }

        public string LeitendeOrganisation { get; set; }
    }
}