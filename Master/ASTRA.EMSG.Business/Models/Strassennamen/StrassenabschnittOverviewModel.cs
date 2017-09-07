using System;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Strassennamen
{
    [Serializable]
    public class StrassenabschnittOverviewModel : StrassenabschnittOverviewModelBase
    {
        public string ErfassungsStatusBezeichnung { get; set; }
    }
}