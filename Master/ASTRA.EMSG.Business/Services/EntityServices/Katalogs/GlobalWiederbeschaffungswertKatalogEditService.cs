using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using System.Linq;

namespace ASTRA.EMSG.Business.Services.EntityServices.Katalogs
{
    public interface IGlobalWiederbeschaffungswertKatalogEditService : IService
    {
        List<WiederbeschaffungswertKatalogEditModel> GetCurrentModels();

        WiederbeschaffungswertKatalogEditModel GetById(Guid id);
        WiederbeschaffungswertKatalogEditModel UpdateEntity(WiederbeschaffungswertKatalogEditModel wiederbeschaffungswertKatalogEditModel);
        void LoadGlobalValues(WiederbeschaffungswertKatalogEditModel wiederbeschaffungswertKatalogEditModel);
    }

    public class GlobalWiederbeschaffungswertKatalogEditService : EntityServiceBase<GlobalWiederbeschaffungswertKatalog, WiederbeschaffungswertKatalogEditModel>, IGlobalWiederbeschaffungswertKatalogEditService
    {
        private readonly ILocalizationService localizationService;
        private readonly ISecurityService securityService;
        private readonly IHistorizationService historizationService;

        public GlobalWiederbeschaffungswertKatalogEditService(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine,
            ILocalizationService localizationService, ISecurityService securityService, IHistorizationService historizationService)
            : base(transactionScopeProvider, entityServiceMappingEngine)
        {
            this.localizationService = localizationService;
            this.securityService = securityService;
            this.historizationService = historizationService;
        }

        protected override void OnModelCreated(GlobalWiederbeschaffungswertKatalog entity, WiederbeschaffungswertKatalogEditModel model)
        {
            model.BelastungskategorieBezeichnung = localizationService.GetLocalizedBelastungskategorieTyp(entity.BelastungskategorieTyp);
            model.BelastungskategorieReihenfolge = entity.Belastungskategorie.Reihenfolge;
        }

        protected override void OnEntityUpdating(GlobalWiederbeschaffungswertKatalog entity)
        {
            base.OnEntityUpdating(entity);
            var wiederbeschaffungswertKatalogs = Query<WiederbeschaffungswertKatalog>()
                .Where(rmk => !rmk.ErfassungsPeriod.IsClosed)
                .Where(wbk => wbk.Belastungskategorie == entity.Belastungskategorie)
                .Where(wbk => !wbk.IsCustomized)
                .ToList();

            foreach (var wiederbeschaffungswertKatalog in wiederbeschaffungswertKatalogs)
            {
                entityServiceMappingEngine.Translate(entity, wiederbeschaffungswertKatalog);
                Update(wiederbeschaffungswertKatalog);
            }
        }

        public void LoadGlobalValues(WiederbeschaffungswertKatalogEditModel wiederbeschaffungswertKatalogEditModel)
        {
            var wiederbeschaffungswertKatalogs = Query<WiederbeschaffungswertKatalog>()
                .Where(wbk => wbk.Mandant == securityService.GetCurrentMandant())
                .Where(wbk => wbk.ErfassungsPeriod == historizationService.GetCurrentErfassungsperiod())
                .Single(wbk => wbk.Belastungskategorie.Id == wiederbeschaffungswertKatalogEditModel.Belastungskategorie);

            var globalWiederbeschaffungswertKatalog = Queryable.Single(gwbk => gwbk.Belastungskategorie.Id == wiederbeschaffungswertKatalogEditModel.Belastungskategorie.Value);
            entityServiceMappingEngine.Translate(globalWiederbeschaffungswertKatalog, wiederbeschaffungswertKatalogEditModel);

            wiederbeschaffungswertKatalogEditModel.Id = wiederbeschaffungswertKatalogs.Id;
        }

        public override List<WiederbeschaffungswertKatalogEditModel> GetCurrentModels()
        {
            return base.GetCurrentModels().OrderBy(wkem => wkem.BelastungskategorieReihenfolge).ToList();
        }
    }
}