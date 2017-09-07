using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Infrastructure.MappingRules;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.ReflectionMapper.Interfaces;
using ASTRA.EMSG.Common.ReflectionMapper;
using ASTRA.EMSG.Business.Entities.GIS;

namespace ASTRA.EMSG.Business.ReflectionMappingConfiguration
{
    public interface IAchsKopieMappingConfiguration:IMappingConfiguration
    { }
    public class GISKopieMappingConfiguration : MappingConfiguration, IAchsKopieMappingConfiguration, IEntityServiceMappingConfiguration
    {
        private readonly IMappingRule[] mappingRules;
        public GISKopieMappingConfiguration(IIgnoreIdMappingRule ignoreIdMappingRule, IMLineStringTo2DMappingRule mLineStringTo2DMappingRule)
        {
            mappingRules = new IMappingRule[] { ignoreIdMappingRule, mLineStringTo2DMappingRule };
            Register(new RuleDrivenReflectingMapper<KopieAchse, Achse>(mappingRules));
            Register(new RuleDrivenReflectingMapper<KopieAchsenSegment, AchsenSegment>(mappingRules));
            Register(new RuleDrivenReflectingMapper<KopieSektor, Sektor>(mappingRules));
        }
    }
}
