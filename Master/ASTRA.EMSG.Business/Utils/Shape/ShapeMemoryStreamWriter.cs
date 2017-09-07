using System.Collections;
using System.Collections.Generic;
using System.IO;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Handlers;
using NetTopologySuite.IO;

namespace ASTRA.EMSG.Business.Utils.Shape
{
    /// <summary>
    /// This class writes ESRI Shapefiles.
    /// </summary>
    public class ShapeMemoryStreamWriter
    {
        private IGeometryFactory geometryFactory = null;
        private MemoryStream shpStream;
        private MemoryStream shxStream;


        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefileWriter" /> class 
        /// using <see cref="GeometryFactory.Default" /> with a <see cref="PrecisionModels.Floating" /> precision.
        /// </summary>
        public ShapeMemoryStreamWriter() : this(GeometryFactory.Default) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefileWriter" /> class
        /// with the given <see cref="GeometryFactory" />.
        /// </summary>
        /// <param name="geometryFactory"></param>
        public ShapeMemoryStreamWriter(IGeometryFactory geometryFactory)
        {
            this.geometryFactory = geometryFactory;
        }

        public MemoryStream GetShpStream()
        {   
            shpStream.Seek(0, SeekOrigin.Begin);
            return shpStream;
        }

        public MemoryStream GetShxStream()
        {
            shxStream.Seek(0, SeekOrigin.Begin);
            return shxStream;
        }

        /// <summary>
        /// Writes a shapefile to disk.
        /// </summary>
        /// <remarks>
        /// Assumes the type given for the first geometry is the same for all subsequent geometries.
        /// For example, is, if the first Geometry is a Multi-polygon/ Polygon, the subsequent geometies are
        /// Muli-polygon/ polygon and not lines or points.
        /// The dbase file for the corresponding shapefile contains one column called row. It contains 
        /// the row number.
        /// </remarks>
        /// <param name="filename">The filename to write to (minus the .shp extension).</param>
        /// <param name="geometryCollection">The GeometryCollection to write.</param>		
        public void Write(IGeometryCollection geometryCollection)
        {
            //FileStream shpStream = new FileStream(filename + ".shp", FileMode.Create);
            //FileStream shxStream = new FileStream(filename + ".shx", FileMode.Create);

            shpStream = new MemoryStream();
            shxStream = new MemoryStream();
            BigEndianBinaryWriter shpBinaryWriter = new BigEndianBinaryWriter(shpStream);
            BigEndianBinaryWriter shxBinaryWriter = new BigEndianBinaryWriter(shxStream);

            // assumes
            ShapeHandler handler = Shapefile.GetShapeHandler(GetShapeType(geometryCollection.Geometries[0]));

            IGeometry body;
            int numShapes = geometryCollection.NumGeometries;
            // calc the length of the shp file, so it can put in the header.
            int shpLength = 50;
            for (int i = 0; i < numShapes; i++)
            {
                body = (IGeometry)geometryCollection.Geometries[i];
                shpLength += 4; // length of header in WORDS
                shpLength += handler.ComputeRequiredLengthInWords(body); // length of shape in WORDS
            }

            int shxLength = 50 + (4 * numShapes);

            // write the .shp header
            ShapefileHeader shpHeader = new ShapefileHeader();
            shpHeader.FileLength = shpLength;

            // get envelope in external coordinates
            Envelope env = geometryCollection.EnvelopeInternal as Envelope;
            Envelope bounds = ShapeHandler.GetEnvelopeExternal(geometryFactory.PrecisionModel, env);
            shpHeader.Bounds = bounds;

            // assumes Geometry type of the first item will the same for all other items
            // in the collection.
            shpHeader.ShapeType = GetShapeType(geometryCollection.Geometries[0]);
            shpHeader.Write(shpBinaryWriter);

            // write the .shx header
            ShapefileHeader shxHeader = new ShapefileHeader();
            shxHeader.FileLength = shxLength;
            shxHeader.Bounds = shpHeader.Bounds;

            // assumes Geometry type of the first item will the same for all other items in the collection.
            shxHeader.ShapeType = GetShapeType(geometryCollection.Geometries[0]);
            shxHeader.Write(shxBinaryWriter);

            // write the individual records.
            int _pos = 50; // header length in WORDS
            for (int i = 0; i < numShapes; i++)
            {
                body = geometryCollection.Geometries[i];
                int recordLength = handler.ComputeRequiredLengthInWords(body);
                shpBinaryWriter.WriteIntBE(i + 1);
                shpBinaryWriter.WriteIntBE(recordLength);

                shxBinaryWriter.WriteIntBE(_pos);
                shxBinaryWriter.WriteIntBE(recordLength);

                _pos += 4; // length of header in WORDS
                handler.Write(body, shpBinaryWriter, geometryFactory);
                _pos += recordLength; // length of shape in WORDS
            }

            shxBinaryWriter.Flush();
            //shxStream.Seek(0, SeekOrigin.Begin);
            ////shxBinaryWriter.Close();
            shpBinaryWriter.Flush();
            //shpStream.Seek(0, SeekOrigin.Begin);
            //shpBinaryWriter.Close();

            // WriteDummyDbf(filename + ".dbf", numShapes);	
        }


        public static ShapeGeometryType GetShapeType(IGeometry geom)
        {
            geom = GetNonEmptyGeometry(geom);

            if (geom == null)
                return ShapeGeometryType.NullShape;

            switch (geom.OgcGeometryType)
            {
                case OgcGeometryType.Point:
                    switch (((IPoint)geom).CoordinateSequence.Ordinates)
                    {
                        case Ordinates.XYM:
                            return ShapeGeometryType.PointM;
                        case Ordinates.XYZ:
                        case Ordinates.XYZM:
                            return ShapeGeometryType.PointZM;
                        default:
                            return ShapeGeometryType.Point;
                    }
                case OgcGeometryType.MultiPoint:
                    switch (((IPoint)geom.GetGeometryN(0)).CoordinateSequence.Ordinates)
                    {
                        case Ordinates.XYM:
                            return ShapeGeometryType.MultiPointM;
                        case Ordinates.XYZ:
                        case Ordinates.XYZM:
                            return ShapeGeometryType.MultiPointZM;
                        default:
                            return ShapeGeometryType.MultiPoint;
                    }
                case OgcGeometryType.LineString:
                case OgcGeometryType.MultiLineString:
                    switch (((ILineString)geom.GetGeometryN(0)).CoordinateSequence.Ordinates)
                    {
                        //Workaround
                        //Nhibernate Spatial seems to default to the biggest ordinates
                        //this suppress ZM to prevent bugs in Arcmap
                        case Ordinates.XYM:
                            //return ShapeGeometryType.LineStringM;
                        case Ordinates.XYZ:
                        case Ordinates.XYZM:
                            //return ShapeGeometryType.LineStringZM;
                        default:
                            return ShapeGeometryType.LineString;
                    }
                case OgcGeometryType.Polygon:
                case OgcGeometryType.MultiPolygon:
                    switch (((IPolygon)geom.GetGeometryN(0)).Shell.CoordinateSequence.Ordinates)
                    {
                        case Ordinates.XYM:
                            return ShapeGeometryType.PolygonM;
                        case Ordinates.XYZ:
                        case Ordinates.XYZM:
                            return ShapeGeometryType.PolygonZM;
                        default:
                            return ShapeGeometryType.Polygon;
                    }
                /*
            case OgcGeometryType.GeometryCollection:
                if (geom.NumGeometries > 1)
                {
                    for (var i = 0; i < geom.NumGeometries; i++)
                    {
                        var sgt = GetShapeType(geom.GetGeometryN(i));
                        if (sgt != ShapeGeometryType.NullShape)
                            return sgt;
                    }
                    return ShapeGeometryType.NullShape;
                }
                throw new NotSupportedException();
             */
                default:
                    throw new System.NotSupportedException();
            }
        }
        private static IGeometry GetNonEmptyGeometry(IGeometry geom)
        {
            if (geom == null || geom.IsEmpty)
                return null;

            for (var i = 0; i < geom.NumGeometries; i++)
            {
                var testGeom = geom.GetGeometryN(i);
                if (testGeom != null && !testGeom.IsEmpty)
                    return testGeom;
            }
            return null;
        }

        public void Close()
        {
            shpStream.Close();
            shxStream.Close();
        }        
    }
}
