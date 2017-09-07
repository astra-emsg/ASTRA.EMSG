using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Master;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;

namespace ASTRA.EMSG.Business.Services.DTOServices
{
    public interface IDTOServiceBase<TEntity, TDataTransferObject>:IService 
    {
        List<TDataTransferObject> GetCurrentDTOs();
        TDataTransferObject GetDTOByID(Guid id);
        void CreateOrUpdateEntityFromDTO(TDataTransferObject dto);
        TDataTransferObject CreateEntity(TDataTransferObject dto);
        void DeleteEntity(Guid id);
        
    }
    public abstract class DTOServiceBase<TEntity, TDataTransferObject>
       : EntityServiceBase<TEntity, TDataTransferObject>, IDTOServiceBase<TEntity, TDataTransferObject>
        where TEntity : class, IEntity, new()
        where TDataTransferObject : IDataTransferObject, IModel, IIdHolder, new()
    {
        protected DTOServiceBase(ITransactionScopeProvider transactionScopeProvider, IDataTransferObjectServiceMappingEngine dataTransferObjectServiceMappingEngine) :
            base(transactionScopeProvider, (IEntityServiceMappingEngine)dataTransferObjectServiceMappingEngine)
        {

        }
        public virtual List<TDataTransferObject> GetCurrentDTOs()
        {
            return base.GetCurrentModels();
        }
        public TDataTransferObject GetDTOByID(Guid id)
        {
            return base.GetById(id);
        }
        public void CreateOrUpdateEntityFromDTO(TDataTransferObject dto)
        {
            var entity = GetEntityById(dto.Id);

            if (entity == null)
            {
                CreateEntityFromDTO(dto);
            }
            else
            {
                UpdateEntityFieldsInternal(dto, entity);
                UpdateEntity(entity);
            }
        }
        public TDataTransferObject CreateEntityFromDTO(TDataTransferObject dto)
        {
            var entity = new TEntity();
            if (dto.Id != Guid.Empty)
            {
                entity.Id = dto.Id;
            }
            else
            {
                dto.Id = entity.Id;
            }
            UpdateEntityFieldsInternal(dto, entity);
            CreateEntity(entity);

            return CreateModel(entity);
        }
    }
}
