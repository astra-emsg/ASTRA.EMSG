using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;

namespace ASTRA.EMSG.Business.Services.EntityServices.Strassennamen
{
    public interface IStrassenabschnittImportService : IService
    {
        List<StrassenabschnittImportModel> GetCurrentModels();
        StrassenabschnittImportModel GetById(Guid id);

        StrassenabschnittImportModel UpdateEntity(StrassenabschnittImportModel strassenabschnittImportModel);
        StrassenabschnittImportModel CreateEntity(StrassenabschnittImportModel strassenabschnittImportModel);
    }

    public class StrassenabschnittImportService : MandantAndErfassungsPeriodDependentEntityServiceBase<Strassenabschnitt, StrassenabschnittImportModel>, IStrassenabschnittImportService
    {
        private readonly IFahrbahnZustandService fahrbahnZustandService;

        public StrassenabschnittImportService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine,
            ISecurityService securityService,
            IFahrbahnZustandService fahrbahnZustandService,
            IHistorizationService historizationService)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.fahrbahnZustandService = fahrbahnZustandService;
        }

        protected override void OnEntityUpdating(StrassenabschnittImportModel model, Strassenabschnitt entity)
        {
            base.OnEntityUpdating(model, entity);
            if (model.Belag != entity.Belag)
            {
                foreach (var zustandsabschnitt in entity.Zustandsabschnitten)
                {
                    fahrbahnZustandService.ResetZustandsabschnittdetails(zustandsabschnitt);   
                }
            }
        }

        protected override Expression<Func<Strassenabschnitt, Mandant>> MandantExpression { get { return s => s.Mandant; } }
        protected override Expression<Func<Strassenabschnitt, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return s => s.ErfassungsPeriod; } }
    }
}