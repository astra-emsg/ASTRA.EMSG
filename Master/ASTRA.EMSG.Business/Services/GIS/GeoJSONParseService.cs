using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using GeoJSON;
using NetTopologySuite.Features;
using System.IO;
using ASTRA.EMSG.Common.Enums;
using GeoAPI.Geometries;
using ASTRA.EMSG.Common.Utils;
using ASTRA.EMSG.Common;
using NetTopologySuite.Geometries;

namespace ASTRA.EMSG.Business.Services.GIS
{
    public interface IGeoJSONParseService : IService
    {
        string GenerateGeoJsonStringFromEntity(IAbschnittGISBase strassenAbschnittGis);
        string GenerateGeoJsonStringFromEntity(InspektionsRouteGIS inspektionsroute);
        IAbschnittGISModelBase GenerateModelFromGeoJsonString(IAbschnittGISModelBase model);
        string GenereateGeoJsonStringfromEntities(IEnumerable<IAbschnittGISBase> entities);
        string GenereateGeoJsonStringfromAchsenSegment(IEnumerable<AchsenSegment> entities);
        bool isAbschnittGISModelBaseValid(IAbschnittGISModelBase model, string geoJsonString);
        bool isAchsenSegmentModelValid(AchsenSegmentModel achsenSegmentModel, string geoJsonString);
        AchsenSegmentModel GenerateAchsenSegmentModelFromGeoJsonString(AchsenSegmentModel model);
    }
    public class GeoJSONParseService : IGeoJSONParseService
    {
        private readonly IAchsenReferenzService achsenReferenzService;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        //private readonly IZustandsabschnittGISService zustandsabschnittGISService;
        
        private readonly IReferenzGruppeService referenzGruppeService;
        private readonly IGISService gisService;
        private readonly IAchsenSegmentService achsenSegmentService;
        private readonly ILocalizationService localizationService;
        private const string geoJSONAttribute_childs = "childs";
        private const string geoJSONAttribute_AchsenId = "AchsenId";
        private const string geoJSONAttribute_AchsenName = "AchsenName";
        private const string geoJSONAttribute_IsInverted = "IsInverted";
        private const string geoJSONAttribute_AchsenSegmentId = "AchsenSegmentId";
        private const string geoJSONAttribute_Strassenname = "Strassenname";
        private const string geoJSONAttribute_Zustandsabschnitte = "Zustandsabschnitte";
        private const string geoJSONAttribute_Strassenabschnitte = "Strassenabschnitte";
        private const string geoJSONAttribute_StrassenabschnittsID = "StrassenabschnittsID";
        private const string geoJSONAttribute_InspektionsrouteID = "InspektionsrouteID";
        private const string geoJSONAttribute_InspektionsrouteBezeichnung = "InspektionsrouteBezeichnung";
        private const string geoJSONAttribute_IsLocked = "IsLocked";


        public GeoJSONParseService(IAchsenReferenzService achsenReferenzService, IGISService gisService, IAchsenSegmentService achsenSegmentService, IReferenzGruppeService referenzGruppeService, ITransactionScopeProvider transactionScopeProvider, ILocalizationService localizationService)
        {
            this.achsenReferenzService = achsenReferenzService;
            //this.zustandsabschnittGISService = zustandsabschnittGISService;
            this.achsenSegmentService = achsenSegmentService;
            this.referenzGruppeService = referenzGruppeService;
            this.transactionScopeProvider = transactionScopeProvider;
            this.gisService = gisService;
            this.localizationService = localizationService;
        }
        public IAbschnittGISModelBase GenerateModelFromGeoJsonString(IAbschnittGISModelBase model)
        {
            string geoJSONString = model.FeatureGeoJSONString;
            List<FeatureWithID> childs = GeoJSONReader.ReadFeatureWithID(new StringReader(model.FeatureGeoJSONString)).Attributes[geoJSONAttribute_childs] as List<FeatureWithID>;
            ReferenzGruppeModel referenzGruppeModel = new ReferenzGruppeModel();

            foreach (FeatureWithID child in childs)
            {
                AchsenReferenzModel newAchsenref = new AchsenReferenzModel();
                newAchsenref.AchsenSegmentModel = achsenSegmentService.GetById(Guid.Parse(child.Attributes[geoJSONAttribute_AchsenSegmentId].ToString()));
                newAchsenref.Shape = child.Geometry;
                newAchsenref.Shape.SRID = GisConstants.SRID;

                referenzGruppeModel.AddAchsenReferenz(newAchsenref);
            }

           
            model.Shape = GeoJSONReader.ReadFeatureWithID(new StringReader(model.FeatureGeoJSONString)).Geometry;
            model.Shape.SRID = GisConstants.SRID;
            model.ReferenzGruppeModel = referenzGruppeModel;
            return model;
        }

        public AchsenSegmentModel GenerateAchsenSegmentModelFromGeoJsonString(AchsenSegmentModel model) {
            string geoJSONString = model.FeatureGeoJSONString;
            FeatureWithID feature = GeoJSONReader.ReadFeatureWithID(new StringReader(model.FeatureGeoJSONString));
            string invertedString = feature.Attributes[geoJSONAttribute_IsInverted] as string;
            bool? invertedbool = feature.Attributes[geoJSONAttribute_IsInverted] as bool?;
            model.IsInverted = (invertedString != null && invertedString.ToLower() == "true") || (invertedbool != null && invertedbool == true);
            //model.IsInverted = (bool)feature.Attributes[geoJSONAttribute_IsInverted];
            model.Shape = feature.Geometry;
            model.Shape.SRID = GisConstants.SRID;
            return model;
        }

        public string GenerateGeoJsonStringFromEntity(IAbschnittGISBase entity)
        {
            if (entity == null)
                return "{ \"type\": \"FeatureCollection\", \"features\": []}";


            FeatureWithID feature = getFeatureWithId(entity);
            TextWriter sw = new StringWriter();

            GeoJSONWriter.WriteWithID(feature, sw);
            string geoJSONstring = sw.ToString();

            return geoJSONstring;
        }

        public string GenereateGeoJsonStringfromEntities(IEnumerable<IAbschnittGISBase> entities)
        {
            IList<FeatureWithID> features = new List<FeatureWithID>();

            foreach (IAbschnittGISBase entity in entities)
            {
                features.Add(getFeatureWithId(entity));
            }
            TextWriter sw = new StringWriter();

            GeoJSONWriter.WritewithID(features, sw);
            string geoJSONstring = sw.ToString();

            return geoJSONstring;
        }
        public string GenereateGeoJsonStringfromAchsenSegment(IEnumerable<AchsenSegment> entities) {
            List<FeatureWithID> achsensegmente = new List<FeatureWithID>();
            StringWriter sw = new StringWriter();
            foreach (var aseg in entities)
            {
                FeatureWithID feat = new FeatureWithID();
                IAttributesTable att = new AttributesTable();
                att.AddAttribute(geoJSONAttribute_AchsenId, aseg.Achse.Id);
                att.AddAttribute(geoJSONAttribute_AchsenName, aseg.Achse.Name);
                att.AddAttribute(geoJSONAttribute_IsInverted, aseg.IsInverted);
                feat.Geometry = aseg.Shape;
                feat.Id = aseg.Id.ToString();
                feat.Attributes = att;
                achsensegmente.Add(feat);
            }
            GeoJSONWriter.WritewithID(achsensegmente, sw);
            sw.Flush();

            return sw.ToString();     
        }
        private FeatureWithID getFeatureWithId(IAbschnittGISBase entity)
        {
            List<AchsenReferenz> achsenreferenzListe = achsenReferenzService.GetAchsenReferenzGruppe(entity.ReferenzGruppe.Id);

            //create geojson
            List<FeatureWithID> featuresListChilds = new List<FeatureWithID>();

            FeatureWithID feature = new FeatureWithID();
            IAttributesTable attribute = new AttributesTable();
            feature.Id = entity.Id.ToString();
            feature.Geometry = entity.Shape;

            //GEOJSON PROPERTIES: Childs (= Achsenreferenzen)

            foreach (AchsenReferenz ar in achsenreferenzListe)
            {
                FeatureWithID feat = new FeatureWithID();
                IAttributesTable attributes = new AttributesTable();

                feat.Id = ar.Id.ToString();
                feat.Geometry = ar.Shape;
                attributes.AddAttribute(geoJSONAttribute_AchsenSegmentId, ar.AchsenSegment.Id);
                attributes.AddAttribute(geoJSONAttribute_IsInverted, ar.AchsenSegment.IsInverted);
                feat.Attributes = attributes;
                featuresListChilds.Add(feat);
            }

            attribute.AddAttribute(geoJSONAttribute_childs, featuresListChilds);

            if (entity is KoordinierteMassnahmeGIS)
            {
                attribute.AddAttribute("Name", ((KoordinierteMassnahmeGIS)entity).Projektname);
            }
            if (entity is MassnahmenvorschlagTeilsystemeGIS)
            {
                attribute.AddAttribute("Name", ((MassnahmenvorschlagTeilsystemeGIS)entity).Projektname);
                attribute.AddAttribute("System", localizationService.GetLocalizedEnum<TeilsystemTyp>(((MassnahmenvorschlagTeilsystemeGIS)entity).Teilsystem));
            }
            //GEOJSON PROPERTIES: ZUSTANDSABSCHNITTE
            if (entity is StrassenabschnittGIS)
            {
                List<FeatureWithID> featuresListZustandsabschnitte = new List<FeatureWithID>();

                List<ZustandsabschnittGIS> zustandsabschnitte = transactionScopeProvider.Queryable<ZustandsabschnittGIS>().Where(za => za.StrassenabschnittGIS.Id == entity.Id).ToList();
                foreach (ZustandsabschnittGIS zustandsabschnitt in zustandsabschnitte)
                {

                    FeatureWithID feat = new FeatureWithID();
                    IAttributesTable att = new AttributesTable();

                    feat.Id = zustandsabschnitt.Id.ToString();
                    feat.Geometry = zustandsabschnitt.Shape;
                    feat.Attributes = att;

                    featuresListZustandsabschnitte.Add(feat);

                }
                var sa = entity as StrassenabschnittGIS;

                attribute.AddAttribute(geoJSONAttribute_Zustandsabschnitte, featuresListZustandsabschnitte);
                attribute.AddAttribute(geoJSONAttribute_IsLocked, sa.IsLocked);
                if (sa.InspektionsRtStrAbschnitte.Count > 0)
                {
                    attribute.AddAttribute(geoJSONAttribute_InspektionsrouteID, sa.InspektionsRtStrAbschnitte.SingleOrDefault().InspektionsRouteGIS.Id);
                }
                else
                {
                    attribute.AddAttribute(geoJSONAttribute_InspektionsrouteID, "");
                }

            }
            else
            {
                if (entity is ZustandsabschnittGIS)
                {
                    attribute.AddAttribute(geoJSONAttribute_StrassenabschnittsID, ((ZustandsabschnittGIS)entity).StrassenabschnittGIS.Id);
                    var za = entity as ZustandsabschnittGIS;
                    attribute.AddAttribute(geoJSONAttribute_IsLocked, za.IsLocked);
                }
            }
            feature.Attributes = attribute;
            return feature;
        }

        public string GenerateGeoJsonStringFromEntity(InspektionsRouteGIS inspektionsroute)
        {
            if (inspektionsroute == null)
                return "{ \"type\": \"FeatureCollection\", \"features\": []}";

            FeatureWithID feature = new FeatureWithID();
            IAttributesTable attributes = new AttributesTable();

            IList<FeatureWithID> strassenabschnitte = new List<FeatureWithID>();


            feature.Id = inspektionsroute.Id.ToString();            
            feature.Geometry = inspektionsroute.Shape;
            attributes.AddAttribute(geoJSONAttribute_InspektionsrouteID, inspektionsroute.Id);
            attributes.AddAttribute(geoJSONAttribute_InspektionsrouteBezeichnung, inspektionsroute.Bezeichnung);
            attributes.AddAttribute(geoJSONAttribute_IsLocked, inspektionsroute.IsLocked);
                        
            //GEOJSON PROPERTIES: Strassenabschnitte
            foreach (InspektionsRtStrAbschnitte inspektionsrouteStrassenabschnitt in inspektionsroute.InspektionsRtStrAbschnitteList)
            {
                FeatureWithID featureIrs = new FeatureWithID();
                IAttributesTable attributeIrs = new AttributesTable();
                IList<AchsenReferenz> achsenreferenzenIrs = achsenReferenzService.GetAchsenReferenzGruppe(inspektionsrouteStrassenabschnitt.StrassenabschnittGIS.ReferenzGruppe.Id);
                IList<FeatureWithID> achsenreferenzen = new List<FeatureWithID>();

                featureIrs.Id = inspektionsrouteStrassenabschnitt.StrassenabschnittGIS.Id.ToString();
                featureIrs.Geometry = inspektionsrouteStrassenabschnitt.StrassenabschnittGIS.Shape;
                

                foreach (AchsenReferenz achsenreferenz in achsenreferenzenIrs)
                {
                    FeatureWithID featAr = new FeatureWithID();
                    IAttributesTable attAr = new AttributesTable();

                    featAr.Id = achsenreferenz.Id.ToString();
                    featAr.Geometry = achsenreferenz.Shape;
                    attAr.AddAttribute(geoJSONAttribute_AchsenSegmentId, achsenreferenz.AchsenSegment.Id);
                    featAr.Attributes = attAr;

                    achsenreferenzen.Add(featAr);
                }

                attributeIrs.AddAttribute(geoJSONAttribute_childs, achsenreferenzen);

                featureIrs.Attributes = attributeIrs;
                strassenabschnitte.Add(featureIrs);
            }

            attributes.AddAttribute(geoJSONAttribute_Strassenabschnitte, strassenabschnitte);
            feature.Attributes = attributes;

            TextWriter sw = new StringWriter();

            GeoJSONWriter.WriteWithID(feature, sw);
            string geoJSONstring = sw.ToString();

            return geoJSONstring;
        }

        public bool isAbschnittGISModelBaseValid(IAbschnittGISModelBase model, string geoJsonString)
        {

            IAbschnittGISModelBase parsedModel = this.GenerateModelFromGeoJsonString(model);
            IGeometry modelShape = parsedModel.Shape;

            var referenzGruppeModel = parsedModel.ReferenzGruppeModel;

            double achsRefLength = 0;
            foreach (var achsRef in referenzGruppeModel.AchsenReferenzenModel)
            {
                achsRefLength += achsRef.Shape.Length;
            }

            return (Math.Abs(modelShape.Length - achsRefLength) <= 0.5 && modelShape.Length != 0);

        }

        public bool isAchsenSegmentModelValid(AchsenSegmentModel achsenSegmentModel, string geoJsonString)
        {
            AchsenSegmentModel model = GenerateAchsenSegmentModelFromGeoJsonString(achsenSegmentModel);
            var isValid = true;
            Envelope maxExtent = GisConstants.MaxExtent;
            foreach (Coordinate coord in model.Shape.Coordinates)
            {
                if (!(coord.X >= maxExtent.MinX && coord.X <= maxExtent.MaxX && coord.Y >= maxExtent.MinY && coord.Y <= maxExtent.MaxY))
                {
                    isValid = false;
                }
            }
            if (!model.Shape.IsSimple)
            {
                isValid = false;
            }
            CoordinateList list = new CoordinateList(model.Shape.Coordinates, false);
            if (list.Count <= 1)
            {
                isValid = false;
            }
           
            return isValid;
        }

    }
}
