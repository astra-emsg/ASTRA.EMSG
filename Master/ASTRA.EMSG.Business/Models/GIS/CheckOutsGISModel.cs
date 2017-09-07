using System;
using System.Collections.Generic;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.GIS
{
    public class CheckOutsGISModel:Model
    {
       
        public virtual Guid? Mandant { get; set; }
        public virtual DateTime? CheckInDatum { get; set; }
        public virtual DateTime CheckOutDatum { get; set; }
        
        public virtual DateTime? CheckedOutUntil { get; set; }
        public virtual string InspectionBy { get; set; }
        public virtual string Description { get; set; }
        public virtual string Comments { get; set; }
        public bool IsCheckedOut { get {return CheckInDatum == null; } }
        public virtual Guid InspektionsRouteGIS { get; set; }
    }
}
