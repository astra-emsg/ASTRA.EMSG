using System;
using System.Collections.Generic;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.Common
{
    [Serializable]
    public class KenngroessenFruehererJahreModel : Model
    {
        public int? Jahr { get; set; }
        public decimal? KostenFuerWerterhaltung { get; set; }

        public List<KenngroessenFruehererJahreDetailModel> KenngroesseFruehereJahrDetailModels { get; set; }
    }
}
