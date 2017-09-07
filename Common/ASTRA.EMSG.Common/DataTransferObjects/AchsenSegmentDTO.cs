using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Common.DataTransferObjects
{
    [Serializable]
    public class AchsenSegmentDTO : DataTransferObject, IDTOGeometryHolder
    {
        public virtual IGeometry Shape { get; set; }
        public virtual bool IsInverted { get; set; }
    }
}
