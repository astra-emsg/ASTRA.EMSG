using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;

namespace ASTRA.EMSG.Business.Services.DTOServices
{
    public interface IReferenzGruppeDTOService : IDTOServiceBase<ReferenzGruppe, ReferenzGruppeDTO>
    { }

    public class ReferenzGruppeDTOService:DTOServiceBase<ReferenzGruppe, ReferenzGruppeDTO>, IReferenzGruppeDTOService
    {
        private readonly IAchsenReferenzDTOService achsenReferenzDTOService;
        public ReferenzGruppeDTOService(ITransactionScopeProvider transactionScopeProvider, IDataTransferObjectServiceMappingEngine dataTransferObjectServiceMappingEngine, IAchsenReferenzDTOService achsenReferenzDTOService)
            : base(transactionScopeProvider, dataTransferObjectServiceMappingEngine)
        {
            this.achsenReferenzDTOService = achsenReferenzDTOService;
        }
        protected override void UpdateEntityFieldsInternal(ReferenzGruppeDTO DataTransferObject, ReferenzGruppe entity)
        {
            List<AchsenReferenzDTO> new_AchsenReferenzen = new List<AchsenReferenzDTO>();
            foreach (AchsenReferenzDTO achsenReferenzDTO in DataTransferObject.AchsenReferenzenDTO)
            {
                AchsenReferenz ar = new AchsenReferenz();
                ar.ReferenzGruppe = entity;
                ar.Shape = achsenReferenzDTO.Shape;
                ar.AchsenSegment = GetEntityById<AchsenSegment>(achsenReferenzDTO.AchsenSegment);
                entity.AddAchsenReferenz(ar);
            }


            DataTransferObject.AchsenReferenzenDTO = new_AchsenReferenzen;
            base.UpdateEntityFieldsInternal(DataTransferObject, entity);
        }
        protected override ReferenzGruppeDTO CreateModel(ReferenzGruppe entity)
        {
            var DTO =  base.CreateModel(entity);

            foreach (var ar in entity.AchsenReferenzen)
            {
                DTO.AddAchsenReferenz(achsenReferenzDTOService.GetDTOByID(ar.Id));
            }
            
            return DTO;
        }
    }
}
