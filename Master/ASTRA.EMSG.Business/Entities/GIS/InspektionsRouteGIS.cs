using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;
using GeoAPI.Geometries;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("IRG")]
    public class InspektionsRouteGIS : Entity, IMandantDependentEntity, IErfassungsPeriodDependentEntity, IShapeHolder
    {
        public InspektionsRouteGIS()
        {
            InspektionsRtStrAbschnitteList = new List<InspektionsRtStrAbschnitte>();
            StatusverlaufList = new List<InspektionsRouteStatusverlauf>();
            CheckOutsGISList = new List<CheckOutsGIS>();
        }

        public virtual IGeometry Shape { get; set; }
        public virtual string Bezeichnung { get; set; }
        public virtual string Beschreibung { get; set; }
        public virtual string Bemerkungen { get; set; }
        public virtual string InInspektionBei { get; set; }
        public virtual DateTime? InInspektionBis { get; set; }

        public virtual IList<CheckOutsGIS> CheckOutsGISList { get; set; }

        public virtual IList<InspektionsRtStrAbschnitte> InspektionsRtStrAbschnitteList { get; set; }
        public virtual int StrassenabschnittCount { get { return InspektionsRtStrAbschnitteList.Distinct().Count(); } }

        public virtual IList<InspektionsRouteStatusverlauf> StatusverlaufList { get; set; }

        public virtual InspektionsRouteStatus Status
        {
            get
            {
                var inspektionsRouteStatusverlauf = StatusverlaufList.OrderByDescending(sv => sv.Datum).FirstOrDefault();
                return inspektionsRouteStatusverlauf == null ? InspektionsRouteStatus.NeuErstellt : inspektionsRouteStatusverlauf.Status;
            }
        }

        public virtual void AddInspektionsRouteStatusverlauf(InspektionsRouteStatusverlauf inspektionsRouteStatusverlauf)
        {
            StatusverlaufList.Add(inspektionsRouteStatusverlauf);
            inspektionsRouteStatusverlauf.InspektionsRouteGIS = this;
        }

        public virtual void AddStrassenabschnittGIS(StrassenabschnittGIS strassenabschnitt)
        {
            InspektionsRtStrAbschnitteList.Add(new InspektionsRtStrAbschnitte { InspektionsRouteGIS = this, StrassenabschnittGIS = strassenabschnitt, Reihenfolge = InspektionsRtStrAbschnitteList.Count });
        }

        public virtual InspektionsRtStrAbschnitte RemoveStrassenabschnittGIS(StrassenabschnittGIS strassenabschnitt)
        {
            return RemoveStrassenabschnittGIS(strassenabschnitt.Id);
        }

        public virtual InspektionsRtStrAbschnitte RemoveStrassenabschnittGIS(Guid strassenabschnittid)
        {
            var irsa = InspektionsRtStrAbschnitteList.Where(ia => ia.StrassenabschnittGIS.Id == strassenabschnittid).First();
            InspektionsRtStrAbschnitteList.Remove(irsa);

            IOrderedEnumerable<InspektionsRtStrAbschnitte> orderedlist = InspektionsRtStrAbschnitteList.OrderBy(ia => ia.Reihenfolge);
            int i = 0;
            foreach (var item in orderedlist)
            {
                item.Reihenfolge = i;
                i++;
            }
            return irsa;
        }

        public virtual bool IsLocked { get { return Status == InspektionsRouteStatus.RouteExportiert; } }

        public virtual Mandant Mandant { get; set; }
        public virtual ErfassungsPeriod ErfassungsPeriod { get; set; }
        public virtual int? LegendNumber { get; set; }
    }
}
