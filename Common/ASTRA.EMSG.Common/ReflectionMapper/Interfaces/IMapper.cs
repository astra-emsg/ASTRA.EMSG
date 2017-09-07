using System;

namespace ASTRA.EMSG.Common.ReflectionMapper.Interfaces
{
    public interface IMapper
    {
        Type SourceType { get; }
        Type DestinationType { get; }
        object Translate(object source, IMappingEngine engine);
        void Translate(object source, object destination, IMappingEngine engine);
    }
}