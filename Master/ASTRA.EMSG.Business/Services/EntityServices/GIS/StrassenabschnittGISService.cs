using System;
using System.Linq;
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
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;
using NHibernate.Util;
using System.Collections.Generic;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.LinearReferencing;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public class StrassenabschnittGISOverviewFilter : IStrassennameFilter
    {
        public string Strassenname { get; set; }
        public string Ortsbezeichnung { get; set; }
    }

    public interface IStrassenabschnittGISService : IService
    {
        StrassenabschnittGISModel GetCurrentStrassenabschnittAt(double x, double y, double tolerance);
        List<StrassenabschnittGIS> GetCurrentBySpatialFilter(IGeometry filtergeom);
        bool IsThereMissingZustandsabschnitte();
        //bool ValidateStrassenabschnitt(StrassenabschnittGISModel strassenabschnittgismodel);
        bool ZustandsabschnittWithinStrassenabschnitt(StrassenabschnittGISModel strassenabschnittgismodel);
        bool IsStrassenabschnittOnAchsensegment(StrassenabschnittGISModel strassenabschnittgismodel);
        StrassenabschnittGISModel GetById(Guid id);
        IQueryable<StrassenabschnittGIS> GetEntitiesBy(ErfassungsPeriod getErfassungsPeriod);
        IQueryable<StrassenabschnittGIS> GetEntitiesBy(ErfassungsPeriod getErfassungsPeriod, Mandant mandant);
        void DeleteEntity(Guid id);
        StrassenabschnittGISModel CreateEntity(StrassenabschnittGISModel strassenabschnittGisModel);
        StrassenabschnittGISModel UpdateEntity(StrassenabschnittGISModel strassenabschnittGisModel);

        decimal GetSumOfZustandsabschnittLaenge(Guid strassenabschnittId);
        void SplitStrassenabschnittGISAtXY(Guid strassenabschnittId, string x, string y);
        bool AreThereLockedStrassenabschnitte();
        List<StrassenabschnittOverviewGISModel> GetCurrentModelsByStrassenname(string strassennameFilter, string ortsbezeichnungFilter);
    }

    public class StrassenabschnittGISService : MandantAndErfassungsPeriodDependentEntityServiceBase<StrassenabschnittGIS, StrassenabschnittGISModel>, IStrassenabschnittGISService
    {
        private readonly IAchsenReferenzService achsenReferenzService;
        private readonly IReferenzGruppeService referenzGruppeService;
        private readonly IZustandsabschnittGISService zustandsabschnittGISService;
        private readonly IGISService gisService;
        private readonly IGeoJSONParseService geoJSONParseService;
        private readonly IFahrbahnZustandGISService fahrbahnZustandService;
        private readonly IAchsenSegmentService achsenSegmentService;
        private readonly IFiltererFactory filtererFactory;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly ILocalizationService localizationService;


        public StrassenabschnittGISService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine,
            ISecurityService securityService, 
            IHistorizationService historizationService, 
            IAchsenReferenzService achsenReferenzService,
            IReferenzGruppeService referenzGruppeService, 
            IGISService gisService, 
            IZustandsabschnittGISService zustandsabschnittGISService, 
            IGeoJSONParseService geoJSONParseService, 
            IFahrbahnZustandGISService fahrbahnZustandService,
            IAchsenSegmentService achsenSegmentService,
            IFiltererFactory filtererFactory, 
            ILocalizationService localizationService)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.transactionScopeProvider = transactionScopeProvider;
            this.achsenSegmentService = achsenSegmentService;
            this.filtererFactory = filtererFactory;
            this.localizationService = localizationService;
            this.achsenReferenzService = achsenReferenzService;
            this.referenzGruppeService = referenzGruppeService; 
            this.zustandsabschnittGISService = zustandsabschnittGISService;
            this.gisService = gisService;
            this.geoJSONParseService = geoJSONParseService;
            this.fahrbahnZustandService = fahrbahnZustandService;
        }
        
        protected override Expression<Func<StrassenabschnittGIS, Mandant>> MandantExpression { get { return sGis => sGis.Mandant; } }
        protected override Expression<Func<StrassenabschnittGIS, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return sGis => sGis.ErfassungsPeriod; } }

        public override List<StrassenabschnittGISModel> GetCurrentModels()
        {
            return FilteredEntities.Fetch(rms => rms.Belastungskategorie).Select(CreateModel).ToList();
        }

        /// <summary>
        /// Updates the Entity from the Model and handle update db relations logic
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entity"></param>
        protected override void UpdateEntityFieldsInternal(StrassenabschnittGISModel model, StrassenabschnittGIS entity)
        {
            if (model.Id != Guid.Empty)
            {
                DeleteReferenzen(model.Id);
            }
            model.ReferenzGruppeModel= referenzGruppeService.CreateEntity(model.ReferenzGruppeModel);
            entity.ReferenzGruppe = GetEntityById<ReferenzGruppe>(model.ReferenzGruppeModel.Id);
            base.UpdateEntityFieldsInternal(model, entity);
        }

        public List<StrassenabschnittGIS> GetCurrentBySpatialFilter(IGeometry filtergeom)
        {
            return GetCurrentEntityListBySpatialFilter(filtergeom).ToList();
        }

        public StrassenabschnittGISModel GetCurrentStrassenabschnittAt(double x, double y, double tolerance)
        {

            IGeometry clickedPoint = GISService.CreateGeometryFactory().CreatePoint(new Coordinate((double)x, (double)y, 0));
            var buffer = clickedPoint.Buffer(tolerance);

            //only strassenabschnitte from current erfassungsperiode
            ErfassungsPeriod currentErfassungsperiod = historizationService.GetCurrentErfassungsperiod();
            IList<StrassenabschnittGIS> strabsliste = GetCurrentBySpatialFilter(buffer);

            StrassenabschnittGIS strabs = gisService.GetNearestGeometry(clickedPoint, strabsliste);

            return new StrassenabschnittGISModel { FeatureGeoJSONString = geoJSONParseService.GenerateGeoJsonStringFromEntity(strabs) };
        }

        private void DeleteReferenzen(Guid strassenabschnittID)
        {
            StrassenabschnittGIS strabs = GetEntityById(strassenabschnittID);

            if (strabs != null && strabs.ReferenzGruppe.Id != Guid.Empty)
            {
                List<AchsenReferenz> arList = achsenReferenzService.GetAchsenReferenzGruppe(strabs.ReferenzGruppe.Id);
                foreach (AchsenReferenz ar in arList)
                {
                    achsenReferenzService.DeleteEntity(ar.Id);
                }
            }

            if (strabs != null && strabs.ReferenzGruppe.Id != Guid.Empty)
                referenzGruppeService.DeleteEntity(strabs.ReferenzGruppe.Id);
        }

        protected override void OnModelCreated(StrassenabschnittGIS entity, StrassenabschnittGISModel model)
        {
            model.FeatureGeoJSONString = geoJSONParseService.GenerateGeoJsonStringFromEntity(entity);
        }

        protected override void OnEntityUpdating(StrassenabschnittGIS entity)
        {
            base.OnEntityUpdating(entity);
            if (entity.IsLocked)
                throw new InvalidOperationException("StrassenabschnittGIS is locked.");
        }

        protected override void OnEntityUpdating(StrassenabschnittGISModel model, StrassenabschnittGIS entity)
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

        public bool IsThereMissingZustandsabschnitte()
        {
            return FilteredEntities.Any(s => !s.Zustandsabschnitten.Any());
        }

               

        public bool ZustandsabschnittWithinStrassenabschnitt(StrassenabschnittGISModel strassenabschnittgismodel)
        {
            
            //check if all zabs associated with the strabs still fit on it
            
            //dont check new strabs
            Guid strassenabschnittsid = strassenabschnittgismodel.Id;
            if (strassenabschnittsid != Guid.Empty)
            {
                
                //get the shape of the strabs
                IGeometry neu_strabsShape = strassenabschnittgismodel.Shape;
                neu_strabsShape.SRID = GisConstants.SRID;

                //get all zabs on the strabs
                IQueryable<ZustandsabschnittGIS> zustandsabschnitte = zustandsabschnittGISService.GetCurrentEntities();
                zustandsabschnitte = zustandsabschnitte.Where(zabs => zabs.StrassenabschnittGIS.Id == strassenabschnittsid);

                foreach (ZustandsabschnittGIS zabs in zustandsabschnitte)
                {
                    if (!gisService.CheckGeometriesIsInControlGeometry(zabs.ReferenzGruppe.AchsenReferenzen, neu_strabsShape))
                        return false;
                }
            }
            return true;
        }

        public bool IsStrassenabschnittOnAchsensegment(StrassenabschnittGISModel strassenabschnittgismodel)
        {
            foreach (var ar in strassenabschnittgismodel.ReferenzGruppeModel.AchsenReferenzenModel)
            {
                var achse = achsenSegmentService.GetById(ar.AchsenSegment);
                if (!gisService.CheckGeometriesIsInControlGeometry(new List<AchsenReferenzModel>(){ar}, achse.Shape))
                {
                    return false;
                }
            }
            return true;
        }

        private StrassenabschnittOverviewGISModel CreateStrassenabschnittOverviewGISModel(StrassenabschnittGIS strassenabschnittGIS)
        {
            var strassenabschnittOverviewGISModel = CreateModelFromEntity<StrassenabschnittOverviewGISModel>(strassenabschnittGIS);
            strassenabschnittOverviewGISModel.BelastungskategorieBezeichnung = localizationService.GetLocalizedBelastungskategorieTyp(strassenabschnittGIS.BelastungskategorieTyp);
            if (!strassenabschnittGIS.Zustandsabschnitten.Any())
                SetErfassungsStatusBezeichnung(strassenabschnittOverviewGISModel, ErfassungsStatusTyp.Nein);
            else if (strassenabschnittGIS.Laenge == strassenabschnittGIS.Zustandsabschnitten.Sum(z => z.Laenge))
                SetErfassungsStatusBezeichnung(strassenabschnittOverviewGISModel, ErfassungsStatusTyp.Ja);
            else
                SetErfassungsStatusBezeichnung(strassenabschnittOverviewGISModel, ErfassungsStatusTyp.Teilweise);

            return strassenabschnittOverviewGISModel;
        }

        private void SetErfassungsStatusBezeichnung(StrassenabschnittOverviewGISModel strassenabschnittOverviewGISModel, ErfassungsStatusTyp erfassungsStatusTyp)
        {
            strassenabschnittOverviewGISModel.ErfassungsStatusBezeichnung = localizationService.GetLocalizedEnum(erfassungsStatusTyp);
        }
        
        protected override void OnEntityDeleting(StrassenabschnittGIS entity)
        {
            base.OnEntityDeleting(entity);
            if (entity.BelongsToInspektionsroute)
                throw new InvalidOperationException("Strassenabschnitt can not be deleted because it belongs to an Inspektionsroute already!");
        }

        public decimal GetSumOfZustandsabschnittLaenge(Guid strassenabschnittId)
        {
            StrassenabschnittGIS entityById = GetEntityById(strassenabschnittId);
            return entityById == null ? 0 : entityById.Zustandsabschnitten.Sum(za => za.Laenge);
        }

        public void SplitStrassenabschnittGISAtXY(Guid strassenabschnittId, string x, string y) 
        {
            StrassenabschnittGIS strassenabschnittToSplit = GetEntityById(strassenabschnittId);

            //check whether the strassenabschnitt (inspektionsroute) is checked out (=locked)
            if (strassenabschnittToSplit.IsLocked)
                return;

            //1. find achsenref. to split
            IGeometry splitPoint = GISService.CreateGeometryFactory().CreatePoint(new Coordinate(double.Parse(x, System.Globalization.NumberFormatInfo.InvariantInfo), double.Parse(y, System.Globalization.NumberFormatInfo.InvariantInfo), 0));
            AchsenReferenz achsenreferenceToSplit = gisService.GetNearestGeometry(splitPoint, strassenabschnittToSplit.ReferenzGruppe.AchsenReferenzen);

            //2. split achsenref
            LengthIndexedLine line = new LengthIndexedLine(achsenreferenceToSplit.Shape);
            IGeometry split1 = line.ExtractLine(0, line.IndexOf(splitPoint.Coordinate));
            IGeometry split2 = line.ExtractLine(line.IndexOf(splitPoint.Coordinate), line.EndIndex);

            //create new strassenabschnitte
            StrassenabschnittGIS copiedStrassenabschnittGIS1 = PrepareNewStrassenabschnitt(strassenabschnittToSplit, achsenreferenceToSplit, split1);
            StrassenabschnittGIS copiedStrassenabschnittGIS2 = PrepareNewStrassenabschnitt(strassenabschnittToSplit, achsenreferenceToSplit, split2);

            //3. relate other achsenrefs to the new two references
            foreach(AchsenReferenz achsref in strassenabschnittToSplit.ReferenzGruppe.AchsenReferenzen.Where(ac => !ac.Equals(achsenreferenceToSplit)))
            {
                if (achsref.Shape.Distance(split1) <= achsref.Shape.Distance(split2))
                {
                    copiedStrassenabschnittGIS1.ReferenzGruppe.AddAchsenReferenz(PrepareAchsenreferenz(achsref));
                    copiedStrassenabschnittGIS1.Shape = copiedStrassenabschnittGIS1.Shape.Union(achsref.Shape);
                }
                else
                {
                    copiedStrassenabschnittGIS2.ReferenzGruppe.AddAchsenReferenz(PrepareAchsenreferenz(achsref));
                    copiedStrassenabschnittGIS2.Shape = copiedStrassenabschnittGIS2.Shape.Union(achsref.Shape);
                }
            }

            copiedStrassenabschnittGIS1.Laenge = getLength(copiedStrassenabschnittGIS1);
            copiedStrassenabschnittGIS2.Laenge = getLength(copiedStrassenabschnittGIS2);
            

            //update inspektionsroute
            strassenabschnittToSplit.InspektionsRtStrAbschnitte.ForEach(s => s.InspektionsRouteGIS.AddStrassenabschnittGIS(copiedStrassenabschnittGIS1));
            strassenabschnittToSplit.InspektionsRtStrAbschnitte.ForEach(s => s.InspektionsRouteGIS.AddStrassenabschnittGIS(copiedStrassenabschnittGIS2));
            strassenabschnittToSplit.InspektionsRtStrAbschnitte.ForEach(s => s.InspektionsRouteGIS.RemoveStrassenabschnittGIS(strassenabschnittToSplit));


            //5. save/delete splitted strassenabschnitte
            Delete(strassenabschnittToSplit);

            CreateEntity(copiedStrassenabschnittGIS1);
            CreateEntity(copiedStrassenabschnittGIS2);
        }

        private decimal getLength(StrassenabschnittGIS strabs)
        {
            decimal length = 0;
            foreach (AchsenReferenz achsref in strabs.ReferenzGruppe.AchsenReferenzen)
            {
                LineString line = achsref.Shape as LineString;
                length += Convert.ToDecimal(Math.Round(line.Length,1));
            }
            return length;
        }

        private StrassenabschnittGIS PrepareNewStrassenabschnitt(StrassenabschnittGIS strassenabschnittToCopy, AchsenReferenz achsenreferenzPart, IGeometry newGeometry)
        {
            StrassenabschnittGIS copiedStrassenabschnittGIS = entityServiceMappingEngine.Translate<StrassenabschnittGIS, StrassenabschnittGIS>(strassenabschnittToCopy);
            copiedStrassenabschnittGIS.ErfassungsPeriod = CurrentErfassungsPeriod;
            copiedStrassenabschnittGIS.Mandant = CurrentMandant;
            copiedStrassenabschnittGIS.Shape = newGeometry;

            //LineString line = newGeometry as LineString;
            //copiedStrassenabschnittGIS.Laenge = (decimal)Math.Round(line.Length,1);

            copiedStrassenabschnittGIS.ReferenzGruppe =  new ReferenzGruppe{
                Mandant = CurrentMandant,
                Erfassungsperiod = CurrentErfassungsPeriod,
            };

            achsenreferenzPart.Shape = newGeometry;
            copiedStrassenabschnittGIS.ReferenzGruppe.AchsenReferenzen.Add(PrepareAchsenreferenz(achsenreferenzPart));
            return copiedStrassenabschnittGIS;
        }

        private AchsenReferenz PrepareAchsenreferenz(AchsenReferenz achsenreferenzToCopy)
        {
            AchsenReferenz copiedAchsensreferenz = entityServiceMappingEngine.Translate<AchsenReferenz, AchsenReferenz>(achsenreferenzToCopy);
            copiedAchsensreferenz.Mandandt = CurrentMandant;
            copiedAchsensreferenz.Erfassungsperiod = CurrentErfassungsPeriod;
            return copiedAchsensreferenz;
        }
        public bool AreThereLockedStrassenabschnitte()
        {
            return GetCurrentEntities().Any(stg => stg.IsLocked);
        }

        public List<StrassenabschnittOverviewGISModel> GetCurrentModelsByStrassenname(string strassennameFilter, string ortsbezeichnungFilter)
        {
            var query = FilteredEntities;

            if (!string.IsNullOrWhiteSpace(strassennameFilter))
                query = query.Where(s => s.Strassenname.ToLower().Contains(strassennameFilter.ToLower()));

            if (!string.IsNullOrWhiteSpace(ortsbezeichnungFilter))
                query = query.Where(s => s.Ortsbezeichnung.ToLower().Contains(ortsbezeichnungFilter.ToLower()));

            return query
                .Select(s => new StrassenabschnittGIS
                {
                    Id = s.Id,
                    Strassenname = s.Strassenname,
                    Abschnittsnummer = s.Abschnittsnummer,
                    BezeichnungVon = s.BezeichnungVon,
                    BezeichnungBis = s.BezeichnungBis,
                    Ortsbezeichnung = s.Ortsbezeichnung,
                    Belastungskategorie = s.Belastungskategorie,
                    Laenge = s.Laenge,
                    BreiteFahrbahn = s.BreiteFahrbahn,
                    BreiteTrottoirLinks = s.BreiteTrottoirLinks,
                    BreiteTrottoirRechts = s.BreiteTrottoirRechts,
                    Zustandsabschnitten = s.Zustandsabschnitten
                        .Select(z => new ZustandsabschnittGIS
                        {
                            Laenge = z.Laenge
                        }).ToHashSet()
                })
                .ToList()
                .Select(CreateStrassenabschnittOverviewGISModel).ToList();
        }
    }
}
