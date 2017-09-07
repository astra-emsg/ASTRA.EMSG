using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Services.EntityServices.Katalogs
{
    public interface IWiederbeschaffungswertKatalogEditService : IService
    {
        List<WiederbeschaffungswertKatalogEditModel> GetCustomizedModels();
        List<Belastungskategorie> GetBelastungskategorienWithoutWiederbeschaffungswertOverride();
        IQueryable<WiederbeschaffungswertKatalog> GetEntitiesBy(ErfassungsPeriod erfassungsPeriod);

        void ResetToGlobal(Guid id);
        WiederbeschaffungswertKatalogEditModel GetById(Guid id);
        WiederbeschaffungswertKatalogEditModel UpdateEntity(WiederbeschaffungswertKatalogEditModel wiederbeschaffungswertKatalogEditModel);
        void Customize(WiederbeschaffungswertKatalogEditModel wiederbeschaffungswertKatalogEditModel);
        WiederbeschaffungswertKatalog CreateCopy(WiederbeschaffungswertKatalog wiederbeschaffungswertKatalogToCopy);
    }

    public class WiederbeschaffungswertKatalogEditService : MandantAndErfassungsPeriodDependentEntityServiceBase<WiederbeschaffungswertKatalog, WiederbeschaffungswertKatalogEditModel>, IWiederbeschaffungswertKatalogEditService
    {
        private readonly ILocalizationService localizationService;

        public WiederbeschaffungswertKatalogEditService(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine,
            ISecurityService securityService, ILocalizationService localizationService, IHistorizationService historizationService)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.localizationService = localizationService;
        }

        public  List<WiederbeschaffungswertKatalogEditModel> GetCustomizedModels()
        {
            return GetCurrentEntities().Where(wbk => wbk.IsCustomized).Select(CreateModel).OrderBy(wkem => wkem.BelastungskategorieReihenfolge).ToList();
        }

        protected override void OnModelCreated(WiederbeschaffungswertKatalog entity, WiederbeschaffungswertKatalogEditModel model)
        {
            model.BelastungskategorieBezeichnung = localizationService.GetLocalizedBelastungskategorieTyp(entity.BelastungskategorieTyp);
            model.BelastungskategorieReihenfolge = entity.Belastungskategorie.Reihenfolge;
        }

        public WiederbeschaffungswertKatalog CreateCopy(WiederbeschaffungswertKatalog wiederbeschaffungswertKatalogToCopy)
        {
            return CreateEntity(entityServiceMappingEngine.Translate<WiederbeschaffungswertKatalog, WiederbeschaffungswertKatalog>(wiederbeschaffungswertKatalogToCopy));
        }

        public void ResetToGlobal(Guid id)
        {
            var wiederbeschaffungswertKatalog = GetEntityById(id);
            var globalWiederbeschaffungswertKatalog = Query<GlobalWiederbeschaffungswertKatalog>()
                .Single(gwbk => gwbk.Belastungskategorie == wiederbeschaffungswertKatalog.Belastungskategorie);

            entityServiceMappingEngine.Translate(globalWiederbeschaffungswertKatalog, wiederbeschaffungswertKatalog);

            wiederbeschaffungswertKatalog.IsCustomized = false;

            Update(wiederbeschaffungswertKatalog);
        }

        public void Customize(WiederbeschaffungswertKatalogEditModel wiederbeschaffungswertKatalogEditModel)
        {
            UpdateEntity(wiederbeschaffungswertKatalogEditModel);
        }

        protected override void OnEntityUpdating(WiederbeschaffungswertKatalog entity)
        {
            base.OnEntityUpdating(entity);
            entity.IsCustomized = true;
        }

        protected override Expression<Func<WiederbeschaffungswertKatalog, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return wbk => wbk.ErfassungsPeriod; } }
        protected override Expression<Func<WiederbeschaffungswertKatalog, Mandant>> MandantExpression { get { return wbk => wbk.Mandant; } }

        public List<Belastungskategorie> GetBelastungskategorienWithoutWiederbeschaffungswertOverride()
        {
            var belastungskategorien = CurrentSession.Query<Belastungskategorie>().OrderBy(b => b.Reihenfolge).ToList();

            foreach (var wiederbeschaffungswertKatalog in GetCurrentEntities().Where(wbk => wbk.IsCustomized).ToList())
                belastungskategorien.Remove(wiederbeschaffungswertKatalog.Belastungskategorie);

            return belastungskategorien;
        }
    }
}