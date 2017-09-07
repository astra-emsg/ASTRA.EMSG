using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("IRS")]
    public class InspektionsRtStrAbschnitte :Entity
    {
        public virtual InspektionsRouteGIS InspektionsRouteGIS { get; set; }
        public virtual StrassenabschnittGIS StrassenabschnittGIS { get; set; }
        public virtual int Reihenfolge { get; set; }
    }
}
