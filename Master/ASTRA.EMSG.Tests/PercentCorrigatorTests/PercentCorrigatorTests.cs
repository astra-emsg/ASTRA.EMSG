using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Reporting;
using NUnit.Framework;
using System.Linq;

namespace ASTRA.EMSG.Tests.PercentCorrigatorTests
{
    [TestFixture]
    public class PercentCorrigatorTests
    {
        [Test]
        public void TestSimpleCase()
        {
            var percentList = new List<IPercentHolder>
                {
                    new Percent(decimal.Divide(100, 3), 1),
                    new Percent(decimal.Divide(100, 3), 2),
                    new Percent(decimal.Divide(100, 3), 3),
                };

            PercentPartitioningCorrector.Corrigate(percentList);

            Assert.AreEqual(percentList.Sum(p => Math.Round(p.DecimalValue.Value)), 100);
            Assert.AreEqual(34, (int)percentList.Single(p => p.SortOrder == 2).DecimalValue.Value);
        }

        [Test]
        public void TestHasNullCase()
        {
            var percentList = new List<IPercentHolder>
                {
                    new Percent(0, 1),
                    new Percent(decimal.Divide(100, 3), 2),
                    new Percent((decimal)0.2, 3),
                    new Percent(decimal.Divide(100, 3), 4),
                    new Percent(0, 5),
                    new Percent(decimal.Divide(100, 3) - (decimal)0.2, 6),
                    new Percent(0, 7)
                };

            PercentPartitioningCorrector.Corrigate(percentList);

            Assert.AreEqual(percentList.Sum(p => Math.Round(p.DecimalValue.Value)), 100);
            Assert.AreEqual(1, (int)percentList.Single(p => p.SortOrder == 3).DecimalValue.Value);
            Assert.AreEqual(0, (int)percentList.Single(p => p.SortOrder == 5).DecimalValue.Value);
            Assert.AreEqual(33, (int)percentList.Single(p => p.SortOrder == 2).DecimalValue.Value);
        }
        
        [Test]
        public void TestWithNegativeCase()
        {
            var percentList = new List<IPercentHolder>
                {
                    new Percent(0, 1),
                    new Percent(decimal.Divide(200, 3), 2),
                    new Percent((decimal)0.2, 3),
                    new Percent((decimal)4.4, 4),
                    new Percent((decimal)0.4, 5),
                    new Percent(decimal.Divide(100, 3) - (decimal)5, 6),
                    new Percent(0, 7)
                };

            PercentPartitioningCorrector.Corrigate(percentList);

            Assert.AreEqual(percentList.Sum(p => Math.Round(p.DecimalValue.Value)), 100);
            Assert.AreEqual(67, (int)percentList.Single(p => p.SortOrder == 2).DecimalValue.Value);
            Assert.AreEqual(0, (int)percentList.Single(p => p.SortOrder == 3).DecimalValue.Value);
            Assert.AreEqual(4, (int)percentList.Single(p => p.SortOrder == 4).DecimalValue.Value);
            Assert.AreEqual(1, (int)percentList.Single(p => p.SortOrder == 5).DecimalValue.Value);
            Assert.AreEqual(28, (int)percentList.Single(p => p.SortOrder == 6).DecimalValue.Value);
        }
        
        [Test]
        public void TestWithSmallAccumulationCase()
        {
            var percentList = new List<IPercentHolder>
                {
                    new Percent(decimal.Divide(100, 9), 1),
                    new Percent(decimal.Divide(100, 3), 2),
                    new Percent(decimal.Divide(100, 9), 3),
                    new Percent(decimal.Divide(100, 3), 4),
                    new Percent(decimal.Divide(100, 9), 5),
                };

            PercentPartitioningCorrector.Corrigate(percentList);

            Assert.AreEqual(percentList.Sum(p => Math.Round(p.DecimalValue.Value)), 100);
            Assert.AreEqual(11, (int)percentList.Single(p => p.SortOrder == 1).DecimalValue.Value);
            Assert.AreEqual(33, (int)percentList.Single(p => p.SortOrder == 2).DecimalValue.Value);
            Assert.AreEqual(12, (int)percentList.Single(p => p.SortOrder == 3).DecimalValue.Value);
            Assert.AreEqual(33, (int)percentList.Single(p => p.SortOrder == 4).DecimalValue.Value);
            Assert.AreEqual(11, (int)percentList.Single(p => p.SortOrder == 5).DecimalValue.Value);
        }

        [Test]
        public void TestWithUnluckyBehaviorCase()
        {
            var percentList = new List<IPercentHolder>
                {
                    new Percent((decimal) 0.51, 1),
                    new Percent((decimal) 4.99, 2),
                    new Percent((decimal) 94.02, 3),
                    new Percent((decimal) 0.48, 4),
                };

            PercentPartitioningCorrector.Corrigate(percentList);

            Assert.AreEqual(percentList.Sum(p => Math.Round(p.DecimalValue.Value)), 100);
            Assert.AreEqual(1, (int) percentList.Single(p => p.SortOrder == 1).DecimalValue.Value);
            Assert.AreEqual(4, (int) percentList.Single(p => p.SortOrder == 2).DecimalValue.Value);
            Assert.AreEqual(95, (int) percentList.Single(p => p.SortOrder == 3).DecimalValue.Value);
        }

        [Test]
        public void TestWithGroupsCase()
        {
            var percentList = new List<IPercentHolder>
                {
                    new Percent(decimal.Divide(100, 3), 1, "group1"),
                    new Percent(decimal.Divide(200, 3), 1, "group2"),
                    new Percent(decimal.Divide(100, 3), 1, "group3"),
                    new Percent(decimal.Divide(100, 3), 2, "group1"),
                    new Percent(0, 2, "group2"),
                    new Percent(decimal.Divide(100, 6), 2, "group3"),
                    new Percent(decimal.Divide(100, 3), 3, "group1"),
                    new Percent(decimal.Divide(100, 3), 3, "group2"),
                    new Percent(decimal.Divide(100, 9), 3, "group3"),
                    new Percent(decimal.Divide(150, 9), 4, "group3"),
                    new Percent(decimal.Divide(200, 9), 5, "group3"),
                };

            PercentPartitioningCorrector.Corrigate(percentList);

            Assert.IsTrue(percentList.GroupBy(item => item.Group).Select(items => items.Sum(p => Math.Round(p.DecimalValue.Value))).All(val => val == (decimal)100));
            Assert.AreEqual(33, (int)percentList.Single(p => p.SortOrder == 1 && p.Group=="group1").DecimalValue.Value);
            Assert.AreEqual(34, (int)percentList.Single(p => p.SortOrder == 2 && p.Group=="group1").DecimalValue.Value);
            Assert.AreEqual(67, (int)percentList.Single(p => p.SortOrder == 1 && p.Group == "group2").DecimalValue.Value);
            Assert.AreEqual(0, (int)percentList.Single(p => p.SortOrder == 2 && p.Group == "group2").DecimalValue.Value);
            Assert.AreEqual(17, (int)percentList.Single(p => p.SortOrder == 4 && p.Group == "group3").DecimalValue.Value);
        }

    }
}
