using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.ReflectionMapper;
using ASTRA.EMSG.Common.ReflectionMapper.Interfaces;

namespace ASTRA.EMSG.Business.ReflectionMappingConfiguration
{
    public interface IDataTransferObjectServiceMappingEngine : IMappingEngine
    { 
    }
    public class DataTransferObjectServiceMappingEngine: MappingEngine,IDataTransferObjectServiceMappingEngine, IEntityServiceMappingEngine
    {
        public DataTransferObjectServiceMappingEngine(IDataTransferObjectServiceMappingConfiguration configuration):
            base(configuration)
        { 
        }
    }
}
