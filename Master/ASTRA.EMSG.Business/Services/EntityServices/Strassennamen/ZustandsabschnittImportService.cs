using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Services.EntityServices.Strassennamen
{
    public interface IZustandsabschnittImportService : IService
    {
        ZustandsabschnittImportModel UpdateEntity(ZustandsabschnittImportModel zustandsabschnittImportModel);
        ZustandsabschnittImportModel CreateEntity(ZustandsabschnittImportModel zustandsabschnittImportModel);
        ZustandsabschnittImportModel GetById(Guid id);
        List<ZustandsabschnittImportModel> GetCurrentModels();
    }

    public class ZustandsabschnittImportService : MandantAndErfassungsPeriodDependentEntityServiceBase<Zustandsabschnitt, ZustandsabschnittImportModel>, IZustandsabschnittImportService
    {
        public ZustandsabschnittImportService(
            ITransactionScopeProvider transactionScopeProvider,
            IEntityServiceMappingEngine entityServiceMappingEngine,
            ISecurityService securityService,
            IHistorizationService historizationService
            )
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
        }

        protected override void OnEntityCreating(ZustandsabschnittImportModel model, Zustandsabschnitt entity)
        {
            base.OnEntityCreating(model, entity);
            UpdateEntityFields(model, entity);
        }

        protected override void OnEntityUpdating(ZustandsabschnittImportModel model, Zustandsabschnitt entity)
        {
            base.OnEntityUpdating(model, entity);
            UpdateEntityFields(model, entity);
        }

        protected override void OnModelCreated(Zustandsabschnitt entity, ZustandsabschnittImportModel model)
        {
            base.OnModelCreated(entity, model);
            model.MassnahmenvorschlagFahrbahnId = GetMassnahmenvorschlagKatalogId(entity.MassnahmenvorschlagFahrbahn);
            model.MassnahmenvorschlagTrottoirLinksId = GetMassnahmenvorschlagKatalogId(entity.MassnahmenvorschlagTrottoirLinks);
            model.MassnahmenvorschlagTrottoirRechtsId = GetMassnahmenvorschlagKatalogId(entity.MassnahmenvorschlagTrottoirRechts);
        }

        private void UpdateEntityFields(ZustandsabschnittImportModel model, Zustandsabschnitt entity)
        {
            entity.MassnahmenvorschlagFahrbahn = GetMassnahmenvorschlagKatalog(model.MassnahmenvorschlagFahrbahnId);
            entity.MassnahmenvorschlagTrottoirLinks = GetMassnahmenvorschlagKatalog(model.MassnahmenvorschlagTrottoirLinksId);
            entity.MassnahmenvorschlagTrottoirRechts = GetMassnahmenvorschlagKatalog(model.MassnahmenvorschlagTrottoirRechtsId);

            entity.DringlichkeitFahrbahn = model.DringlichkeitFahrbahn;
            entity.DringlichkeitTrottoirLinks = model.DringlichkeitTrottoirLinks;
            entity.DringlichkeitTrottoirRechts = model.DringlichkeitTrottoirRechts;

            entity.Erfassungsmodus = ZustandsErfassungsmodus.Manuel;
        }

        private MassnahmenvorschlagKatalog GetMassnahmenvorschlagKatalog(Guid? massnahmenvorschlagFahrbahn)
        {
            if (massnahmenvorschlagFahrbahn == null) 
                return null;
            
            return GetEntityById<MassnahmenvorschlagKatalog>(massnahmenvorschlagFahrbahn.Value);
        }

        private static Guid? GetMassnahmenvorschlagKatalogId(MassnahmenvorschlagKatalog massnahmenvorschlag)
        {
            return massnahmenvorschlag == null ? (Guid?) null : massnahmenvorschlag.Id;
        }

        protected override Expression<Func<Zustandsabschnitt, Mandant>> MandantExpression { get { return za => za.Strassenabschnitt.Mandant; } }
        protected override Expression<Func<Zustandsabschnitt, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return za => za.Strassenabschnitt.ErfassungsPeriod; } }
    }
}
