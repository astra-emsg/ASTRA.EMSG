using System;
using ASTRA.EMSG.Business.Models.Strassennamen;

namespace ASTRA.EMSG.Business.Models.GIS
{
    [Serializable]
    public class StrassenabschnittOverviewGISModel : StrassenabschnittOverviewModelBase
    {
        public bool BelongsToInspektionsroute { get; set; }
        public string ErfassungsStatusBezeichnung { get; set; }
    }
}