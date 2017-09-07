using System;
using System.Collections.Generic;
using System.Linq;
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
using ASTRA.EMSG.Common.Utils;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public interface IInspektionsRouteGISOverviewService : IService
    {
        List<InspektionsRouteGISOverviewModel> GetCurrentModels();
        InspektionsRouteGISOverviewModel GetById(Guid id);
        void CancellExport(Guid inspektionsRouteId);
    }

    public class InspektionsRouteGISOverviewService : MandantAndErfassungsPeriodDependentEntityServiceBase<InspektionsRouteGIS, InspektionsRouteGISOverviewModel>, IInspektionsRouteGISOverviewService
    {
        private readonly ILocalizationService localizationService;
        private readonly IInspektionsRouteStatusverlaufService inspektionsRouteStatusverlaufService;
        private readonly IInspektionsRouteLockingService inspektionsRouteLockingService;
        private readonly ICheckOutsGISService checkOutsGISService;
        private readonly ITimeService timeService;

        public InspektionsRouteGISOverviewService(
            ITransactionScopeProvider transactionScopeProvider,
            IEntityServiceMappingEngine entityServiceMappingEngine,
            ISecurityService securityService,
            IHistorizationService historizationService,
            ILocalizationService localizationService,
            IInspektionsRouteStatusverlaufService inspektionsRouteStatusverlaufService,
            IInspektionsRouteLockingService inspektionsRouteLockingService,
            ICheckOutsGISService checkOutsGISService,
            ITimeService timeService
            )
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.localizationService = localizationService;
            this.inspektionsRouteStatusverlaufService = inspektionsRouteStatusverlaufService;
            this.inspektionsRouteLockingService = inspektionsRouteLockingService;
            this.checkOutsGISService = checkOutsGISService;
            this.timeService = timeService;
        }

        protected override Expression<Func<InspektionsRouteGIS, Mandant>> MandantExpression { get { return ir => ir.Mandant; } }
        protected override Expression<Func<InspektionsRouteGIS, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return ir => ir.ErfassungsPeriod; } }

        protected override void OnModelCreated(InspektionsRouteGIS entity, InspektionsRouteGISOverviewModel model)
        {
            base.OnModelCreated(entity, model);
            model.StatusBezeichnung = localizationService.GetLocalizedEnum(entity.Status);
        }

        public override List<InspektionsRouteGISOverviewModel> GetCurrentModels()
        {
            return FilteredEntities.Select(ins => new InspektionsRouteGIS()
            {
                Id = ins.Id,
                Bezeichnung = ins.Bezeichnung,
                InspektionsRtStrAbschnitteList = ins.InspektionsRtStrAbschnitteList,
                InInspektionBei = ins.InInspektionBei,
                InInspektionBis = ins.InInspektionBis,
                StatusverlaufList = ins.StatusverlaufList
            }).Select(CreateModel).ToList();
        }

        public void CancellExport(Guid inspektionsRouteId)
        {
            var inspektionsRouteGIS = GetEntityById(inspektionsRouteId);
            inspektionsRouteStatusverlaufService.HistorizeRouteExportCancelled(inspektionsRouteGIS);
            var checkOutsGIS = checkOutsGISService.GetCurrentEntities().Where(co => co.InspektionsRouteGIS.Id == inspektionsRouteId && co.CheckInDatum==null);
            foreach (var checkOutGIS in checkOutsGIS)
            {
                checkOutGIS.CheckInDatum = timeService.Now;
            }
            inspektionsRouteLockingService.UnLockInspektionsRoute(inspektionsRouteGIS);
        }
    }
}
