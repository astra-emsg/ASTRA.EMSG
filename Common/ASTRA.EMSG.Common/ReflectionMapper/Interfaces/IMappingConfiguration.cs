using System;

namespace ASTRA.EMSG.Common.ReflectionMapper.Interfaces
{
    public interface IMappingConfiguration
    {
        IMapper GetTranslator(Type sourceType, Type destinationType);
    }
}