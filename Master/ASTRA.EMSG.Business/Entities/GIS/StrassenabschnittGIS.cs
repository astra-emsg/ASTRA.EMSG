using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;
using GeoAPI.Geometries;
using System.Linq;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("STG")]
    public class StrassenabschnittGIS : StrassenabschnittBase, IAbschnittGISBase
    {
        public StrassenabschnittGIS()
        {
            Zustandsabschnitten = new HashSet<ZustandsabschnittGIS>();
            InspektionsRtStrAbschnitte  = new HashSet<InspektionsRtStrAbschnitte>();
        }

        public virtual IGeometry Shape { get; set; }


        public override bool ShouldCheckBelagChange { get { return Zustandsabschnitten.Any(); } }

        public virtual ISet<ZustandsabschnittGIS> Zustandsabschnitten { get; set; }
        public virtual ISet<InspektionsRtStrAbschnitte> InspektionsRtStrAbschnitte { get; set; }
        public virtual bool BelongsToInspektionsroute { get { return InspektionsRtStrAbschnitte.Any(); } }

        public virtual ReferenzGruppe ReferenzGruppe { get; set; }

        public virtual bool IsLocked { get; set; }

        public virtual string DisplayName
        {
            get { return Strassenname + " (" + BezeichnungVon + " - " + BezeichnungBis + ")"; }
        }

        public virtual StrassenabschnittGIS CopiedFrom { get; set; }
    }
}