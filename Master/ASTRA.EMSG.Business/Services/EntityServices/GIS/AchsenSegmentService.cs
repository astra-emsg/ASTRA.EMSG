using System.Collections.Generic;
using System.IO;
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
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using FluentValidation;
using NetTopologySuite.Geometries;
using GeoAPI.Geometries;
using System;
using GeoJSON;
using NetTopologySuite.Features;
using System.Globalization;
using NHibernate.Criterion;
using NHibernate.Spatial.Criterion;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Master.GeoJSON;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public interface IAchsenSegmentService : IService
    {
        string GetNearestAchsenSegment(double x, double y, double tolerance);
        string GetAchsensegmentCollection(string achsensegmentIds);
        string GetMandantAxisEnvelope();
        AchsenSegmentModel GetById(Guid id);
        IQueryable<AchsenSegment> GetCurrentEntities();
        List<AchsenSegment> GetCurrentBySpatialFilter(IGeometry filtergeom);
        List<AchsenSegmentOverviewModel> GetCurrentModelsByName(string name);
        IQueryable<AchsenSegment> GetEntitiesBy(ErfassungsPeriod getErfassungsPeriod);
        AchsenSegmentModel UpdateEntity(AchsenSegmentModel model);
        AchsenSegmentModel CreateEntity(AchsenSegmentModel model);
        void DeleteEntity(Guid id);
    }

    public class AchsenSegmentService : MandantAndErfassungsPeriodDependentEntityServiceBase<AchsenSegment, AchsenSegmentModel>, IAchsenSegmentService
    {
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IValidatorFactory validatorFactory;
        private readonly ILocalizationService localizationService;
        private readonly IGISService gisService;
        private readonly IAchsenReferenzService achsenreferenzService;
        private readonly IReferenzGruppeService referenzgruppeService;


        public AchsenSegmentService(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine,
            IValidatorFactory validatorFactory, ILocalizationService localizationService, ISecurityService securityService,
            IHistorizationService historizationService, IGISService gisService, IAchsenReferenzService achsenreferenzService, IReferenzGruppeService referenzgruppeService)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.transactionScopeProvider = transactionScopeProvider;
            this.validatorFactory = validatorFactory;
            this.localizationService = localizationService;
            this.gisService = gisService;
            
            this.achsenreferenzService = achsenreferenzService;
            this.referenzgruppeService = referenzgruppeService;
        }
        
        public List<AchsenSegment> GetCurrentBySpatialFilter(IGeometry filtergeom)
        {
            return GetCurrentEntityListBySpatialFilter(filtergeom).ToList();
        }

        protected override void OnModelCreated(AchsenSegment entity, AchsenSegmentModel model)
        {
            base.OnModelCreated(entity, model);
            model.Name = entity.Achse.Name;
        }

        protected override void OnEntityUpdating(AchsenSegmentModel model, AchsenSegment entity)
        {
            base.OnEntityUpdating(model, entity);
            Envelope maxExtent = GisConstants.MaxExtent;
            foreach (ICoordinate coord in model.Shape.Coordinates)
            {
                if (!(coord.X >= maxExtent.MinX && coord.X <= maxExtent.MaxX && coord.Y >= maxExtent.MinY && coord.Y <= maxExtent.MaxY))
                {
                    throw new Exception(string.Format("Invalid Coordinate in Geometry:\"{0}\" in Achsensgment: {1}", entity.Shape.ToString(), entity.Id.ToString()));
                }
            }
            if (entity.Achse != null)
                entity.Achse.Name = model.Name;
        }

        public List<AchsenSegmentOverviewModel> GetCurrentModelsByName(string name)
        {
            var query = FilteredEntities;

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(a => a.Achse.Name.ToLower().Contains(name.ToLower()));

            return query.Select(a => new AchsenSegmentOverviewModel { AchsenName = a.Achse.Name, Id = a.Id }).ToList();
        }

        protected override Expression<Func<AchsenSegment, Mandant>> MandantExpression { get { return sGis => sGis.Mandant; } }
        protected override Expression<Func<AchsenSegment, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return sGis => sGis.ErfassungsPeriod; } }
                
        public string GetNearestAchsenSegment(double x, double y,  double tolerance)
        {
            IGeometry clickedPoint = EMSG.Business.Services.GIS.GISService.CreateGeometryFactory().CreatePoint(new Coordinate((double)x, (double)y, 0));
            var buffer = clickedPoint.Buffer(tolerance);

            IList<AchsenSegment> achsegmentList = GetCurrentEntityListBySpatialFilter(buffer);
            AchsenSegment selectedAchsensegment = gisService.GetNearestGeometry(clickedPoint, achsegmentList);

            TextWriter tw = new StringWriter();
            IAttributesTable table = new AttributesTable();                  

            //GEOJSON PROPERTIES: STRASSENABSCHNITTE            

            List<FeatureWithID> features = new List<FeatureWithID>();
            if (selectedAchsensegment == null) {
                return GeoJSONStrings.GeoJSONFailure("No Achsen found");
            }
            foreach (AchsenReferenz achsenreferenz in selectedAchsensegment.AchsenReferenzen.Where( ar => ar.Erfassungsperiod == historizationService.GetCurrentErfassungsperiod()))
            {
                if (achsenreferenz.ReferenzGruppe.StrassenabschnittGIS != null)
                {
                    FeatureWithID feat = new FeatureWithID();
                    IAttributesTable att = new AttributesTable();

                    feat.Id = achsenreferenz.ReferenzGruppe.StrassenabschnittGIS.Id.ToString();
                    feat.Geometry = achsenreferenz.ReferenzGruppe.StrassenabschnittGIS.Shape;
                    feat.Attributes = att;

                    if (!features.Contains(feat))
                    {
                        features.Add(feat);
                    }
                }                
            }

            table.AddAttribute("Strassenabschnitte", features);

            table.AddAttribute("AchsenId", selectedAchsensegment.Achse.Id);
            table.AddAttribute("AchsenName", selectedAchsensegment.Achse.Name);
            table.AddAttribute("IsInverted", selectedAchsensegment.IsInverted);

            FeatureWithID feature = new FeatureWithID();
            feature.Id = selectedAchsensegment.Id.ToString();
            feature.Geometry = selectedAchsensegment.Shape;
            feature.Attributes = table;
            TextWriter sw = new StringWriter();
            GeoJSONWriter.WriteFeatureWithID(feature, sw);

            return sw.ToString();            
        }

        protected override void OnEntityCreating(AchsenSegmentModel model, AchsenSegment entity) {
            if (entity.Achse == null)
            {
                //Create and save Dummy Achse
                Achse achse = new Achse();
                achse.VersionValidFrom = DateTime.Now;
                achse.Id = Guid.NewGuid();
                achse.Name = model.Name;
                achse.Mandant = this.CurrentMandant;
                achse.ErfassungsPeriod = this.CurrentErfassungsPeriod;
                achse.AchsenSegmente.Add(entity);
                entity.Achse = achse;
                this.transactionScopeProvider.CurrentTransactionScope.Session.Save(achse);
                model.AchsenId = achse.Id;
            }
        }
        public string GetAchsensegmentCollection(string achsensegmentIds)
        {
            var splitachsensegmentIds = achsensegmentIds.Split(',');
            List<FeatureWithID> achsensegmente = new List<FeatureWithID>();
            StringWriter sw = new StringWriter();
            
            foreach(string achsensegmentId in splitachsensegmentIds)
            {                
                AchsenSegment aseg = GetEntityById(Guid.Parse(achsensegmentId));
                FeatureWithID feat = new FeatureWithID();
                IAttributesTable att = new AttributesTable();
                List<FeatureWithID> strassenabschnitte = new List<FeatureWithID>();
                
                foreach (AchsenReferenz achsenreferenz in aseg.AchsenReferenzen.Where( a => a.Erfassungsperiod == historizationService.GetCurrentErfassungsperiod()))
                {
                    if (achsenreferenz.ReferenzGruppe.StrassenabschnittGIS != null)
                    {
                        FeatureWithID strabsfeat = new FeatureWithID();
                        IAttributesTable strabsatt = new AttributesTable();

                        strabsfeat.Id = achsenreferenz.ReferenzGruppe.StrassenabschnittGIS.Id.ToString();
                        strabsfeat.Geometry = achsenreferenz.ReferenzGruppe.StrassenabschnittGIS.Shape;
                        strabsfeat.Attributes = strabsatt;
                                                
                        if (!strassenabschnitte.Contains(strabsfeat))
                        {
                            strassenabschnitte.Add(strabsfeat);
                        }
                    }
                }
                att.AddAttribute("AchsenId", aseg.Achse.Id);
                att.AddAttribute("AchsenName", aseg.Achse.Name);
                att.AddAttribute("IsInverted", aseg.IsInverted);
                att.AddAttribute("Strassenabschnitte", strassenabschnitte);
                feat.Geometry = aseg.Shape;
                feat.Id = aseg.Id.ToString();
                feat.Attributes = att;
                achsensegmente.Add(feat);
            }
            
            GeoJSONWriter.WritewithID(achsensegmente, sw);
            sw.Flush();
            
            return sw.ToString();            
        }

        public string GetMandantAxisEnvelope()
        {
            var session=transactionScopeProvider.CurrentTransactionScope.Session;
            var crit = session.CreateCriteria(typeof(AchsenSegment));
            crit.Add(Restrictions.Eq(ExpressionHelper.GetPropertyName<AchsenSegment, ErfassungsPeriod>(e => e.ErfassungsPeriod), this.CurrentErfassungsPeriod));
            crit.SetProjection(SpatialProjections.Envelope(ExpressionHelper.GetPropertyName<AchsenSegment, IGeometry>(e => e.Shape)));
            IGeometry geom = crit.UniqueResult()as IGeometry;
            Envelope envelope;
            if (geom != null)
            {
                envelope = geom.EnvelopeInternal;
            }
            else
            {
                envelope = new Envelope();
            }
            
            return string.Format(CultureInfo.InvariantCulture.NumberFormat,"{0}, {1}, {2}, {3}", envelope.MinX, envelope.MinY, envelope.MaxX, envelope.MaxY);
        }

    }
}
