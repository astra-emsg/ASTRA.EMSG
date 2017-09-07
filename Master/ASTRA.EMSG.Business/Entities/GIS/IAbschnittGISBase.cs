using ASTRA.EMSG.Business.Entities.Common;
using GeoAPI.Geometries;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    public interface IAbschnittGISBase : IEntity, IShapeHolder
    {
        ReferenzGruppe ReferenzGruppe { get; set; }
        string DisplayName { get; }
        decimal Laenge { get; set; }
    }
}
