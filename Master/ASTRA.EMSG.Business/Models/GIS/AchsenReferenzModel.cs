using System;
using ASTRA.EMSG.Common;
using GeoAPI.Geometries;
using ASTRA.EMSG.Business.Entities.GIS;

namespace ASTRA.EMSG.Business.Models.GIS
{
    [Serializable]
    public class AchsenReferenzModel : Model, IShapeHolder
    {
        public int Version { get; set; }
        private AchsenSegmentModel achsenSegmentModel;
        public AchsenSegmentModel AchsenSegmentModel { get { return achsenSegmentModel; } set { AchsenSegment = value.Id; achsenSegmentModel = value; } }
        public ReferenzGruppeModel ReferenzGruppeModel { get; set; }
        public Guid AchsenSegment { get; set; }
        
        public string Strassenname { get; set; }
        public int VonRBBS { get; set; }
        public int NachRBBS { get; set; }
        [NonSerialized]
        private IGeometry shape;
        public IGeometry Shape { get { return shape; } set { shape = value; } }
    }
}