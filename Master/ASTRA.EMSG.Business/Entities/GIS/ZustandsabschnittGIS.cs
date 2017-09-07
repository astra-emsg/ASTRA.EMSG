using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Mapping;
using GeoAPI.Geometries;
using NHibernate.Util;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("ZSG")]
    public class ZustandsabschnittGIS : ZustandsabschnittBase, IAbschnittGISBase
    {
        public override string Strassenname { get { return StrassenabschnittGIS.Strassenname; } }
        public override StrassenabschnittBase StrassenabschnittBase { get { return StrassenabschnittGIS; } }

        public override string InspektionsroutenName
        {
            get
            {
                return EnumerableExtensions.Any(StrassenabschnittGIS.InspektionsRtStrAbschnitte)
                           ? StrassenabschnittGIS.InspektionsRtStrAbschnitte.First().InspektionsRouteGIS.Bezeichnung
                           : string.Empty;
            }
        }

        public virtual bool IsLocked { get { return StrassenabschnittGIS.IsLocked; } }

        public virtual StrassenabschnittGIS StrassenabschnittGIS { get; set; }
        public virtual ReferenzGruppe ReferenzGruppe { get; set; }

        public virtual IGeometry Shape { get; set; }

        public virtual string DisplayName
        {
            get { return Strassenname + " (" + BezeichnungVon + " - " + BezeichnungBis + ")"; }
        }
        public virtual ZustandsabschnittGIS CopiedFrom { get; set; }
    }
}
