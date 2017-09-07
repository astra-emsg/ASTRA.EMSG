using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Reports.EineListeVonRealisiertenMassnahmenGeordnetNachJahren;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Common;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.Tests.Common.Utils;
using ASTRA.EMSG.Web.Areas.Auswertungen.Controllers;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using NUnit.Framework;

namespace ASTRA.EMSG.IntegrationTests.ReportTests
{
    [TestFixture]
    public abstract class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenBase<TPo> : ReportTestBase
    {
        protected override void Init()
        {
            base.Init();
            InsertTestData();
        }

        private void InsertTestData()
        {
            ErfassungsPeriod oldErfassungPeriod;
            using (var scope = new NHibernateTestScope())
            {
                var current = scope.Session.Get<ErfassungsPeriod>(GetClosedErfassungPeriod(scope).Id);
                oldErfassungPeriod = DbHandlerUtils.CreateErfassungsPeriod(scope.Session, current.Mandant, Erfassungmodus);
                oldErfassungPeriod.IsClosed = true;
                oldErfassungPeriod.Erfassungsjahr = new DateTime(2009, 1, 1);
            }

            using (var scope = new NHibernateTestScope())
            {
                var entiyOne = TestDataHelpers.GetRealisierteMassnahmeGIS(GetCurrentErfassungsPeriod(scope), "RMG01", "ORG1", "BeschreibungGIS1");
                scope.Session.Save(entiyOne);

                var entiyTwo = TestDataHelpers.GetRealisierteMassnahmeGIS(GetCurrentErfassungsPeriod(scope), "RMG02", "ORG2", "BeschreibungGIS2");
                scope.Session.Save(entiyTwo);

                var entiyThree = TestDataHelpers.GetRealisierteMassnahmeGIS(GetClosedErfassungPeriod(scope), "RMG03", "ORG3", "BeschreibungGIS3");
                scope.Session.Save(entiyThree);

                var entityOtherMandant = TestDataHelpers.GetRealisierteMassnahmeGIS(GetOtherErfassungPeriod(scope), "RMGO01", "ORG1", "BeschreibungGIS4");
                scope.Session.Save(entityOtherMandant);

                var oldEntity = TestDataHelpers.GetRealisierteMassnahmeGIS(oldErfassungPeriod, "RMG01", "ORG1", "BeschreibungGIS5");
                scope.Session.Save(oldEntity);

            }

            using (var scope = new NHibernateTestScope())
            {
                var entiyOne = TestDataHelpers.GetRealisierteMassnahmeSummarisch(GetCurrentErfassungsPeriod(scope), "RMS01", "BeschreibungSum1");
                scope.Session.Save(entiyOne);

                var entiyTwo = TestDataHelpers.GetRealisierteMassnahmeSummarisch(GetCurrentErfassungsPeriod(scope), "RMS02", "BeschreibungSum2");
                scope.Session.Save(entiyTwo);

                var entiyThree = TestDataHelpers.GetRealisierteMassnahmeSummarisch(GetClosedErfassungPeriod(scope), "RMS03", "BeschreibungSum3");
                scope.Session.Save(entiyThree);

                var entityOtherMandant = TestDataHelpers.GetRealisierteMassnahmeSummarisch(GetOtherErfassungPeriod(scope), "RMSO01", "BeschreibungSum4");
                scope.Session.Save(entityOtherMandant);

                var oldEntity = TestDataHelpers.GetRealisierteMassnahmeSummarisch(GetOtherErfassungPeriod(scope), "RMS01", "BeschreibungSum5");
                scope.Session.Save(oldEntity);
            }

            using (var scope = new NHibernateTestScope())
            {
                var entiyOne = TestDataHelpers.GetRealisierteMassnahme(GetCurrentErfassungsPeriod(scope), "RMT01", "BeschreibungTab1");
                scope.Session.Save(entiyOne);

                var entiyTwo = TestDataHelpers.GetRealisierteMassnahme(GetCurrentErfassungsPeriod(scope), "RMT02", "BeschreibungTab2");
                scope.Session.Save(entiyTwo);

                var entiyThree = TestDataHelpers.GetRealisierteMassnahme(GetClosedErfassungPeriod(scope), "RMT03", "BeschreibungTab3");
                scope.Session.Save(entiyThree);

                var entityOtherMandant = TestDataHelpers.GetRealisierteMassnahme(GetOtherErfassungPeriod(scope), "RMTO01", "BeschreibungTab4");
                scope.Session.Save(entityOtherMandant);

                var oldEntity = TestDataHelpers.GetRealisierteMassnahme(GetOtherErfassungPeriod(scope), "RMT01", "BeschreibungTab5");
                scope.Session.Save(oldEntity);
            }
        }

        protected void AssertEntityOne(List<TPo> pos)
        {
            AssertPoIsTheExpected(pos, "RMG01", "ORG1", "BeschreibungGIS1");
        }

        protected void AssertEntityTwo(List<TPo> pos)
        {
            AssertPoIsTheExpected(pos, "RMG02", "ORG2", "BeschreibungGIS2");
        }

        protected void AssertEntityThree(List<TPo> pos)
        {
            AssertPoIsTheExpected(pos, "RMG03", "ORG3", "BeschreibungGIS3");
        }

        protected void AssertEntityFour(List<TPo> pos)
        {
            AssertPoIsTheExpected(pos, "RMS01", null, "BeschreibungSum1");
        }

        protected void AssertEntityFive(List<TPo> pos)
        {
            AssertPoIsTheExpected(pos, "RMS02", null, "BeschreibungSum2");
        }

        protected void AssertEntitySix(List<TPo> pos)
        {
            AssertPoIsTheExpected(pos, "RMS03", null, "BeschreibungSum3");
        }

        protected void AssertEntitySeven(List<TPo> pos)
        {
            AssertPoIsTheExpected(pos, "RMT01", null, "BeschreibungTab1");
        }

        protected void AssertEntityEight(List<TPo> pos)
        {
            AssertPoIsTheExpected(pos, "RMT02", null, "BeschreibungTab2");
        }

        protected void AssertEntityNine(List<TPo> pos)
        {
            AssertPoIsTheExpected(pos, "RMT03", null, "BeschreibungTab3");
        }

        protected abstract void AssertPoIsTheExpected(IEnumerable<TPo> pos, string projektName, string leitendeOrganisation, string beschreibung);
    }

    [TestFixture]
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGIS : EineListeVonRealisiertenMassnahmenGeordnetNachJahrenBase<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPo>
    {
        protected override NetzErfassungsmodus Erfassungmodus
        {
            get { return NetzErfassungsmodus.Gis; }
        }

        [Test]
        public void TestWithNoFilter()
        {
            var filter = new EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISGridCommand
                             {
                                ErfassungsPeriodIdVon   = GetClosedErfassungPeriodId(),
                                ErfassungsPeriodIdBis = GetCurrentErfassungsPeriodId()
                             };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(5, pos.Count);
            AssertEntityOne(pos);
            AssertEntityTwo(pos);
            AssertEntityThree(pos);
            AssertEntitySix(pos);
            AssertEntityNine(pos);
        }

        [Test]
        public void TestWithProjektname()
        {
            var filter = new EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISGridCommand
            {
                ErfassungsPeriodIdVon   = GetClosedErfassungPeriodId(),
                ErfassungsPeriodIdBis = GetCurrentErfassungsPeriodId(),
                Projektname = "01"
            };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(1, pos.Count);
            AssertEntityOne(pos);
        }

        [Test]
        public void TestWithLeitendeOrganisationFilter()
        {
            var filter = new EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISGridCommand
            {
                ErfassungsPeriodIdVon = GetClosedErfassungPeriodId(),
                ErfassungsPeriodIdBis = GetCurrentErfassungsPeriodId(),
                LeitendeOrganisation = "ORG1",
            };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(1, pos.Count);
            AssertEntityOne(pos);
        }

        [Test]
        public void TestWithAllFilter()
        {
            var filter = new EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISGridCommand
            {
                ErfassungsPeriodIdVon = GetClosedErfassungPeriodId(),
                ErfassungsPeriodIdBis = GetCurrentErfassungsPeriodId(),
                Projektname = "01",
                LeitendeOrganisation = "ORG1",
            };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(1, pos.Count);
            AssertEntityOne(pos);
        }

        protected  override void AssertPoIsTheExpected(IEnumerable<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPo> pos, string projektName, string leitendeOrganisation, string beschreibung)
        {
            var po = pos.Single(p => p.Projektname == projektName && p.Beschreibung == beschreibung);

            Assert.AreEqual(projektName, po.Projektname);
            Assert.AreEqual(leitendeOrganisation, po.LeitendeOrganisation);
            Assert.AreEqual(beschreibung, po.Beschreibung);
        }

        private List<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPo> GetPosWithFilter(EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISGridCommand filter)
        {
            //Generate Report
            BrowserDriver.GeneratReports(filter, rp => BrowserDriver.InvokePostAction<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISController, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISGridCommand>((c, r) => c.GetReport(r), rp, false));

            //Assert on Po-s
            return GetPos<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISPo>();
        }
    }

    [TestFixture]
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarisch : EineListeVonRealisiertenMassnahmenGeordnetNachJahrenBase<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPo>
    {
        protected override NetzErfassungsmodus Erfassungmodus
        {
            get { return NetzErfassungsmodus.Summarisch; }
        }

        [Test]
        public void TestWithNoFilter()
        {
            var filter = new EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischGridCommand
            {
                ErfassungsPeriodIdVon = GetClosedErfassungPeriodId(),
                ErfassungsPeriodIdBis = GetCurrentErfassungsPeriodId()
            };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(5, pos.Count);
            
            AssertEntityThree(pos);
            
            AssertEntityFour(pos);
            AssertEntityFive(pos);
            AssertEntitySix(pos);
            
            AssertEntityNine(pos);
        }

        [Test]
        public void TestWithProjektname()
        {
            var filter = new EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischGridCommand
            {
                ErfassungsPeriodIdVon = GetClosedErfassungPeriodId(),
                ErfassungsPeriodIdBis = GetCurrentErfassungsPeriodId(),
                Projektname = "01"
            };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(1, pos.Count);
            AssertEntityFour(pos);
        }

        protected override void AssertPoIsTheExpected(IEnumerable<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPo> pos, string projektName, string leitendeOrganisation, string beschreibung)
        {
            var po = pos.Single(p => p.Projektname == projektName && p.Beschreibung == beschreibung);

            Assert.AreEqual(projektName, po.Projektname);
            Assert.AreEqual(beschreibung, po.Beschreibung);
        }
        
        private List<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPo> GetPosWithFilter(EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischGridCommand filter)
        {
            //Generate Report
            BrowserDriver.GeneratReports(filter, rp => BrowserDriver.InvokePostAction<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischController, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischGridCommand>((c, r) => c.GetReport(r), rp, false));

            //Assert on Po-s
            return GetPos<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischPo>();
        }
    }

    [TestFixture]
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahren : EineListeVonRealisiertenMassnahmenGeordnetNachJahrenBase<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPo>
    {
        protected override NetzErfassungsmodus Erfassungmodus
        {
            get { return NetzErfassungsmodus.Tabellarisch; }
        }

        [Test]
        public void TestWithNoFilter()
        {
            var filter = new EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGridCommand
            {
                ErfassungsPeriodIdVon = GetClosedErfassungPeriodId(),
                ErfassungsPeriodIdBis = GetCurrentErfassungsPeriodId()
            };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(5, pos.Count);
            
            AssertEntityThree(pos);
            
            AssertEntitySix(pos);
            
            AssertEntitySeven(pos);
            AssertEntityEight(pos);
            AssertEntityNine(pos);
        }

        [Test]
        public void TestWithProjektname()
        {
            var filter = new EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGridCommand
            {
                ErfassungsPeriodIdVon = GetClosedErfassungPeriodId(),
                ErfassungsPeriodIdBis = GetCurrentErfassungsPeriodId(),
                Projektname = "01"
            };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(1, pos.Count);
            AssertEntitySeven(pos);
        }

        protected override void AssertPoIsTheExpected(IEnumerable<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPo> pos, string projektName, string leitendeOrganisation, string beschreibung)
        {
            var po = pos.Single(p => p.Projektname == projektName && p.Beschreibung == beschreibung);

            Assert.AreEqual(projektName, po.Projektname);
            Assert.AreEqual(beschreibung, po.Beschreibung);
        }

        private List<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPo> GetPosWithFilter(EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGridCommand filter)
        {
            //Generate Report
            BrowserDriver.GeneratReports(filter, rp => BrowserDriver.InvokePostAction<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenController, EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGridCommand>((c, r) => c.GetReport(r), rp, false));

            //Assert on Po-s
            return GetPos<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenPo>();
        }
    }
}
