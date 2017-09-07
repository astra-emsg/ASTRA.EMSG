using System;
using System.Collections.Generic;
using System.Text;
using GeoAPI.Geometries;
using NetTopologySuite.Features;
using System.IO;
using Jayrock.Json;

namespace GeoJSON
{
    public class GeoJSONWriter
    {
        public static void Write(IGeometry geometry, JsonTextWriter jwriter)
        {
            if (geometry == null)
                return;
            if (jwriter == null)
                throw new ArgumentNullException("jwriter", "A valid JSON writer object is required.");

            if (geometry is IPoint)
                Write(geometry as IPoint, jwriter);
            else if (geometry is ILineString)
                Write(geometry as ILineString, jwriter);
            else if (geometry is IPolygon)
                Write(geometry as IPolygon, jwriter);
            else if (geometry is IMultiPoint)
                Write(geometry as IMultiPoint, jwriter);
            else if (geometry is IMultiLineString)
                Write(geometry as IMultiLineString, jwriter);
            else if (geometry is IMultiPolygon)
                Write(geometry as IMultiPolygon, jwriter);
            else if (geometry is IGeometryCollection)
                Write(geometry as IGeometryCollection, jwriter);
        }

        public static void Write(IGeometry geometry, TextWriter writer)
        {
            if (geometry == null)
                return;
            if (writer == null)
                throw new ArgumentNullException("writer", "A valid text writer object is required.");

            JsonTextWriter jwriter = new JsonTextWriter(writer);
            Write(geometry, jwriter);
        }

        public static void Write(ICoordinate coordinate, JsonTextWriter jwriter)
        {
            if (coordinate == null)
                return;
            if (jwriter == null)
                throw new ArgumentNullException("jwriter", "A valid JSON writer object is required.");

            jwriter.WriteStartArray();
            jwriter.WriteNumber(coordinate.X);
            jwriter.WriteNumber(coordinate.Y);
            if (!double.IsNaN(coordinate.Z))
                jwriter.WriteNumber(coordinate.Z);
            jwriter.WriteEndArray();
        }

        public static void Write(ICoordinate coordinate, TextWriter writer)
        {
            if (coordinate == null)
                return;
            if (writer == null)
                throw new ArgumentNullException("writer", "A valid text writer object is required.");

            JsonTextWriter jwriter = new JsonTextWriter(writer);
            Write(coordinate, jwriter);
        }

        public static void Write(ICoordinate[] coordinates, JsonTextWriter jwriter)
        {

            if (coordinates == null)
                return;
            if (jwriter == null)
                throw new ArgumentNullException("jwriter", "A valid JSON writer object is required.");

            jwriter.WriteStartArray();

            foreach (ICoordinate entry in coordinates)
                Write(entry, jwriter);
            jwriter.WriteEndArray();
        }

        public static void Write(ICoordinate[] coordinates, TextWriter writer)
        {
            if (coordinates == null)
                return;
            if (writer == null)
                throw new ArgumentNullException("writer", "A valid text writer object is required.");

            JsonTextWriter jwriter = new JsonTextWriter(writer);
            Write(coordinates, jwriter);
        }

        public static void Write(IPoint point, JsonTextWriter jwriter)
        {
            if (point == null)
                return;
            if (jwriter == null)
                throw new ArgumentNullException("jwriter", "A valid JSON writer object is required.");

            jwriter.WriteStartObject();
            
            jwriter.WriteMember("type");
            jwriter.WriteString("Point");

            jwriter.WriteMember("coordinates");
            jwriter.WriteStartArray();
            jwriter.WriteNumber(point.X);
            jwriter.WriteNumber(point.Y);
            if (!double.IsNaN(point.Z))
                jwriter.WriteNumber(point.Z);
            jwriter.WriteEndArray();

            jwriter.WriteEndObject();
        }

        public static void Write(IPoint point, TextWriter writer)
        {
            if (point == null)
                return;
            if (writer == null)
                throw new ArgumentNullException("writer", "A valid text writer object is required.");

            JsonTextWriter jwriter = new JsonTextWriter(writer);
            Write(point, jwriter);
        }

        public static void Write(IMultiPoint points, JsonTextWriter jwriter)
        {
            if (points == null)
                return;
            if (jwriter == null)
                throw new ArgumentNullException("jwriter", "A valid JSON writer object is required.");

            jwriter.WriteStartObject();

            jwriter.WriteMember("type");
            jwriter.WriteString("MultiPoint");

            jwriter.WriteMember("coordinates");
            jwriter.WriteStartArray();
            foreach (ICoordinate entry in points.Coordinates)
                Write(entry, jwriter);
            jwriter.WriteEndArray();

            jwriter.WriteEndObject();
        }

        public static void Write(IMultiPoint points, TextWriter writer)
        {
            if (points == null)
                return;
            if (writer == null)
                throw new ArgumentNullException("writer", "A valid text writer object is required.");

            JsonTextWriter jwriter = new JsonTextWriter(writer);
            Write(points, jwriter);
        }

        public static void Write(ILineString line, JsonTextWriter jwriter)
        {
            if (line == null)
                return;
            if (jwriter == null)
                throw new ArgumentNullException("jwriter", "A valid JSON writer object is required.");

            jwriter.WriteStartObject();

            jwriter.WriteMember("type");
            jwriter.WriteString("LineString");

            jwriter.WriteMember("coordinates");
            jwriter.WriteStartArray();
            foreach (ICoordinate entry in line.Coordinates)
                Write(entry, jwriter);
            jwriter.WriteEndArray();

            jwriter.WriteEndObject();
        }

        public static void Write(ILineString line, TextWriter writer)
        {
            if (line == null)
                return;
            if (writer == null)
                throw new ArgumentNullException("writer", "A valid text writer object is required.");

            JsonTextWriter jwriter = new JsonTextWriter(writer);
            Write(line, jwriter);
        }

        public static void Write(IMultiLineString lines, JsonTextWriter jwriter)
        {
            if (lines == null)
                return;
            if (jwriter == null)
                throw new ArgumentNullException("jwriter", "A valid JSON writer object is required.");

            jwriter.WriteStartObject();

            jwriter.WriteMember("type");
            jwriter.WriteString("MultiLineString");

            jwriter.WriteMember("coordinates");
            jwriter.WriteStartArray();
            foreach (ILineString line in lines.Geometries)
                Write(line.Coordinates, jwriter);
            jwriter.WriteEndArray();

            jwriter.WriteEndObject();
        }

        public static void Write(IMultiLineString lines, TextWriter writer)
        {
            if (lines == null)
                return;
            if (writer == null)
                throw new ArgumentNullException("writer", "A valid text writer object is required.");

            JsonTextWriter jwriter = new JsonTextWriter(writer);
            Write(lines, jwriter);
        }

        public static void Write(IPolygon area, JsonTextWriter jwriter)
        {
            if (area == null)
                return;
            if (jwriter == null)
                throw new ArgumentNullException("jwriter", "A valid JSON writer object is required.");

            jwriter.WriteStartObject();

            jwriter.WriteMember("type");
            jwriter.WriteString("Polygon");

            jwriter.WriteMember("coordinates");
            jwriter.WriteStartArray();

            //Write the exterior boundary or shell
            Write(area.Shell.Coordinates, jwriter);

            //Write all the holes
            foreach (ILineString hole in area.Holes)
                Write(hole.Coordinates, jwriter);
            
            jwriter.WriteEndArray();

            jwriter.WriteEndObject();
        }

        public static void Write(IPolygon area, TextWriter writer)
        {
            if (area == null)
                return;
            if (writer == null)
                throw new ArgumentNullException("writer", "A valid text writer object is required.");

            JsonTextWriter jwriter = new JsonTextWriter(writer);
            Write(area, jwriter);
        }

        public static void Write(IMultiPolygon areas, JsonTextWriter jwriter)
        {
            if (areas == null)
                return;
            if (jwriter == null)
                throw new ArgumentNullException("jwriter", "A valid JSON writer object is required.");

            jwriter.WriteStartObject();

            jwriter.WriteMember("type");
            jwriter.WriteString("MultiPolygon");

            jwriter.WriteMember("coordinates");
            jwriter.WriteStartArray();

            foreach (IPolygon area in areas.Geometries)
            {
                jwriter.WriteStartArray();

                //Write the exterior boundary or shell
                Write(area.Shell.Coordinates, jwriter);

                //Write all the holes
                foreach (ILineString hole in area.Holes)
                    Write(hole.Coordinates, jwriter);
                
                jwriter.WriteEndArray();
            }

            jwriter.WriteEndArray();


            jwriter.WriteEndObject();
        }

        public static void Write(IMultiPolygon areas, TextWriter writer)
        {
            if (areas == null)
                return;
            if (writer == null)
                throw new ArgumentNullException("writer", "A valid text writer object is required.");

            JsonTextWriter jwriter = new JsonTextWriter(writer);
            Write(areas, jwriter);
        }

        public static void Write(IGeometryCollection geometries, JsonTextWriter jwriter)
        {
            if (geometries == null)
                return;
            if (jwriter == null)
                throw new ArgumentNullException("jwriter", "A valid JSON writer object is required.");

            jwriter.WriteStartObject();

            jwriter.WriteMember("type");
            jwriter.WriteString("GeometryCollection");

            jwriter.WriteMember("geometries");
            jwriter.WriteStartArray();

            foreach (IGeometry geometry in geometries.Geometries)
                Write(geometry, jwriter);
            
            jwriter.WriteEndArray();

            jwriter.WriteEndObject();
        }

        public static void Write(IGeometryCollection geometries, TextWriter writer)
        {
            if (geometries == null)
                return;
            if (writer == null)
                throw new ArgumentNullException("writer", "A valid text writer object is required.");

            JsonTextWriter jwriter = new JsonTextWriter(writer);
            Write(geometries, jwriter);
        }

        public static void Write(Feature feature, JsonTextWriter jwriter)
        {
            if (feature == null)
                return;
            if (jwriter == null)
                throw new ArgumentNullException("jwriter", "A valid JSON writer object is required.");

            jwriter.WriteStartObject();

            jwriter.WriteMember("type");
            jwriter.WriteString("Feature");

            jwriter.WriteMember("geometry");
            Write(feature.Geometry, jwriter);

            jwriter.WriteMember("properties");
            Write(feature.Attributes, jwriter);

            jwriter.WriteEndObject();
        }

        public static void WriteWithID(FeatureWithID feature, JsonTextWriter jwriter)
        {
            if (feature == null)
                return;
            if (jwriter == null)
                throw new ArgumentNullException("jwriter", "A valid JSON writer object is required.");

            jwriter.WriteStartObject();

            jwriter.WriteMember("type");
            jwriter.WriteString("Feature");

            jwriter.WriteMember("id");
            jwriter.WriteString(feature.Id);

            jwriter.WriteMember("geometry");
            Write(feature.Geometry, jwriter);

            jwriter.WriteMember("properties");
            WritewithID(feature.Attributes, jwriter);

            jwriter.WriteEndObject();
        }


        public static void Write(Feature feature, TextWriter writer)
        {
            if (feature == null)
                return;
            if (writer == null)
                throw new ArgumentNullException("writer", "A valid text writer object is required.");

            JsonTextWriter jwriter = new JsonTextWriter(writer);
            Write(feature, jwriter);
        }

        public static void WriteWithID(FeatureWithID feature, TextWriter writer)
        {
            if (feature == null)
                return;
            if (writer == null)
                throw new ArgumentNullException("writer", "A valid text writer object is required.");

            JsonTextWriter jwriter = new JsonTextWriter(writer);
            WriteWithID(feature, jwriter);
        }


        public static void WriteFeatureWithID(FeatureWithID feature, TextWriter writer)
        {
            if (feature == null)
                return;
            if (writer == null)
                throw new ArgumentNullException("writer", "A valid text writer object is required.");

            JsonTextWriter jwriter = new JsonTextWriter(writer);
            WriteWithID(feature, jwriter);
        }


        public static void Write(IEnumerable<Feature> features, JsonTextWriter jwriter)
        {
            if (features == null)
                return;
            if (jwriter == null)
                throw new ArgumentNullException("jwriter", "A valid JSON writer object is required.");

            jwriter.WriteStartObject();

            jwriter.WriteMember("type");
            jwriter.WriteString("FeatureCollection");

            jwriter.WriteMember("features");
            jwriter.WriteStartArray();

            foreach (Feature feature in features)
                Write(feature, jwriter);
            
            jwriter.WriteEndArray();

            jwriter.WriteEndObject();
        }

        public static void WritewithID(IEnumerable<FeatureWithID> features, TextWriter writer)
        {
            if (features == null)
                return;
            if (writer == null)
                throw new ArgumentNullException("writer", "A valid text writer object is required.");

            JsonTextWriter jwriter = new JsonTextWriter(writer);            
            WritewithID(features, jwriter);
        }


        public static void WritewithID(IEnumerable<FeatureWithID> features, JsonTextWriter jwriter)
        {
            if (features == null)
                return;
            if (jwriter == null)
                throw new ArgumentNullException("jwriter", "A valid JSON writer object is required.");

            jwriter.WriteStartObject();

            jwriter.WriteMember("type");
            jwriter.WriteString("FeatureCollection");

            jwriter.WriteMember("features");
            jwriter.WriteStartArray();

            foreach (FeatureWithID feature in features)
                WriteWithID(feature, jwriter);

            jwriter.WriteEndArray();

            jwriter.WriteEndObject();
        }


        public static void Write(IEnumerable<Feature> features, TextWriter writer)
        {
            if (features == null)
                return;
            if (writer == null)
                throw new ArgumentNullException("writer", "A valid text writer object is required.");

            JsonTextWriter jwriter = new JsonTextWriter(writer);
            Write(features, jwriter);
        }

        public static void Write(IAttributesTable attributes, JsonTextWriter jwriter)
        {
            if (attributes == null)
                return;
            if (jwriter == null)
                throw new ArgumentNullException("jwriter", "A valid JSON writer object is required.");

            jwriter.WriteStartObject();

            string[] names = attributes.GetNames();
            foreach (string name in names)
            {
                jwriter.WriteMember(name);

                var attr = attributes[name];

                if (attr is IEnumerable<Feature>)
                {
                    IEnumerable<Feature> fcoll = (IEnumerable<Feature>)attr;
                    Write(fcoll, jwriter);
                }
                else
                {
                    // TODO: Numerische Attribute?
                    jwriter.WriteString(attributes[name].ToString());
                }
            }

            jwriter.WriteEndObject();
        }

        public static void WritewithID(IAttributesTable attributes, JsonTextWriter jwriter)
        {
            if (attributes == null)
                return;
            if (jwriter == null)
                throw new ArgumentNullException("jwriter", "A valid JSON writer object is required.");

            jwriter.WriteStartObject();

            string[] names = attributes.GetNames();
            foreach (string name in names)
            {
                jwriter.WriteMember(name);

                var attr = attributes[name];

                if (attr is IEnumerable<FeatureWithID>)
                {
                    IEnumerable<FeatureWithID> fcoll = (IEnumerable<FeatureWithID>)attr;
                    WritewithID(fcoll, jwriter);
                }
                else
                {
                    // TODO: Numerische Attribute?
                    jwriter.WriteString(attributes[name]!=null?attributes[name].ToString():"");
                }
            }

            jwriter.WriteEndObject();
        }

        public static void Write(IAttributesTable attributes, TextWriter writer)
        {
            if (attributes == null)
                return;
            if (writer == null)
                throw new ArgumentNullException("writer", "A valid text writer object is required.");

            JsonTextWriter jwriter = new JsonTextWriter(writer);
            Write(attributes, jwriter);
        }
    }
}
