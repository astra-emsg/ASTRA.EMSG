using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Common.DataTransferObjects
{
    public interface IReferenzGruppeDTOHolder : IDTOGeometryHolder
    {
        ReferenzGruppeDTO ReferenzGruppeDTO { get; set; }
    }
}
