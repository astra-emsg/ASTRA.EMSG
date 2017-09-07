using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Map.Services
{
    public class InspektionsRouteChangedEventArgs:EventArgs
    {
        public Guid inspektionsRouteId { get; set; }
        public InspektionsRouteChangedEventArgs(Guid inspektionsRouteId)
        {
            this.inspektionsRouteId = inspektionsRouteId;
        }
    }
}
