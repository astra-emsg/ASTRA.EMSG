using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;
using GeoAPI.Geometries;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("ACS")]
    public class AchsenSegment : Entity, IMandantDependentEntity, IErfassungsPeriodDependentEntity, IShapeHolder
    {
        public AchsenSegment()
        {
            Sektoren = new List<Sektor>();
            AchsenReferenzen = new List<AchsenReferenz>();
        }

        public virtual Guid BsId { get; set; }
        public virtual Achse Achse { get; set; }

        public virtual int Operation { get; set; }
        public virtual string Name { get; set; }
        public virtual int Sequence { get; set; }
        public virtual int ImpNr { get; set; }

        public virtual IGeometry Shape { get; set; }
        public virtual IGeometry Shape4d { get; set; }
        public virtual int? Version { get; set; }  // Obsolete
        public virtual Guid AchsenId { get; set; } // Obsolete
        
        public virtual Mandant Mandant { get; set; }
        public virtual ErfassungsPeriod ErfassungsPeriod { get; set; }
        public virtual IList<AchsenReferenz> AchsenReferenzen { get; set; }
        public virtual IList<Sektor> Sektoren { get; set; }

        public virtual AchsenSegment CopiedFrom { get; set; }
        public virtual bool IsInverted { get; set; }
    }
}