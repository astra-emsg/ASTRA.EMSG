using System;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.Administration
{
    [Serializable]
    public class EreignisLogOverviewModel : Model
    {
        public string Benutzer { get; set; }

        public string MandantName { get; set; }

        public DateTime Zeit { get; set; }

        public EreignisTyp EreignisTyp { get; set; }

        public string EreignisTypBezeichnung { get; set; }

        public string EreignisData { get; set; }
    }
}