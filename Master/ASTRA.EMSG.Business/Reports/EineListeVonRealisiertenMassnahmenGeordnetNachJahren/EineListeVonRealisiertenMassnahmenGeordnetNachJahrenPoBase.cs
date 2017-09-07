using System;

namespace ASTRA.EMSG.Business.Reports.EineListeVonRealisiertenMassnahmenGeordnetNachJahren
{
    [Serializable]
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPoBase
    {
        public Guid Id { get; set; }

        public string Projektname { get; set; }

        public string Beschreibung { get; set; }

        public DateTime? AusfuehrungsEnde { get; set; }
    }
}