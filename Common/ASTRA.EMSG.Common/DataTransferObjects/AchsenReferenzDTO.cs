using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Common.DataTransferObjects
{
    [Serializable]
    public class AchsenReferenzDTO:DataTransferObject, IDTOGeometryHolder
    {
        public string Strassenname { get; set; }
        private AchsenSegmentDTO achsenSegmentDTO;
        public AchsenSegmentDTO AchsenSegmentDTO { get { return achsenSegmentDTO; } set { AchsenSegment = value.Id; achsenSegmentDTO = value; } }
        public ReferenzGruppeDTO ReferenzGruppeDTO { get; set; }
        public Guid AchsenSegment { get; set; }
        public IGeometry Shape { get; set; }
    }
}
