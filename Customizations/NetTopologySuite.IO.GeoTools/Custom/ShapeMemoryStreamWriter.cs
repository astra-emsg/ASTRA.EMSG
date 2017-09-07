using System.Collections;
using System.Collections.Generic;
using System.IO;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Handlers;

namespace NetTopologySuite.IO
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
            ShapeHandler handler = Shapefile.GetShapeHandler(Shapefile.GetShapeType(geometryCollection.Geometries[0]));

            IGeometry body;
            int numShapes = geometryCollection.NumGeometries;
            // calc the length of the shp file, so it can put in the header.
            int shpLength = 50;
            for (int i = 0; i < numShapes; i++)
            {
                body = (IGeometry)geometryCollection.Geometries[i];
                shpLength += 4; // length of header in WORDS
                shpLength += handler.GetLength(body); // length of shape in WORDS
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
            shpHeader.ShapeType = Shapefile.GetShapeType(geometryCollection.Geometries[0]);
            shpHeader.Write(shpBinaryWriter);

            // write the .shx header
            ShapefileHeader shxHeader = new ShapefileHeader();
            shxHeader.FileLength = shxLength;
            shxHeader.Bounds = shpHeader.Bounds;

            // assumes Geometry type of the first item will the same for all other items in the collection.
            shxHeader.ShapeType = Shapefile.GetShapeType(geometryCollection.Geometries[0]);
            shxHeader.Write(shxBinaryWriter);

            // write the individual records.
            int _pos = 50; // header length in WORDS
            for (int i = 0; i < numShapes; i++)
            {
                body = geometryCollection.Geometries[i];
                int recordLength = handler.GetLength(body);
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

        public void Close()
        {
            shpStream.Close();
            shxStream.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="recordCount"></param>
        public static void WriteDummyDbf(string filename, int recordCount)
        {
            DbaseFileHeader dbfHeader = new DbaseFileHeader();
            dbfHeader.NumRecords = recordCount;
            dbfHeader.AddColumn("Description", 'C', 20, 0);

            DbaseFileWriter dbfWriter = new DbaseFileWriter(filename);
            dbfWriter.Write(dbfHeader);
            for (int i = 0; i < recordCount; i++)
            {
                List<double> columnValues = new List<double>();
                columnValues.Add((double)i);
                dbfWriter.Write(columnValues);
            }
            // End of file flag (0x1A)
            dbfWriter.Write(0x1A);
            dbfWriter.Close();
        }
    }
}
