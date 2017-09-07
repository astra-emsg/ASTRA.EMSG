using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Common;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.Tests.Common.Utils;
using ASTRA.EMSG.Web.Areas.Administration.Controllers;
using NHibernate.Linq;
using NUnit.Framework;

namespace ASTRA.EMSG.IntegrationTests.JahresAbschlussTests
{
    [TestFixture]
    public class JahresAbschlussTests : IntegrationTestBase
    {
        protected override void DbInit()
        {
            //Setup role
            using (var scope = new NHibernateTestScope())
            {
                var mandant = DbHandlerUtils.CreateMandant(scope.Session, TestMandantName, "0", NetzErfassungsmodus.Gis);
                DbHandlerUtils.CreateTestUser(scope.Session, TestUserName, new[] { mandant }, new List<Rolle> { Rolle.DataManager, Rolle.DataReader, Rolle.Benutzeradministrator, Rolle.Benchmarkteilnehmer });
            }
        }

        [Test]
        public void TestCreationOfNewErfassungsPeriod()
        {
            using (var scope = new NHibernateTestScope())
            {
                var erfassungsPeriod = scope.Session.Query<ErfassungsPeriod>().Single(m => !m.IsClosed);
                erfassungsPeriod.NetzErfassungsmodus = NetzErfassungsmodus.Tabellarisch;

                scope.Session.Save(erfassungsPeriod);
            }
            
            DoJahresabschluss();

            using (var scope = new NHibernateTestScope())
            {
                var erfassungsPeriodCount = scope.Session.Query<ErfassungsPeriod>().Count();
                var currentErfassungsPeriod = scope.Session.Query<ErfassungsPeriod>().SingleOrDefault(m => !m.IsClosed);
                var previousErfassungsPeriod = scope.Session.Query<ErfassungsPeriod>().SingleOrDefault(m => m.IsClosed);

                Assert.AreEqual(2, erfassungsPeriodCount);
                Assert.NotNull(currentErfassungsPeriod);
                Assert.NotNull(previousErfassungsPeriod);
                Assert.AreEqual(NetzErfassungsmodus.Tabellarisch, currentErfassungsPeriod.NetzErfassungsmodus);
                Assert.AreEqual(NetzErfassungsmodus.Tabellarisch, previousErfassungsPeriod.NetzErfassungsmodus);
                Assert.NotNull(currentErfassungsPeriod.Mandant);
                Assert.AreEqual(previousErfassungsPeriod.Mandant.Id, currentErfassungsPeriod.Mandant.Id);
            }
        }
        
        [Test]
        public void TestClearAbschnitteInSummarischeModus()
        {
            using (var scope = new NHibernateTestScope())
            {
                var erfassungsPeriod = scope.Session.Query<ErfassungsPeriod>().Single(m => !m.IsClosed);

                erfassungsPeriod.NetzErfassungsmodus = NetzErfassungsmodus.Summarisch;

                scope.Session.Save(erfassungsPeriod);

                var belastungskategorie = TestDataHelpers.GetBelastungskategorie(scope, "IA");

                var strassenabschnittOne = TestDataHelpers.GetStrassenabschnitt(erfassungsPeriod, "SA0", belastungskategorie, EigentuemerTyp.Gemeinde);
                scope.Session.Save(strassenabschnittOne);

                var zustandsabschnittOne = TestDataHelpers.GetZustandsabschnitt(strassenabschnittOne, 1);
                scope.Session.Save(zustandsabschnittOne);
            }

            DoJahresabschluss();

            using (var scope = new NHibernateTestScope())
            {
                var erfassungsPeriod = scope.Session.Query<ErfassungsPeriod>().Single(m => !m.IsClosed);

                var strassenabschnittCount = scope.Session.Query<Strassenabschnitt>().Count(sa => sa.ErfassungsPeriod == erfassungsPeriod);
                var zustandsabschnittCount = scope.Session.Query<Zustandsabschnitt>().Count(za => za.Strassenabschnitt.ErfassungsPeriod == erfassungsPeriod);

                Assert.AreEqual(0, strassenabschnittCount);
                Assert.AreEqual(0, zustandsabschnittCount);
            }
        }

        [Test]
        public void TestCopyAndClearNetzSummarischDetailsInSummarischeModus()
        {
            using (var scope = new NHibernateTestScope())
            {
                var erfassungsPeriod = scope.Session.Query<ErfassungsPeriod>().Single(m => !m.IsClosed);

                erfassungsPeriod.NetzErfassungsmodus = NetzErfassungsmodus.Summarisch;

                var netzSummarischDetail = GetNetzSummarischDetail(scope, erfassungsPeriod, "IA");
                netzSummarischDetail.Fahrbahnflaeche = 10;
                netzSummarischDetail.Fahrbahnlaenge = 1000;
                netzSummarischDetail.MittlererZustand = 2.5m;

                scope.Session.Save(erfassungsPeriod);
                scope.Session.Save(netzSummarischDetail);
            }

            DoJahresabschluss();

            using (var scope = new NHibernateTestScope())
            {
                var erfassungsPeriod = scope.Session.Query<ErfassungsPeriod>().Single(m => !m.IsClosed);

                var netzSummarischCount = scope.Session.Query<NetzSummarisch>().Count();
                var netzSummarischDetailCount = scope.Session.Query<NetzSummarischDetail>().Count();
                var netzSummarischDetail = GetNetzSummarischDetail(scope, erfassungsPeriod, "IA");

                Assert.AreEqual(2, netzSummarischCount);
                Assert.AreEqual(22, netzSummarischDetailCount);
                Assert.AreEqual(10, netzSummarischDetail.Fahrbahnflaeche);
                Assert.AreEqual(1000, netzSummarischDetail.Fahrbahnlaenge);
                Assert.AreEqual(null, netzSummarischDetail.MittlererZustand);
            }
        }
        
        [Test]
        public void TestCopyAndClearStrassenabschnitteAndZustandsabschnitteInTabellarischModus()
        {
            using (var scope = new NHibernateTestScope())
            {
                var erfassungsPeriod = scope.Session.Query<ErfassungsPeriod>().Single(m => !m.IsClosed);

                erfassungsPeriod.NetzErfassungsmodus = NetzErfassungsmodus.Tabellarisch;

                scope.Session.Save(erfassungsPeriod);

                var belastungskategorie = TestDataHelpers.GetBelastungskategorie(scope, "IA");

                var strassenabschnittOne = TestDataHelpers.GetStrassenabschnitt(erfassungsPeriod, "SA0", belastungskategorie, EigentuemerTyp.Gemeinde);
                scope.Session.Save(strassenabschnittOne);

                var zustandsabschnittOne = TestDataHelpers.GetZustandsabschnitt(strassenabschnittOne, 1);
                scope.Session.Save(zustandsabschnittOne);
            }

            DoJahresabschluss();

            using (var scope = new NHibernateTestScope())
            {
                var erfassungsPeriod = scope.Session.Query<ErfassungsPeriod>().Single(m => !m.IsClosed);

                var strassenabschnitt = scope.Session.Query<Strassenabschnitt>().SingleOrDefault(sa => sa.ErfassungsPeriod == erfassungsPeriod);
                var zustandsabschnitt = scope.Session.Query<Zustandsabschnitt>().SingleOrDefault(za => za.Strassenabschnitt.ErfassungsPeriod == erfassungsPeriod);

                Assert.NotNull(strassenabschnitt);
                Assert.NotNull(zustandsabschnitt);
                Assert.AreEqual(1, zustandsabschnitt.Zustandsindex);
            }
        }

        private static NetzSummarischDetail GetNetzSummarischDetail(NHibernateTestScope scope, ErfassungsPeriod erfassungsPeriod, string belastrungskategorieTyp)
        {
            return scope.Session.Query<NetzSummarischDetail>().Single(nsd => nsd.NetzSummarisch.ErfassungsPeriod == erfassungsPeriod && nsd.Belastungskategorie.Typ == belastrungskategorieTyp);
        }

        private void DoJahresabschluss()
        {
            var erfassungsabschlussModel = new ErfassungsabschlussModel {AbschlussDate = new DateTime(2010, 1, 1)};
            BrowserDriver.InvokePostAction<ErfassungsPeriodAbschlussController, ErfassungsabschlussModel>(
                (c, r) => c.ErfassungsPeriodAbschluss(r), erfassungsabschlussModel, false);
        }
    }
}
