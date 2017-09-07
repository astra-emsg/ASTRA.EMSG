using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public interface IRealisierteMassnahmeGISModelService : IService
    {
        void DeleteEntity(Guid id);
        RealisierteMassnahmeGISModel CreateEntity(RealisierteMassnahmeGISModel model);
        RealisierteMassnahmeGISModel GetById(Guid id);
        RealisierteMassnahmeGISModel UpdateEntity(RealisierteMassnahmeGISModel realisierteMassnahmeGISModel);
        bool IsRealisierteMassnahmeOnAchsensegment(RealisierteMassnahmeGISModel realisierteMassnahmeGISModel);
        List<RealisierteMassnahmeGIS> GetBySpatialFilter(IGeometry filtergeom);
        RealisierteMassnahmeGISModel GetRealisierteMassnahmeAt(double x, double y, double tolerance);
        bool Validate(RealisierteMassnahmeGISModel model);
        IQueryable<RealisierteMassnahmeGIS> GetEntitiesBy(ErfassungsPeriod erfassungsPeriod);
        IQueryable<RealisierteMassnahmeGIS> GetEntitiesBy(ErfassungsPeriod erfassungsPeriod, Mandant mandant);
    }

    public class RealisierteMassnahmeGISModelService : MandantAndErfassungsPeriodDependentEntityServiceBase<RealisierteMassnahmeGIS, RealisierteMassnahmeGISModel>, IRealisierteMassnahmeGISModelService
    {
        private readonly IReferenzGruppeService referenzGruppeService;
        private readonly IAchsenReferenzService achsenReferenzService;
        private readonly IGeoJSONParseService geoJSONParseService;
        private readonly IGISService gisService;
        private readonly IAchsenSegmentService achsenSegmentService;
        public RealisierteMassnahmeGISModelService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine, 
            ISecurityService securityService,
            IReferenzGruppeService referenzGruppeService,
            IAchsenReferenzService achsenReferenzService,
            IGeoJSONParseService geoJSONParseService,
            IGISService gisService,
            IAchsenSegmentService achsenSegmentService,
            IHistorizationService historizationService
            )
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.achsenSegmentService = achsenSegmentService;
            this.gisService = gisService;
            this.geoJSONParseService = geoJSONParseService;
            this.referenzGruppeService = referenzGruppeService;
            this.achsenReferenzService = achsenReferenzService;
        }
        
        protected override void UpdateEntityFieldsInternal(RealisierteMassnahmeGISModel model, RealisierteMassnahmeGIS entity)
        {
            if (model.Id != Guid.Empty)
            {
                DeleteReferenzen(model.Id);
            }
            model.ReferenzGruppeModel = referenzGruppeService.CreateEntity(model.ReferenzGruppeModel);
            entity.ReferenzGruppe = GetEntityById<ReferenzGruppe>(model.ReferenzGruppeModel.Id);
            base.UpdateEntityFieldsInternal(model, entity);
        }

        private void DeleteReferenzen(Guid realisierteMassnahmeGISID)
        {
            RealisierteMassnahmeGIS rem = GetEntityById(realisierteMassnahmeGISID);

            if (rem != null && rem.ReferenzGruppe.Id != Guid.Empty)
            {
                List<AchsenReferenz> arList = achsenReferenzService.GetAchsenReferenzGruppe(rem.ReferenzGruppe.Id);
                foreach (AchsenReferenz ar in arList)
                {
                    achsenReferenzService.DeleteEntity(ar.Id);
                }
            }

            if (rem != null && rem.ReferenzGruppe.Id != Guid.Empty)
                referenzGruppeService.DeleteEntity(rem.ReferenzGruppe.Id);
        }
        
        protected override void OnModelCreated(RealisierteMassnahmeGIS entity, RealisierteMassnahmeGISModel model)
        {
            model.FeatureGeoJSONString = geoJSONParseService.GenerateGeoJsonStringFromEntity(entity);
            if (entity.BreiteFahrbahn == 0)
                model.BreiteFahrbahn = null;
        }

        public bool IsRealisierteMassnahmeOnAchsensegment(RealisierteMassnahmeGISModel realisierteMassnahmeGISModel)
        {
            foreach (var ar in realisierteMassnahmeGISModel.ReferenzGruppeModel.AchsenReferenzenModel)
            {
                var achse = achsenSegmentService.GetById(ar.AchsenSegment);
                if (!gisService.CheckGeometriesIsInControlGeometry(new List<AchsenReferenzModel> { ar }, achse.Shape))
                {
                    return false;
                }
            }
            return true;
        }
        
        protected override Expression<Func<RealisierteMassnahmeGIS, Mandant>> MandantExpression { get { return rm => rm.Mandant; } }

        protected override Expression<Func<RealisierteMassnahmeGIS, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return i => i.ErfassungsPeriod; } }
      
        public bool Validate(RealisierteMassnahmeGISModel model)
        {
            foreach (var ar in model.ReferenzGruppeModel.AchsenReferenzenModel)
            {
                IGeometry achsensegment = achsenSegmentService.GetById(ar.AchsenSegment).Shape;

                if (!gisService.CheckGeometriesIsInControlGeometry(new List<AchsenReferenzModel> { ar }, achsensegment))
                {
                    return false;
                }
            }
            return true;
        }

        public List<RealisierteMassnahmeGIS> GetBySpatialFilter(IGeometry filtergeom)
        {
            return GetEntityListBySpatialFilter(filtergeom).ToList();
        }
        public List<RealisierteMassnahmeGIS> GetCurrentBySpatialFilter(IGeometry filtergeom)
        {
            return this.GetBySpatialFilter(filtergeom).Where(rmg => rmg.ErfassungsPeriod == historizationService.GetCurrentErfassungsperiod()).ToList();
        }

        public RealisierteMassnahmeGISModel GetRealisierteMassnahmeAt(double x, double y,  double tolerance)
        {

            IGeometry clickedPoint = GISService.CreateGeometryFactory().CreatePoint(new Coordinate(x, y, 0));
            var buffer = clickedPoint.Buffer(tolerance);

            IList<RealisierteMassnahmeGIS> realisierteMassnahmenliste = GetCurrentBySpatialFilter(buffer).ToList();

            RealisierteMassnahmeGIS realisierteMassnahme = gisService.GetNearestGeometry(buffer, realisierteMassnahmenliste);

            return new RealisierteMassnahmeGISModel { FeatureGeoJSONString = geoJSONParseService.GenerateGeoJsonStringFromEntity(realisierteMassnahme) };
        }
    }
}
