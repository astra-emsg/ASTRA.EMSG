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

namespace NetTopologySuite.Geometries.MGeometries
{
	public class EventLocator
	{
		/**
		 * 
		 * @return a Point Geometry as a point geometry
		 * @throws MGeometryException
		 */
		public static Point GetPointGeometry(IMGeometry lrs, double position)
		{
			Coordinate c = lrs.GetCoordinateAtM(position);
			ICoordinateSequence cs = lrs.Factory.CoordinateSequenceFactory.Create(new Coordinate[] { c });
			return new Point(cs, lrs.Factory);
		}

		public static MultiMLineString GetLinearGeometry(IMGeometry lrs, double begin, double end)
		{
			MGeometryFactory factory = (MGeometryFactory) lrs.Factory;
			ICoordinateSequence[] cs = lrs.GetCoordinatesBetween(begin, end);
			MLineString[] mlar = new MLineString[cs.Length];
			for (int i = 0; i < cs.Length; i++) {
				MLineString ml = factory.CreateMLineString(cs[i]);
				mlar[i] = ml;
			}
			return factory.CreateMultiMLineString(mlar);
		}
	}
}