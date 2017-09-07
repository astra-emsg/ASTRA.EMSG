using System;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.GIS
{
    [Serializable]
    public class InspektionsRouteGISOverviewModel : Model
    {
        public string Bezeichnung { get; set; }
        public int StrassenabschnittCount { get; set; }
        public string StatusBezeichnung { get; set; }

        public virtual string InInspektionBei { get; set; }
        public virtual DateTime? InInspektionBis { get; set; }

        public InspektionsRouteStatus Status { get; set; }
        public bool CanEdit { get { return Status != InspektionsRouteStatus.RouteExportiert; } }
        public bool CanCancell { get { return Status == InspektionsRouteStatus.RouteExportiert; } }
    }
}
