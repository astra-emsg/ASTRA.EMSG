using System;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("IRV")]
    public class InspektionsRouteStatusverlauf : Entity, IMandantDependentEntity, IErfassungsPeriodDependentEntity
    {
        public virtual DateTime Datum { get; set; }
        public virtual InspektionsRouteStatus Status { get; set; }
        public virtual InspektionsRouteGIS InspektionsRouteGIS { get; set; }

        public virtual Mandant Mandant
        {
            get { return InspektionsRouteGIS.Mandant; }
            set { /*NOP */ }
        }

        public virtual ErfassungsPeriod ErfassungsPeriod
        {
            get { return InspektionsRouteGIS.ErfassungsPeriod; }
            set { /*NOP */ }
        }
    }
}