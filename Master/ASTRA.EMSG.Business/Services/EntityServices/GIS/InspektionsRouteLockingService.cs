using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public interface IInspektionsRouteLockingService : IService
    {
        void LockInspektionsRoute(InspektionsRouteGIS inspektionsRouteGIS);
        void UnLockInspektionsRoute(InspektionsRouteGIS inspektionsRouteGIS);
    }

    public class InspektionsRouteLockingService : IInspektionsRouteLockingService
    {
        public void LockInspektionsRoute(InspektionsRouteGIS inspektionsRouteGIS)
        {
            foreach (var inspektionsRtStrAbschnitte in inspektionsRouteGIS.InspektionsRtStrAbschnitteList)
            {
                inspektionsRtStrAbschnitte.StrassenabschnittGIS.IsLocked = true;
            }
        }

        public void UnLockInspektionsRoute(InspektionsRouteGIS inspektionsRouteGIS)
        {
            foreach (var inspektionsRtStrAbschnitte in inspektionsRouteGIS.InspektionsRtStrAbschnitteList)
            {
                inspektionsRtStrAbschnitte.StrassenabschnittGIS.IsLocked = false;
            }
        }
    }
}