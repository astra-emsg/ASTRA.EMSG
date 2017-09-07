using System;
using ASTRA.EMSG.Business.Models.Common;

namespace ASTRA.EMSG.Business.Models.GIS
{
    [Serializable]
    public class ZustandsabschnittOverviewGISModel : ZustandsabschnittOverviewModelBase
    {
        public bool IsLocked { get; set; }
    }
}