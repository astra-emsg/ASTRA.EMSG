using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using System.Linq;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public interface IInspektionsRouteStatusverlaufService : IService
    {
        List<InspektionsRouteStatusverlaufModel> GetInspektionsRouteStatusverlaufModels(Guid inspektionsrouteId);
        void HistorizeNeuErstellt(InspektionsRouteGIS inspektionsRouteGIS);
        void HistorizeAktualisiert(InspektionsRouteGIS inspektionsRouteGIS);
        void HistorizeRouteExportiert(InspektionsRouteGIS inspektionsRouteGIS);
        void HistorizeRouteImportiert(InspektionsRouteGIS inspektionsRouteGIS);
        void HistorizeRouteExportCancelled(InspektionsRouteGIS inspektionsRouteGIS);
    }

    public class InspektionsRouteStatusverlaufService : MandantAndErfassungsPeriodDependentEntityServiceBase<InspektionsRouteStatusverlauf, InspektionsRouteStatusverlaufModel>, IInspektionsRouteStatusverlaufService
    {
        private readonly ILocalizationService localizationService;
        private readonly ITimeService timeService;

        public InspektionsRouteStatusverlaufService(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine,
            ISecurityService securityService, IHistorizationService historizationService, ILocalizationService localizationService, ITimeService timeService)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.localizationService = localizationService;
            this.timeService = timeService;
        }

        protected override Expression<Func<InspektionsRouteStatusverlauf, Mandant>> MandantExpression { get { return irsv => irsv.InspektionsRouteGIS.Mandant; } }
        protected override Expression<Func<InspektionsRouteStatusverlauf, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return irsv => irsv.InspektionsRouteGIS.ErfassungsPeriod; } }

        protected override void OnModelCreated(InspektionsRouteStatusverlauf entity, InspektionsRouteStatusverlaufModel model)
        {
            base.OnModelCreated(entity, model);
            model.StatusBezeichnung = localizationService.GetLocalizedEnum(entity.Status);
        }

        public List<InspektionsRouteStatusverlaufModel> GetInspektionsRouteStatusverlaufModels(Guid inspektionsrouteId)
        {
            return FilteredEntities.Where(irsv => irsv.InspektionsRouteGIS.Id == inspektionsrouteId).Select(CreateModel).OrderBy(m => m.Datum).ToList();
        }

        public void HistorizeNeuErstellt(InspektionsRouteGIS inspektionsRouteGIS)
        {
            Historize(inspektionsRouteGIS, InspektionsRouteStatus.NeuErstellt);
        }

        public void HistorizeAktualisiert(InspektionsRouteGIS inspektionsRouteGIS)
        {
            Historize(inspektionsRouteGIS, InspektionsRouteStatus.Aktualisiert);
        }

        public void HistorizeRouteExportiert(InspektionsRouteGIS inspektionsRouteGIS)
        {
            Historize(inspektionsRouteGIS, InspektionsRouteStatus.RouteExportiert);
        }

        public void HistorizeRouteImportiert(InspektionsRouteGIS inspektionsRouteGIS)
        {
            Historize(inspektionsRouteGIS, InspektionsRouteStatus.RouteImportiert);
        }

        public void HistorizeRouteExportCancelled(InspektionsRouteGIS inspektionsRouteGIS)
        {
            Historize(inspektionsRouteGIS, InspektionsRouteStatus.ExportCancelled);
        }

        private void Historize(InspektionsRouteGIS inspektionsRouteGIS, InspektionsRouteStatus inspektionsRouteStatus)
        {
            var inspektionsRouteStatusverlauf = new InspektionsRouteStatusverlauf
                                                    {
                                                        Datum = timeService.Now, 
                                                        Status = inspektionsRouteStatus
                                                    };

            Create(inspektionsRouteStatusverlauf);

            inspektionsRouteGIS.AddInspektionsRouteStatusverlauf(inspektionsRouteStatusverlauf);
        }
    }
}
