using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.Katalogs;

namespace ASTRA.EMSG.Business.Reports.EineListeVonRealisiertenMassnahmenGeordnetNachJahren
{
    [Serializable]
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPo : EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPoBase
    {
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }

        public string MassnahmenbeschreibungFahrbahnTyp { get; set; }

        public decimal FlaecheFahrbahn { get; set; }
        public decimal? FlaecheTrottoirLinks { get; set; }
        public decimal? FlaecheTrottoirRechts { get; set; }

        public decimal? KostenGesamtprojekt { get; set; }
        public decimal? KostenFahrbahn { get; set; }
    }
}
