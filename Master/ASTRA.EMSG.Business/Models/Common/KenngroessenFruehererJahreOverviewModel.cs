using System;
using System.Collections.Generic;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.Common
{
    [Serializable]
    public class KenngroessenFruehererJahreOverviewModel : Model
    {
        public int Jahr { get; set; }
        public decimal KostenFuerWerterhaltung { get; set; }

        public List<KenngroessenFruehererJahreDetailOverviewModel> KenngroesseFruehereJahrDetailOverviewModels { get; set; }
    }
}