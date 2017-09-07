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
using ASTRA.EMSG.Business.Services.Historization;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System.Collections;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public interface IInspektionsRouteGISService : IService
    {
        string GetInspektionsRouteBezeichnung(Guid id);
        IQueryable<InspektionsRouteGIS> GetCurrentEntities();
        InspektionsRouteGISModel CreateEntity(InspektionsRouteGISModel model);
        void DeleteEntity(Guid id);
        InspektionsRouteGISModel GetById(Guid id);
        InspektionsRouteGIS GetInspektionsRouteById(Guid id);
        InspektionsRouteGISModel UpdateEntity(InspektionsRouteGISModel inspektionsRouteGISModel);
        void LockInspektionsRouten(List<Guid> ids);
        void UnLockInspektionsRouten(List<Guid> ids);
        string GetStrassenabschnitteFromInspektionsroute(Guid inspektionsrouteId);
        void UpdateInspektionsroutenGeometry(StrassenabschnittGISModel changedStrassenabschnitt);
        IQueryable<InspektionsRouteGIS> GetEntitiesBy(ErfassungsPeriod erfassungsperiod);
        InspektionsRouteGISModel GetInspektionsRouteGISAt(double x, double y, double tolerance);
        IList<InspektionsRouteGIS> GetInspektionsrouteByFilterGeom(IGeometry shape);
    }

    public class InspektionsRouteGISService : MandantAndErfassungsPeriodDependentEntityServiceBase<InspektionsRouteGIS, InspektionsRouteGISModel>, IInspektionsRouteGISService
    {

        private readonly IInspektionsRtStrAbschnitteService inspektionsRtStrAbschnitteService;
        private readonly IInspektionsRouteStatusverlaufService inspektionsRouteStatusverlaufService;
        private readonly IInspektionsRouteLockingService inspektionsRouteLockingService;
        private readonly IStrassenabschnittGISService strassenabschnittGISService;
        private readonly IGeoJSONParseService geoJSONParseService;
        private readonly IGISService gisService;
       
        public InspektionsRouteGISService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine,
            IInspektionsRtStrAbschnitteService inspektionsRtStrAbschnitteService, 
            IGeoJSONParseService geoJSONParseService,
            ISecurityService securityService, 
            IHistorizationService historizationService, 
            IInspektionsRouteStatusverlaufService inspektionsRouteStatusverlaufService, 
            IInspektionsRouteLockingService inspektionsRouteLockingService,
            IStrassenabschnittGISService strassenabschnittGISService, 
            IGISService gisService
            ) :
            base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.gisService = gisService;
            this.strassenabschnittGISService = strassenabschnittGISService;
            this.inspektionsRouteStatusverlaufService = inspektionsRouteStatusverlaufService;
            this.inspektionsRtStrAbschnitteService = inspektionsRtStrAbschnitteService;
            this.inspektionsRouteLockingService = inspektionsRouteLockingService;
            this.geoJSONParseService = geoJSONParseService;
        }
        public InspektionsRouteGIS GetInspektionsRouteById(Guid id)
        {
            return base.GetEntityById(id);
        }
        public InspektionsRouteGISModel GetInspektionsRouteGISAt(double x, double y, double tolerance)
        {

            IGeometry clickedPoint = GISService.CreateGeometryFactory().CreatePoint(new Coordinate((double)x, (double)y, 0));
            IGeometry buffer = clickedPoint.Buffer(tolerance);

            IList<InspektionsRouteGIS> inspektionsRouteGISListe = GetCurrentEntityListBySpatialFilter(buffer);

            InspektionsRouteGIS inspektionsRoute = gisService.GetNearestGeometry(buffer, inspektionsRouteGISListe);
            if (inspektionsRoute != null)
            {
                return new InspektionsRouteGISModel { FeatureGeoJSONString = geoJSONParseService.GenerateGeoJsonStringFromEntity(inspektionsRoute) };
            }
            else
            {
                return new InspektionsRouteGISModel { FeatureGeoJSONString = "{ \"type\": \"FeatureCollection\", \"features\": []}" };
            }
        }

        public void UpdateInspektionsroutenGeometry(StrassenabschnittGISModel changedStrassenabschnitt)
        {
            var inspektionsrouten =this.GetCurrentEntities().Where(ir => ir.InspektionsRtStrAbschnitteList.Where(irs => irs.StrassenabschnittGIS.Id == changedStrassenabschnitt.Id).Count() > 0);
            if (inspektionsrouten.Count() > 0)
            {
                IGeometry shape = null;
                var inspektionsroute = inspektionsrouten.Single();
                foreach (var abschnitt in inspektionsroute.InspektionsRtStrAbschnitteList)
                {
                    
                    if (shape != null)
                    { shape = shape.Union(abschnitt.StrassenabschnittGIS.Shape); }
                    else
                    { shape = abschnitt.StrassenabschnittGIS.Shape; }
                }
                inspektionsroute.Shape = shape;
                this.UpdateEntity(inspektionsroute);
            }
        }
        protected override void UpdateEntityFieldsInternal(InspektionsRouteGISModel model, InspektionsRouteGIS entity)
        {
            base.UpdateEntityFieldsInternal(model, entity);
            IGeometry shape = null;

            entity.InspektionsRtStrAbschnitteList.Clear();
            
            foreach (InspektionsRtStrAbschnitteModel irsa in model.InspektionsRtStrAbschnitteModelList)
            {
                var str = GetEntityById<StrassenabschnittGIS>(irsa.StrassenabschnittId);
                if (shape != null)
                {
                    shape = shape.Union(str.Shape);
                }
                else
                {
                    shape = str.Shape;
                }
                entity.AddStrassenabschnittGIS(str);
            }
            entity.Shape = shape;
            
            CurrentSession.Flush(); //Make sure that InspektionsRtStrAbschnitten are created
            if (entity.LegendNumber == null)
            {
                entity.LegendNumber = GetLegendNumber();

            }
        }
        private int GetLegendNumber()
        {
            var lnList = GetCurrentEntities().Where(ir => ir.LegendNumber != null).Select(ir => (int)ir.LegendNumber).ToList();
            var result = Enumerable.Range(1, 59).Except(lnList);
            var orderedList = result.OrderBy(ln => ln);
            if (orderedList.Count() > 0)
            {
                return orderedList.First();
            }
            else
            {
                bool lnFound = false;
                while (!lnFound)
                {
                    foreach (int i in Enumerable.Range(1, 59).OrderBy(r => r))
                    {
                       lnFound = !lnList.Remove(i);
                       if (lnFound)
                       {
                           return i;
                       }
                    }
                }
                return (GetCurrentEntities().Count() % 59) + 1;
            }
        }
        protected override InspektionsRouteGISModel CreateModel(InspektionsRouteGIS entity)
        {
            var model= base.CreateModel(entity);
            foreach (var item in entity.InspektionsRtStrAbschnitteList)
            {
                model.InspektionsRtStrAbschnitteModelList.Add(inspektionsRtStrAbschnitteService.GetById(item.Id));
            }
            return model;

        }
       
        public string GetInspektionsRouteBezeichnung(Guid id)
        {
            return GetEntityById(id).Bezeichnung;
        }
       
        public void LockInspektionsRouten(List<Guid> ids)
        {
            foreach(Guid id in ids)
            {
                InspektionsRouteGIS inspektionsroute = GetEntityById(id);
                inspektionsRouteLockingService.LockInspektionsRoute(inspektionsroute);
            }
        }
        public void UnLockInspektionsRouten(List<Guid> ids)
        {
            foreach (Guid id in ids)
            {
                InspektionsRouteGIS inspektionsroute = GetEntityById(id);
                inspektionsRouteLockingService.UnLockInspektionsRoute(inspektionsroute);
            }
 
        }
        public string GetStrassenabschnitteFromInspektionsroute(Guid inspektionsrouteId)
        {
            InspektionsRouteGIS inspektionsroute = GetEntityById(inspektionsrouteId);

            return geoJSONParseService.GenerateGeoJsonStringFromEntity(inspektionsroute);
        }

        public IList<InspektionsRouteGIS> GetInspektionsrouteByFilterGeom(IGeometry shape)
        {         
            

            return base.GetCurrentEntityListBySpatialFilter(shape);
        }

        protected override Expression<Func<InspektionsRouteGIS, Mandant>> MandantExpression { get { return i => i.Mandant; } }
        protected override Expression<Func<InspektionsRouteGIS, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return i => i.ErfassungsPeriod; } }

        protected override void OnEntityCreating(InspektionsRouteGIS entity)
        {
            base.OnEntityCreating(entity);
            inspektionsRouteStatusverlaufService.HistorizeNeuErstellt(entity);
        }

        protected override void OnEntityUpdating(InspektionsRouteGIS entity)
        {
            base.OnEntityUpdating(entity);
            inspektionsRouteStatusverlaufService.HistorizeAktualisiert(entity);
        }
    }
}
