using System;
using System.Collections.Generic;
using System.Text;
using GeoAPI.Geometries;
using NetTopologySuite.Features;
using System.IO;
using Jayrock.Json;

namespace GeoJSON
{
    public class GeoJSONReader
    {
        public static IGeometry Read(JsonTextReader jreader)
        {
            if (jreader == null)
                throw new ArgumentNullException("reader", "A valid JSON reader object is required.");

            IGeometry geometry = null;
            if (jreader.MoveToContent())
            {
                if (jreader.TokenClass == JsonTokenClass.Object)
                {
                    jreader.ReadToken(JsonTokenClass.Object);

                    //Read the 'type' property that indicates the type of the geometry or object
                    jreader.ReadMember();
                    string geometryType = jreader.ReadString();

                    switch (geometryType)
                    {
                        case "Point":
                            //Read the 'coordinates' property
                            jreader.ReadMember();
                            geometry = ReadPoint(jreader);
                            break;

                        case "MultiPoint":
                            //Read the 'coordinates' property
                            jreader.ReadMember();
                            geometry = ReadMultiPoint(jreader);
                            break;

                        case "LineString":
                            //Read the 'coordinates' property
                            jreader.ReadMember();
                            geometry = ReadLineString(jreader);
                            break;

                        case "MultiLineString":
                            //Read the 'coordinates' property
                            jreader.ReadMember();
                            geometry = ReadMultiLineString(jreader);
                            break;

                        case "Polygon":
                            //Read the 'coordinates' property
                            jreader.ReadMember();
                            geometry = ReadPolygon(jreader);
                            break;

                        case "MultiPolygon":
                            //Read the 'coordinates' property
                            jreader.ReadMember();
                            geometry = ReadMultiPolygon(jreader);
                            break;

                        case "GeometryCollection":
                            //Read the 'coordinates' property
                            jreader.ReadMember();
                            geometry = ReadGeometryCollection(jreader);
                            break;

                        default:
                            break;
                    }

                    jreader.ReadToken(JsonTokenClass.EndObject);
                }
            }
            return geometry;
        }

        public static IGeometry Read(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("writer", "A valid text reader object is required.");

            JsonTextReader jreader = new JsonTextReader(reader);
            return Read(jreader);
        }

        public static void Read(ref Coordinate coordinate, JsonTextReader jreader)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate", "A valid coordinate reference is required.");
            if (jreader == null)
                throw new ArgumentNullException("jreader", "A valid JSON reader object is required.");

            if (jreader.MoveToContent() && jreader.TokenClass == JsonTokenClass.Array)
            {
                jreader.ReadToken(JsonTokenClass.Array);
                coordinate.X = Convert.ToDouble(jreader.ReadNumber());
                coordinate.Y = Convert.ToDouble(jreader.ReadNumber());
                coordinate.Z = double.NaN;
                if (jreader.TokenClass == JsonTokenClass.Number)
                    coordinate.Z = Convert.ToDouble(jreader.ReadNumber());
                jreader.ReadToken(JsonTokenClass.EndArray);
            }
        }

        public static void Read(ref Coordinate coordinate, TextReader reader)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate", "A valid coordinate reference is required.");
            if (reader == null)
                throw new ArgumentNullException("reader", "A valid text reader object is required.");

            JsonTextReader jreader = new JsonTextReader(reader);
            Read(ref coordinate, jreader);
        }

        public static void Read(ref Coordinate[] coordinates, JsonTextReader jreader)
        {
            if (jreader == null)
                throw new ArgumentNullException("reader", "A valid JSON reader object is required.");

            if (jreader.MoveToContent() && jreader.TokenClass == JsonTokenClass.Array)
            {
                jreader.ReadToken(JsonTokenClass.Array);
                List<Coordinate> list = new List<Coordinate>();
                while (jreader.TokenClass == JsonTokenClass.Array)
                {
                    Coordinate item = new Coordinate();
                    Read(ref item, jreader);
                    list.Add(item);
                }
                jreader.ReadToken(JsonTokenClass.EndArray);

                coordinates = list.ToArray();
            }
        }

        public static void Read(ref Coordinate[] coordinates, TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "A valid text reader object is required.");

            JsonTextReader jreader = new JsonTextReader(reader);
            Read(ref coordinates, jreader);
        }

        private static IPoint ReadPoint(JsonTextReader jreader)
        {
            if (jreader == null)
                throw new ArgumentNullException("reader", "A valid JSON reader object is required.");

            IPoint point = null;
            if (jreader.TokenClass == JsonTokenClass.Array)
            {
                jreader.ReadToken(JsonTokenClass.Array);
                double x = Convert.ToDouble(jreader.ReadNumber());
                double y = Convert.ToDouble(jreader.ReadNumber());
                double z = double.NaN;
                if (jreader.TokenClass == JsonTokenClass.Number)
                    z = Convert.ToDouble(jreader.ReadNumber());
                jreader.ReadToken(JsonTokenClass.EndArray);
                point = new NetTopologySuite.Geometries.Point(x, y, z);
            }
            return point;
        }

        private static IPoint ReadPoint(ref IPoint point, TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "A valid text reader object is required.");

            JsonTextReader jreader = new JsonTextReader(reader);
            return ReadPoint(jreader);
        }

        private static IMultiPoint ReadMultiPoint(JsonTextReader jreader)
        {
            if (jreader == null)
                throw new ArgumentNullException("reader", "A valid JSON reader object is required.");

            IMultiPoint points = null;
            if (jreader.TokenClass == JsonTokenClass.Array)
            {
                jreader.ReadToken(JsonTokenClass.Array);
                List<IPoint> list = new List<IPoint>();
                while (jreader.TokenClass == JsonTokenClass.Array)
                {
                    Coordinate item = new Coordinate();
                    Read(ref item, jreader);
                    list.Add(new NetTopologySuite.Geometries.Point(item));
                }
                jreader.ReadToken(JsonTokenClass.EndArray);
                points = new NetTopologySuite.Geometries.MultiPoint(list.ToArray());
            }
            return points;
        }

        private static IMultiPoint ReadMultiPoint(ref IMultiPoint points, TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "A valid text reader object is required.");

            JsonTextReader jreader = new JsonTextReader(reader);
            return ReadMultiPoint(jreader);
        }

        private static ILineString ReadLineString(JsonTextReader jreader)
        {
            if (jreader == null)
                throw new ArgumentNullException("reader", "A valid JSON reader object is required.");

            ILineString line = null;
            if (jreader.TokenClass == JsonTokenClass.Array)
            {
                jreader.ReadToken(JsonTokenClass.Array);
                List<Coordinate> list = new List<Coordinate>();
                while (jreader.TokenClass == JsonTokenClass.Array)
                {
                    Coordinate item = new Coordinate();
                    Read(ref item, jreader);
                    list.Add(item);
                }
                jreader.ReadToken(JsonTokenClass.EndArray);
                line = new NetTopologySuite.Geometries.LineString(list.ToArray());
            }
            return line;
        }

        private static ILineString ReadLineString(ref ILineString line, TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "A valid text reader object is required.");

            JsonTextReader jreader = new JsonTextReader(reader);
            return ReadLineString(jreader);
        }

        private static IMultiLineString ReadMultiLineString(JsonTextReader jreader)
        {
            if (jreader == null)
                throw new ArgumentNullException("reader", "A valid JSON reader object is required.");

            IMultiLineString lines = null;
            if (jreader.TokenClass == JsonTokenClass.Array)
            {
                jreader.ReadToken(JsonTokenClass.Array);
                List<ILineString> list = new List<ILineString>();
                while (jreader.TokenClass == JsonTokenClass.Array)
                    list.Add(ReadLineString(jreader));
                jreader.ReadToken(JsonTokenClass.EndArray);
                lines = new NetTopologySuite.Geometries.MultiLineString(list.ToArray());
            }
            return lines;
        }

        private static IMultiLineString ReadMultiLineString(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "A valid text reader object is required.");

            JsonTextReader jreader = new JsonTextReader(reader);
            return ReadMultiLineString(jreader);
        }

        private static IPolygon ReadPolygon(JsonTextReader jreader)
        {
            if (jreader == null)
                throw new ArgumentNullException("reader", "A valid JSON reader object is required.");

            IPolygon area = null;
            if (jreader.TokenClass == JsonTokenClass.Array)
            {
                jreader.ReadToken(JsonTokenClass.Array);

                //Read the outer shell
                ILinearRing shell = null;
                if (jreader.TokenClass == JsonTokenClass.Array)
                {
                    Coordinate[] coordinates = new Coordinate[] { };
                    Read(ref coordinates, jreader);
                    shell = new NetTopologySuite.Geometries.LinearRing(coordinates);
                }

                //Read all the holes
                List<ILinearRing> list = new List<ILinearRing>();
                while (jreader.TokenClass == JsonTokenClass.Array)
                {
                    Coordinate[] coordinates = new Coordinate[] { };
                    Read(ref coordinates, jreader);
                    ILinearRing hole = new NetTopologySuite.Geometries.LinearRing(coordinates);
                    list.Add(hole);
                }
                jreader.ReadToken(JsonTokenClass.EndArray);

                //An outer shell was found so a polygon can be created
                if (shell != null)
                    area = new NetTopologySuite.Geometries.Polygon(shell, list.ToArray());
            }
            return area;
        }

        private static IPolygon ReadPolygon(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "A valid text reader object is required.");

            JsonTextReader jreader = new JsonTextReader(reader);
            return ReadPolygon(jreader);
        }

        private static IMultiPolygon ReadMultiPolygon(JsonTextReader jreader)
        {
            if (jreader == null)
                throw new ArgumentNullException("reader", "A valid JSON reader object is required.");

            IMultiPolygon areas = null;
            if (jreader.TokenClass == JsonTokenClass.Array)
            {
                jreader.ReadToken(JsonTokenClass.Array);
                List<IPolygon> polygons = new List<IPolygon>();
                    while (jreader.TokenClass == JsonTokenClass.Array)
                    {
                        jreader.ReadToken(JsonTokenClass.Array);

                            //Read the outer shell
                            ILinearRing shell = null;
                            if (jreader.TokenClass == JsonTokenClass.Array)
                            {
                                Coordinate[] coordinates = new Coordinate[] { };
                                Read(ref coordinates, jreader);
                                shell = new NetTopologySuite.Geometries.LinearRing(coordinates);
                            }

                            //Read all the holes
                            List<ILinearRing> list = new List<ILinearRing>();
                            while (jreader.TokenClass == JsonTokenClass.Array)
                            {
                                Coordinate[] coordinates = new Coordinate[] { };
                                Read(ref coordinates, jreader);
                                ILinearRing hole = new NetTopologySuite.Geometries.LinearRing(coordinates);
                                list.Add(hole);
                            }

                        jreader.ReadToken(JsonTokenClass.EndArray);

                        //An outer shell was found so a polygon can be created
                        if (shell != null)
                        {
                            IPolygon area = new NetTopologySuite.Geometries.Polygon(shell, list.ToArray());
                            polygons.Add(area);
                        }
                    }
                jreader.ReadToken(JsonTokenClass.EndArray);

                areas = new NetTopologySuite.Geometries.MultiPolygon(polygons.ToArray());
            }
            return areas;
        }

        private static IMultiPolygon ReadMultiPolygon(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "A valid text reader object is required.");

            JsonTextReader jreader = new JsonTextReader(reader);
            return ReadMultiPolygon(jreader);
        }

        private static IGeometryCollection ReadGeometryCollection(JsonTextReader jreader)
        {
            if (jreader == null)
                throw new ArgumentNullException("reader", "A valid JSON reader object is required.");

            IGeometryCollection geometries = null;
            if (jreader.TokenClass == JsonTokenClass.Array)
            {
                jreader.ReadToken(JsonTokenClass.Array);
                List<IGeometry> list = new List<IGeometry>();
                while (jreader.TokenClass == JsonTokenClass.Object)
                {
                    IGeometry geometry = Read(jreader);
                    list.Add(geometry);
                }
                jreader.ReadToken(JsonTokenClass.EndArray);
                geometries = new NetTopologySuite.Geometries.GeometryCollection(list.ToArray());
            }
            return geometries;
        }

        private static IGeometryCollection ReadGeometryCollection(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "A valid text reader object is required.");

            JsonTextReader jreader = new JsonTextReader(reader);
            return ReadGeometryCollection(jreader);
        }

        public static FeatureWithID ReadFeatureWithID(JsonTextReader jreader)
        {
            if (jreader == null)
                throw new ArgumentNullException("jreader", "A valid JSON reader object is required.");

            FeatureWithID feature = new FeatureWithID();
            if (jreader.MoveToContent() && jreader.TokenClass == JsonTokenClass.Object)
            {
                jreader.ReadToken(JsonTokenClass.Object);

                string key = "";

                JsonTokenClass tokenClass;

                while ((tokenClass = jreader.TokenClass) == JsonTokenClass.Member)
                {
                 
                    key = jreader.ReadMember();
                    switch (key) { 
                      case "type":
                        if (!jreader.ReadString().Equals("Feature"))
                          throw new Exception("jreader - A valid JSON reader object is required.");
                        break;
                      case "id":
                        feature.Id = jreader.ReadString();
                        break;
                      case "geometry":
                        feature.Geometry = Read(jreader); //reads the geometry value 
                        //jreader.ReadMember();   //reads 'properties'
                        //feature.Attributes = ReadAttributesTableWithID(jreader);
                        break;
                      case "properties":
                        feature.Attributes = ReadAttributesTableWithID(jreader);
                        //jreader.ReadMember();   //reads 'geometry'
                        //feature.Geometry = Read(jreader); //reads the geometry value 
                        break;
                      default:
                        throw new Exception("jreader - Feature - unexpected member");
                        break;
                    }
              }

                //if (tokenClass != JsonTokenClass.EndObject)
                //{
                //  throw new Exception("expected endobject");
                //}

                jreader.ReadToken(JsonTokenClass.EndObject);
            }
            return feature;
        }

        public static Feature ReadFeature(JsonTextReader jreader)
        {
            if (jreader == null)
                throw new ArgumentNullException("jreader", "A valid JSON reader object is required.");

            Feature feature = new Feature();
            if (jreader.MoveToContent() && jreader.TokenClass == JsonTokenClass.Object)
            {
                jreader.ReadToken(JsonTokenClass.Object);

                //Read the 'Feature' as the type
                jreader.ReadMember(); //reads 'type'
                jreader.ReadString(); //reads 'Feature'

                //Read the 'geometry'
                jreader.ReadMember(); //reads 'geometry'
                feature.Geometry = Read(jreader); //reads the geometry value

                //Read the 'properties'
                jreader.ReadMember(); //reads 'properties'
                feature.Attributes = ReadAttributesTable(jreader);

                jreader.ReadToken(JsonTokenClass.EndObject);
            }
            return feature;
        }

        public static FeatureWithID ReadFeatureWithID(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "A valid text reader object is required.");

            JsonTextReader jreader = new JsonTextReader(reader);
            return ReadFeatureWithID(jreader);
        }

        public static Feature ReadFeature(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "A valid text reader object is required.");

            JsonTextReader jreader = new JsonTextReader(reader);
            return ReadFeature(jreader);
        }

        public static IAttributesTable ReadAttributesTableWithID(JsonTextReader jreader)
        {
            if (jreader == null)
                throw new ArgumentNullException("jreader", "A valid JSON reader object is required.");

            IAttributesTable attributes = null;
            if (jreader.MoveToContent() && jreader.TokenClass == JsonTokenClass.Object)
            {
                attributes = new AttributesTable();
                jreader.ReadToken(JsonTokenClass.Object);
                
                while (jreader.TokenClass == JsonTokenClass.Member)
                {
                    string key = jreader.ReadMember();
                    if (jreader.TokenClass == JsonTokenClass.Object)
                    {
                        var value = ReadFeatureCollectionWithID(jreader);

                        if (!attributes.Exists(key))
                            attributes.AddAttribute(key, null);

                        attributes[key] = value;
                    }
                    else
                    {
                        //string key = jreader.ReadMember();
                        string value = jreader.ReadString();

                        if (!attributes.Exists(key))
                            attributes.AddAttribute(key, null);
                        attributes[key] = value;
                    }
                }

                jreader.ReadToken(JsonTokenClass.EndObject);
            }
            return attributes;
        }

        public static IAttributesTable ReadAttributesTable(JsonTextReader jreader)
        {
            if (jreader == null)
                throw new ArgumentNullException("jreader", "A valid JSON reader object is required.");

            IAttributesTable attributes = null;
            if (jreader.MoveToContent() && jreader.TokenClass == JsonTokenClass.Object)
            {
                attributes = new AttributesTable();
                jreader.ReadToken(JsonTokenClass.Object);

                while (jreader.TokenClass == JsonTokenClass.Member)
                {
                    string key = jreader.ReadMember();
                    if (jreader.TokenClass == JsonTokenClass.Object)
                    {
                        var value = ReadFeatureCollection(jreader);

                        if (!attributes.Exists(key))
                            attributes.AddAttribute(key, null);

                        attributes[key] = value;
                    }
                    else
                    {
                        //string key = jreader.ReadMember();
                        string value = jreader.ReadString();

                        if (!attributes.Exists(key))
                            attributes.AddAttribute(key, null);
                        attributes[key] = value;
                    }
                }

                jreader.ReadToken(JsonTokenClass.EndObject);
            }
            return attributes;
        }

        public static IAttributesTable ReadAttributesTable(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "A valid text reader object is required.");

            JsonTextReader jreader = new JsonTextReader(reader);
            return ReadAttributesTable(jreader);
        }

        public static IEnumerable<FeatureWithID> ReadFeatureCollectionWithID(JsonTextReader jreader)
        {
            if (jreader == null)
                throw new ArgumentNullException("jreader", "A valid JSON reader object is required.");

            List<FeatureWithID> features = new List<FeatureWithID>();
            if (jreader.MoveToContent() && jreader.TokenClass == JsonTokenClass.Object)
            {
                jreader.ReadToken(JsonTokenClass.Object);

                //Read the 'FeatureCollection' as the type
                jreader.ReadMember(); //reads 'type'
                jreader.ReadString(); //reads 'FeatureCollection'

                //Read the 'features' property
                jreader.ReadMember(); //reads 'features'
                if (jreader.TokenClass == JsonTokenClass.Array)
                {
                    jreader.ReadToken(JsonTokenClass.Array);
                    while (jreader.TokenClass == JsonTokenClass.Object)
                        features.Add(ReadFeatureWithID(jreader));
                    jreader.ReadToken(JsonTokenClass.EndArray);
                }
                jreader.ReadToken(JsonTokenClass.EndObject);
                return features;
            }
            return null;
        }


        public static IEnumerable<Feature> ReadFeatureCollection(JsonTextReader jreader)
        {
            if (jreader == null)
                throw new ArgumentNullException("jreader", "A valid JSON reader object is required.");

            List<Feature> features = new List<Feature>();
            if (jreader.MoveToContent() && jreader.TokenClass == JsonTokenClass.Object)
            {
                jreader.ReadToken(JsonTokenClass.Object);

                //Read the 'FeatureCollection' as the type
                jreader.ReadMember(); //reads 'type'
                jreader.ReadString(); //reads 'FeatureCollection'

                //Read the 'features' property
                jreader.ReadMember(); //reads 'features'
                if (jreader.TokenClass == JsonTokenClass.Array)
                {
                    jreader.ReadToken(JsonTokenClass.Array);                    
                    while (jreader.TokenClass == JsonTokenClass.Object)
                        features.Add(ReadFeature(jreader));
                    jreader.ReadToken(JsonTokenClass.EndArray);
                }
                jreader.ReadToken(JsonTokenClass.EndObject);
                return features;
            }
            return null;
        }

        public static IEnumerable<Feature> ReadFeatureCollection(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "A valid text reader object is required.");

            JsonTextReader jreader = new JsonTextReader(reader);
            return ReadFeatureCollection(jreader);
        }
    }
}
