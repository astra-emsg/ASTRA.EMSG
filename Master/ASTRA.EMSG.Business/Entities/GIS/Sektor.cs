using System;
using ASTRA.EMSG.Business.Entities.Common;
using GeoAPI.Geometries;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("SEK")]
    public class Sektor : Entity
    {
        public virtual Guid BsId { get; set; }
        public virtual AchsenSegment AchsenSegment { get; set; }
        public virtual double Km { get; set; }
        public virtual double SectorLength { get; set; }
        public virtual string Name { get; set; }
        public virtual double Sequence { get; set; }
        public virtual IPoint MarkerGeom { get; set; }
        public virtual int Operation { get; set; }
        public virtual int ImpNr { get; set; }
        public virtual Sektor CopiedFrom { get; set; }
    }
}
