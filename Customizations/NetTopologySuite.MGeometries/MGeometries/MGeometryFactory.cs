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
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System;

namespace NetTopologySuite.Geometries.MGeometries
{
	/**
	 * Extension of the GeometryFactory for constructing Geometries with Measure
	 * support.
	 * 
	 * @see com.vividsolutions.jts.geom.GeometryFactory
	 */
    [Serializable]
	public class MGeometryFactory : GeometryFactory
	{
		public MGeometryFactory(IPrecisionModel precisionModel, int SRID,
				MCoordinateSequenceFactory coordinateSequenceFactory)
			: base(precisionModel, SRID, coordinateSequenceFactory)
		{
		}

		public MGeometryFactory(MCoordinateSequenceFactory coordinateSequenceFactory)
			: base(coordinateSequenceFactory)
		{
		}

		public MGeometryFactory(IPrecisionModel precisionModel)
			: this(precisionModel, 0, MCoordinateSequenceFactory.Instance)
		{
		}

		public MGeometryFactory(IPrecisionModel precisionModel, int SRID)
			: this(precisionModel, SRID, MCoordinateSequenceFactory.Instance)
		{
		}

		public MGeometryFactory()
			: this(new PrecisionModel(), 0)
		{
		}

		/**
		 * Constructs a MLineString using the given Coordinates; a null or empty
		 * array will create an empty MLineString.
		 * 
		 * @param coordinates
		 *            array of MCoordinate defining this geometry's vertices
		 * @see #createLineString(com.vividsolutions.jts.geom.Coordinate[])
		 * @return An instance of MLineString containing the coordinates
		 */
		public MLineString CreateMLineString(MCoordinate[] coordinates)
		{
			return CreateMLineString(
				coordinates != null
					? CoordinateSequenceFactory.Create(coordinates)
					: null);
		}

		public MultiMLineString CreateMultiMLineString(MLineString[] mlines, double mGap)
		{
			return new MultiMLineString(mlines, mGap, this);
		}

		public MultiMLineString CreateMultiMLineString(MLineString[] mlines)
		{
			return new MultiMLineString(mlines, 0.0d, this);
		}

		/**
		 * Creates a MLineString using the given CoordinateSequence; a null or empty
		 * CoordinateSequence will create an empty MLineString.
		 * 
		 * @param coordinates
		 *            a CoordinateSequence possibly empty, or null
		 * @return An MLineString instance based on the <code>coordinates</code>
		 * @see #createLineString(com.vividsolutions.jts.geom.CoordinateSequence)
		 */
		public MLineString CreateMLineString(ICoordinateSequence coordinates)
		{
			return new MLineString(coordinates, this);
		}

	}
}
