using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using System.Linq;
using ASTRA.EMSG.Business.Utils;
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NHibernate.Spatial.Criterion;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using NHibernate.SqlCommand;
using NHibernate.Criterion;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public class ZustandsabschnittGISOverviewFilter : IStrassennameFilter
    {
        public string Strassenname { get; set; }
    }

    public interface IZustandsabschnittGISService : IService
    {
        List<ZustandsabschnittGISModel> GetAllZustandsabschnittModel(Guid strassenabschnittId);
        ZustandsabschnittGISModel GetZustandsabschnittAt(double x, double y, double tolerance);
        List<ZustandsabschnittOverviewGISModel> GetOverviewList(ZustandsabschnittGISOverviewFilter filter);
        //bool ValidateZustandsabschnitt(ZustandsabschnittGISModel zustandsabschnittgismodel);
        bool IsZustandsabschnittWithinStrassenabschnitt(ZustandsabschnittGISModel zustandsabschnittGISModel);
        IQueryable<ZustandsabschnittGIS> GetCurrentEntities();
        ZustandsabschnittGISModel GetById(Guid id);
        void DeleteEntity(Guid id);
        ZustandsabschnittGISModel CreateEntity(ZustandsabschnittGISModel zustandsabschnittGISModel);
        ZustandsabschnittGISModel UpdateEntity(ZustandsabschnittGISModel zustandsabschnittGISModel);
        List<ZustandsabschnittGIS> GetAllZustandsabschnittEnity(Guid strassenabschnittId);
        bool IsZustandsabschnittLaengeValid(Guid strassenabschnittId, Guid zustandsabschnittId, decimal zustandsabschnittLaenge);
        bool IsZustandsabschnittLaengeValid(Guid strassenabschnittId, Guid zustandsabschnittId, decimal zustandsabschnittLaenge, IList<Guid> deletedZustandsabschnitte);
        IQueryable<ZustandsabschnittGIS> GetEntitiesBy(ErfassungsPeriod erfassungsPeriod);
        IQueryable<ZustandsabschnittGIS> GetEntitiesBy(ErfassungsPeriod erfassungsPeriod, Mandant mandant);
    }

    public class ZustandsabschnittGISService : MandantAndErfassungsPeriodDependentEntityServiceBase<ZustandsabschnittGIS, ZustandsabschnittGISModel>, IZustandsabschnittGISService
    {
        private readonly IServerConfigurationProvider serverConfigurationProvider;

        private readonly IAchsenReferenzService achsenReferenzService;
        private readonly IReferenzGruppeService referenzGruppeService;
        private readonly IGISService gisService;
        private readonly IGeoJSONParseService geoJSONParseService;
        private readonly IFiltererFactory filtererFactory;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private const string geoJSONAttribute_childs = "childs";
        private const string geoJSONAttribute_AchsenSegmentId = "AchsenSegmentId";
        private const string geoJSONAttribute_StrassenabschnittsID = "StrassenabschnittsID";

        public ZustandsabschnittGISService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine, 
            ISecurityService securityService, 
            IHistorizationService historizationService, 
            IGISService gisService, 
            IAchsenReferenzService achsenReferenzService, 
            IReferenzGruppeService referenzGruppeService, 
            IGeoJSONParseService geoJSONParseService,
            IFiltererFactory filtererFactory,
            IServerConfigurationProvider serverConfigurationProvider)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.transactionScopeProvider = transactionScopeProvider;
            this.achsenReferenzService = achsenReferenzService;
            this.referenzGruppeService = referenzGruppeService;
            this.gisService = gisService;
            this.geoJSONParseService = geoJSONParseService;
            this.filtererFactory = filtererFactory;
            this.serverConfigurationProvider = serverConfigurationProvider;
        }

        protected override Expression<Func<ZustandsabschnittGIS, Mandant>> MandantExpression { get { return z => z.StrassenabschnittGIS.Mandant; } }
        protected override Expression<Func<ZustandsabschnittGIS, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return z => z.StrassenabschnittGIS.ErfassungsPeriod; } }

        public List<ZustandsabschnittGISModel> GetAllZustandsabschnittModel(Guid strassenabschnittId)
        {
            return FilteredEntities.Where(za => za.StrassenabschnittGIS.Id == strassenabschnittId)
                .Fetch(z => z.StrassenabschnittGIS).ToList().Select(CreateModel).ToList();
        }
        
        public List<ZustandsabschnittGIS> GetAllZustandsabschnittEnity(Guid strassenabschnittId)
        {
            return FilteredEntities.Where(za => za.StrassenabschnittGIS.Id == strassenabschnittId).ToList();
        }

        //ToDo: Separate to a separate Service
        public List<ZustandsabschnittOverviewGISModel> GetOverviewList(ZustandsabschnittGISOverviewFilter filter)
        {
            var zustandsabschnittGis = filtererFactory.CreateFilterer<ZustandsabschnittGIS>(filter).Filter(GetCurrentEntities());
            var zustandsabschnittOverviewGisModels = zustandsabschnittGis.Fetch(s => s.StrassenabschnittGIS)
                .Select(z =>
                    new ZustandsabschnittGIS()
                    {
                        Id = z.Id,
                        StrassenabschnittGIS = new StrassenabschnittGIS()
                        {
                            Abschnittsnummer = z.StrassenabschnittGIS.Abschnittsnummer,
                            BezeichnungBis = z.StrassenabschnittGIS.BezeichnungBis,
                            BezeichnungVon = z.StrassenabschnittGIS.BezeichnungVon,
                            Ortsbezeichnung = z.StrassenabschnittGIS.Ortsbezeichnung,
                            Laenge = z.StrassenabschnittGIS.Laenge,
                            Strassenname = z.StrassenabschnittGIS.Strassenname,
                            BreiteTrottoirLinks = z.StrassenabschnittGIS.BreiteTrottoirLinks,
                            BreiteTrottoirRechts = z.StrassenabschnittGIS.BreiteTrottoirRechts,
                            BreiteFahrbahn = z.StrassenabschnittGIS.BreiteFahrbahn,
                            ErfassungsPeriod = new ErfassungsPeriod()
                            {
                                Erfassungsjahr = z.StrassenabschnittGIS.ErfassungsPeriod.Erfassungsjahr
                            },
                        },
                        Abschnittsnummer = z.Abschnittsnummer,
                        Aufnahmedatum = z.Aufnahmedatum,
                        BezeichnungVon = z.BezeichnungVon,
                        BezeichnungBis = z.BezeichnungBis,
                        Bemerkung = z.Bemerkung,
                        Zustandsindex = z.Zustandsindex,
                        Laenge = z.Laenge
                    }
                )
                .ToArray().Select(z =>
                   {
                       var r = CreateModelFromEntity<ZustandsabschnittOverviewGISModel>(z);
                       r.BemerkungShort = z.Bemerkung.TrimToMaxLength(serverConfigurationProvider.BemerkungMaxDisplayLength);
                       r.Sreassenabschnittsnummer = z.StrassenabschnittGIS.Abschnittsnummer;
                       r.StrasseBezeichnungBis = z.StrassenabschnittGIS.BezeichnungBis;
                       r.StrasseBezeichnungVon = z.StrassenabschnittGIS.BezeichnungVon;
                       r.StrasseOrtsbezeichnung = z.StrassenabschnittGIS.Ortsbezeichnung;
                       r.StrasseLaenge = z.StrassenabschnittGIS.Laenge;
                       return r;
                   }).ToList();

            return zustandsabschnittOverviewGisModels;
        }

        protected override void UpdateEntityFieldsInternal(ZustandsabschnittGISModel model, ZustandsabschnittGIS entity)
        {
            //If it is an Update (ID is not empty) -> Delete the References first (Referenzgruppe, Achsenreferenzen)
            if (model.Id != Guid.Empty)
            {
                DeleteReferenzen(model.Id);
            }
            model.ReferenzGruppeModel = referenzGruppeService.CreateEntity(model.ReferenzGruppeModel);
            base.UpdateEntityFieldsInternal(model, entity);
        }

        public ZustandsabschnittGISModel GetZustandsabschnittAt(double x, double y, double tolerance)
        {
            IGeometry clickedPoint = GISService.CreateGeometryFactory().CreatePoint(new Coordinate(x, y, 0));
            var buffer = clickedPoint.Buffer(tolerance);

            //ZustandsabschnittGIS zustandsabschnitt = gisService.GetNearestGeometry(buffer, GetEntityListBySpatialFilter(buffer).Where(e=> e.ErfassungsPeriod==CurrentErfassungsPeriod).ToList());
            ZustandsabschnittGIS zustandsabschnitt = gisService.GetNearestGeometry(clickedPoint, GetCurrentZustandsAbschnitteBySpatialFilter(buffer, CurrentErfassungsPeriod));
            var zustandsabschnittmodel = new ZustandsabschnittGISModel();//base.GetById(zustandsabschnitt.Id);
            zustandsabschnittmodel.FeatureGeoJSONString = geoJSONParseService.GenerateGeoJsonStringFromEntity(zustandsabschnitt);
            return zustandsabschnittmodel;
        }
        private IList<ZustandsabschnittGIS> GetCurrentZustandsAbschnitteBySpatialFilter(IGeometry filter, ErfassungsPeriod erfassungsperiod)
        
        {
            var cr = transactionScopeProvider.CurrentTransactionScope.Session.CreateCriteria<ZustandsabschnittGIS>();
            cr.CreateAlias(typeof(StrassenabschnittGIS).Name, "strassenabschnittgis", JoinType.InnerJoin);
            cr.Add(SpatialRestrictions.Filter(ExpressionHelper.GetPropertyName<ZustandsabschnittGIS, IGeometry>(za => za.Shape), filter));
            //var test = cr.List();
            cr.Add(Restrictions.Eq("strassenabschnittgis."+ ExpressionHelper.GetPropertyName<StrassenabschnittGIS, ErfassungsPeriod>(sa => sa.ErfassungsPeriod),erfassungsperiod));
            //test = cr.List();
            return cr.List<ZustandsabschnittGIS>().Where(z => z.Shape.Intersects(filter)).ToList();
        }
        protected override void OnModelCreated(ZustandsabschnittGIS entity, ZustandsabschnittGISModel model)
        {
            base.OnModelCreated(entity, model);
            model.FeatureGeoJSONString = geoJSONParseService.GenerateGeoJsonStringFromEntity(entity);
            model.HasTrottoir = entity.StrassenabschnittBase.Trottoir != TrottoirTyp.NochNichtErfasst && entity.StrassenabschnittBase.Trottoir != TrottoirTyp.KeinTrottoir;
            model.StrasseBezeichnungBis = entity.StrassenabschnittGIS.BezeichnungBis;
            model.StrasseBezeichnungVon = entity.StrassenabschnittGIS.BezeichnungVon;
        }
        
        private void DeleteReferenzen(Guid zustandsabschnittId)
        {
            ZustandsabschnittGIS zustandsabschnitt = GetEntityById(zustandsabschnittId);

            if (zustandsabschnitt != null && zustandsabschnitt.ReferenzGruppe.Id != Guid.Empty)
            {
                List<AchsenReferenz> achsenreferenzen = achsenReferenzService.GetAchsenReferenzGruppe(zustandsabschnitt.ReferenzGruppe.Id);

                foreach (AchsenReferenz achsenreferenz in achsenreferenzen)
                {
                    achsenReferenzService.DeleteEntity(achsenreferenz.Id);
                }
            }

            if (zustandsabschnitt != null && zustandsabschnitt.ReferenzGruppe.Id != Guid.Empty)
            {
                referenzGruppeService.DeleteEntity(zustandsabschnitt.ReferenzGruppe.Id);
            }
        }

        
        public bool IsZustandsabschnittWithinStrassenabschnitt(ZustandsabschnittGISModel zustandsabschnittGISModel)
        {
            Guid strabsid = zustandsabschnittGISModel.StrassenabschnittGIS;
            StrassenabschnittGIS strab = GetEntityById<StrassenabschnittGIS>(strabsid);
            IList<AchsenReferenzModel> neueAchsenreferenzen = zustandsabschnittGISModel.ReferenzGruppeModel.AchsenReferenzenModel;
            return gisService.CheckGeometriesIsInControlGeometry(neueAchsenreferenzen, strab.Shape); 
        }

        protected override void OnEntityUpdating(ZustandsabschnittGIS entity)
        {
            base.OnEntityUpdating(entity);
            if (entity.IsLocked)
                throw new InvalidOperationException("ZustandsabschnittGIS is locked.");
        }

        public bool IsZustandsabschnittLaengeValid(Guid strassenabschnittId, Guid zustandsabschnittId, decimal zustandsabschnittLaenge)
        {
            return IsZustandsabschnittLaengeValid(strassenabschnittId, zustandsabschnittId,zustandsabschnittLaenge,  new List<Guid>());
        }
        public bool IsZustandsabschnittLaengeValid(Guid strassenabschnittId, Guid zustandsabschnittId, decimal zustandsabschnittLaenge, IList<Guid> deletedZustandsabschnitte)
        {
            var strassenabschnitt = GetEntityById<StrassenabschnittGIS>(strassenabschnittId);
            var zustandsabschnitt = GetEntityById(zustandsabschnittId);

            var zustandsabschnitten = strassenabschnitt.Zustandsabschnitten;
            if (zustandsabschnitt != null)
                zustandsabschnitten = zustandsabschnitten.Where(za => za.Id != zustandsabschnitt.Id&&!(deletedZustandsabschnitte.Contains(za.Id))).ToHashSet();
                        
            return strassenabschnitt.Laenge + 0.5m >= zustandsabschnitten.Sum(za => za.Laenge) + zustandsabschnittLaenge;
        }

        protected override void OnEntityCreating(ZustandsabschnittGIS entity)
        {
            base.OnEntityCreating(entity);
            entity.Erfassungsmodus = ZustandsErfassungsmodus.Manuel;
        }
    }
}
