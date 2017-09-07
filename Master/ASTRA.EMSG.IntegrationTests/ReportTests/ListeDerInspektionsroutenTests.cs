using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Reports.ListeDerInspektionsrouten;
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
    public class ListeDerInspektionsroutenTests : ReportTestBase
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
            var listeDerInspektionsroutenGridCommand = new ListeDerInspektionsroutenGridCommand();

            var pos = GetListeDerInspektionsroutenPos(listeDerInspektionsroutenGridCommand);

            Assert.AreEqual(2, pos.Count);
            AssertPoIsTheExpected(pos, "IR01", "SA01", EigentuemerTyp.Gemeinde, null);
            AssertPoIsTheExpected(pos, "IR02", "SA02", EigentuemerTyp.Kanton, new DateTime(2010, 10, 10));
        }

        [Test]
        public void TestWithErfassungPeriodFilter()
        {
            var filter = new ListeDerInspektionsroutenGridCommand() { ErfassungsPeriodId = GetClosedErfassungPeriodId() };

            var pos = GetListeDerInspektionsroutenPos(filter);

            Assert.AreEqual(1, pos.Count);
            AssertPoIsTheExpected(pos, "IR03", "SA03", EigentuemerTyp.Korporation, null);
        }

        [Test]
        public void TestWithStrassenameFilter()
        {
            var listeDerInspektionsroutenGridCommand = new ListeDerInspektionsroutenGridCommand { Strassenname = "SA01" };

            var pos = GetListeDerInspektionsroutenPos(listeDerInspektionsroutenGridCommand);

            Assert.AreEqual(1, pos.Count);
            AssertPoIsTheExpected(pos, "IR01", "SA01", EigentuemerTyp.Gemeinde, null);
        }
        
        [Test]
        public void TestWithInspektionsroutenameFilter()
        {
            var listeDerInspektionsroutenGridCommand = new ListeDerInspektionsroutenGridCommand { Inspektionsroutename = "IR01" };

            var pos = GetListeDerInspektionsroutenPos(listeDerInspektionsroutenGridCommand);

            Assert.AreEqual(1, pos.Count);
            AssertPoIsTheExpected(pos, "IR01", "SA01", EigentuemerTyp.Gemeinde, null);
        }
        
        [Test]
        public void TestWithInspektionsrouteInInspektionBeiFilter()
        {
            var listeDerInspektionsroutenGridCommand = new ListeDerInspektionsroutenGridCommand { InspektionsrouteInInspektionBei = "IR01".GetInInspektionBei() };

            var pos = GetListeDerInspektionsroutenPos(listeDerInspektionsroutenGridCommand);

            Assert.AreEqual(1, pos.Count);
            AssertPoIsTheExpected(pos, "IR01", "SA01", EigentuemerTyp.Gemeinde, null);
        }
        
        [Test]
        public void TestWithEigentuemerFilter()
        {
            var listeDerInspektionsroutenGridCommand = new ListeDerInspektionsroutenGridCommand { Eigentuemer = (int)EigentuemerTyp.Gemeinde };

            var pos = GetListeDerInspektionsroutenPos(listeDerInspektionsroutenGridCommand);

            Assert.AreEqual(1, pos.Count);
            AssertPoIsTheExpected(pos, "IR01", "SA01", EigentuemerTyp.Gemeinde, null);
        }
        
        [Test]
        public void TestWithInspektionsrouteInInspektionBisVonFilter()
        {
            var listeDerInspektionsroutenGridCommand = new ListeDerInspektionsroutenGridCommand
                                                           {
                                                               InspektionsrouteInInspektionBisVon = new DateTime(2010,1,1), 
                                                               InspektionsrouteInInspektionBisBis = new DateTime(2010,11,11)
                                                           };

            var pos = GetListeDerInspektionsroutenPos(listeDerInspektionsroutenGridCommand);

            Assert.AreEqual(1, pos.Count);
            AssertPoIsTheExpected(pos, "IR02", "SA02", EigentuemerTyp.Kanton, new DateTime(2010, 10, 10));
        }
        
        [Test]
        public void TestWithAllFilter()
        {
            var listeDerInspektionsroutenGridCommand = new ListeDerInspektionsroutenGridCommand
                                                           {
                                                               //Strassenname = "SA02", //SQLite dos not support .Contains(.ToLower())
                                                               Inspektionsroutename = "R02",
                                                               InspektionsrouteInInspektionBei = "R02".GetInInspektionBei(),
                                                               Eigentuemer = (int)EigentuemerTyp.Kanton,
                                                               InspektionsrouteInInspektionBisVon = new DateTime(2010,1,1), 
                                                               InspektionsrouteInInspektionBisBis = new DateTime(2010,11,11)
                                                           };

            var pos = GetListeDerInspektionsroutenPos(listeDerInspektionsroutenGridCommand);

            Assert.AreEqual(1, pos.Count);
            AssertPoIsTheExpected(pos, "IR02", "SA02", EigentuemerTyp.Kanton, new DateTime(2010, 10, 10));
        }

        private void AssertPoIsTheExpected(IEnumerable<ListeDerInspektionsroutenPo> pos, string inspektionsrouteName, string strassenabschnittName, EigentuemerTyp eigentuemerTyp, DateTime? inInspektionBis)
        {
            var po = pos.Single(p => p.Inspektionsroutename == inspektionsrouteName);

            Assert.AreEqual(inspektionsrouteName, po.Inspektionsroutename);
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
            Assert.AreEqual(inspektionsrouteName.GetInInspektionBei(), po.InInspektionBei);
            Assert.AreEqual(inInspektionBis, po.InInspektionBis);
        }

        private List<ListeDerInspektionsroutenPo> GetListeDerInspektionsroutenPos(ListeDerInspektionsroutenGridCommand listeDerInspektionsroutenGridCommand)
        {
            //Generate Report
            BrowserDriver.GeneratReports(listeDerInspektionsroutenGridCommand, rp => BrowserDriver.InvokePostAction<ListeDerInspektionsroutenController, ListeDerInspektionsroutenGridCommand>((c, r) => c.GetReport(r), rp, false));

            //Assert on Po-s
            return GetPos<ListeDerInspektionsroutenPo>();
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

                var inspektionsRouteGISOne = TestDataHelpers.GetInspektionsRouteGIS(GetCurrentErfassungsPeriod(scope), "IR01", null, entityThree);
                scope.Session.Save(inspektionsRouteGISOne);

                var inspektionsRouteGISTwo = TestDataHelpers.GetInspektionsRouteGIS(GetCurrentErfassungsPeriod(scope), "IR02", new DateTime(2010, 10, 10), entityTwo);
                scope.Session.Save(inspektionsRouteGISTwo);

                var inspektionsRouteGISThree = TestDataHelpers.GetInspektionsRouteGIS(GetClosedErfassungPeriod(scope), "IR03", null, entiyThree);
                scope.Session.Save(inspektionsRouteGISThree);

                var inspektionsRouteGISFour = TestDataHelpers.GetInspektionsRouteGIS(GetOtherErfassungPeriod(scope), "IR01", null, entityOtherMandant);
                scope.Session.Save(inspektionsRouteGISFour);
            }
        }
    }
}
