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
using GeoAPI.Geometries;
using NetTopologySuite.Algorithm;

namespace NHibernate.Spatial.Oracle
{
    internal class OracleGeometryWriter
    {
        private const int ElementTupleSize = 3;

        public SdoGeometry Write(IGeometry geometry)
        {
            return WriteGeometry(geometry);
        }

        private SdoGeometry WriteGeometry(IGeometry geometry)
        {
         
            if (geometry is IPoint)
            {
                return WritePoint(geometry);
            }
            if (geometry is ILineString)
            {
                return WriteLineString(geometry);
            }
            if (geometry is IPolygon)
            {
                return WritePolygon(geometry);
            }
            if (geometry is IMultiPoint)
            {
                return WriteMultiPoint(geometry);
            }
            if (geometry is IMultiLineString)
            {
                return WriteMultiLineString(geometry);
            }
            if (geometry is IMultiPolygon)
            {
                return WriteMultiPolygon(geometry);
            }
            if (geometry is IGeometryCollection)
            {
                return WriteGeometryCollection(geometry);
            }
            return null;
        }

        private SdoGeometry WritePoint(IGeometry geometry)
        {
            int dimension = GetGeometryDimension(geometry);
            int lrsPos = GetCoordinateLrsPosition(geometry);
            double[] coord = ConvertCoordinates(geometry.Coordinates, dimension, HasLRS(lrsPos));

            SdoGeometry sdoGeometry = new SdoGeometry();
            sdoGeometry.GeometryType = (int)SdoGeometryTypes.GTYPE.POINT;
            sdoGeometry.Dimensionality = dimension;
            sdoGeometry.LRS = lrsPos;
            sdoGeometry.Sdo_Srid = geometry.SRID;
            sdoGeometry.ElemArrayOfInts = new [] { 1, (int)SdoGeometryTypes.ETYPE_SIMPLE.POINT, 1 };
            sdoGeometry.OrdinatesArrayOfDoubles = coord;
            sdoGeometry.PropertiesToGTYPE();
            return sdoGeometry;
        }

        private SdoGeometry WriteLineString(IGeometry geometry)
        {
            int dimension = GetGeometryDimension(geometry);
            int lrsPos = GetCoordinateLrsPosition(geometry);
            double[] ordinates = ConvertCoordinates(geometry.Coordinates, dimension, HasLRS(lrsPos));
            SdoGeometry sdoGeometry = new SdoGeometry();
            sdoGeometry.GeometryType = (int)SdoGeometryTypes.GTYPE.LINE;
            sdoGeometry.Dimensionality = dimension;
            sdoGeometry.LRS = lrsPos;
            sdoGeometry.Sdo_Srid = geometry.SRID;
            sdoGeometry.ElemArrayOfInts = new[] { 1, (int)SdoGeometryTypes.ETYPE_SIMPLE.LINE, 1 };
            sdoGeometry.OrdinatesArrayOfDoubles = ordinates;
            sdoGeometry.PropertiesToGTYPE();
            return sdoGeometry;
        }

        private SdoGeometry WriteMultiLineString(IGeometry geometry)
        {
            int dimension = GetGeometryDimension(geometry);
            int lrsPos = GetCoordinateLrsPosition(geometry);
            SdoGeometry sdoGeometry = new SdoGeometry();
            sdoGeometry.GeometryType = (int)SdoGeometryTypes.GTYPE.MULTILINE;
            sdoGeometry.Dimensionality = dimension;
            sdoGeometry.LRS = lrsPos;
            sdoGeometry.Sdo_Srid = geometry.SRID;
            int[] elements = new int[geometry.NumGeometries * ElementTupleSize];
            int oordinatesOffset = 1;
            double[] ordinates = new double[] { };
            for (int i = 0; i < geometry.NumGeometries; i++)
            {
                elements[i * ElementTupleSize + 0] = oordinatesOffset;
                elements[i * ElementTupleSize + 1] = (int)SdoGeometryTypes.ETYPE_SIMPLE.LINE;
                elements[i * ElementTupleSize + 2] = 1;
                ordinates = AppendCoordinates(ordinates, ConvertCoordinates(geometry.GetGeometryN(i).Coordinates, dimension, HasLRS(lrsPos)));
                oordinatesOffset = ordinates.Length + 1;
            }
            sdoGeometry.ElemArrayOfInts = elements;
            sdoGeometry.OrdinatesArrayOfDoubles = ordinates;
            sdoGeometry.PropertiesToGTYPE();
            return sdoGeometry;
        }

        private SdoGeometry WriteMultiPoint(IGeometry geometry)
        {
            int dimension = GetGeometryDimension(geometry);
            int lrsPos = GetCoordinateLrsPosition(geometry);
            SdoGeometry sdoGeometry = new SdoGeometry();
            sdoGeometry.GeometryType = (int)SdoGeometryTypes.GTYPE.MULTIPOINT;
            sdoGeometry.Dimensionality = dimension;
            sdoGeometry.LRS = lrsPos;
            sdoGeometry.Sdo_Srid = geometry.SRID;

            int[] elements = new int[geometry.NumPoints * ElementTupleSize];
            int oordinatesOffset = 1;
            double[] ordinates = new double[0];
            for (int i = 0; i < geometry.NumPoints; i++)
            {
                elements[i * ElementTupleSize + 0] = oordinatesOffset;
                elements[i * ElementTupleSize + 1] = (int)SdoGeometryTypes.ETYPE_SIMPLE.POINT;
                elements[i * ElementTupleSize + 2] = 0;
                ordinates = AppendCoordinates(ordinates, ConvertCoordinates(geometry.GetGeometryN(i).Coordinates, dimension, HasLRS(lrsPos)));
                oordinatesOffset = ordinates.Length + 1;
            }
            sdoGeometry.ElemArrayOfInts = elements;
            sdoGeometry.OrdinatesArrayOfDoubles = ordinates;
            sdoGeometry.PropertiesToGTYPE();
            return sdoGeometry;
        }

        private SdoGeometry WritePolygon(IGeometry geometry)
        {
            int dimension = GetGeometryDimension(geometry);
            int lrsPos = GetCoordinateLrsPosition(geometry);
            SdoGeometry sdoGeometry = new SdoGeometry();
            sdoGeometry.GeometryType = (int)SdoGeometryTypes.GTYPE.POLYGON;
            sdoGeometry.Dimensionality = dimension;
            sdoGeometry.LRS = lrsPos;
            sdoGeometry.Sdo_Srid = geometry.SRID;
            AddPolygon(sdoGeometry, geometry as IPolygon, dimension, lrsPos);
            sdoGeometry.PropertiesToGTYPE();
            return sdoGeometry;
        }

        private SdoGeometry WriteMultiPolygon(IGeometry geometry)
        {
            int dimension = GetGeometryDimension(geometry);
            int lrsPos = GetCoordinateLrsPosition(geometry);
            SdoGeometry sdoGeometry = new SdoGeometry();
            sdoGeometry.GeometryType = (int)SdoGeometryTypes.GTYPE.MULTIPOLYGON;
            sdoGeometry.Dimensionality = dimension;
            sdoGeometry.LRS = lrsPos;
            sdoGeometry.Sdo_Srid = geometry.SRID;
            for (int i = 0; i < geometry.NumGeometries; i++)
            {
                try
                {
                    IPolygon pg = (IPolygon)geometry.GetGeometryN(i);
                    AddPolygon(sdoGeometry, pg, dimension, lrsPos);
                }
                catch (Exception e)
                {
                    throw new ApplicationException(
                        "Found geometry that was not a Polygon in MultiPolygon", e);
                }
            }
            sdoGeometry.PropertiesToGTYPE();
            return sdoGeometry;
        }

        private SdoGeometry WriteGeometryCollection(IGeometry geometry)
        {
            SdoGeometry[] sdoElements = new SdoGeometry[geometry.NumGeometries];
            for (int i = 0; i < geometry.NumGeometries; i++)
            {
                IGeometry sdoGeometry = geometry.GetGeometryN(i);
                sdoElements[i] = WriteGeometry(sdoGeometry);
            }
            ;
            return SdoGeometry.Join(sdoElements);
        }

        private void AddPolygon(SdoGeometry sdoGeometry, IPolygon polygon, int dimention, int lrsPos)
        {
            int numInteriorRings = polygon.NumInteriorRings;
            int[] elements = new int[(numInteriorRings + 1) * ElementTupleSize];
            int ordinatesOffset = 1;
            ICoordinate[] coords;
            if (sdoGeometry.OrdinatesArray != null && sdoGeometry.OrdinatesArray.Length>0)
            {
                ordinatesOffset = sdoGeometry.OrdinatesArray.Length+1;
            }
            double[] ordinates = new double[] { };

            //addExterior
            elements[0] = ordinatesOffset;
            elements[1] = (int) SdoGeometryTypes.ETYPE_SIMPLE.POLYGON_EXTERIOR;
            elements[2] = 1;
            coords = polygon.ExteriorRing.Coordinates;
            if (!CGAlgorithms.IsCCW(coords))
            {
                coords = ReverseRing(coords);
            }
            ordinates = AppendCoordinates(ordinates, ConvertCoordinates(coords, dimention, HasLRS(lrsPos)));
            ordinatesOffset += ordinates.Length ;
            //add holes
            for (int i = 0; i < numInteriorRings; i++)
            {

                coords = polygon.InteriorRings[i].Coordinates;
                if (CGAlgorithms.IsCCW(coords))
                {
                    coords = ReverseRing(coords);
                }


                elements[(i + 1) * ElementTupleSize + 0] = ordinatesOffset;
                elements[(i + 1) * ElementTupleSize + 1] = (int)SdoGeometryTypes.ETYPE_SIMPLE.POLYGON_INTERIOR;
                elements[(i + 1) * ElementTupleSize + 2] = 1;
                ordinates = AppendCoordinates(ordinates, ConvertCoordinates(coords, dimention, HasLRS(lrsPos)));
                ordinatesOffset = ordinates.Length + 1;
            }
            sdoGeometry.AddElement(elements);
            sdoGeometry.AddOrdinates(ordinates);
        }

        private ICoordinate[] ReverseRing(ICoordinate[] coordinates)
        {
            for (int i = 0; i < coordinates.Length / 2; i++)
            {
                ICoordinate cs = coordinates[i];
                coordinates[i] = coordinates[coordinates.Length - 1 - i];
                coordinates[coordinates.Length - 1 - i] = cs;
            }
            return coordinates;
        }

        private double[] AppendCoordinates(double[] ordinates,
                                        double[] appendedOrdinates)
        {

            double[] newordinates = new double[ordinates.Length + appendedOrdinates.Length];
            Array.Copy(ordinates, 0, newordinates, 0, ordinates.Length);
            Array.Copy(appendedOrdinates, 0, newordinates, ordinates.Length, appendedOrdinates.Length);
            return newordinates;
        }

        /// <summary>
        /// Convert the coordinates to a double array for purposes of persisting them
        /// to the database.
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="dim"></param>
        /// <returns></returns>
        private double[] ConvertCoordinates(ICoordinate[] coordinates, int dim, bool IsLRS)
        {
            if (dim > 4)
            {
                throw new ArgumentException("Dim parameter value cannot be greater than 4");
            }
            double[] coordinateArray = new double[coordinates.Length * dim];
            for (int i = 0; i < coordinates.Length; i++)
            {
                ICoordinate coordinate = coordinates[i];

                // set the X and Y values
                coordinateArray[i * dim] = coordinate.X;
                coordinateArray[i * dim + 1] = coordinate.Y;
                if (dim == 3)
                {
                	coordinateArray[i * dim + 2] = IsLRS ? coordinate.M : coordinate.Z;
                } 
                else if (dim == 4)
                {
                	coordinateArray[i * dim + 2] = coordinate.Z;
                	coordinateArray[i * dim + 3] = coordinate.M;
            	}

            }
            return coordinateArray;
        }
        
        /// <summary>
        /// Return the dimension required for building the gType in the SdoGeometry
        /// object. Has support for LRS type geometries.
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        private int GetGeometryDimension(IGeometry geom)
        {
            ICoordinate coordinate = geom.Coordinate;
            int dimension = 0;
            if (coordinate != null)
            {
                if (!Double.IsNaN(coordinate.X))
                    dimension++;
                if (!Double.IsNaN(coordinate.Y))
                    dimension++;
                if (!Double.IsNaN(coordinate.Z))
                    dimension++;
                if (!Double.IsNaN(coordinate.M))
                    dimension++;
            }
            return dimension;
        }
        
        private int GetCoordinateLrsPosition(IGeometry geom) {
        	ICoordinate coordinate = geom.Coordinate;
        	int measurePos = 0;
        	if (coordinate != null && !Double.IsNaN(coordinate.M)) {
            	measurePos = (Double.IsNaN(coordinate.Z)) ? 3 : 4;
        	}
        	return measurePos;
   		}
        
        private bool HasLRS(int lrsPosition)
        {
        	return lrsPosition > 0;
        }
    }
}