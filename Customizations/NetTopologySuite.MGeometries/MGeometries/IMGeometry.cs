﻿/**
 * $Id$
 *
 * This file is part of Hibernate Spatial, an extension to the 
 * hibernate ORM solution for geographic data. 
 *  
 * Copyright © 2007 Geovise BVBA
 * Copyright © 2007 K.U. Leuven LRD, Spatial Applications Division, Belgium
 *
 * This work was partially supported by the European Commission, 
 * under the 6th Framework Programme, contract IST-2-004688-STP.
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 * For more information, visit: http://www.hibernatespatial.org/
 */

using System;
using GeoAPI.Geometries;

namespace NetTopologySuite.Geometries.MGeometries
{

	public enum MGeometryType
	{
		/// <summary>
		/// Measures are increasing in the direction of the geometry 
		/// </summary>
		Increasing = 1,

		/// <summary>
		/// Measures are constant across the geometry
		/// </summary>
		Constant = 0,

		/// <summary>
		/// Measures are decreasing in the direction of the geometry 
		/// </summary>
		Decreasing = -1,

		/// <summary>
		/// Measures are not monotone along the geometry 
		/// </summary>
		NonMonotone = -3
	}

	/// <summary>
	/// Defines geometries that carry measures in their CoordinateSequence. 
	/// @author Karel Maesen
	/// </summary>
	public interface IMGeometry : ICloneable
	{

		/**
		 * Returns the measure value at the Coordinate
		 * 
		 * @param c
		 *            the Coordinate for which the measure value is sought
		 * @param tolerance
		 *            distance to the IMGeometry within which Coordinate c has to lie
		 * @return the measure value if Coordinate c is within tolerance of the
		 *         Geometry, else Double.NaN
		 *         <p>
		 *         When the geometry is a ring or is self-intersecting more
		 *         coordinates may be determined by one coordinate. In that case,
		 *         the lowest measure is returned.
		 * @throws MGeometryException
		 *             when this IMGeometry is not monotone
		 */
		double GetMatCoordinate(Coordinate c, double tolerance);


		/**
		 * Builds measures along the Geometry based on the length from the beginning
		 * (first coordinate) of the Geometry.
		 * 
		 * @param keepBeginMeasure -
		 *            if true, the measure of the first coordinate is maintained and
		 *            used as start value, unless this measure is Double.NaN
		 */
		void MeasureOnLength(bool keepBeginMeasure);

		/**
		 * Returns the Coordinate along the Geometry at the measure value
		 * 
		 * @param M
		 *            measure value
		 * @return the Coordinate if M is on the IMGeometry otherwise null
		 * @throws MGeometryException
		 *             when IMGeometry is not monotone
		 */
		Coordinate GetCoordinateAtM(double m);

		/**
		 * Returns the coordinatesequence(s) containing all coordinates between the
		 * begin and end measures.
		 * 
		 * @param begin
		 *            begin measure
		 * @param end
		 *            end measure
		 * @return an array containing all coordinatesequences in order between
		 *         begin and end. Each CoordinateSequence covers a contiguous
		 *         stretch of the IMGeometry.
		 * @throws MGeometryException
		 *             when this IMGeometry is not monotone
		 */
		ICoordinateSequence[] GetCoordinatesBetween(double begin, double end);

		/**
		 * Returns the GeometryFactory of the IMGeometry
		 * 
		 * @return the GeometryFactory of this IMGeometry
		 */
		IGeometryFactory Factory { get; }

		/**
		 * Returns the minimum M-value of the IMGeometry
		 * 
		 * @return the minimum M-value
		 */
		double GetMinM();

		/**
		 * Returns the maximum M-value of the IMGeometry
		 * 
		 * @return the maximum M-value
		 */
		double GetMaxM();

		/**
		 * Determine whether the LRS measures (not the x,y,z coordinates) in the
		 * Coordinate sequence of the geometry is Monotone. Monotone implies that
		 * all measures in a sequence of coordinates are consecutively increasing,
		 * decreasing or equal according to the definition of the implementing
		 * geometry. Monotonicity is a pre-condition for most operations on
		 * MGeometries. The following are examples on Monotone measure sequences on
		 * a line string:
		 * <ul>
		 * <li> [0,1,2,3,4] - Monotone Increasing
		 * <li> [4,3,2,1] - Monotone Decreasing
		 * <li> [0,1,1,2,3] - Non-strict Monotone Increasing
		 * <li> [5,3,3,0] - Non-strict Monotone Decreasing
		 * </ul>
		 * 
		 * @return true if the coordinates in the CoordinateSequence of the geometry
		 *         are monotone.
		 */
		bool IsMonotone(bool strict);

		// /**
		// * Strict Monotone is similar to Monotone, with the added constraint that
		// all measure coordinates
		// * in the CoordinateSequence are ONLY consecutively increasing or
		// decreasing. No consecutive
		// * duplicate measures are allowed.
		// *
		// * @return true if the coordinates in the CoordinateSequence of the
		// geometry are strictly monotone; that is, consitently
		// * increasing or decreasing with no duplicate measures.
		// * @see #IsMonotone()
		// */
		// bool isStrictMonotone();

	}

}
