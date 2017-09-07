using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Infrastructure.MappingRules;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.ReflectionMapper.Interfaces;
using ASTRA.EMSG.Common.ReflectionMapper;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Business.Entities;

namespace ASTRA.EMSG.Business.ReflectionMappingConfiguration
{
    public interface IDataTransferObjectServiceMappingConfiguration : IMappingConfiguration
    { }
    public class DataTransferObjectServiceMappingConfiguration :MappingConfiguration, IDataTransferObjectServiceMappingConfiguration, IEntityServiceMappingConfiguration
    {
        private readonly IMappingRule[] mappingRules;
        public DataTransferObjectServiceMappingConfiguration(IIgnoreReadonlyPropertiesMappingRule ignoreReadonlyPropertiesMappingRule, IIdToEntityMappingRule idToEntityMappingRule,
            IEntityToIdMappingRule entityToIdMappingRule, INullableToNonNullableMappingRule nullableToNonNullableMappingRule)
        {
            mappingRules = new IMappingRule[] { ignoreReadonlyPropertiesMappingRule, idToEntityMappingRule, entityToIdMappingRule, nullableToNonNullableMappingRule };
            Register(new RuleDrivenReflectingMapper<StrassenabschnittGIS, StrassenabschnittGISDTO>(mappingRules));
            Register(new RuleDrivenReflectingMapper<AchsenSegment, AchsenSegmentDTO>(mappingRules));
            Register(new RuleDrivenReflectingMapper<Belastungskategorie, BelastungskategorieDTO>(mappingRules));
            Register(new RuleDrivenReflectingMapper<MassnahmenvorschlagKatalog, MassnahmenvorschlagKatalogDTO>(mappingRules));
            RegisterTwoDirectionMapping<AchsenReferenzDTO, AchsenReferenz>();
            RegisterTwoDirectionMapping<ZustandsabschnittGISDTO, ZustandsabschnittGIS>();
            RegisterTwoDirectionMapping<ReferenzGruppeDTO, ReferenzGruppe>();
            RegisterTwoDirectionMapping<SchadendetailDTO, Schadendetail>();
            RegisterTwoDirectionMapping<SchadengruppeDTO, Schadengruppe>();

            //RegisterTwoDirectionMapping<MassnahmenvorschlagDTO, Massnahmenvorschlag>();
        }
        
        private void RegisterTwoDirectionMapping<TDataTransferObject, TEntity>()
            where TDataTransferObject : IDataTransferObject ,IModel
            where TEntity : IEntity
        {
            Register(new RuleDrivenReflectingMapper<TDataTransferObject, TEntity>(mappingRules));
            Register(new RuleDrivenReflectingMapper<TEntity, TDataTransferObject>(mappingRules));
        }
    }
}
