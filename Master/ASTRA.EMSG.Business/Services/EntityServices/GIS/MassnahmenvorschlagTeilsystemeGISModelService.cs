using System;
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
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public interface IMassnahmenvorschlagTeilsystemeGISModelService : IService
    {
        void DeleteEntity(Guid id);
        IQueryable<MassnahmenvorschlagTeilsystemeGIS> GetCurrentEntities();
        MassnahmenvorschlagTeilsystemeGISModel CreateEntity(MassnahmenvorschlagTeilsystemeGISModel model);
        MassnahmenvorschlagTeilsystemeGISModel GetById(Guid id);
        MassnahmenvorschlagTeilsystemeGISModel UpdateEntity(MassnahmenvorschlagTeilsystemeGISModel inspektionsRouteGISModel);
        List<MassnahmenvorschlagTeilsystemeGIS> GetBySpatialFilter(IGeometry filtergeom);
        MassnahmenvorschlagTeilsystemeGISModel GetKoordinierteMassnahmeAt(double x, double y, double tolerance);
        bool Validate(MassnahmenvorschlagTeilsystemeGISModel model);
        IList<MassnahmenvorschlagTeilsystemeGIS> GetAllKoordinierteMassnahmenAt(double x, double y, double tolerance);
    }

    public class MassnahmenvorschlagTeilsystemeGISModelService : MandantDependentEntityServiceBase<MassnahmenvorschlagTeilsystemeGIS, MassnahmenvorschlagTeilsystemeGISModel>, IMassnahmenvorschlagTeilsystemeGISModelService
    {
        private readonly IAchsenReferenzService achsenReferenzService;
        private readonly IReferenzGruppeService referenzGruppeService;
        private readonly IGISService gisService;
        private readonly IGeoJSONParseService geoJSONParseService;
        private readonly IAchsenSegmentService achsenSegmentService;

        public MassnahmenvorschlagTeilsystemeGISModelService(
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
            this.achsenReferenzService = achsenReferenzService;
            this.referenzGruppeService = referenzGruppeService;
            this.gisService = gisService;
            this.geoJSONParseService = geoJSONParseService;
            this.achsenSegmentService = achsenSegmentService;
        }

        protected override Expression<Func<MassnahmenvorschlagTeilsystemeGIS, Mandant>> MandantExpression { get { return ir => ir.Mandant; } }

        /// <summary>
        /// Updates the Entity from the Model and handle update db relations logic
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entity"></param>
        protected override void UpdateEntityFieldsInternal(MassnahmenvorschlagTeilsystemeGISModel model, MassnahmenvorschlagTeilsystemeGIS entity)
        {
            if (model.Id != Guid.Empty)
            {
                DeleteReferenzen(model.Id);
            }

            
            model.ReferenzGruppeModel = referenzGruppeService.CreateEntity(model.ReferenzGruppeModel);
            entity.ReferenzGruppe = GetEntityById<ReferenzGruppe>(model.ReferenzGruppeModel.Id);
            base.UpdateEntityFieldsInternal(model, entity);
        }

        public List<MassnahmenvorschlagTeilsystemeGIS> GetBySpatialFilter(IGeometry filtergeom)
        {
            return GetEntityListBySpatialFilter(filtergeom).ToList();
        }

        public MassnahmenvorschlagTeilsystemeGISModel GetKoordinierteMassnahmeAt(double x, double y, double tolerance)
        {

            IGeometry clickedPoint = GISService.CreateGeometryFactory().CreatePoint(new Coordinate((double)x, (double)y, 0));
            var buffer = clickedPoint.Buffer(tolerance);

            IList<MassnahmenvorschlagTeilsystemeGIS> koordinierteMassnahmenliste = GetBySpatialFilter(buffer).Where(mts => mts.Status!= EMSG.Common.Enums.StatusTyp.Abgeschlossen).ToList();

            MassnahmenvorschlagTeilsystemeGIS koordinierteMassnahme = gisService.GetNearestGeometry(buffer, koordinierteMassnahmenliste);

            return new MassnahmenvorschlagTeilsystemeGISModel { FeatureGeoJSONString = geoJSONParseService.GenerateGeoJsonStringFromEntity(koordinierteMassnahme) };
        }

        public IList<MassnahmenvorschlagTeilsystemeGIS> GetAllKoordinierteMassnahmenAt(double x, double y, double tolerance)
        {
            IGeometry clickedPoint = GISService.CreateGeometryFactory().CreatePoint(new Coordinate((double)x, (double)y, 0));
            var buffer = clickedPoint.Buffer(tolerance);

            IList<MassnahmenvorschlagTeilsystemeGIS> koordinierteMassnahmenliste = GetBySpatialFilter(buffer).Where(mts => mts.Status != EMSG.Common.Enums.StatusTyp.Abgeschlossen).ToList();

            return koordinierteMassnahmenliste;
        }
        public bool Validate(MassnahmenvorschlagTeilsystemeGISModel model)
        {
            foreach(var ar in model.ReferenzGruppeModel.AchsenReferenzenModel)
            {
                IGeometry achsensegment = achsenSegmentService.GetById(ar.AchsenSegment).Shape;

                if (!gisService.CheckGeometriesIsInControlGeometry(new List<AchsenReferenzModel>() { ar }, achsensegment))
                {
                    return false;
                }
            }
            return true;
        }
        private void DeleteReferenzen(Guid koordinierteMassnahmeID)
        {
            MassnahmenvorschlagTeilsystemeGIS koordinierteMassnahme = GetEntityById(koordinierteMassnahmeID);

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

        protected override void OnModelCreated(MassnahmenvorschlagTeilsystemeGIS entity, MassnahmenvorschlagTeilsystemeGISModel model)
        {
            model.FeatureGeoJSONString = geoJSONParseService.GenerateGeoJsonStringFromEntity(entity);
        }
    }
}