using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using NetTopologySuite.LinearReferencing;

namespace ASTRA.EMSG.Business.AchsenUpdate.UpdateReferences
{
    public enum GeometryTransformerResultState
        {
            Success,
            FailedWouldBeTooShort,
            FailedWouldBeOutside
        }
    public class GeometryTransformerResult
    {
        public GeometryTransformerResult(GeometryTransformerResultState resultState, ILineString newRefGeometry)
        {
            this.ResultState = resultState;
            this.NewRefGeometry = newRefGeometry;
        }
        public GeometryTransformerResultState ResultState { get; private set; }
        public ILineString NewRefGeometry { get; private set; }
 
    }
    public class GeometryTransformer
    {
        

        private readonly ILineString oldRefGeometry; 
        private readonly ILineString newSegmentGeometry; 
        private ILineString newRefGeometryOut = null;

        public GeometryTransformer(ILineString oldRefGeometry, ILineString newSegmentGeometry)
        {
            this.oldRefGeometry = oldRefGeometry;
            this.newSegmentGeometry = newSegmentGeometry;
        }


        protected virtual Coordinate TransformCoordiante(Coordinate oldCoordinate)
        {
            return oldCoordinate; // transformation optionally done by subclasses
        }

        public GeometryTransformerResult Transform()
        {
            newRefGeometryOut = null;

            int n = oldRefGeometry.Coordinates.Length;

            Coordinate startCoodinate = TransformCoordiante(oldRefGeometry.Coordinates[0]);
            Coordinate endCoordinate = TransformCoordiante(oldRefGeometry.Coordinates[n - 1]);

            LengthIndexedLine lil = new LengthIndexedLine(newSegmentGeometry);

            double projStartIndex = lil.IndexOf(startCoodinate);
            double projEndIndex = lil.IndexOf(endCoordinate);
            if (projStartIndex > projEndIndex) 
            {
                double temp = projStartIndex;
                projStartIndex = projEndIndex;
                projEndIndex = temp;
            }
            if (Math.Abs(projStartIndex - projEndIndex) < 1)
            {
                if (projStartIndex == 0 || (Math.Abs(projStartIndex - lil.EndIndex) < double.Epsilon))
                {
                    return new GeometryTransformerResult(GeometryTransformerResultState.FailedWouldBeOutside, newRefGeometryOut);
                }
                return new GeometryTransformerResult(GeometryTransformerResultState.FailedWouldBeTooShort, newRefGeometryOut);
            }

            newRefGeometryOut = (ILineString)lil.ExtractLine(projStartIndex, projEndIndex);


            return new GeometryTransformerResult(GeometryTransformerResultState.Success, newRefGeometryOut);
        }


      

        public static double CalculateGeometryDifferenceRatio(ILineString oldGeom, ILineString newGeom)
        {
            int n = newGeom.Coordinates.Length;

            Coordinate startCoodinate = newGeom.Coordinates[0];
            Coordinate endCoordinate = newGeom.Coordinates[n - 1];

            LengthIndexedLine lil = new LengthIndexedLine(oldGeom);

            double projStartIndex = lil.IndexOf(startCoodinate);
            double projEndIndex = lil.IndexOf(endCoordinate);

            double lenOld = oldGeom.Length;

            double lenNew = Math.Abs(projEndIndex - projStartIndex);

            return (lenNew / lenOld);
        }


    }
}
