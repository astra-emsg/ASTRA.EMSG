using System;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.GIS
{
    public class InspektionsRouteStatusverlaufModel : Model
    {
        public DateTime Datum { get; set; }
        public InspektionsRouteStatus Status { get; set; }
        public string StatusBezeichnung { get; set; }
    }
}
