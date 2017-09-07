using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Reports.StrassenabschnitteListeOhneInspektionsroute;
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
    public class StrassenabschnitteListeOhneInspektionsrouteTest : ReportTestBase
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

        [Test]
        public void TestWithNoFilter()
        {
            var strassenabschnitteListeOhneInspektionsrouteGridCommand = new StrassenabschnitteListeOhneInspektionsrouteGridCommand();

            var pos = GetStrassenabschnitteListeOhneInspektionsroutePos(strassenabschnitteListeOhneInspektionsrouteGridCommand);

            Assert.AreEqual(2, pos.Count);
            AssertPoIsTheExpected(pos, "SA01", EigentuemerTyp.Gemeinde);
            AssertPoIsTheExpected(pos, "SA02", EigentuemerTyp.Kanton);
        }

        [Test]
        public void TestWithErfassungPeriodFilter()
        {
            var filter = new StrassenabschnitteListeOhneInspektionsrouteGridCommand() { ErfassungsPeriodId = GetClosedErfassungPeriodId() };

            var pos = GetStrassenabschnitteListeOhneInspektionsroutePos(filter);

            Assert.AreEqual(1, pos.Count);
            AssertPoIsTheExpected(pos, "SA03", EigentuemerTyp.Korporation);
        }


        [Test]
        public void TestWithStrassennameFilter()
        {
            var strassenabschnitteListeOhneInspektionsrouteGridCommand = new StrassenabschnitteListeOhneInspektionsrouteGridCommand { Strassenname = "SA01" };

            var pos = GetStrassenabschnitteListeOhneInspektionsroutePos(strassenabschnitteListeOhneInspektionsrouteGridCommand);

            Assert.AreEqual(1, pos.Count);
            AssertPoIsTheExpected(pos, "SA01", EigentuemerTyp.Gemeinde);
        }

        [Test]
        public void TestWithEigentuemerFilter()
        {
            var strassenabschnitteListeOhneInspektionsrouteGridCommand = new StrassenabschnitteListeOhneInspektionsrouteGridCommand { Eigentuemer = EigentuemerTyp.Gemeinde };

            var pos = GetStrassenabschnitteListeOhneInspektionsroutePos(strassenabschnitteListeOhneInspektionsrouteGridCommand);

            Assert.AreEqual(1, pos.Count);
            AssertPoIsTheExpected(pos, "SA01", EigentuemerTyp.Gemeinde);
        }

        [Test]
        public void TestWithAllFilter()
        {
            var strassenabschnitteListeOhneInspektionsrouteGridCommand = new StrassenabschnitteListeOhneInspektionsrouteGridCommand
            {
                Strassenname = "SA02",
                Eigentuemer = EigentuemerTyp.Kanton
            };

            var pos = GetStrassenabschnitteListeOhneInspektionsroutePos(strassenabschnitteListeOhneInspektionsrouteGridCommand);

            Assert.AreEqual(1, pos.Count);
            AssertPoIsTheExpected(pos, "SA02", EigentuemerTyp.Kanton);
        }

        private void AssertPoIsTheExpected(IEnumerable<StrassenabschnitteListeOhneInspektionsroutePo> pos, string strassenabschnittName, EigentuemerTyp eigentuemerTyp)
        {
            var po = pos.Single(p => p.Strassenname == strassenabschnittName);

            Assert.AreEqual(strassenabschnittName, po.Strassenname);
            Assert.AreEqual("IA", po.BelastungskategorieTyp);
            Assert.AreEqual(LocalizationService.GetLocalizedBelastungskategorieTyp(po.BelastungskategorieTyp), po.BelastungskategorieBezeichnung);
            Assert.AreEqual(strassenabschnittName.GetStrassennameBezeichnungVon(), po.BezeichnungVon);
            Assert.AreEqual(strassenabschnittName.GetStrassennameBezeichnungBis(), po.BezeichnungBis);
            Assert.AreEqual(eigentuemerTyp, po.Strasseneigentuemer);
            Assert.AreEqual(LocalizationService.GetLocalizedEnum(po.Strasseneigentuemer), po.StrasseneigentuemerBezeichnung);
            Assert.AreEqual(2000, po.FlaecheFahrbahn);
            Assert.AreEqual(500, po.FlaecheTrottoirLinks);
            Assert.AreEqual(1000, po.FlaecheTrottoirRechts);
        }

        private List<StrassenabschnitteListeOhneInspektionsroutePo> GetStrassenabschnitteListeOhneInspektionsroutePos(StrassenabschnitteListeOhneInspektionsrouteGridCommand strassenabschnitteListeOhneInspektionsrouteGridCommand)
        {
            //Generate Report
            BrowserDriver.GeneratReports(strassenabschnitteListeOhneInspektionsrouteGridCommand, rp => BrowserDriver.InvokePostAction<StrassenabschnitteListeOhneInspektionsrouteController, StrassenabschnitteListeOhneInspektionsrouteGridCommand>((c, r) => c.GetReport(r), rp, false));

            //Assert on Po-s
            return GetPos<StrassenabschnitteListeOhneInspektionsroutePo>();
        }

        private void InsertTestData()
        {
            using (var scope = new NHibernateTestScope())
            {
                var belastungskategorie = TestDataHelpers.GetBelastungskategorie(scope, "IA");

                var entityThree = TestDataHelpers.GetStrassenabschnittGIS(GetCurrentErfassungsPeriod(scope), "SA01", belastungskategorie, EigentuemerTyp.Gemeinde);
                scope.Session.Save(entityThree);

                var entityTwo = TestDataHelpers.GetStrassenabschnittGIS(GetCurrentErfassungsPeriod(scope), "SA02", belastungskategorie, EigentuemerTyp.Kanton);
                scope.Session.Save(entityTwo);

                var entiyThree = TestDataHelpers.GetStrassenabschnittGIS(GetClosedErfassungPeriod(scope), "SA03", belastungskategorie, EigentuemerTyp.Korporation);
                scope.Session.Save(entiyThree);

                var entityOtherMandant = TestDataHelpers.GetStrassenabschnittGIS(GetOtherErfassungPeriod(scope), "SA01", belastungskategorie, EigentuemerTyp.Gemeinde);
                scope.Session.Save(entityOtherMandant);
            }
        }
    }
}
