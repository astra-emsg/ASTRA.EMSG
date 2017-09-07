using System;
using ASTRA.EMSG.Business.Entities.Common;
using GeoAPI.Geometries;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("KSK")]
    public class KopieSektor : Entity, IKopie
    {
        public virtual Guid SegmentId { get; set; }
        public virtual double Km { get; set; }
        public virtual double SectorLength { get; set; }
        public virtual string Name { get; set; }
        public virtual double Sequence { get; set; }
        public virtual IPoint MarkerGeom { get; set; }
        public virtual int Operation { get; set; }
        public virtual int ImpNr { get; set; }

        public override bool Equals(object obj)
        {
            KopieSektor other = obj as KopieSektor;
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
