using ASTRA.EMSG.Common.ReflectionMapper;
using ASTRA.EMSG.Common.ReflectionMapper.Interfaces;

namespace ASTRA.EMSG.Business.ReflectionMappingConfiguration
{
    public interface IEntityServiceMappingEngine : IMappingEngine
    {
    }

    public class EntityServiceMappingEngine : MappingEngine, IEntityServiceMappingEngine
    {
        public EntityServiceMappingEngine(IEntityServiceMappingConfiguration configuration)
            : base(configuration)
        {
        }
    }
}
