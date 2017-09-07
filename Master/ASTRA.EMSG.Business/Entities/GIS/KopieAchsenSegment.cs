using System;
using ASTRA.EMSG.Business.Entities.Common;
using GeoAPI.Geometries;
using ASTRA.EMSG.Business.Entities.Mapping;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("KSG")]
    public class KopieAchsenSegment : Entity, IKopie, IShapeHolder
    {
        public virtual IGeometry Shape { get; set; }
        public virtual int Operation { get; set; }
        public virtual string Name { get; set; }
        public virtual int Sequence { get; set; }
        public virtual Guid AchsenId { get; set; }
        public virtual int ImpNr { get; set; }

        public override bool Equals(object obj)
        {
            KopieAchsenSegment other = obj as KopieAchsenSegment;
            if (other == null) return false;
            if (Id == null || other.Id == null)
                return (object)this == other;
            else
                return Id == other.Id;
        }

        public override int GetHashCode()
        {
            if (Id == null) return base.GetHashCode();
            string stringRepresentation =
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName
                + "#" + Id.ToString();
            return stringRepresentation.GetHashCode();
        }
    }
}
