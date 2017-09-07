using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;
using GeoAPI.Geometries;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("ACR")]
    public class AchsenReferenz : Entity, IShapeHolder
    {
        public virtual int? Version { get; set; }
        public virtual AchsenSegment AchsenSegment { get; set; }
        public virtual ReferenzGruppe ReferenzGruppe { get; set; }
        public virtual string Strassenname { get; set; }
        public virtual int? VonRBBS { get; set; }
        public virtual int? NachRBBS { get; set; }
        public virtual IGeometry Shape { get; set; }

        public virtual Mandant Mandandt
        {
            get { return ReferenzGruppe.Mandant; }
            set { }
        }

        public virtual ErfassungsPeriod Erfassungsperiod
        {
            get { return ReferenzGruppe.Erfassungsperiod; }
            set { }
        }
        public virtual AchsenReferenz CopiedFrom { get; set; }
    }
}