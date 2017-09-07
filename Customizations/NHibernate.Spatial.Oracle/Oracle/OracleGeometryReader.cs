// Copyright 2008 - Ricardo Stuven (rstuven@gmail.com)
//
// This file is part of NHibernate.Spatial.
// NHibernate.Spatial is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// NHibernate.Spatial is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with NHibernate.Spatial; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 

using System;
using System.Collections.Generic;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.MGeometries;

namespace NHibernate.Spatial.Oracle
{
    internal class OracleGeometryReader
    {
        private readonly IGeometryFactory _factory;

        private const int ElementTupleSize = 3;
        /// <summary>
        /// Initialize reader with a standard <c>GeometryFactory</c>. 
        /// </summary>
        public OracleGeometryReader() : this(new MGeometryFactory()) { }

        /// <summary>
        /// Initialize reader with the given <c>GeometryFactory</c>.
        /// </summary>
        /// <param name="factory"></param>
        public OracleGeometryReader(IGeometryFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// <c>Geometry</c> builder.
        /// </summary>
        protected virtual IGeometryFactory Factory
        {
            get { return _factory; }
        }

        public IGeometry Read(SdoGeometry geometry)
        {
            return ReadGeometry(geometry);
        }

        private IGeometry ReadGeometry(SdoGeometry sdoGeom)
        {
            if (sdoGeom==null)
            {
                return null;
            }
            sdoGeom.PropertiesFromGTYPE();
            if (sdoGeom.GeometryType == (int)SdoGeometryTypes.GTYPE.POINT)
            {
                return ReadPoint(sdoGeom);
            }
            if (sdoGeom.GeometryType == (int)SdoGeometryTypes.GTYPE.LINE)
            {
                return ReadLine(sdoGeom);
            }
            if (sdoGeom.GeometryType == (int)SdoGeometryTypes.GTYPE.POLYGON)
            {
                return ReadPolygon(sdoGeom);
            }

            if (sdoGeom.GeometryType == (int)SdoGeometryTypes.GTYPE.MULTIPOINT)
            {
                return ReadMultiPoint(sdoGeom);
            }
            if (sdoGeom.GeometryType == (int)SdoGeometryTypes.GTYPE.MULTILINE)
            {
                return ReadMultiLine(sdoGeom);
            }
            if (sdoGeom.GeometryType == (int)SdoGeometryTypes.GTYPE.MULTIPOLYGON)
            {
                return ReadMultiPolygon(sdoGeom);
            }
            if (sdoGeom.GeometryType == (int)SdoGeometryTypes.GTYPE.COLLECTION)
            {
                return ReadGeometryCollection(sdoGeom);
            }

            throw new ArgumentException(String.Format("Geometry with SDO Geometry Type {0} is not supported",+ sdoGeom.Sdo_Gtype));
        }

        #region multi Geometries


        private IGeometry ReadGeometryCollection(SdoGeometry sdoGeom)
        {
            return GeometryCollection.Empty;
            //List<IGeometry> geometries = new List<IGeometry>();
            //foreach (SdoGeometry elemGeom in sdoGeom.getElementGeometries())
            //{
            //    geometries.AppendCoordinateSequence(ReadGeometry(elemGeom));
            //}
            //return factory.CreateGeometryCollection(geometries.ToArray());
        }

        private IMultiPoint ReadMultiPoint( SdoGeometry sdoGeom)
        {
            if (sdoGeom.OrdinatesArray.Length == 0)
                return MultiPoint.Empty;
            double[] ordinates = sdoGeom.OrdinatesArrayOfDoubles;
            ICoordinateSequence cs = ConvertOrdinateArray(ordinates, sdoGeom);
            IMultiPoint multipoint = _factory.CreateMultiPoint(cs);
            multipoint.SRID = (int)sdoGeom.Sdo_Srid;
            return multipoint;
        }

        private IMultiLineString ReadMultiLine(SdoGeometry sdoGeom)
        {

            int[] elements = sdoGeom.ElemArrayOfInts;
            ILineString[] lines = sdoGeom.IsLrsGeometry 
            	? new MLineString[sdoGeom.ElemArray.Length/ElementTupleSize] 
            	: new ILineString[sdoGeom.ElemArray.Length/ElementTupleSize];
            int i = 0;
            while (i < elements.Length/ElementTupleSize)
            {
                ICoordinateSequence cs = null;
                if (GetElementType(elements,i)==(int)SdoGeometryTypes.ETYPE_COMPOUND.FOURDIGIT)
                {
                    int numCompounds =GetNumCompounds(elements,i);
                    cs = AppendCoordinateSequence(cs, GetCompoundCSeq(i + 1, i + numCompounds, sdoGeom));
                    ILineString line = sdoGeom.IsLrsGeometry 
                    	? ((MGeometryFactory)_factory).CreateMLineString(cs) 
                    	: _factory.CreateLineString(cs);
                    lines[i] = line;
                    i += 1 + numCompounds;
                }
                else
                {
                    cs = AppendCoordinateSequence(cs, GetElementCoordinateSequence(i, sdoGeom, false));
                    ILineString line = line = sdoGeom.IsLrsGeometry 
                    	? ((MGeometryFactory)_factory).CreateMLineString(cs) 
                    	: _factory.CreateLineString(cs);
                    lines[i] = line;
                    i++;
                }
            }

            IMultiLineString mls;
            if (sdoGeom.IsLrsGeometry)
            {
            	mls = ((MGeometryFactory)_factory).CreateMultiMLineString((MLineString[])lines);
            }
            else
            {
            	mls = _factory.CreateMultiLineString(lines);
            }            
            mls.SRID = (int)sdoGeom.Sdo_Srid;
            return mls;
        }

        private IMultiPolygon ReadMultiPolygon(SdoGeometry sdoGeom)
        {
            if (sdoGeom.OrdinatesArray.Length == 0)
                return MultiPolygon.Empty;
            List<ILinearRing> holes = new List<ILinearRing>();
            List<IPolygon> polygons = new List<IPolygon>();
            int[] elements = sdoGeom.ElemArrayOfInts;
            ILinearRing shell = null;
            int i = 0;
            while (i < elements.Length/ElementTupleSize)
            {
                ICoordinateSequence coordinateSequence = null;
                int elementType = GetElementType(elements, i);
                int numCompounds = 0;
                if (elementType == (int)SdoGeometryTypes.ETYPE_COMPOUND.POLYGON_EXTERIOR ||
                    elementType == (int)SdoGeometryTypes.ETYPE_COMPOUND.POLYGON_INTERIOR)
                {
                    numCompounds = GetNumCompounds(elements, i);
                    coordinateSequence = AppendCoordinateSequence(coordinateSequence, GetCompoundCSeq(i + 1, i + numCompounds, sdoGeom));
                }
                else
                {
                    coordinateSequence = AppendCoordinateSequence(coordinateSequence, GetElementCoordinateSequence(i, sdoGeom, false));
                }
                if (elementType == (int)SdoGeometryTypes.ETYPE_COMPOUND.POLYGON_INTERIOR
                    || elementType == (int)SdoGeometryTypes.ETYPE_SIMPLE.POLYGON_INTERIOR)
                {
                    ILinearRing linearRing = _factory.CreateLinearRing(coordinateSequence);
                    linearRing.SRID = (int)sdoGeom.Sdo_Srid;
                    holes.Add(linearRing);
                }
                else
                {
                    if (shell != null)
                    {
                        IPolygon polygon = _factory.CreatePolygon(shell, holes.ToArray());
                        polygon.SRID = (int)sdoGeom.Sdo_Srid;
                        polygons.Add(polygon);
                        shell = null;
                    }
                    shell = _factory.CreateLinearRing(coordinateSequence);
                    shell.SRID = (int)sdoGeom.Sdo_Srid;
                    holes = new List<ILinearRing>();
                }
                i += 1 + numCompounds;
            }
            if (shell != null)
            {
                IPolygon polygon = _factory.CreatePolygon(shell, holes.ToArray());
                polygon.SRID = (int)sdoGeom.Sdo_Srid;
                polygons.Add(polygon);
            }
            IMultiPolygon multiPolygon = _factory.CreateMultiPolygon(polygons.ToArray());
            multiPolygon.SRID = (int)sdoGeom.Sdo_Srid;
            return multiPolygon;
        }

        #endregion

        private IPoint ReadPoint(SdoGeometry sdoGeom)
        {
            if (sdoGeom.OrdinatesArray.Length == 0) 
                return Point.Empty;
            double[] ordinates = sdoGeom.OrdinatesArrayOfDoubles;
            if (ordinates.Length == 0)
            {
                if (sdoGeom.Dimensionality == 2)
                {
                    ordinates = new[] { (double)sdoGeom.Point.X.Value, (double)sdoGeom.Point.Y.Value };
                }
                else
                {
                    ordinates = new[] { (double)sdoGeom.Point.X.Value, (double)sdoGeom.Point.Y.Value, (double)sdoGeom.Point.Z.Value };
                }
            }
            ICoordinateSequence cs = ConvertOrdinateArray(ordinates, sdoGeom);
            IPoint point = _factory.CreatePoint(cs);
            point.SRID = (int)sdoGeom.Sdo_Srid;
            return point;
        }

        private ILineString ReadLine(SdoGeometry sdoGeom)
        {
            if (sdoGeom.OrdinatesArray.Length == 0)
                return LineString.Empty;
            int[] elements = sdoGeom.ElemArrayOfInts;
            ICoordinateSequence cs = null;

            int i = 0;
            while (i < elements.Length / ElementTupleSize)
            {

                if (GetElementType(elements, i) == (int)SdoGeometryTypes.ETYPE_COMPOUND.FOURDIGIT)
                {
                    throw new NotImplementedException("NhibernateSpatial does not support compound Lines");
                }
                else
                {
                    cs = AppendCoordinateSequence(cs, GetElementCoordinateSequence(i, sdoGeom, false));
                    i++;
                }
            }
            
            ILineString ls;
            if (sdoGeom.IsLrsGeometry)
            {
            	ls = ((MGeometryFactory)_factory).CreateMLineString(cs);
            }
            else
            {
            	ls = _factory.CreateLineString(cs);
            }
            
            if (sdoGeom.Sdo_Srid.HasValue)
            {
            	ls.SRID = (int)sdoGeom.Sdo_Srid;
            }
            return ls;
        }

        private int GetNumCompounds(int[] info, int i)
        {
            return info[i * 3 + 2];
        }

        private int GetElementType(int[] info, int i)
        {
            return info[i * 3 + 1];

        }

        private IGeometry ReadPolygon(SdoGeometry sdoGeom)
        {
            if (sdoGeom.OrdinatesArray.Length == 0)
                return Polygon.Empty;
            ILinearRing shell = null;
            ILinearRing[] holes = new LinearRing[sdoGeom.NumElements - 1];
            int[] info = sdoGeom.ElemArrayOfInts;
            int i = 0;
            int idxInteriorRings = 0;
            while (i < info.Length / ElementTupleSize)
            {
                ICoordinateSequence coordinates = null;
                int numCompounds = 0;
                int elementType = GetElementType(info, i);
                if (elementType == (int)SdoGeometryTypes.ETYPE_COMPOUND.POLYGON_EXTERIOR ||
                    elementType == (int)SdoGeometryTypes.ETYPE_COMPOUND.POLYGON_INTERIOR)
                {
                    numCompounds = GetNumCompounds(info, i);
                    coordinates = AppendCoordinateSequence(coordinates, GetCompoundCSeq(i + 1, i + numCompounds, sdoGeom));
                }
                else
                {
                    coordinates = AppendCoordinateSequence(coordinates, GetElementCoordinateSequence(i, sdoGeom, false));
                }
                if (elementType == (int)SdoGeometryTypes.ETYPE_SIMPLE.POLYGON_INTERIOR ||
                    elementType == (int)SdoGeometryTypes.ETYPE_COMPOUND.POLYGON_INTERIOR)
                {
                    holes[idxInteriorRings] = _factory.CreateLinearRing(coordinates);
                    holes[idxInteriorRings].SRID = (int)sdoGeom.Sdo_Srid;
                    idxInteriorRings++;
                }
                else
                {
                    shell = _factory.CreateLinearRing(coordinates);
                    shell.SRID = (int)sdoGeom.Sdo_Srid;
                }
                i += 1 + numCompounds;
            }
            IPolygon polygon = _factory.CreatePolygon(shell, holes);
            polygon.SRID = (int)sdoGeom.Sdo_Srid;
            return polygon;
        }

        /**
         * Gets the ICoordinateSequence corresponding to a compound element.
         * 
         * @param idxFirst
         *            the first sub-element of the compound element
         * @param idxLast
         *            the last sub-element of the compound element
         * @param sdoGeom
         *            the SdoGeometry that holds the compound element.
         * @return
         */
        private ICoordinateSequence GetCompoundCSeq(int idxFirst, int idxLast,
                                                    SdoGeometry sdoGeom)
        {
            ICoordinateSequence cs = null;
            for (int i = idxFirst; i <= idxLast; i++)
            {
                // pop off the last element as it is added with the next
                // coordinate sequence
                if (cs != null && cs.Count > 0)
                {
                    ICoordinate[] coordinates = cs.ToCoordinateArray();
                    ICoordinate[] newCoordinates = new ICoordinate[coordinates.Length - 1];
                    Array.Copy(coordinates, 0, newCoordinates, 0, coordinates.Length - 1);
                    cs = _factory.CoordinateSequenceFactory.Create(newCoordinates);
                }
                cs = AppendCoordinateSequence(cs, GetElementCoordinateSequence(i, sdoGeom, (i < idxLast)));
            }
            return cs;
        }


        /// <summary>
        /// Get coordinate sequence of an element
        /// </summary>
        /// <param name="i"></param>
        /// <param name="sdoGeom"></param>
        /// <param name="hasNextSE"></param>
        /// <returns></returns>
        private ICoordinateSequence GetElementCoordinateSequence(int i, SdoGeometry sdoGeom, bool hasNextSE)
        {
            int type = sdoGeom.ElemArrayOfInts[i * ElementTupleSize + 1];
            int interpretation = sdoGeom.ElemArrayOfInts[i * ElementTupleSize + 2];
            Double[] elemOrdinates = ExtractOrdinatesOfElement(i, sdoGeom, hasNextSE);

            ICoordinateSequence cs;

            if (interpretation ==1)
            {
                cs = ConvertOrdinateArray(elemOrdinates, sdoGeom);
            }
            else if (interpretation == 2 || interpretation == 4)
            {
                throw new NotImplementedException("Arc segment elements are not supported");
            }
            else if (interpretation == 3)
            {
               
               cs = ConvertRectangleOrdinateArray(elemOrdinates, sdoGeom);
            }
            else
            {
                throw new ApplicationException(String.Format("Unsupportes element interpretation {0} in compound type{1}",interpretation,type));
            }
            return cs;
        }

        private ICoordinateSequence AppendCoordinateSequence(ICoordinateSequence seq1,
                                        ICoordinateSequence seq2)
        {
            if (seq1 == null)
            {
                return seq2;
            }
            if (seq2 == null)
            {
                return seq1;
            }
            ICoordinate[] c1 = seq1.ToCoordinateArray();
            ICoordinate[] c2 = seq2.ToCoordinateArray();
            ICoordinate[] c3 = new Coordinate[c1.Length + c2.Length];
            Array.Copy(c1, 0, c3, 0, c1.Length);
            Array.Copy(c2, 0, c3, c1.Length, c2.Length);
            return _factory.CoordinateSequenceFactory.Create(c3);
        }

        private Double[] ExtractOrdinatesOfElement(int elementIndex, SdoGeometry sdoGeom, bool hasNextSE)
        {
            int start = (int)sdoGeom.ElemArray[elementIndex * 3];
            if (((elementIndex + 1) * 3) < sdoGeom.ElemArray.Length - 1)
            {
                int end = (int)sdoGeom.ElemArray[(elementIndex + 1) * 3];
                // if this is a subelement of a compound geometry,
                // the last point is the first point of
                // the next subelement.
                if (hasNextSE)
                {
                    end += sdoGeom.Dimensionality;
                }
                return SubArray(sdoGeom.OrdinatesArrayOfDoubles, start - 1, end - start);
            }
            else
            {
                return SubArray(sdoGeom.OrdinatesArrayOfDoubles, start-1, sdoGeom.OrdinatesArrayOfDoubles.Length - start+1);
            }
        }

        public static T[] SubArray<T>(T[] data, int index, int length)
        {
         
                T[] result = new T[length];
                Array.Copy(data, index, result, 0, length);
                return result;
           
        }
        private ICoordinateSequence ConvertRectangleOrdinateArray(Double[] oordinates, SdoGeometry sdoGeom)
        {
            ICoordinate[] coordinates = getCoordinates(oordinates, sdoGeom);
            if(coordinates.Length != 2)
                throw new Exception("Geometry is not a Rectangle");

           
            ICoordinate[] rectCoordinates = new ICoordinate[5];
            rectCoordinates[0] = coordinates[0];
            rectCoordinates[1] = new Coordinate(coordinates[0].X, coordinates[1].Y);
            rectCoordinates[2] = coordinates[1];
            rectCoordinates[3] = new Coordinate(coordinates[1].X, coordinates[0].Y);
            rectCoordinates[4] = coordinates[0];

            return _factory.CoordinateSequenceFactory.Create(rectCoordinates);

        }
                                                        

        private ICoordinateSequence ConvertOrdinateArray(Double[] oordinates, SdoGeometry sdoGeom
                                                        )
        {
            ICoordinate[] coordinates = getCoordinates(oordinates, sdoGeom);
            return _factory.CoordinateSequenceFactory.Create(coordinates);
        }

        private ICoordinate[] getCoordinates(Double[] oordinates, SdoGeometry sdoGeom)
        {
            int dimension = sdoGeom.Dimensionality;

            ICoordinate[] coordinates = new ICoordinate[oordinates.Length / dimension];

            int zDim = sdoGeom.ZDimension - 1;
            int lrsDim = sdoGeom.LRSDimension - 1;

            for (int i = 0; i < coordinates.Length; i++)
            {
                if (dimension == 2)
                {
                    coordinates[i] = new Coordinate(
                        oordinates[i * dimension],
                        oordinates[i * dimension + 1]);
                }
                else if (dimension == 3)
                {
                    if (sdoGeom.IsLrsGeometry)
                    {
                        coordinates[i] = MCoordinate.Create2dWithMeasure(
                        oordinates[i * dimension],
                        oordinates[i * dimension + 1],
                        oordinates[i * dimension + lrsDim]);
                    }
                    else
                    {
                        coordinates[i] = new Coordinate(
                        oordinates[i * dimension],
                        oordinates[i * dimension + 1],
                        oordinates[i * dimension + zDim]);
                    }
                }
                else if (dimension == 4)
                {
                    // This must be an LRS Geometry
                    if (!sdoGeom.IsLrsGeometry)
                        throw new Exception(
                            "4 dimensional Geometries must be LRS geometry");

                    coordinates[i] = MCoordinate.Create3dWithMeasure(
                        oordinates[i * dimension],
                        oordinates[i * dimension + 1],
                        oordinates[i * dimension + zDim],
                        oordinates[i * dimension + lrsDim]);
                }
            }
            return coordinates;

        }

        // reverses ordinates in a coordinate array in-place
        private ICoordinate[] ReverseRing(ICoordinate[] ar)
        {
            for (int i = 0; i < ar.Length / 2; i++)
            {
                ICoordinate cs = ar[i];
                ar[i] = ar[ar.Length - 1 - i];
                ar[ar.Length - 1 - i] = cs;
            }
            return ar;
        }
    }
}