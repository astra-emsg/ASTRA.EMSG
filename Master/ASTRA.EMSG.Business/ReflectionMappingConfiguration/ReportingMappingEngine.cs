using ASTRA.EMSG.Common.ReflectionMapper;
using ASTRA.EMSG.Common.ReflectionMapper.Interfaces;

namespace ASTRA.EMSG.Business.ReflectionMappingConfiguration
{
    public interface IReportingMappingEngine : IMappingEngine
    {
    }
    
    public class ReportingMappingEngine : MappingEngine, IReportingMappingEngine
    {
        public ReportingMappingEngine(IReportingMappingConfiguration configuration)
            : base(configuration)
        {
        }
    }
}