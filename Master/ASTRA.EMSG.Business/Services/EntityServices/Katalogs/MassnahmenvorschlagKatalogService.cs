using System;
using System.Collections.Generic;
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
using System.Linq;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Services.EntityServices.Katalogs
{
    public interface IMassnahmenvorschlagKatalogService : IService
    {
        decimal? GetMassnahmenvorschlagKosten(Guid? massnahmenvorschlagKatalogenId);
        List<MassnahmenvorschlagKatalogModel> GetMassnahmenvorschlagKatalogModelList(MassnahmenvorschlagKatalogTyp katalogTyp, string belastungsKategorieTyp);
        List<BLKIndependentMassnahmenvorschlagKatalogModel> GetMassnahmenvorschlagKatalogModelList(MassnahmenvorschlagKatalogTyp katalogTyp);
        IQueryable<MassnahmenvorschlagKatalog> GetCurrentEntities();
        IQueryable<MassnahmenvorschlagKatalog> GetAllEntities();
    }

    public class MassnahmenvorschlagKatalogService : MandantAndErfassungsPeriodDependentEntityServiceBase<MassnahmenvorschlagKatalog, MassnahmenvorschlagKatalogModel>, IMassnahmenvorschlagKatalogService
    {
        private readonly ILocalizationService localizationService;

        public MassnahmenvorschlagKatalogService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine, 
            ISecurityService securityService, 
            IHistorizationService historizationService,
            ILocalizationService localizationService
            )
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.localizationService = localizationService;
        }

        public List<MassnahmenvorschlagKatalogModel> GetMassnahmenvorschlagKatalogModelList(MassnahmenvorschlagKatalogTyp katalogTyp, string belastungsKategorieTyp)
        {
            var massnahmenvorschlagKatalogModels = FilterByTyp(FilteredEntities, katalogTyp, belastungsKategorieTyp).Select(CreateModel);
            return massnahmenvorschlagKatalogModels.ToList();
        }

        public List<BLKIndependentMassnahmenvorschlagKatalogModel> GetMassnahmenvorschlagKatalogModelList(MassnahmenvorschlagKatalogTyp katalogTyp)
        {
            var massnahmenvorschlagKatalogModels = Query<MassnahmentypKatalog>().Where(m => m.KatalogTyp == katalogTyp).Select(mk => new BLKIndependentMassnahmenvorschlagKatalogModel()
            {
                Id = mk.Id,
                KatalogTyp = mk.KatalogTyp,
                Typ = mk.Typ,
                TypBezeichnung = localizationService.GetLocalizedMassnahmenvorschlagTyp(mk.Typ)
            });
            return massnahmenvorschlagKatalogModels.ToList();
        }

        private IEnumerable<MassnahmenvorschlagKatalog> FilterByTyp(IQueryable<MassnahmenvorschlagKatalog> source, MassnahmenvorschlagKatalogTyp katalogTyp, string belastungsKategorieTyp)
        {
            var result = source.Where(mk => mk.Parent.KatalogTyp == katalogTyp);
            if (belastungsKategorieTyp != null)
                return result.Where(mk => mk.Belastungskategorie.Typ == belastungsKategorieTyp);
            return result;
        }

        public decimal? GetMassnahmenvorschlagKosten(Guid? massnahmenvorschlagKatalogenId)
        {
            if (!massnahmenvorschlagKatalogenId.HasValue)
                return 0;

            return GetById(massnahmenvorschlagKatalogenId.Value).DefaultKosten;
        }

        protected override void OnModelCreated(MassnahmenvorschlagKatalog entity, MassnahmenvorschlagKatalogModel model)
        {
            base.OnModelCreated(entity, model);
            model.TypBezeichnung = localizationService.GetLocalizedMassnahmenvorschlagTyp(entity.Typ);
        }

        protected override Expression<Func<MassnahmenvorschlagKatalog, Mandant>> MandantExpression
        {
            get { return mk => mk.Mandant; }
        }

        protected override Expression<Func<MassnahmenvorschlagKatalog, ErfassungsPeriod>> ErfassungsPeriodExpression
        {
            get { return mk => mk.ErfassungsPeriod; }
        }

        public IQueryable<MassnahmenvorschlagKatalog> GetAllEntities()
        {
            return Queryable;
        }
    }
}