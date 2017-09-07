using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.ReflectionMapper;
using System.Reflection;
using NetTopologySuite.Geometries.MGeometries;
using GeoAPI.Geometries;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Business.Infrastructure.MappingRules
{
    public interface IMLineStringTo2DMappingRule : IMappingRule
    { }
    class MLineStringTo2DMappingRule : IMLineStringTo2DMappingRule
    {
        public bool CanApply(PropertyInfo sourceProperty, PropertyInfo destinationProperty)
        {
            return typeof(IGeometry).IsAssignableFrom(sourceProperty.PropertyType);
        }

        public void Apply<TSource, TDestination>(PropertyInfo sourceProperty, PropertyInfo destinationProperty, RuleDrivenReflectingMapper<TSource, TDestination> ruleDrivenReflectingMapper)
        {
            ruleDrivenReflectingMapper.SetValueFrom(destinationProperty, source => ConvertMLineStringTo2D(source, sourceProperty, destinationProperty));
        }

        private object ConvertMLineStringTo2D(object source, PropertyInfo sourceProperty, PropertyInfo destinationProperty)
        {
            IGeometryFactory gf = GISService.CreateGeometryFactory();
            var sourceEntityPropertyValue = sourceProperty.GetValue(source, new object[0]);
            if (sourceEntityPropertyValue == null)
                return null;

            var mlineString = sourceEntityPropertyValue as MLineString;
            if (mlineString != null)
            {
                return GeometryUtils.ConvertMLineStringTo2D(gf, mlineString);
            }
            else
            {
                return sourceEntityPropertyValue;
            }
        }

    }
}
