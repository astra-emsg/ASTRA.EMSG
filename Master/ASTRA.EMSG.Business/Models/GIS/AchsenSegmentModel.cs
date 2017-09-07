using System;
using ASTRA.EMSG.Common;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Business.Models.GIS
{
    [Serializable]
    public class AchsenSegmentModel : Model
    {
        public string FeatureGeoJSONString { get; set; }
        [NonSerialized]
        private IGeometry shape;
        public IGeometry Shape { get { return shape; } set { shape = value; } }
        public int? Version { get; set; }
        public Guid AchsenId { get; set; }
        //public Guid Mandant { get; set; }
        public bool IsInverted { get; set; }
        public string Name { get; set; }

        public Guid[] ModifiedEntites { get; set; }
        /// <summary>
        /// Two possible values: "Change", "Delete"
        /// </summary>
        public AchseModificationAction ModificationAction { get; set; }
    }

    public enum AchseModificationAction
    {
        Change,
        Delete
    }
}
