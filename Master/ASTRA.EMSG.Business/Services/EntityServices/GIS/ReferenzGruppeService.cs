using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using FluentValidation;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public interface IReferenzGruppeService : IService
    {
        void UpdateEntityInternal(ReferenzGruppe entity);
        ReferenzGruppeModel CreateEntity(ReferenzGruppeModel referenzGruppeModel);
        ReferenzGruppe CreateReferenzgruppe(ReferenzGruppe referenzGruppe);
        void DeleteEntity(Guid id);
    }

    public class ReferenzGruppeService : EntityServiceBase<ReferenzGruppe, ReferenzGruppeModel>, IReferenzGruppeService
    {
        private readonly IValidatorFactory validatorFactory;
        private readonly ILocalizationService localizationService;
        private readonly IAchsenReferenzService achsenReferenzService;

        public ReferenzGruppeService(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine,
            IValidatorFactory validatorFactory, ILocalizationService localizationService, ISecurityService securityService,
            IHistorizationService historizationService, IAchsenReferenzService achsenReferenzService)
            : base(transactionScopeProvider, entityServiceMappingEngine)
        {
            this.validatorFactory = validatorFactory;
            this.localizationService = localizationService;
            this.achsenReferenzService = achsenReferenzService;
        }
        public  ReferenzGruppe CreateReferenzgruppe(ReferenzGruppe referenzGruppe)
        {
            return base.CreateEntity(referenzGruppe);
        }
        protected override void UpdateEntityFieldsInternal(ReferenzGruppeModel model, ReferenzGruppe entity)
        {
            List<AchsenReferenzModel> new_AchsenReferenzen = new List<AchsenReferenzModel>();
            foreach (AchsenReferenzModel achsenReferenzModel in model.AchsenReferenzenModel)
            {
                AchsenReferenz ar = new AchsenReferenz();
                ar.ReferenzGruppe = entity;
                ar.Shape = achsenReferenzModel.Shape;
                ar.AchsenSegment = GetEntityById<AchsenSegment>(achsenReferenzModel.AchsenSegment);
                entity.AddAchsenReferenz(ar);
            }


            model.AchsenReferenzenModel = new_AchsenReferenzen;
            base.UpdateEntityFieldsInternal(model, entity);
        }
        public void UpdateEntityInternal(ReferenzGruppe entity)
        { 
            //update achsenreferenz
            achsenReferenzService.CreateOrUpdateList(entity.AchsenReferenzen);
            
            //update referenzgruppe
            Create(CreateEntity(entity));
        }
    }
}