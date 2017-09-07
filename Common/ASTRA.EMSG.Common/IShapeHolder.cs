using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Common
{
    public interface IShapeHolder
    {
        IGeometry Shape { get; set; }
    }
}
