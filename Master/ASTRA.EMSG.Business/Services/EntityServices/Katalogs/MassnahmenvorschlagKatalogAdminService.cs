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
    public interface IMassnahmenvorschlagKatalogEditService : IService
    {
        MassnahmenvorschlagKatalogEditModel GetMassnahmenvorschlagKatalogModel(string massnahmenvorschlagTyp);
        IEnumerable<MassnahmenvorschlagKatalogModel> GetPossibleMassnahmenvorschlagen();
        void Customize(MassnahmenvorschlagKatalogEditModel editModel);
        void ResetToGlobal(string typ);
        MassnahmenvorschlagKatalog CreateCopy(MassnahmenvorschlagKatalog massnahmenvorschlagKatalog);
        void UpdateMassnahmenvorschlag(MassnahmenvorschlagKatalogEditModel editModel);
        IQueryable<MassnahmenvorschlagKatalog> GetEntitiesBy(ErfassungsPeriod erfassungsPeriod);
    }

    public class MassnahmenvorschlagKatalogEditService : MandantAndErfassungsPeriodDependentEntityServiceBase<MassnahmenvorschlagKatalog, MassnahmenvorschlagKatalogEditModel>, IMassnahmenvorschlagKatalogEditService
    {
        private readonly ILocalizationService localizationService;

        public MassnahmenvorschlagKatalogEditService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine, 
            ISecurityService securityService, 
            IHistorizationService historizationService, 
            ILocalizationService localizationService) : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.localizationService = localizationService;
        }

        public IEnumerable<MassnahmenvorschlagKatalogModel> GetPossibleMassnahmenvorschlagen()
        {
            var massnahmenvorschlagKatalogs =
                CurrentSession.Query<GlobalMassnahmenvorschlagKatalog>().ToArray().GroupBy(m => m.Typ)
                    .Select( g =>
                             new MassnahmenvorschlagKatalogModel
                                 {
                                     KatalogTyp = g.First().KatalogTyp,
                                     Typ = g.First().Typ,
                                     TypBezeichnung = localizationService.GetLocalizedMassnahmenvorschlagTyp(g.First().Typ)
                                 }).ToList();

            if (securityService.GetCurrentApplicationMode() == ApplicationMode.Mandant)
            {
                foreach (var model in FilteredEntities.Where(m => m.IsCustomized).Select(CreateModel))
                    massnahmenvorschlagKatalogs.RemoveAll(e => e.Typ == model.Typ);
            }
            return massnahmenvorschlagKatalogs;
        }

        public MassnahmenvorschlagKatalogEditModel GetMassnahmenvorschlagKatalogModel(string massnahmenvorschlagTyp)
        {
            return CreateModels(GetCurrentEntities().Where(m => m.Parent.Typ == massnahmenvorschlagTyp).ToArray()).Single();
        }

        public void ResetToGlobal(string typ)
        {
            var globalValues = CurrentSession.Query<GlobalMassnahmenvorschlagKatalog>().Where(m => m.Parent.Typ == typ).ToList();

            foreach (var massnahmenvorschlagKatalog in GetCurrentEntities().Where(m => m.Parent.Typ == typ))
            {
                massnahmenvorschlagKatalog.IsCustomized = false;
                massnahmenvorschlagKatalog.DefaultKosten = 
                    globalValues.Single(g => g.Belastungskategorie.Id == massnahmenvorschlagKatalog.Belastungskategorie.Id).DefaultKosten;
            }
        }

        public MassnahmenvorschlagKatalog CreateCopy(MassnahmenvorschlagKatalog massnahmenvorschlagKatalog)
        {
            return CreateEntity(entityServiceMappingEngine.Translate<MassnahmenvorschlagKatalog, MassnahmenvorschlagKatalog>(massnahmenvorschlagKatalog));
        }

        protected override void OnEntityUpdating(MassnahmenvorschlagKatalog entity)
        {
            base.OnEntityUpdating(entity);
            entity.IsCustomized = true;
        }

        public void Customize(MassnahmenvorschlagKatalogEditModel editModel)
        {
            foreach (var kosten in editModel.KonstenModels)
            {
                var entity = GetCurrentEntities().Single(k => k.Parent.Typ == editModel.Typ && k.Belastungskategorie.Id == kosten.Belastungskategorie);
                entity.DefaultKosten = kosten.DefaultKosten ?? 0;
                UpdateEntity(entity);
            }
        }

        public void UpdateMassnahmenvorschlag(MassnahmenvorschlagKatalogEditModel model)
        {
            foreach (var kosten in model.KonstenModels)
            {
                var entity = GetEntityById(kosten.Id);
                entity.DefaultKosten = kosten.DefaultKosten ?? 0;
                UpdateEntity(entity);
            }
        }

        private List<MassnahmenvorschlagKatalogEditModel> CreateModels(IEnumerable<MassnahmenvorschlagKatalog> massnahmenvorschlagKatalogs)
        {
            var result = massnahmenvorschlagKatalogs.GroupBy(m => m.Parent.Typ)
                .Select(m => new MassnahmenvorschlagKatalogEditModel
                                 {
                                     Typ = m.Key,
                                     KatalogTyp = m.First().KatalogTyp,
                                     KonstenModels = m.OrderBy(mk => mk.Belastungskategorie.Reihenfolge).Select(k => new MassnahmenvorschlagKatalogKonstenEditModel
                                                                       {
                                                                           Id = k.Id,
                                                                           DefaultKosten = k.DefaultKosten,
                                                                           Belastungskategorie = k.Belastungskategorie.Id,
                                                                           BelastungskategorieBezeichnung = localizationService.GetLocalizedBelastungskategorieTyp(k.BelastungskategorieTyp)
                                                                       }).ToList()
                                 }
                )
                .ToList();
            return result;
        }

        protected override Expression<Func<MassnahmenvorschlagKatalog, Mandant>> MandantExpression
        {
            get { return mk => mk.Mandant; }
        }

        protected override Expression<Func<MassnahmenvorschlagKatalog, ErfassungsPeriod>> ErfassungsPeriodExpression
        {
            get { return mk => mk.ErfassungsPeriod; }
        }
    }
}
