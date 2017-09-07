using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Common.Enums;
using NUnit.Framework;

namespace ASTRA.EMSG.Tests.SchadenerfassenTest
{
    [TestFixture]
    public class MatrixCalculationTest
    {
        [Test]
        public void MatrixCalculationWithKeineSchadenLessThenTenPercentAusmass()
        {
            var schadendetailModel = new SchadendetailModel
                                         {
                                             SchadenausmassTyp = SchadenausmassTyp.A0,
                                             SchadenschwereTyp = SchadenschwereTyp.S1
                                         };

            Assert.AreEqual(0, schadendetailModel.Matrix);
        }

        [Test]
        public void MatrixCalculationWithKeineSchadenFiftyPercenAusmass()
        {
            var schadendetailModel = new SchadendetailModel
                                         {
                                             SchadenausmassTyp = SchadenausmassTyp.A0,
                                             SchadenschwereTyp = SchadenschwereTyp.S2
                                         };

            Assert.AreEqual(0, schadendetailModel.Matrix);
        }

        [Test]
        public void MatrixCalculationWithKleineSchadenTenPercentAusmass()
        {
            var schadendetailModel = new SchadendetailModel
                                         {
                                             SchadenausmassTyp = SchadenausmassTyp.A1,
                                             SchadenschwereTyp = SchadenschwereTyp.S1
                                         };

            Assert.AreEqual(1, schadendetailModel.Matrix);
        }

        [Test]
        public void MatrixCalculationWithGroessteSchadenMorThenFiftyPercentAusmass()
        {
            var schadendetailModel = new SchadendetailModel
                                         {
                                             SchadenausmassTyp = SchadenausmassTyp.A3,
                                             SchadenschwereTyp = SchadenschwereTyp.S3
                                         };

            Assert.AreEqual(9, schadendetailModel.Matrix);
        }
    }
}