using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.ZustandsspiegelProJahrGrafische;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Common;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.Web.Areas.Auswertungen.Controllers;
using NUnit.Framework;

namespace ASTRA.EMSG.IntegrationTests.ReportTests
{
    [TestFixture]
    public class ZustandsspiegelProJahrGrafischeTests : ReportTestBase
    {
        protected override void Init()
        {
            base.Init();
            InsertTestData();
        }

        [Test]
        public void TestWithCurrentErfassugnsPeriod()
        {
            var parameter = new ZustandsspiegelProJahrGrafischeParameter
                                {
                                    ErfassungsPeriodIdVon = GetCurrentErfassungsPeriodId(),
                                    ErfassungsPeriodIdBis = GetCurrentErfassungsPeriodId(),
                                };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.2_Tabellarisch_CurrentErfassungsPeriodVonBis");
        }
        
        [Test]
        public void TestWithClosedErfassungPeriodVonBis()
        {
            var parameter = new ZustandsspiegelProJahrGrafischeParameter
                                {
                                    ErfassungsPeriodIdVon = GetClosedErfassungPeriodId(),
                                    ErfassungsPeriodIdBis = GetClosedErfassungPeriodId(),
                                };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.2_Tabellarisch_ClosedErfassungPeriodVonBis");
        }

        [Test]
        public void TestWithVonClosedErfassugnsPeriodBisCurrentErfassungsPeriod()
        {
            var parameter = new ZustandsspiegelProJahrGrafischeParameter
                                {
                                    ErfassungsPeriodIdVon = GetClosedErfassungPeriodId(),
                                    ErfassungsPeriodIdBis = GetCurrentErfassungsPeriodId(),
                                };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.2_Tabellarisch_VonClosedErfassugnsPeriodBisCurrentErfassungsPeriod");
        }

        [Test]
        public void TestWithCurrentErfassugnsPeriodWithGemeindeFilter()
        {
            var parameter = new ZustandsspiegelProJahrGrafischeParameter
            {
                ErfassungsPeriodIdVon = GetCurrentErfassungsPeriodId(),
                ErfassungsPeriodIdBis = GetCurrentErfassungsPeriodId(),
                Eigentuemer = EigentuemerTyp.Gemeinde
            };

            GenerateReportWithFilter(parameter);
            AssertReportToReferenzReport("W5.2_Tabellarisch_CurrentErfassugnsPeriodWithGemeindeFilter");
        }

        private void GenerateReportWithFilter(ZustandsspiegelProJahrGrafischeParameter parameter)
        {
            //Generate Report
            BrowserDriver.GeneratReports((EmsgReportParameter)parameter, rp => BrowserDriver.InvokePostAction<ZustandsspiegelProJahrGrafischeController, ZustandsspiegelProJahrGrafischeParameter>((c, r) => c.GetReport(r), (ZustandsspiegelProJahrGrafischeParameter)rp, false));
        }

        protected override NetzErfassungsmodus Erfassungmodus { get { return NetzErfassungsmodus.Tabellarisch; } }

        private void InsertTestData()
        {
            using (var scope = new NHibernateTestScope())
            {
                var belastungskategorie = TestDataHelpers.GetBelastungskategorie(scope, "IA");

                var strassenabschnittOne = TestDataHelpers.GetStrassenabschnitt(GetCurrentErfassungsPeriod(scope), "SA01", belastungskategorie, EigentuemerTyp.Gemeinde);
                scope.Session.Save(strassenabschnittOne);
                scope.Session.Save(TestDataHelpers.GetZustandsabschnitt(strassenabschnittOne, 1));

                var strassenabschnittTwo = TestDataHelpers.GetStrassenabschnitt(GetCurrentErfassungsPeriod(scope), "SA02", belastungskategorie, EigentuemerTyp.Kanton);
                scope.Session.Save(strassenabschnittTwo);
                scope.Session.Save(TestDataHelpers.GetZustandsabschnitt(strassenabschnittTwo, 2));

                var strassenabschnittThree = TestDataHelpers.GetStrassenabschnitt(GetClosedErfassungPeriod(scope), "SA03", belastungskategorie, EigentuemerTyp.Korporation);
                scope.Session.Save(strassenabschnittThree);
                scope.Session.Save(TestDataHelpers.GetZustandsabschnitt(strassenabschnittThree, 3));

                var strassenabschnittOtherMandant = TestDataHelpers.GetStrassenabschnitt(GetOtherErfassungPeriod(scope), "SA01", belastungskategorie, EigentuemerTyp.Gemeinde);
                scope.Session.Save(strassenabschnittOtherMandant);
                scope.Session.Save(TestDataHelpers.GetZustandsabschnitt(strassenabschnittOtherMandant, 1));
            }
        }
    }
}
