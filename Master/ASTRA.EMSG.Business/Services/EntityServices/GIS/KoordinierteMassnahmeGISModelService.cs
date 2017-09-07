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
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Business.Services.Security;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public interface IKoordinierteMassnahmeGISModelService : IService
    {
        void DeleteEntity(Guid id);
        List<KoordinierteMassnahmeGISModel> GetCurrentModelsByProjektname(string projektnameFilter);
        KoordinierteMassnahmeGISModel CreateEntity(KoordinierteMassnahmeGISModel model);
        KoordinierteMassnahmeGISModel GetById(Guid id);
        KoordinierteMassnahmeGISModel UpdateEntity(KoordinierteMassnahmeGISModel inspektionsRouteGISModel);
        List<KoordinierteMassnahmeGIS> GetBySpatialFilter(IGeometry filtergeom);
        IList<KoordinierteMassnahmeGIS> GetAllKoordinierteMassnahmenAt(double x, double y, double tolerance);
        KoordinierteMassnahmeGISModel GetKoordinierteMassnahmeAt(double x, double y, double tolerance);
        bool Validate(KoordinierteMassnahmeGISModel model);
    }

    public class KoordinierteMassnahmeGISModelService : MandantDependentEntityServiceBase<KoordinierteMassnahmeGIS, KoordinierteMassnahmeGISModel>, IKoordinierteMassnahmeGISModelService
    {
        private readonly IAchsenReferenzService achsenReferenzService;
        private readonly IReferenzGruppeService referenzGruppeService;
        private readonly IGISService gisService;
        private readonly IGeoJSONParseService geoJSONParseService;
        private readonly IAchsenSegmentService achsenSegmentService;
        private readonly IEntityServiceMappingEngine entityServiceMappingEngine;

        public KoordinierteMassnahmeGISModelService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine, 
            ISecurityService securityService,
            IAchsenReferenzService achsenReferenzService,
            IReferenzGruppeService referenzGruppeService,
            IGISService gisService,
            IGeoJSONParseService geoJSONParseService,
            IAchsenSegmentService achsenSegmentService)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService)
        {
            this.achsenSegmentService = achsenSegmentService;
            this.achsenReferenzService = achsenReferenzService;
            this.referenzGruppeService = referenzGruppeService;
            this.gisService = gisService;
            this.geoJSONParseService = geoJSONParseService;
            this.entityServiceMappingEngine = entityServiceMappingEngine;
        }

        protected override Expression<Func<KoordinierteMassnahmeGIS, Mandant>> MandantExpression { get { return ir => ir.Mandant; } }

        public List<KoordinierteMassnahmeGISModel> GetCurrentModelsByProjektname(string projektnameFilter)
        {
            var query = FilteredEntities;

            if (!string.IsNullOrWhiteSpace(projektnameFilter))
                query = query.Where(s => s.Projektname.ToLower().Contains(projektnameFilter.ToLower()));

            return query.Select(CreateModel).ToList();
        }

        /// <summary>
        /// Updates the Entity from the Model and handle update db relations logic
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entity"></param>
        protected override void UpdateEntityFieldsInternal(KoordinierteMassnahmeGISModel model, KoordinierteMassnahmeGIS entity)
        {
            if (model.Id != Guid.Empty)
            {
                DeleteReferenzen(model.Id);
            }

            
            model.ReferenzGruppeModel = referenzGruppeService.CreateEntity(model.ReferenzGruppeModel);
            entity.ReferenzGruppe = GetEntityById<ReferenzGruppe>(model.ReferenzGruppeModel.Id);
            base.UpdateEntityFieldsInternal(model, entity);
        }

        public List<KoordinierteMassnahmeGIS> GetBySpatialFilter(IGeometry filtergeom)
        {
            return GetEntityListBySpatialFilter(filtergeom).ToList();
        }

        public KoordinierteMassnahmeGISModel GetKoordinierteMassnahmeAt(double x, double y, double tolerance)
        {

            IGeometry clickedPoint = GISService.CreateGeometryFactory().CreatePoint(new Coordinate((double)x, (double)y, 0));
            var buffer = clickedPoint.Buffer(tolerance);

            IList<KoordinierteMassnahmeGIS> koordinierteMassnahmenliste = GetBySpatialFilter(buffer).Where(km => km.Status != EMSG.Common.Enums.StatusTyp.Abgeschlossen).ToList();

            KoordinierteMassnahmeGIS koordinierteMassnahme = gisService.GetNearestGeometry(buffer, koordinierteMassnahmenliste);

            return new KoordinierteMassnahmeGISModel { FeatureGeoJSONString = geoJSONParseService.GenerateGeoJsonStringFromEntity(koordinierteMassnahme) };
        }

        public IList<KoordinierteMassnahmeGIS> GetAllKoordinierteMassnahmenAt(double x, double y, double tolerance)
        {

            IGeometry clickedPoint = GISService.CreateGeometryFactory().CreatePoint(new Coordinate((double)x, (double)y, 0));
            var buffer = clickedPoint.Buffer(tolerance);

            IList<KoordinierteMassnahmeGIS> koordinierteMassnahmenliste = GetBySpatialFilter(buffer).Where(km => km.Status != EMSG.Common.Enums.StatusTyp.Abgeschlossen).ToList();


            return koordinierteMassnahmenliste;
        }

        private void DeleteReferenzen(Guid koordinierteMassnahmeID)
        {
            KoordinierteMassnahmeGIS koordinierteMassnahme = GetEntityById(koordinierteMassnahmeID);

            if (koordinierteMassnahme != null && koordinierteMassnahme.ReferenzGruppe.Id != Guid.Empty)
            {
                List<AchsenReferenz> arList = achsenReferenzService.GetAchsenReferenzGruppe(koordinierteMassnahme.ReferenzGruppe.Id);
                foreach (AchsenReferenz ar in arList)
                {
                    achsenReferenzService.DeleteEntity(ar.Id);
                }
            }

            if (koordinierteMassnahme != null && koordinierteMassnahme.ReferenzGruppe.Id != Guid.Empty)
                referenzGruppeService.DeleteEntity(koordinierteMassnahme.ReferenzGruppe.Id);
        }
        public bool Validate(KoordinierteMassnahmeGISModel model)
        {
            foreach (var ar in model.ReferenzGruppeModel.AchsenReferenzenModel)
            {
                IGeometry achsensegment = achsenSegmentService.GetById(ar.AchsenSegment).Shape;

                if (!gisService.CheckGeometriesIsInControlGeometry(new List<AchsenReferenzModel>() { ar }, achsensegment))
                {
                    return false;
                }
            }
            return true;
        }

        protected override void OnModelCreated(KoordinierteMassnahmeGIS entity, KoordinierteMassnahmeGISModel model)
        {
            model.FeatureGeoJSONString = geoJSONParseService.GenerateGeoJsonStringFromEntity(entity);
        }
    }
}
