using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
//using NetTopologySuite.Geometries;
using NetTopologySuite.Features;

namespace GeoJSON
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class FeatureWithID
    {

        #region Fields

        private IGeometry geometry = null;

        /// <summary>
        /// Geometry representation of the feature.
        /// </summary>
        public virtual IGeometry Geometry
        {
            get { return geometry; }
            set { geometry = value; }
        }

        private IAttributesTable attributes = null;

        /// <summary>
        /// Attributes table of the feature.
        /// </summary>
        public virtual IAttributesTable Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        private string id = null;

                /// <summary>
        /// Attributes table of the feature.
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="attributes"></param>
        public FeatureWithID(IGeometry geometry, IAttributesTable attributes, string id)
            : this()
        {
            this.geometry = geometry;
            this.attributes = attributes;
            this.id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        public FeatureWithID() { }

        #endregion

        public override bool Equals(object obj)
        {
            FeatureWithID otherFeature = obj as FeatureWithID;

            if (otherFeature != null)
            {
                if (this.Id == otherFeature.Id && this.Geometry == otherFeature.Geometry)
                {
                    return true;
                }
                else return false;
            }
            else return false;
            
            //return base.Equals(obj);
        }
    }
}
