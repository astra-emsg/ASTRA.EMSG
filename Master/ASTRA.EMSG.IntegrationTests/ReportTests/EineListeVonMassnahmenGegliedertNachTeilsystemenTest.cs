using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Reports.EineListeVonMassnahmenGegliedertNachTeilsystemen;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Common;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.Web.Areas.Auswertungen.Controllers;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using NHibernate.Criterion;
using NHibernate.Linq;
using NUnit.Framework;

namespace ASTRA.EMSG.IntegrationTests.ReportTests
{
    [TestFixture]
    public class EineListeVonMassnahmenGegliedertNachTeilsystemenTest : ReportTestBase
    {
        protected override void Init()
        {
            base.Init();
            InsertTestData();
        }

        protected override NetzErfassungsmodus Erfassungmodus
        {
            get { return NetzErfassungsmodus.Gis; }
        }

        private void InsertTestData()
        {
            using (var scope = new NHibernateTestScope())
            {
                var entityOne = TestDataHelpers.GetMassnahmenvorschlagTeilsystemeGIS(GetCurrentErfassungsPeriod(scope), "MT01", StatusTyp.Abgeschlossen, DringlichkeitTyp.Mittelfristig, TeilsystemTyp.Strassen);
                scope.Session.Save(entityOne);

                var entiyTwo = TestDataHelpers.GetMassnahmenvorschlagTeilsystemeGIS(GetCurrentErfassungsPeriod(scope), "MT02", StatusTyp.Vorgeschlagen, DringlichkeitTyp.Dringlich, TeilsystemTyp.Beleuchtungsanlagen);
                scope.Session.Save(entiyTwo);

                var entiyThree = TestDataHelpers.GetMassnahmenvorschlagTeilsystemeGIS(GetClosedErfassungPeriod(scope), "MT03", StatusTyp.InKoordination, DringlichkeitTyp.Langfristig, TeilsystemTyp.Gruenanlagen);
                scope.Session.Save(entiyThree);

                var entityOtherMandant = TestDataHelpers.GetMassnahmenvorschlagTeilsystemeGIS(GetOtherErfassungPeriod(scope), "MT01", StatusTyp.Abgeschlossen, DringlichkeitTyp.Mittelfristig, TeilsystemTyp.Strassen);
                scope.Session.Save(entityOtherMandant);
            }
        }

        private void AssertEntityOne(List<EineListeVonMassnahmenGegliedertNachTeilsystemenPo> pos)
        {
            AssertPoIsTheExpected(pos, "MT01", StatusTyp.Abgeschlossen, DringlichkeitTyp.Mittelfristig, TeilsystemTyp.Strassen);
        }

        private void AssertEntityTwo(List<EineListeVonMassnahmenGegliedertNachTeilsystemenPo> pos)
        {
            AssertPoIsTheExpected(pos, "MT02", StatusTyp.Vorgeschlagen, DringlichkeitTyp.Dringlich, TeilsystemTyp.Beleuchtungsanlagen);
        }

        private void AssertEntityThree(List<EineListeVonMassnahmenGegliedertNachTeilsystemenPo> pos)
        {
            AssertPoIsTheExpected(pos, "MT03", StatusTyp.InKoordination, DringlichkeitTyp.Langfristig, TeilsystemTyp.Gruenanlagen);
        }

        [Test]
        public void TestWithNoFilter()
        {
            var filter = new EineListeVonMassnahmenGegliedertNachTeilsystemenGridCommand();

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(3, pos.Count);
            AssertEntityOne(pos);
            AssertEntityTwo(pos);
            AssertEntityThree(pos);
        }

        [Test]
        public void TestWithProjektname()
        {
            var filter = new EineListeVonMassnahmenGegliedertNachTeilsystemenGridCommand
                             {
                                 Projektname = "MT01"
                             };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(1, pos.Count);
            AssertEntityOne(pos);
        }

        [Test]
        public void TestWithStatus()
        {
            var filter = new EineListeVonMassnahmenGegliedertNachTeilsystemenGridCommand
                             {
                                 Status = (int?)StatusTyp.Abgeschlossen
                             };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(1, pos.Count);
            AssertEntityOne(pos);
        }

        [Test]
        public void TestWithDringlichkeit()
        {
            var filter = new EineListeVonMassnahmenGegliedertNachTeilsystemenGridCommand
                             {
                                 Dringlichkeit = (int?)DringlichkeitTyp.Dringlich
                             };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(1, pos.Count);
            AssertEntityTwo(pos);
        }

        [Test]
        public void TestWithTeilsystem()
        {
            var filter = new EineListeVonMassnahmenGegliedertNachTeilsystemenGridCommand
                             {
                                 Teilsystem = (int?)TeilsystemTyp.Strassen
                             };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(1, pos.Count);
            AssertEntityOne(pos);
        }

        [Test]
        public void TestWithAllFilter()
        {
            var filter = new EineListeVonMassnahmenGegliedertNachTeilsystemenGridCommand
                             {
                                 Projektname = "MT01",
                                 Status = (int?)StatusTyp.Abgeschlossen,
                                 Teilsystem = (int?)TeilsystemTyp.Strassen,
                                 Dringlichkeit = (int?)DringlichkeitTyp.Mittelfristig,
                             };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(1, pos.Count);
            AssertEntityOne(pos);
        }

        private void AssertPoIsTheExpected(IEnumerable<EineListeVonMassnahmenGegliedertNachTeilsystemenPo> pos, string projektName, StatusTyp statusTyp, DringlichkeitTyp dringlichkeit, TeilsystemTyp teilsystemTyp)
        {
            var po = pos.Single(p => p.Projektname == projektName);

            Assert.AreEqual(projektName, po.Projektname);
            Assert.AreEqual(statusTyp, po.Status);
            Assert.AreEqual(dringlichkeit, po.Dringlichkeit);
            Assert.AreEqual(teilsystemTyp, po.Teilsystem);
            //TODO: Extend asserts
            //Assert.AreEqual(ausfuehrungsEnde, po.BezeichnungVon);
            //Assert.AreEqual(ausfuehrungsEnde, po.BezeichnungBis);
            //Assert.AreEqual(ausfuehrungsEnde, po.Laenge);
            //Assert.AreEqual(ausfuehrungsEnde, po.FlaecheFahrbahn);
            //Assert.AreEqual(ausfuehrungsEnde, po.FlaecheTrottoirLinks);
            //Assert.AreEqual(ausfuehrungsEnde, po.FlaecheTrottoirRechts);
            //Assert.AreEqual(ausfuehrungsEnde, po.BeteiligteSystemeListe);
            //Assert.AreEqual(ausfuehrungsEnde, po.KostenGesamtprojekt);
            //Assert.AreEqual(ausfuehrungsEnde, po.KostenStrasse);
            //Assert.AreEqual(ausfuehrungsEnde, po.Beschreibung);
            //Assert.AreEqual(ausfuehrungsEnde, po.LeitendeOrganisation);
        }

        private List<EineListeVonMassnahmenGegliedertNachTeilsystemenPo> GetPosWithFilter(EineListeVonMassnahmenGegliedertNachTeilsystemenGridCommand filter)
        {
            //Generate Report
            BrowserDriver.GeneratReports(filter, rp => BrowserDriver.InvokePostAction<EineListeVonMassnahmenGegliedertNachTeilsystemenController, EineListeVonMassnahmenGegliedertNachTeilsystemenGridCommand>((c, r) => c.GetReport(r), rp, false));

            //Assert on Po-s
            return GetPos<EineListeVonMassnahmenGegliedertNachTeilsystemenPo>();
        }


    }
}
