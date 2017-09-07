using System;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafische;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Common;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.Web.Areas.Auswertungen.Controllers;
using NUnit.Framework;
using NHibernate.Linq;
using System.Collections.Generic;

namespace ASTRA.EMSG.IntegrationTests.ReportTests
{
    [TestFixture]
    public class RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeTests : ReportTestBase
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
            var parameter = new RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter
                                {
                                    JahrIdVon = GetCurrentErfassungsPeriodId(),
                                    JahrIdBis = GetCurrentErfassungsPeriodId(),
                                };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.4_CurrentErfassungsPeriodVonBis");
        }

        [Test]
        public void TestWitKenngroessenVonBis()
        {
            var parameter = new RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter
                                {
                                    JahrIdVon = kenngroessen,
                                    JahrIdBis = kenngroessen,
                                };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.4_Kenngroessen");
        }

        [Test]
        public void TestWithSummarichErfassungsPeriod()
        {
            var parameter = new RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter
                                {
                                    JahrIdVon = ep1.Id,
                                    JahrIdBis = ep1.Id,
                                };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.4_Summarich");
        }

        [Test]
        public void TestWithTabellarischErfassungsPeriod()
        {
            var parameter = new RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter
            {
                JahrIdVon = ep2.Id,
                JahrIdBis = ep2.Id,
            };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.4_Tabellarisch");
        }

        [Test]
        public void TestWithGISErfassungsPeriod()
        {
            var parameter = new RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter
            {
                JahrIdVon = ep3.Id,
                JahrIdBis = ep3.Id,
            };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.4_GIS");
        }

        [Test]
        public void TestWithMultiple()
        {
            var parameter = new RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter
            {
                JahrIdVon = kenngroessen,
                JahrIdBis = GetCurrentErfassungsPeriodId(),
            };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.4_Multiple");
        }

        [Test]
        public void TestWithCurrentErfassugnsPeriodWithGemeindeFilter()
        {
            var parameter = new RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter
            {
                JahrIdVon = kenngroessen,
                JahrIdBis = GetCurrentErfassungsPeriodId(),
                Eigentuemer = EigentuemerTyp.Gemeinde
            };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.4_GemeindeFilter");
        }

        private void GenerateReportWithFilter(RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter parameter)
        {
            //Generate Report
            BrowserDriver.GeneratReports((EmsgReportParameter)parameter, rp => BrowserDriver.InvokePostAction<RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeController, RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter>((c, r) => c.GetReport(r), (RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter)rp, false));
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
                                                      KostenFuerWerterhaltung = 50000,
                                                      KenngroesseFruehereJahrDetails = new[]
                                                                                           {
                                                                                              new KenngroessenFruehererJahreDetail { Belastungskategorie = scope.GetBelastungskategorie("IA"),  Fahrbahnflaeche = 1000, Fahrbahnlaenge = 200, MittlererZustand = 3 },
                                                                                              new KenngroessenFruehererJahreDetail { Belastungskategorie = scope.GetBelastungskategorie("IB"), Fahrbahnflaeche = 1000, Fahrbahnlaenge = 200, MittlererZustand = 1  }
                                                                                           }
                                                  });

                ep1 = scope.CreateErfassungsPeriod(2005, NetzErfassungsmodus.Summarisch, TestMandantName);

                ep1.AddEntity(scope, new NetzSummarisch
                               {
                                   NetzSummarischDetails = new[]
                                                               {
                                                                   new NetzSummarischDetail { Belastungskategorie = scope.GetBelastungskategorie("IA"), Fahrbahnflaeche = 1000, Fahrbahnlaenge = 200, MittlererZustand = 5 },
                                                                   new NetzSummarischDetail { Belastungskategorie = scope.GetBelastungskategorie("IB"), Fahrbahnflaeche = 1000, Fahrbahnlaenge = 200, MittlererZustand = 1 }
                                                               }
                               });

                ep1.AddEntity(scope, new RealisierteMassnahmeSummarsich { Belastungskategorie = scope.GetBelastungskategorie("IA"), Fahrbahnflaeche = 1000, KostenFahrbahn = 30000 });
                ep1.AddEntity(scope, new RealisierteMassnahmeSummarsich { Belastungskategorie = scope.GetBelastungskategorie("IB"), Fahrbahnflaeche = 1000, KostenFahrbahn = 10000 });

                ep1.AddEntity(scope, new WiederbeschaffungswertKatalog { AlterungsbeiwertII = 40, GesamtflaecheFahrbahn = 50, Belastungskategorie = scope.GetBelastungskategorie("IA") });
                ep1.AddEntity(scope, new WiederbeschaffungswertKatalog { AlterungsbeiwertII = 20, GesamtflaecheFahrbahn = 10, Belastungskategorie = scope.GetBelastungskategorie("IB") });

                ep2 = scope.CreateErfassungsPeriod(2006, NetzErfassungsmodus.Tabellarisch, TestMandantName);
                ep2.AddEntity(scope, new Strassenabschnitt { Belastungskategorie = scope.GetBelastungskategorie("IA"), Strasseneigentuemer = EigentuemerTyp.Privat,   Laenge = 50, BreiteFahrbahn = 10, 
                    Zustandsabschnitten = new [] {
                                                   new Zustandsabschnitt { Zustandsindex = 3, Laenge = 20, Aufnahmedatum = DateTime.Now},
                                                   new Zustandsabschnitt { Zustandsindex = 1, Laenge = 30, Aufnahmedatum = DateTime.Now },
                                               }});
                ep2.AddEntity(scope, new Strassenabschnitt { Belastungskategorie = scope.GetBelastungskategorie("IA"), Strasseneigentuemer = EigentuemerTyp.Gemeinde, Laenge = 50, BreiteFahrbahn = 10, Trottoir = TrottoirTyp.BeideSeiten, BreiteTrottoirLinks = 2, BreiteTrottoirRechts = 1, 
                    Zustandsabschnitten = new [] {
                                                   new Zustandsabschnitt { Zustandsindex = 2, Laenge = 20, Aufnahmedatum = DateTime.Now },
                                                   new Zustandsabschnitt { Zustandsindex = 5, Laenge = 30, Aufnahmedatum = DateTime.Now },
                                               }});
                ep2.AddEntity(scope, new Strassenabschnitt { Belastungskategorie = scope.GetBelastungskategorie("IB"), Strasseneigentuemer = EigentuemerTyp.Kanton,   Laenge = 50, BreiteFahrbahn = 10, 
                    Zustandsabschnitten = new [] {
                                                   new Zustandsabschnitt { Zustandsindex = 1, Laenge = 20, Aufnahmedatum = DateTime.Now },
                                                   new Zustandsabschnitt { Zustandsindex = 5, Laenge = 30, Aufnahmedatum = DateTime.Now },
                                               }});
                ep2.AddEntity(scope, new Strassenabschnitt { Belastungskategorie = scope.GetBelastungskategorie("IB"), Strasseneigentuemer = EigentuemerTyp.Gemeinde, Laenge = 50, BreiteFahrbahn = 10, 
                    Zustandsabschnitten = new [] {
                                                   new Zustandsabschnitt { Zustandsindex = 1, Laenge = 20, Aufnahmedatum = DateTime.Now },
                                                   new Zustandsabschnitt { Zustandsindex = 5, Laenge = 30, Aufnahmedatum = DateTime.Now },
                                               }});

                ep2.AddEntity(scope, new RealisierteMassnahme {  Laenge = 50, BreiteFahrbahn = 10, KostenFahrbahn = 30000 });
                ep2.AddEntity(scope, new RealisierteMassnahme {  Laenge = 50, BreiteFahrbahn = 10, KostenFahrbahn = 10000 });
                ep2.AddEntity(scope, new RealisierteMassnahme {  Laenge = 50, BreiteFahrbahn = 10, BreiteTrottoirLinks = 5, BreiteTrottoirRechts = 5, KostenFahrbahn = 10000 });

                ep2.AddEntity(scope, new WiederbeschaffungswertKatalog { AlterungsbeiwertII = 40, GesamtflaecheFahrbahn = 50, FlaecheFahrbahn = 20, FlaecheTrottoir = 20, Belastungskategorie = scope.GetBelastungskategorie("IA") });
                ep2.AddEntity(scope, new WiederbeschaffungswertKatalog { AlterungsbeiwertII = 20, GesamtflaecheFahrbahn = 10, FlaecheFahrbahn = 20, FlaecheTrottoir = 20, Belastungskategorie = scope.GetBelastungskategorie("IB") });

                ep3 = scope.CreateErfassungsPeriod(2007, NetzErfassungsmodus.Gis, TestMandantName);
                ep3.AddEntity(scope, new StrassenabschnittGIS { Belastungskategorie = scope.GetBelastungskategorie("IA"), Strasseneigentuemer = EigentuemerTyp.Privat, Laenge = 50, BreiteFahrbahn = 10, 
                    Zustandsabschnitten = new HashSet<ZustandsabschnittGIS> {
                                                   new ZustandsabschnittGIS { Zustandsindex = 1, Laenge = 20, Aufnahmedatum = DateTime.Now },
                                                   new ZustandsabschnittGIS { Zustandsindex = 1, Laenge = 30, Aufnahmedatum = DateTime.Now },
                                               }});
                ep3.AddEntity(scope, new StrassenabschnittGIS { Belastungskategorie = scope.GetBelastungskategorie("IA"), Strasseneigentuemer = EigentuemerTyp.Gemeinde, Laenge = 50, BreiteFahrbahn = 10, Trottoir = TrottoirTyp.BeideSeiten, BreiteTrottoirLinks = 2, BreiteTrottoirRechts = 1 });
                ep3.AddEntity(scope, new StrassenabschnittGIS { Belastungskategorie = scope.GetBelastungskategorie("IB"), Strasseneigentuemer = EigentuemerTyp.Kanton, Laenge = 50, BreiteFahrbahn = 10 });
                ep3.AddEntity(scope, new StrassenabschnittGIS { Belastungskategorie = scope.GetBelastungskategorie("IB"), Strasseneigentuemer = EigentuemerTyp.Gemeinde, Laenge = 50, BreiteFahrbahn = 10 });

                ep3.AddEntity(scope, new RealisierteMassnahmeGIS { Laenge = 50, BreiteFahrbahn = 10, KostenFahrbahn = 30000 });
                ep3.AddEntity(scope, new RealisierteMassnahmeGIS { Laenge = 50, BreiteFahrbahn = 10, KostenFahrbahn = 10000 });
                ep3.AddEntity(scope, new RealisierteMassnahmeGIS { Laenge = 50, BreiteFahrbahn = 10, BreiteTrottoirLinks = 5, BreiteTrottoirRechts = 5, KostenFahrbahn = 10000 });

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
}
