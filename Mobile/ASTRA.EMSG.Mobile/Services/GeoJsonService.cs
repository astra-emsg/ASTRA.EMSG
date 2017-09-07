using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoJSON;
using ASTRA.EMSG.Common.DataTransferObjects;
using NetTopologySuite.Features;
using System.IO;
using ASTRA.EMSG.Common.Utils;
using System.Globalization;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Mobile.Services
{
    public interface IGeoJsonService
    {
        string GenerateGeoJsonStringFromEntity(IDTOGeometryHolder entity);
        string GenerateGeoJsonStringFromEntities(IEnumerable<IDTOGeometryHolder> entities);
        ReferenzGruppeDTO GenerateReferenzGruppeFromGeoJson(string geoJson);
        
    }
    public class GeoJsonService : IGeoJsonService
    {
        private readonly IDTOService dtoService;

        private const string geoJSONAttribute_childs = "childs";
        private const string geoJSONAttribute_AchsenSegmentId = "AchsenSegmentId";
        private const string geoJSONAttribute_Strassenname = "Strassenname";
        private const string geoJSONAttribute_Zustandsabschnitte = "Zustandsabschnitte";
        private const string geoJSONAttribute_Strassenabschnitte = "Strassenabschnitte";
        private const string geoJSONAttribute_StrassenabschnittsID = "StrassenabschnittsID";
        private const string geoJSONAttribute_InspektionsrouteID = "InspektionsrouteID";
        private const string geoJSONAttribute_Zustandsindex = "Zustandsindex";
        private const string geoJSONAttribute_IsInverted = "IsInverted";
        private const string geoJSONAttribute_Trottoir = "Trottoir";
        private const string geoJSONAttribute_ZustandsabschnittId = "Zustandsabschnitt";
        private const string geoJSON_EmptyFeatureCollection = "{ \"type\": \"FeatureCollection\", \"features\": []}";

        public GeoJsonService(IDTOService dtoService)
        {
            this.dtoService = dtoService;
        }

        public IDTOGeometryHolder GenerateModelFromGeoJsonString(IReferenzGruppeDTOHolder model, string geoJSONString)
        {

            List<FeatureWithID> childs = GeoJSONReader.ReadFeatureWithID(new StringReader(geoJSONString)).Attributes[geoJSONAttribute_childs] as List<FeatureWithID>;
            ReferenzGruppeDTO referenzGruppeModel = new ReferenzGruppeDTO();

            foreach (FeatureWithID child in childs)
            {
                AchsenReferenzDTO newAchsenref = new AchsenReferenzDTO();
                newAchsenref.AchsenSegmentDTO = dtoService.GetDTOByID<AchsenSegmentDTO>(Guid.Parse(child.Attributes[geoJSONAttribute_AchsenSegmentId].ToString()));
                newAchsenref.Shape = child.Geometry;
                newAchsenref.Shape.SRID = GisConstants.SRID;

                referenzGruppeModel.AddAchsenReferenz(newAchsenref);
            }


            model.Shape = GeoJSONReader.ReadFeatureWithID(new StringReader(geoJSONString)).Geometry;
            model.Shape.SRID = GisConstants.SRID;
            model.ReferenzGruppeDTO = referenzGruppeModel;
            return model;
        }

        public string GenerateGeoJsonStringFromEntity(IDTOGeometryHolder entity)
        {
            if (entity == null)
                return geoJSON_EmptyFeatureCollection;


            FeatureWithID feature = getFeatureWithId(entity);
            TextWriter sw = new StringWriter();

            GeoJSONWriter.WriteWithID(feature, sw);
            string geoJSONstring = sw.ToString();

            return geoJSONstring;
        }

        public string GenerateGeoJsonStringFromEntities(IEnumerable<IDTOGeometryHolder> entities)
        {
            IList<FeatureWithID> features = new List<FeatureWithID>();

            foreach (IDTOGeometryHolder entity in entities)
            {
                features.Add(getFeatureWithId(entity));
            }
            TextWriter sw = new StringWriter();

            GeoJSONWriter.WritewithID(features, sw);
            string geoJSONstring = sw.ToString();

            return geoJSONstring;
        }

        private FeatureWithID getFeatureWithId(IDTOGeometryHolder entity)
        {
            

            //create geojson
            List<FeatureWithID> featuresListChilds = new List<FeatureWithID>();

            FeatureWithID feature = new FeatureWithID();
            IAttributesTable attribute = new AttributesTable();
            feature.Id = entity.Id.ToString();

            IGeometry shape2d = (IGeometry)entity.Shape.Clone();
            foreach (Coordinate coord in shape2d.Coordinates)
            {
                coord.Z = Double.NaN;
            }

            feature.Geometry = shape2d;



            //GEOJSON PROPERTIES: Childs (= Achsenreferenzen)
            if (entity is IReferenzGruppeDTOHolder)
            {
                IReferenzGruppeDTOHolder holder = entity as IReferenzGruppeDTOHolder;
                List<AchsenReferenzDTO> achsenreferenzListe = holder.ReferenzGruppeDTO.AchsenReferenzenDTO;
                foreach (AchsenReferenzDTO ar in achsenreferenzListe)
                {
                    FeatureWithID feat = new FeatureWithID();
                    IAttributesTable attributes = new AttributesTable();

                    feat.Id = ar.Id.ToString();
                    feat.Geometry = ar.Shape;
                    attributes.AddAttribute(geoJSONAttribute_AchsenSegmentId, ar.AchsenSegment);
                    AchsenSegmentDTO achs = null;
                    if (ar.AchsenSegmentDTO == null)
                    {
                        achs = (AchsenSegmentDTO)dtoService.GetDTOByID(ar.AchsenSegment);
                    }
                    else {
                        achs = ar.AchsenSegmentDTO;
                    }
                    attributes.AddAttribute(geoJSONAttribute_IsInverted, achs.IsInverted.ToString());
                    feat.Attributes = attributes;
                    featuresListChilds.Add(feat);
                }

                attribute.AddAttribute(geoJSONAttribute_childs, featuresListChilds);
                if (entity is AchsenSegmentDTO)
                {
                    AchsenSegmentDTO achs = entity as AchsenSegmentDTO;
                    attribute.AddAttribute(geoJSONAttribute_IsInverted, achs.IsInverted.ToString());
                }
               
                //GEOJSON PROPERTIES: ZUSTANDSABSCHNITTE
                if (entity is StrassenabschnittGISDTO)
                {
                    StrassenabschnittGISDTO sa = entity as StrassenabschnittGISDTO;
                    List<FeatureWithID> featuresListZustandsabschnitte = new List<FeatureWithID>();

                    IEnumerable<ZustandsabschnittGISDTO> zustandsabschnitte = this.dtoService.Get<ZustandsabschnittGISDTO>().Where(z => sa.ZustandsabschnittenId.Any(id=> id.Equals(z.Id)));
                    foreach (ZustandsabschnittGISDTO zustandsabschnitt in zustandsabschnitte)
                    {

                        FeatureWithID feat = new FeatureWithID();
                        IAttributesTable att = new AttributesTable();

                        feat.Id = zustandsabschnitt.Id.ToString();
                        feat.Geometry = zustandsabschnitt.Shape;
                        feat.Attributes = att;

                        featuresListZustandsabschnitte.Add(feat);

                    }

                    attribute.AddAttribute(geoJSONAttribute_Zustandsabschnitte, featuresListZustandsabschnitte);                   
                    
                    attribute.AddAttribute(geoJSONAttribute_InspektionsrouteID, sa.InspektionsRouteId);
                   

                }
                else
                {
                    if (entity is ZustandsabschnittGISDTO)
                    {
                        var za = entity as ZustandsabschnittGISDTO;
                        attribute.AddAttribute(geoJSONAttribute_StrassenabschnittsID, za.StrassenabschnittGIS);
                        attribute.AddAttribute(geoJSONAttribute_Zustandsindex, za.Zustandsindex.ToString(CultureInfo.InvariantCulture));
                        StrassenabschnittGISDTO strab = this.dtoService.GetDTOByID<StrassenabschnittGISDTO>(za.StrassenabschnittGIS);
                        IList<FeatureWithID> trottoirs = new List<FeatureWithID>();

                        if (strab.Trottoir == Common.Enums.TrottoirTyp.BeideSeiten || strab.Trottoir == Common.Enums.TrottoirTyp.Links)
                        {
                            FeatureWithID trottfeature = new FeatureWithID();
                            List<ILineString> lines = new List<ILineString>();
                            IAttributesTable attributes = new AttributesTable();
                            trottfeature.Id = Guid.NewGuid().ToString();
                            foreach (AchsenReferenzDTO referenz in strab.ReferenzGruppeDTO.AchsenReferenzenDTO)
                            {
                                AchsenSegmentDTO segment = this.dtoService.GetDTOByID<AchsenSegmentDTO>(referenz.AchsenSegment);
                                decimal offset = (decimal)(strab.BreiteFahrbahn + strab.BreiteTrottoirLinks) / 2;
                                if (segment.IsInverted)
                                {
                                    offset *= -1;
                                }
                                lines.Add(GeometryUtils.createOffsetLineNew(referenz.Shape.Factory, (ILineString)referenz.Shape, (double)offset));
                            }
                            IGeometry shape = za.Shape.Factory.CreateMultiLineString(lines.Where(l => l != null).ToArray());

                            trottfeature.Geometry = shape;
                            attributes.AddAttribute(geoJSONAttribute_ZustandsabschnittId, za.Id.ToString());
                            attributes.AddAttribute(geoJSONAttribute_Zustandsindex, (int)za.ZustandsindexTrottoirLinks);
                            trottfeature.Attributes = attributes;
                            trottoirs.Add(trottfeature);
                        }
                        if (strab.Trottoir == Common.Enums.TrottoirTyp.BeideSeiten || strab.Trottoir == Common.Enums.TrottoirTyp.Rechts)
                        {
                            FeatureWithID trottfeature = new FeatureWithID();
                            IAttributesTable attributes = new AttributesTable();
                            List<ILineString> lines = new List<ILineString>();
                            trottfeature.Id = Guid.NewGuid().ToString();
                            foreach (AchsenReferenzDTO referenz in za.ReferenzGruppeDTO.AchsenReferenzenDTO)
                            {
                                AchsenSegmentDTO segment = this.dtoService.GetDTOByID<AchsenSegmentDTO>(referenz.AchsenSegment);
                                decimal offset = (decimal)(strab.BreiteFahrbahn + strab.BreiteTrottoirRechts) / 2;
                                if (!segment.IsInverted)
                                {
                                    offset *= -1;
                                }
                                lines.Add(GeometryUtils.createOffsetLineNew(referenz.Shape.Factory, (ILineString)referenz.Shape, (double)offset));
                            }
                            IGeometry shape = za.Shape.Factory.CreateMultiLineString(lines.Where(l => l != null).ToArray());

                            trottfeature.Geometry = shape;
                            attributes.AddAttribute(geoJSONAttribute_ZustandsabschnittId, za.Id.ToString());
                            attributes.AddAttribute(geoJSONAttribute_Zustandsindex, (int)za.ZustandsindexTrottoirRechts);
                            trottfeature.Attributes = attributes;
                            trottoirs.Add(trottfeature);
                        }
                        if (trottoirs.IsEmpty())
                        {
                            attribute.AddAttribute(geoJSONAttribute_Trottoir, geoJSON_EmptyFeatureCollection);
                        }
                        else 
                        {
                            attribute.AddAttribute(geoJSONAttribute_Trottoir, trottoirs);
                        }
                    }
                }
            }
            feature.Attributes = attribute;
            return feature;
        }
        public ReferenzGruppeDTO GenerateReferenzGruppeFromGeoJson(string geoJson)
        {
            List<FeatureWithID> childs = GeoJSONReader.ReadFeatureWithID(new StringReader(geoJson)).Attributes[geoJSONAttribute_childs] as List<FeatureWithID>;
            ReferenzGruppeDTO referenzGruppeModel = new ReferenzGruppeDTO();

            foreach (FeatureWithID child in childs)
            {
                AchsenReferenzDTO newAchsenref = new AchsenReferenzDTO();
                newAchsenref.AchsenSegmentDTO = dtoService.GetDTOByID <AchsenSegmentDTO>(Guid.Parse(child.Attributes[geoJSONAttribute_AchsenSegmentId].ToString()));
                newAchsenref.Shape = child.Geometry;
                newAchsenref.Shape.SRID = GisConstants.SRID;

                referenzGruppeModel.AddAchsenReferenz(newAchsenref);
            }
            return referenzGruppeModel;
        }
    }
}
