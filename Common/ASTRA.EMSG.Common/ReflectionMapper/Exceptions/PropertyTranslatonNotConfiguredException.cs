using System;
using System.Reflection;

namespace ASTRA.EMSG.Common.ReflectionMapper.Exceptions
{
    public class PropertyMappingNotConfiguredException : Exception
    {
        public PropertyInfo DestinationProperty { get; private set; }

        public PropertyMappingNotConfiguredException(PropertyInfo destinationProperty)
        {
            DestinationProperty = destinationProperty;
        }

        public override string Message
        {
            get
            {
                return string.Format("No mapping configured for property {0}.{1}",
                                     DestinationProperty.ReflectedType.Name, DestinationProperty.Name);
            }
        }
    }
}