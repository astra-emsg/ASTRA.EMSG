using System;

namespace ASTRA.EMSG.Common.ReflectionMapper.Exceptions
{
    public class MapperNotFoundException : Exception
    {
        public Type SourceType { get; private set; }

        public Type DestinationType { get; private set; }

        public MapperNotFoundException(Type sourceType, Type destinationType)
        {
            SourceType = sourceType;
            DestinationType = destinationType;
        }

        public override string Message
        {
            get
            {
                return string.Format("Mapper from {0} to {1} not found.", SourceType, DestinationType);
            }
        }
    }
}