using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Reports.EineListeVonKoordiniertenMassnahmen;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Common;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.Web.Areas.Auswertungen.Controllers;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using NHibernate.Linq;
using NUnit.Framework;

namespace ASTRA.EMSG.IntegrationTests.ReportTests
{

    [TestFixture]
    public class EineListeVonKoordiniertenMassnahmenTest : ReportTestBase
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
                var entiyOne = TestDataHelpers.GetKoordinierteMassnahmeGIS(GetCurrentErfassungsPeriod(scope), "KM01", StatusTyp.Abgeschlossen, new DateTime(2012, 06, 04));
                scope.Session.Save(entiyOne);

                var entiyTwo = TestDataHelpers.GetKoordinierteMassnahmeGIS(GetCurrentErfassungsPeriod(scope), "KM02", StatusTyp.Vorgeschlagen, new DateTime(2012, 06, 8));
                scope.Session.Save(entiyTwo);

                var entiyThree = TestDataHelpers.GetKoordinierteMassnahmeGIS(GetClosedErfassungPeriod(scope), "KM03", StatusTyp.InKoordination, null);
                scope.Session.Save(entiyThree);

                var entityOtherMandant = TestDataHelpers.GetKoordinierteMassnahmeGIS(GetOtherErfassungPeriod(scope), "KM01", StatusTyp.Abgeschlossen, new DateTime(2012, 06, 04));
                scope.Session.Save(entityOtherMandant);
                
            }
        }

        private void AssertEntityOne(List<EineListeVonKoordiniertenMassnahmenPo> pos)
        {
            AssertPoIsTheExpected(pos, "KM01", StatusTyp.Abgeschlossen, new DateTime(2012, 06, 04));
        }

        private void AssertEntityTwo(List<EineListeVonKoordiniertenMassnahmenPo> pos)
        {
            AssertPoIsTheExpected(pos, "KM02", StatusTyp.Vorgeschlagen, new DateTime(2012, 06, 8));
        }

        private void AssertEntityThree(List<EineListeVonKoordiniertenMassnahmenPo> pos)
        {
            AssertPoIsTheExpected(pos, "KM03", StatusTyp.InKoordination, null);
        }

        [Test]
        public void TestWithNoFilter()
        {
            var filter = new EineListeVonKoordiniertenMassnahmenGridCommand();

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(3, pos.Count);
            AssertEntityOne(pos);
            AssertEntityTwo(pos);
            AssertEntityThree(pos);
        }   

        [Test]
        public void TestWithProjektname()
        {
            var filter = new EineListeVonKoordiniertenMassnahmenGridCommand
                             {
                                 Projektname = "KM01"
                             };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(1, pos.Count);
            AssertEntityOne(pos);
        }

        [Test]
        public void TestWithStatus()
        {
            var filter = new EineListeVonKoordiniertenMassnahmenGridCommand
                             {
                                 Status = (int?) StatusTyp.Abgeschlossen
                             };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(1, pos.Count);
            AssertEntityOne(pos);
        }

        [Test]
        public void TestWithAusfuehrungsanfangVon()
        {
            var filter = new EineListeVonKoordiniertenMassnahmenGridCommand
                             {
                                 AusfuehrungsanfangVon = new DateTime(2012, 06, 5)
                             };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(1, pos.Count);
            AssertEntityTwo(pos);
        }

        [Test]
        public void TestWithAusfuehrungsanfangBis()
        {
            var filter = new EineListeVonKoordiniertenMassnahmenGridCommand
                             {
                                 AusfuehrungsanfangBis = new DateTime(2012, 06, 5)
                             };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(1, pos.Count);
            AssertEntityOne(pos);
        }

        [Test]
        public void TestWithAllFilter()
        {
            var filter = new EineListeVonKoordiniertenMassnahmenGridCommand
                             {
                                 Projektname = "KM01",
                                 Status = (int?) StatusTyp.Abgeschlossen,
                                 AusfuehrungsanfangVon = new DateTime(2012, 06, 1),
                                 AusfuehrungsanfangBis = new DateTime(2012, 06, 17)
                             };

            var pos = GetPosWithFilter(filter);

            Assert.AreEqual(1, pos.Count);
            AssertEntityOne(pos);
        }

        private void AssertPoIsTheExpected(IEnumerable<EineListeVonKoordiniertenMassnahmenPo> pos, string projektName, StatusTyp statusTyp,DateTime? ausfuehrungsAnfang)
        {
            var po = pos.Single(p => p.Projektname == projektName);

            Assert.AreEqual(projektName, po.Projektname);
            Assert.AreEqual(statusTyp, po.Status);
            Assert.AreEqual(ausfuehrungsAnfang, po.AusfuehrungsAnfang);
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

        private List<EineListeVonKoordiniertenMassnahmenPo> GetPosWithFilter(EineListeVonKoordiniertenMassnahmenGridCommand filter)
        {
            //Generate Report
            BrowserDriver.GeneratReports(filter, rp => BrowserDriver.InvokePostAction<EineListeVonKoordiniertenMassnahmenController, EineListeVonKoordiniertenMassnahmenGridCommand>((c, r) => c.GetReport(r), rp, false));

            //Assert on Po-s
            return GetPos<EineListeVonKoordiniertenMassnahmenPo>();
        }
    }
}
