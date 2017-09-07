using System.Collections.Generic;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Common.Enums;
using NUnit.Framework;

namespace ASTRA.EMSG.Tests.SchadenerfassenTest
{
    [TestFixture]
    public class ZustandsindexCalculationTest
    {
        [Test]
        public void ZustandsindexCalculationInManuelMode()
        {
            ZustandsabschnittdetailsModel zustandsabschnittdetailsModel = new ZustandsabschnittdetailsModel();
            zustandsabschnittdetailsModel.Erfassungsmodus = ZustandsErfassungsmodus.Manuel;
            zustandsabschnittdetailsModel.Zustandsindex = 3.5m;

            Assert.AreEqual(3.5m, zustandsabschnittdetailsModel.ZustandsindexCalculated);
        }

        [Test]
        public void ZustandsindexCalculationInGrobModeWithResultLessThenFive()
        {
            ZustandsabschnittdetailsModel zustandsabschnittdetailsModel = new ZustandsabschnittdetailsModel();
            zustandsabschnittdetailsModel.Erfassungsmodus = ZustandsErfassungsmodus.Grob;
            zustandsabschnittdetailsModel.SchadengruppeModelList.Add(new SchadengruppeModel { SchadenausmassTyp = SchadenausmassTyp.A1, SchadenschwereTyp = SchadenschwereTyp.S2, Gewicht = 20 });

            Assert.AreEqual(4m, zustandsabschnittdetailsModel.ZustandsindexCalculated);
        }

        [Test]
        public void ZustandsindexCalculationInGrobModeWithResultMoreThenFive()
        {
            ZustandsabschnittdetailsModel zustandsabschnittdetailsModel = new ZustandsabschnittdetailsModel();
            zustandsabschnittdetailsModel.Erfassungsmodus = ZustandsErfassungsmodus.Grob;
            zustandsabschnittdetailsModel.SchadengruppeModelList.Add(new SchadengruppeModel { SchadenausmassTyp = SchadenausmassTyp.A2, SchadenschwereTyp = SchadenschwereTyp.S2, Gewicht = 20 });

            Assert.AreEqual(5m, zustandsabschnittdetailsModel.ZustandsindexCalculated);
        }

        [Test]
        public void ZustandsindexCalculationInDetailedModeWithResultLessThenFive()
        {
            ZustandsabschnittdetailsModel zustandsabschnittdetailsModel = new ZustandsabschnittdetailsModel();
            zustandsabschnittdetailsModel.Erfassungsmodus = ZustandsErfassungsmodus.Detail;
            zustandsabschnittdetailsModel.SchadengruppeModelList.Add(new SchadengruppeModel
            {
                SchadendetailModelList = new List<SchadendetailModel>
                                             {
                                                 new SchadendetailModel{SchadenausmassTyp = SchadenausmassTyp.A3, SchadenschwereTyp = SchadenschwereTyp.S3}
                                             },
                                             Gewicht = 1
            });

            Assert.AreEqual(0.9m, zustandsabschnittdetailsModel.ZustandsindexCalculated);
        }

        [Test]
        public void ZustandsindexCalculationInDetailedModeWithResultMoreThenFive()
        {
            ZustandsabschnittdetailsModel zustandsabschnittdetailsModel = new ZustandsabschnittdetailsModel();
            zustandsabschnittdetailsModel.Erfassungsmodus = ZustandsErfassungsmodus.Detail;
            AddOneSchadenGruppe(zustandsabschnittdetailsModel, SchadenausmassTyp.A3, SchadenschwereTyp.S3, 2);
            AddOneSchadenGruppe(zustandsabschnittdetailsModel, SchadenausmassTyp.A3, SchadenschwereTyp.S3, 2);
            AddOneSchadenGruppe(zustandsabschnittdetailsModel, SchadenausmassTyp.A3, SchadenschwereTyp.S3, 2);

            Assert.AreEqual(5m, zustandsabschnittdetailsModel.ZustandsindexCalculated);
        }

        private static void AddOneSchadenGruppe(ZustandsabschnittdetailsModel zustandsabschnittdetailsModel, SchadenausmassTyp schadenausmassTyp, SchadenschwereTyp schadenschwereTyp, int gewicht)
        {
            zustandsabschnittdetailsModel.SchadengruppeModelList.Add(new SchadengruppeModel
                                                                         {
                                                                             SchadendetailModelList = new List<SchadendetailModel>
                                                                                     {
                                                                                         new SchadendetailModel
                                                                                             {
                                                                                                 SchadenausmassTyp = schadenausmassTyp,
                                                                                                 SchadenschwereTyp = schadenschwereTyp
                                                                                             },
                                                                                     },
                                                                             Gewicht = gewicht
                                                                         });
        }
    }
}