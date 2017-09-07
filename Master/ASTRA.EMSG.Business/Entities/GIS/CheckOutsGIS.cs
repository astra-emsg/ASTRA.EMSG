using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("COG")]
    public class CheckOutsGIS : Entity, IMandantDependentEntity
    {
       
        public virtual Mandant Mandant { get; set; }
        public virtual DateTime? CheckInDatum { get; set; }
        public virtual DateTime CheckOutDatum { get; set; }
        public virtual DateTime? CheckedOutUntil { get; set; }
        public virtual string InspectionBy { get; set; }
        public virtual string Description { get; set; }
        public virtual string Comments { get; set; }
        public virtual InspektionsRouteGIS InspektionsRouteGIS { get; set; }
    }
}
