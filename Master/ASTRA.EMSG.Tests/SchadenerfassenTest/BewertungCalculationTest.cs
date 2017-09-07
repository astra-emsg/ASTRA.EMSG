using System.Collections.Generic;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Common.Enums;
using NUnit.Framework;

namespace ASTRA.EMSG.Tests.SchadenerfassenTest
{
    [TestFixture]
    public class BewertungCalculationTest
    {
        [Test]
        public void BewertungCalculationWithKeineSchaden()
        {
            var schadengruppeModel = new SchadengruppeModel
                                         {
                                             SchadendetailModelList = new List<SchadendetailModel>
                                                                          {
                                                                              new SchadendetailModel
                                                                                  {
                                                                                      SchadenausmassTyp = SchadenausmassTyp.A0,
                                                                                      SchadenschwereTyp = SchadenschwereTyp.S1
                                                                                  },
                                                                          },
                                             Gewicht = 1
                                         };

            Assert.AreEqual(0, schadengruppeModel.Bewertung);
        }

        [Test]
        public void BewertungCalculationWithKleneSchadenAndMoreThenFiftypercenAusmass()
        {
            var schadengruppeModel = new SchadengruppeModel
                                         {
                                             SchadendetailModelList = new List<SchadendetailModel>
                                                                          {
                                                                              new SchadendetailModel
                                                                                  {
                                                                                      SchadenausmassTyp = SchadenausmassTyp.A3,
                                                                                      SchadenschwereTyp = SchadenschwereTyp.S1
                                                                                  },
                                                                          },
                                             Gewicht = 1
                                         };

            Assert.AreEqual(3, schadengruppeModel.Bewertung);
        }
    }
}