using System;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;
using GeoAPI.Geometries;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("AUC")]
    public class AchsenUpdateConflict : Entity, IErfassungsPeriodDependentEntity, IShapeHolder
    {
        public const int ITEMTYPE_SEGMENT = 1;
        public const int ITEMTYPE_STRASSENABSCHNITT = 2;
        public const int ITEMTYPE_ZUSTAND = 3;
        public const int ITEMTYPE_KOORDINIERTEMASSNAHME = 4;
        public const int ITEMTYPE_MASSNVORSCHLAGTEILSYSTEM = 5;
        public const int ITEMTYPE_REALISIERTEMASSNAHME = 6;

        public const int CONFLICTTYPE_NEWSEGMENT = 1;
        public const int CONFLICTTYPE_PARTIAL_SEGMENT_DELETED = 2;
        public const int CONFLICTTYPE_PARTIAL_REFERENCE_OUTSIDE = 3;
        public const int CONFLICTTYPE_PARTIAL_REFERENCE_TOOSHORT = 4;

        public const int CONFLICTTYPE_COMPLETELOSS_SEGMENT_DELETED = 16 | 2;   // 18
        public const int CONFLICTTYPE_COMPLETELOSS_REFERENCE_OUTSIDE = 16 | 3;  // 19
        public const int CONFLICTTYPE_COMPLETELOSS_REFERENCE_TOOSHORT = 16 | 4;  // 20

        public static int ItemTypeFromClass(IAbschnittGISBase item)
        {
            if (item is StrassenabschnittGIS) return AchsenUpdateConflict.ITEMTYPE_STRASSENABSCHNITT;
            if (item is ZustandsabschnittGIS) return AchsenUpdateConflict.ITEMTYPE_ZUSTAND;
            if (item is KoordinierteMassnahmeGIS) return AchsenUpdateConflict.ITEMTYPE_KOORDINIERTEMASSNAHME;
            if (item is MassnahmenvorschlagTeilsystemeGIS) return AchsenUpdateConflict.ITEMTYPE_MASSNVORSCHLAGTEILSYSTEM;
            if (item is RealisierteMassnahmeGIS) return AchsenUpdateConflict.ITEMTYPE_REALISIERTEMASSNAHME;
            return -1;
        }

        public AchsenUpdateConflict()
        {
        }

        public virtual string Name { get; set; }
        public virtual int ConflictType { get; set; }
        public virtual int ItemType { get; set; }
        public virtual Guid ItemId { get; set; }
        public virtual Guid SegmentId { get; set; }
        public virtual IGeometry Shape { get; set; }

        public virtual Mandant Mandant { get; set; }
        public virtual ErfassungsPeriod ErfassungsPeriod { get; set; }
 
    }
}