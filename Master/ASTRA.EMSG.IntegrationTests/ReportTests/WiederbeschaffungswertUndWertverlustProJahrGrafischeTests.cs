using System;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProJahrGrafische;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Common;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.Web.Areas.Auswertungen.Controllers;
using NUnit.Framework;
using NHibernate.Linq;

namespace ASTRA.EMSG.IntegrationTests.ReportTests
{
    [TestFixture]
    public class WiederbeschaffungswertUndWertverlustProJahrGrafischeTests : ReportTestBase
    {
        private ErfassungsPeriod ep1;
        private Guid kenngroessen;
        private ErfassungsPeriod ep2;
        private ErfassungsPeriod ep3;

        protected override void Init()
        {
            base.Init();
            InsertTestData();
        }

        [Test]
        public void TestWithCurrentErfassugnsPeriod()
        {
            var parameter = new WiederbeschaffungswertUndWertverlustProJahrGrafischeParameter
                                {
                                    JahrIdVon = GetCurrentErfassungsPeriodId(),
                                    JahrIdBis = GetCurrentErfassungsPeriodId(),
                                };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.3_CurrentErfassungsPeriodVonBis");
        }

        [Test]
        public void TestWitKenngroessenVonBis()
        {
            var parameter = new WiederbeschaffungswertUndWertverlustProJahrGrafischeParameter
                                {
                                    JahrIdVon = kenngroessen,
                                    JahrIdBis = kenngroessen,
                                };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.3_Kenngroessen");
        }

        [Test]
        public void TestWithSummarichErfassungsPeriod()
        {
            var parameter = new WiederbeschaffungswertUndWertverlustProJahrGrafischeParameter
                                {
                                    JahrIdVon = ep1.Id,
                                    JahrIdBis = ep1.Id,
                                };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.3_Summarich");
        }

        [Test]
        public void TestWithTabellarischErfassungsPeriod()
        {
            var parameter = new WiederbeschaffungswertUndWertverlustProJahrGrafischeParameter
            {
                JahrIdVon = ep2.Id,
                JahrIdBis = ep2.Id,
            };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.3_Tabellarisch");
        }

        [Test]
        public void TestWithGISErfassungsPeriod()
        {
            var parameter = new WiederbeschaffungswertUndWertverlustProJahrGrafischeParameter
            {
                JahrIdVon = ep3.Id,
                JahrIdBis = ep3.Id,
            };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.3_GIS");
        }

        [Test]
        public void TestWithMultiple()
        {
            var parameter = new WiederbeschaffungswertUndWertverlustProJahrGrafischeParameter
            {
                JahrIdVon = kenngroessen,
                JahrIdBis = GetCurrentErfassungsPeriodId(),
            };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.3_Multiple");
        }

        [Test]
        public void TestWithCurrentErfassugnsPeriodWithGemeindeFilter()
        {
            var parameter = new WiederbeschaffungswertUndWertverlustProJahrGrafischeParameter
            {
                JahrIdVon = kenngroessen,
                JahrIdBis = GetClosedErfassungPeriodId(),
                Eigentuemer = EigentuemerTyp.Gemeinde
            };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.3_GemeindeFilter");
        }

        private void GenerateReportWithFilter(WiederbeschaffungswertUndWertverlustProJahrGrafischeParameter parameter)
        {
            //Generate Report
            BrowserDriver.GeneratReports((EmsgReportParameter)parameter, rp => BrowserDriver.InvokePostAction<WiederbeschaffungswertUndWertverlustProJahrGrafischeController, WiederbeschaffungswertUndWertverlustProJahrGrafischeParameter>((c, r) => c.GetReport(r), (WiederbeschaffungswertUndWertverlustProJahrGrafischeParameter)rp, false));
        }

        protected override NetzErfassungsmodus Erfassungmodus { get { return NetzErfassungsmodus.Tabellarisch; } }

        private void InsertTestData()
        {
            using (var scope = new NHibernateTestScope())
            {

                kenngroessen = scope.Save(new KenngroessenFruehererJahre
                                                  {
                                                      Mandant = scope.GetMandant(TestMandantName),
                                                      Jahr = 2004,
                                                      KenngroesseFruehereJahrDetails = new[]
                                                                                           {
                                                                                              new KenngroessenFruehererJahreDetail { Belastungskategorie = scope.GetBelastungskategorie("IA"),  Fahrbahnflaeche = 1000, Fahrbahnlaenge = 200 },
                                                                                              new KenngroessenFruehererJahreDetail { Belastungskategorie = scope.GetBelastungskategorie("IB"), Fahrbahnflaeche = 1000, Fahrbahnlaenge = 200 }
                                                                                           }
                                                  });

                ep1 = scope.CreateErfassungsPeriod(2005, NetzErfassungsmodus.Summarisch, TestMandantName);

                ep1.AddEntity(scope, new NetzSummarisch
                               {
                                   NetzSummarischDetails = new[]
                                                               {
                                                                   new NetzSummarischDetail { Belastungskategorie = scope.GetBelastungskategorie("IA"), Fahrbahnflaeche = 1000, Fahrbahnlaenge = 200 },
                                                                   new NetzSummarischDetail { Belastungskategorie = scope.GetBelastungskategorie("IB"), Fahrbahnflaeche = 1000, Fahrbahnlaenge = 200 }
                                                               }
                               });

                ep1.AddEntity(scope, new WiederbeschaffungswertKatalog { AlterungsbeiwertII = 40, GesamtflaecheFahrbahn = 50, Belastungskategorie = scope.GetBelastungskategorie("IA") });
                ep1.AddEntity(scope, new WiederbeschaffungswertKatalog { AlterungsbeiwertII = 20, GesamtflaecheFahrbahn = 10, Belastungskategorie = scope.GetBelastungskategorie("IB") });

                ep2 = scope.CreateErfassungsPeriod(2006, NetzErfassungsmodus.Tabellarisch, TestMandantName);
                ep2.AddEntity(scope, new Strassenabschnitt { Belastungskategorie = scope.GetBelastungskategorie("IA"), Strasseneigentuemer = EigentuemerTyp.Privat,   Laenge = 50, BreiteFahrbahn = 10 });
                ep2.AddEntity(scope, new Strassenabschnitt { Belastungskategorie = scope.GetBelastungskategorie("IA"), Strasseneigentuemer = EigentuemerTyp.Gemeinde, Laenge = 50, BreiteFahrbahn = 10, Trottoir = TrottoirTyp.BeideSeiten, BreiteTrottoirLinks = 2, BreiteTrottoirRechts = 1 });
                ep2.AddEntity(scope, new Strassenabschnitt { Belastungskategorie = scope.GetBelastungskategorie("IB"), Strasseneigentuemer = EigentuemerTyp.Kanton,   Laenge = 50, BreiteFahrbahn = 10 });
                ep2.AddEntity(scope, new Strassenabschnitt { Belastungskategorie = scope.GetBelastungskategorie("IB"), Strasseneigentuemer = EigentuemerTyp.Gemeinde, Laenge = 50, BreiteFahrbahn = 10 });

                ep2.AddEntity(scope, new WiederbeschaffungswertKatalog { AlterungsbeiwertII = 40, GesamtflaecheFahrbahn = 50, FlaecheFahrbahn = 20, FlaecheTrottoir = 20, Belastungskategorie = scope.GetBelastungskategorie("IA") });
                ep2.AddEntity(scope, new WiederbeschaffungswertKatalog { AlterungsbeiwertII = 20, GesamtflaecheFahrbahn = 10, FlaecheFahrbahn = 20, FlaecheTrottoir = 20, Belastungskategorie = scope.GetBelastungskategorie("IB") });

                ep3 = scope.CreateErfassungsPeriod(2007, NetzErfassungsmodus.Gis, TestMandantName);
                ep3.AddEntity(scope, new StrassenabschnittGIS { Belastungskategorie = scope.GetBelastungskategorie("IA"), Strasseneigentuemer = EigentuemerTyp.Privat, Laenge = 50, BreiteFahrbahn = 10 });
                ep3.AddEntity(scope, new StrassenabschnittGIS { Belastungskategorie = scope.GetBelastungskategorie("IA"), Strasseneigentuemer = EigentuemerTyp.Gemeinde, Laenge = 50, BreiteFahrbahn = 10, Trottoir = TrottoirTyp.BeideSeiten, BreiteTrottoirLinks = 2, BreiteTrottoirRechts = 1 });
                ep3.AddEntity(scope, new StrassenabschnittGIS { Belastungskategorie = scope.GetBelastungskategorie("IB"), Strasseneigentuemer = EigentuemerTyp.Kanton, Laenge = 50, BreiteFahrbahn = 10 });
                ep3.AddEntity(scope, new StrassenabschnittGIS { Belastungskategorie = scope.GetBelastungskategorie("IB"), Strasseneigentuemer = EigentuemerTyp.Gemeinde, Laenge = 50, BreiteFahrbahn = 10 });
                  
                ep3.AddEntity(scope, new WiederbeschaffungswertKatalog { AlterungsbeiwertII = 40, GesamtflaecheFahrbahn = 50, FlaecheFahrbahn = 20, FlaecheTrottoir = 20, Belastungskategorie = scope.GetBelastungskategorie("IA") });
                ep3.AddEntity(scope, new WiederbeschaffungswertKatalog { AlterungsbeiwertII = 20, GesamtflaecheFahrbahn = 10, FlaecheFahrbahn = 20, FlaecheTrottoir = 20, Belastungskategorie = scope.GetBelastungskategorie("IB") });

                var ep4 = scope.CreateErfassungsPeriod(2006, NetzErfassungsmodus.Gis, OtherTestMandantName);
                ep4.AddEntity(scope, new StrassenabschnittGIS { Belastungskategorie = scope.GetBelastungskategorie("IA"), Laenge = 50, BreiteFahrbahn = 10 });
                ep4.AddEntity(scope, new StrassenabschnittGIS { Belastungskategorie = scope.GetBelastungskategorie("IA"), Laenge = 50, BreiteFahrbahn = 10, Trottoir = TrottoirTyp.BeideSeiten, BreiteTrottoirLinks = 2, BreiteTrottoirRechts = 1 });
                ep4.AddEntity(scope, new StrassenabschnittGIS { Belastungskategorie = scope.GetBelastungskategorie("IB"), Laenge = 50, BreiteFahrbahn = 10 });
                ep4.AddEntity(scope, new StrassenabschnittGIS { Belastungskategorie = scope.GetBelastungskategorie("IB"), Laenge = 50, BreiteFahrbahn = 10 });
                  
                ep4.AddEntity(scope, new WiederbeschaffungswertKatalog { AlterungsbeiwertII = 40, GesamtflaecheFahrbahn = 50, FlaecheFahrbahn = 20, FlaecheTrottoir = 20, Belastungskategorie = scope.GetBelastungskategorie("IA") });
                ep4.AddEntity(scope, new WiederbeschaffungswertKatalog { AlterungsbeiwertII = 20, GesamtflaecheFahrbahn = 10, FlaecheFahrbahn = 20, FlaecheTrottoir = 20, Belastungskategorie = scope.GetBelastungskategorie("IB") });

                GetCurrentErfassungsPeriod(scope).AddEntity(scope, new Strassenabschnitt { Belastungskategorie = scope.GetBelastungskategorie("IA"), Laenge = 40, BreiteFahrbahn = 10 });
                GetCurrentErfassungsPeriod(scope).AddEntity(scope, new Strassenabschnitt { Belastungskategorie = scope.GetBelastungskategorie("IA"), Laenge = 50, BreiteFahrbahn = 20, Trottoir = TrottoirTyp.BeideSeiten, BreiteTrottoirLinks = 2, BreiteTrottoirRechts = 1 });
                GetCurrentErfassungsPeriod(scope).AddEntity(scope, new Strassenabschnitt { Belastungskategorie = scope.GetBelastungskategorie("IB"), Laenge = 20, BreiteFahrbahn = 10 });
                GetCurrentErfassungsPeriod(scope).AddEntity(scope, new Strassenabschnitt { Belastungskategorie = scope.GetBelastungskategorie("IB"), Laenge = 60, BreiteFahrbahn = 10 });
            }
        }
    }

    public static class NHibernateTestScopeExtensions
    {
        public static Mandant GetMandant(this NHibernateTestScope scope, string mandantName)
        {
            return scope.Session.Query<Mandant>().Single(m => m.MandantName == mandantName);
        }

        public static Belastungskategorie GetBelastungskategorie(this NHibernateTestScope scope, string typ)
        {
            return scope.Session.Query<Belastungskategorie>().Single(m => m.Typ == typ);
        }

        public static Guid Save<T>(this NHibernateTestScope scope, T entity) where T : IEntity
        {
            return (Guid)scope.Session.Save(entity);
        }

        public static ErfassungsPeriod CreateErfassungsPeriod(this NHibernateTestScope scope, int year, NetzErfassungsmodus mode, string mandantName, bool isClosed = true)
        {
            var erfassungsPeriod = new ErfassungsPeriod { Erfassungsjahr = new DateTime(year, 01, 01), IsClosed = isClosed, NetzErfassungsmodus = mode, Mandant = scope.GetMandant(mandantName) };
            erfassungsPeriod.Id = scope.Save(erfassungsPeriod);
            return erfassungsPeriod;
        }

        public static T AddEntity<T>(this ErfassungsPeriod ep, NHibernateTestScope scope, T entity) where T : IErfassungsPeriodDependentEntity
        {
            entity.ErfassungsPeriod = ep;
            entity.Mandant = ep.Mandant;
            var id = scope.Save(entity);
            entity.Id = id;
            return entity;
        }
    }
}
