using System.Collections.Generic;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.GIS
{
    public class InspektionsRouteStatusverlaufOverviewModel : Model
    {
        public string Bezeichnung { get; set; }
        public List<InspektionsRouteStatusverlaufModel> InspektionsRouteStatusverlaufModels { get; set; }
    }
}