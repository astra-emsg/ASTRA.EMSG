using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.ReflectionMapper.Interfaces;
using ASTRA.EMSG.Common.ReflectionMapper;

namespace ASTRA.EMSG.Business.ReflectionMappingConfiguration
{
    public interface IAchsKopieMappingEngine:IMappingEngine
    {
    }
    public class GISKopieMappingEngine : MappingEngine, IAchsKopieMappingEngine, IEntityServiceMappingEngine
    {
        public GISKopieMappingEngine(IAchsKopieMappingConfiguration configuration):base(configuration)
        { }
    }
}
