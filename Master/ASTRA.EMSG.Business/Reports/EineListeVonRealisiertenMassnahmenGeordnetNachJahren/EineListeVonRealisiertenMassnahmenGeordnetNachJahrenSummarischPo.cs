using System;

namespace ASTRA.EMSG.Business.Reports.EineListeVonRealisiertenMassnahmenGeordnetNachJahren
{
    [Serializable]
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPo : EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPoBase
    {
        public decimal? KostenFahrbahn { get; set; }

        public decimal FlaecheFahrbahn { get; set; }

        public string BelastungskategorieTyp { get; set; }
    }
}