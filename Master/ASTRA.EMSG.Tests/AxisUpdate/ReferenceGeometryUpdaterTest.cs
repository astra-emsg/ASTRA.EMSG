using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NetTopologySuite.Geometries;
using GeoAPI.Geometries;
using ASTRA.EMSG.Business.AchsenUpdate.UpdateReferences;


namespace ASTRA.EMSG.Tests.AxisUpdate
{
    [TestFixture]
    public class ReferenceGeometryUpdaterTest
    {
        [Test]
        public void GeometryUpdaterTest_SuccessEqualGeoms()
        {
            IGeometryFactory gf = new GeometryFactory();

            ILineString oldRefGeometry = gf.CreateLineString(new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 0) });
            ILineString newSegmentGeometry = gf.CreateLineString(new Coordinate[] { new Coordinate(0, 0), new Coordinate(2, 0) });
            
            GeometryTransformer gt = new GeometryTransformer(oldRefGeometry, newSegmentGeometry);
            var result = gt.Transform();
            ILineString newRefGeometry = result.NewRefGeometry;

            Assert.AreEqual(oldRefGeometry, newRefGeometry);
            Assert.IsTrue(result.ResultState == GeometryTransformerResultState.Success);
        }

        [Test]
        public void GeometryUpdaterTest_SuccessShorter()
        {
            IGeometryFactory gf = new GeometryFactory();

            ILineString oldRefGeometry = gf.CreateLineString(new Coordinate[] { new Coordinate(0, 0), new Coordinate(2, 0) });
            ILineString newSegmentGeometry = gf.CreateLineString(new Coordinate[] { new Coordinate(0.5, 1), new Coordinate(2.5, 1) });

            GeometryTransformer gt = new GeometryTransformer(oldRefGeometry, newSegmentGeometry);
            var result = gt.Transform();
            ILineString newRefGeometry = result.NewRefGeometry;

            ILineString expectedGeometry = gf.CreateLineString(new Coordinate[] { new Coordinate(0.5, 1), new Coordinate(2, 1) });

            Assert.AreEqual(expectedGeometry, newRefGeometry);
            Assert.IsTrue(result.ResultState == GeometryTransformerResultState.Success);
        }

        [Test]
        public void GeometryUpdaterTest_FailedOutside()
        {
            IGeometryFactory gf = new GeometryFactory();

            ILineString oldRefGeometry = gf.CreateLineString(new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 0) });
            ILineString newSegmentGeometry = gf.CreateLineString(new Coordinate[] { new Coordinate(1, 0), new Coordinate(2, 0) });

            GeometryTransformer gt = new GeometryTransformer(oldRefGeometry, newSegmentGeometry);
            var result = gt.Transform();
            ILineString newRefGeometry = result.NewRefGeometry;

            Assert.IsNull(newRefGeometry);
            Assert.IsTrue(result.ResultState == GeometryTransformerResultState.FailedWouldBeOutside);
        }

        [Test]
        public void GeometryUpdaterTest_FailedOutside2()
        {
            IGeometryFactory gf = new GeometryFactory();

            ILineString oldRefGeometry = gf.CreateLineString(new Coordinate[] { new Coordinate(1, 0), new Coordinate(2, 0) });
            ILineString newSegmentGeometry = gf.CreateLineString(new Coordinate[] { new Coordinate(0, 1), new Coordinate(0.5, 1) });

            GeometryTransformer gt = new GeometryTransformer(oldRefGeometry, newSegmentGeometry);
            var result = gt.Transform();
            ILineString newRefGeometry = result.NewRefGeometry;

            Assert.IsNull(newRefGeometry);
            Assert.IsTrue(result.ResultState == GeometryTransformerResultState.FailedWouldBeOutside);
        }

        [Test]
        public void GeometryUpdaterTest_FailedTooShort()
        {
            IGeometryFactory gf = new GeometryFactory();

            ILineString oldRefGeometry = gf.CreateLineString(new Coordinate[] { new Coordinate(0.4, 1), new Coordinate(0.6, 1) });
            ILineString newSegmentGeometry = gf.CreateLineString(new Coordinate[] { new Coordinate(0, 0), new Coordinate(0.5, 0.5), new Coordinate(1, 0) });

            GeometryTransformer gt = new GeometryTransformer(oldRefGeometry, newSegmentGeometry);
            var result = gt.Transform();
            ILineString newRefGeometry = result.NewRefGeometry;

            Assert.IsNull(newRefGeometry);
            Assert.IsTrue(result.ResultState == GeometryTransformerResultState.FailedWouldBeTooShort);
        }
    }
}
