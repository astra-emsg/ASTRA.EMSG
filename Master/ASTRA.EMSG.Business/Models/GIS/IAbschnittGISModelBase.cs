using ASTRA.EMSG.Common;
using GeoAPI.Geometries;
using System;

namespace ASTRA.EMSG.Business.Models.GIS
{
    public interface IAbschnittGISModelBase: IModel
    {
        Guid Id { get; set; }
        string FeatureGeoJSONString { get; set; }
        IGeometry Shape { get; set; }
        ReferenzGruppeModel ReferenzGruppeModel { get; set; }
        //Guid ReferenzGruppe { get; set; }
    }
}
