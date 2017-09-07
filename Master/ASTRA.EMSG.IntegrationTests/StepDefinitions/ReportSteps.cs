using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.MengeProBelastungskategorie;
using ASTRA.EMSG.Business.Reports.MengeProBelastungskategorieGrafische;
using ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProStrassenabschnitt;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.Utils;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using ASTRA.EMSG.IntegrationTests.Support.ObjectReader;
using ASTRA.EMSG.Web.Areas.Auswertungen.Controllers;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using NHibernate.Linq;

namespace ASTRA.EMSG.IntegrationTests.StepDefinitions
{
    [Binding]
    public class ReportSteps : StepsBase
    {
        public Guid? LastGeneratedReportErfassungsPeriodIdFilter
        {
            get { return (Guid?)ScenarioContext.Current["LastGeneratedReportErfassungsPeriodIdFilter"]; }
            set { ScenarioContext.Current["LastGeneratedReportErfassungsPeriodIdFilter"] = value; }
        }

        public ReportSteps(BrowserDriver browserDriver)
            : base(browserDriver)
        {
        }

        [When(@"ich die Grafik mit Mengen pro Belastungskategorie generiere")]
        public void WennIchDieGrafikMitMengenProBelastungskategorieGeneriere(Table table)
        {
            ReportFilters reportFilters = GetReportFilters(table);

            var mengeProBelastungskategorieGrafischeParameter = new MengeProBelastungskategorieGrafischeParameter { Eigentuemer = reportFilters.EigentuemerTyp, ErfassungsPeriodId = reportFilters.ErfassungsPeriodId };
            GeneratReports((EmsgReportParameter)mengeProBelastungskategorieGrafischeParameter, rp => BrowserDriver.InvokePostAction<MengeProBelastungskategorieGrafischeController, MengeProBelastungskategorieGrafischeParameter>((c, r) => c.GetReport(r), (MengeProBelastungskategorieGrafischeParameter)rp, false));
        }

        [When(@"ich die Tabelle mit Mengen pro Belastungskategorie generiere")]
        public void WennIchDieTabelleMitMengenProBelastungskategorieGeneriere(Table table)
        {
            ReportFilters reportFilters = GetReportFilters(table);

            var mengeProBelastungskategorieParameter = new MengeProBelastungskategorieGridCommand { ErfassungsPeriodId = reportFilters.ErfassungsPeriodId };
            GeneratReports(mengeProBelastungskategorieParameter, rp => BrowserDriver.InvokePostAction<MengeProBelastungskategorieController, MengeProBelastungskategorieGridCommand>((c, r) => c.GetReport(r), rp, false));
        }

        [When(@"ich die Tabelle mit Wiederbeschaffungswert und Wertverlust nach Belastungskategorie generiere")]
        public void WennIchDieTabelleMitWiederbeschaffungswertUndWertverlustNachBelastungskategorieGeneriere(Table table)
        {
            ReportFilters reportFilters = GetReportFilters(table);

            var mengeProBelastungskategorieParameter = new WiederbeschaffungswertUndWertverlustProStrassenabschnittGridCommand { ErfassungsPeriodId = reportFilters.ErfassungsPeriodId };
            GeneratReports(mengeProBelastungskategorieParameter, rp => BrowserDriver.InvokePostAction<WiederbeschaffungswertUndWertverlustProStrassenabschnittController, WiederbeschaffungswertUndWertverlustProStrassenabschnittGridCommand>((c, r) => c.GetReport(r), rp, false));
        }

        public ReportFilters GetReportFilters(Table table)
        {
            var reportFilters = new ReportFilters();
            var reportFilterRows = table.CreateSet<ReportFilterRow>();

            foreach (var filterRow in reportFilterRows)
            {
                switch (filterRow.Filter)
                {
                    case "Erfassungsperiode":
                        using (var nhScope = new NHibernateSpecflowScope())
                        {
                            var mandant = nhScope.GetCurrentMandant();
                            int year = int.Parse(filterRow.FilterWert);
                            var erfassungsPeriod = nhScope.GetClosedErfassungsperiods(mandant.MandantName).Single(e => e.Erfassungsjahr.Year == year);
                            reportFilters.ErfassungsPeriodId = erfassungsPeriod.Id;
                            LastGeneratedReportErfassungsPeriodIdFilter = reportFilters.ErfassungsPeriodId;
                        }
                        break;
                    case "Strasseneigentümer":
                        reportFilters.EigentuemerTyp = (EigentuemerTyp?)Enum.Parse(typeof(EigentuemerTyp), filterRow.FilterWert);
                        break;
                }
            }

            return reportFilters;
        }

        [When("zeigt die Tabelle mit Mengen pro Belastungskategorie folgende Mengen:")]
        public void ZeigtDieTabelleMitMengenProBelastungskategorieFolgendeMengen(Table table)
        {
            var mengeProBelastungskategorieRows = table.CreateSet<MengeProBelastungskategorieRow>().ToList();

            var mengeProBelastungskategoriePos = GetPos<MengeProBelastungskategoriePo>();

            using (new NHibernateSpecflowScope())
            {
                foreach (var po in mengeProBelastungskategoriePos)
                {
                    decimal? menge = po.Fahrbahnflaeche;
                    var mengeProBelastungskategorieRow = mengeProBelastungskategorieRows.SingleOrDefault(r => r.BelastungskategorieTyp == po.BelastungskategorieTyp);
                    if (mengeProBelastungskategorieRow != null)
                        Assert.AreEqual(mengeProBelastungskategorieRow.Menge.ParseNullableDecimal(), menge);
                }
            }
        }

        [Then(@"zeigt die Tabelle mit Wiederbeschaffungswert und Wertverlust des Strassennetzes des Mandanten gegliedert nach Belastungskategorie folgende Daten:  Tabelle mit Wiederbeschaffungswert und Wertverlust des Strassennetzes des Mandanten gegliedert nach Belastungskategorie")]
        public void DannZeigtDieTabelleMitWiederbeschaffungswertUndWertverlustDesStrassennetzesDesMandantenGegliedertNachBelastungskategorieFolgendeDatenTabelleMitWiederbeschaffungswertUndWertverlustDesStrassennetzesDesMandantenGegliedertNachBelastungskategorie(Table table)
        {
            var mengeProBelastungskategoriePos = GetPos<WiederbeschaffungswertUndWertverlustProStrassenabschnittPo>();
            bool areObjectListWithTableEqual = GetObjectReaderConfigurationFor<WiederbeschaffungswertUndWertverlustProStrassenabschnittPo>()
                .GetObjectReader()
                .AreObjectListWithTableEqual(mengeProBelastungskategoriePos, table);

            Assert.IsTrue(areObjectListWithTableEqual);
        }

        private static List<TPresentationObject> GetPos<TPresentationObject>()
        {
            var xmlSerializer = new XmlSerializer(typeof(TPresentationObject[]));

            IEnumerable<TPresentationObject> pos = null;
            string poXmlPath = Path.Combine(TestDeploymentHelper.GetTestOutputDir(), string.Format("CurrentReport.xml"));
            using (var sr = new StreamReader(poXmlPath))
            {
                pos = (IEnumerable<TPresentationObject>)xmlSerializer.Deserialize(sr);
            }

            return pos.ToList();
        }

        public void GeneratReports(EmsgReportParameter emsgReportParameter, Action<EmsgReportParameter> generateReport)
        {
            GenerateReportForAllOutputFormat(outputFormat => { emsgReportParameter.IsPreview = false; emsgReportParameter.OutputFormat = outputFormat; generateReport(emsgReportParameter); });
        }

        public void GeneratReports<T>(T emsgReportParameter, Action<T> generateReport) where T : ReportGridCommand
        {
            GenerateReportForAllOutputFormat(outputFormat => { emsgReportParameter.OutputFormat = outputFormat; generateReport(emsgReportParameter); });
        }

        public void GenerateReportForAllOutputFormat(Action<OutputFormat> action)
        {
            var outputFormatList = new List<OutputFormat> { OutputFormat.Xml, OutputFormat.Excel, OutputFormat.Pdf, OutputFormat.Image };

            foreach (var outputFormat in outputFormatList)
            {
                action(outputFormat);

                var result = BrowserDriver.GetRequestResult<TestFileContentResult>();
                File.WriteAllBytes(Path.Combine(TestDeploymentHelper.GetTestOutputDir(), string.Format("CurrentReport.{0}", outputFormat.ToFileExtension())), result.Content);
            }
        }

        [Then(@"ist das Ergebnis das gleiche wie in der Referenz Auswertung '(.*)'")]
        public void DannIstDasErgebnisDasGleicheWieInDerReferenzAuswertung(string reportName)
        {
            var outPutDir = TestDeploymentHelper.GetTestOutputDir();

            var referenceTiffFilePath = Directory.GetFiles(Path.Combine(outPutDir, "RefrenzAuswertungen"), string.Format("{0}.tiff", reportName), SearchOption.AllDirectories).FirstOrDefault();

            var actuallNamedPdfFilePath = GetActuallNamedFilePath(OutputFormat.Pdf, reportName);
            GetActuallNamedFilePath(OutputFormat.Excel, reportName);
            GetActuallNamedFilePath(OutputFormat.Xml, reportName);

            string actualNamedTiffFilePath = GetActuallNamedFilePath(OutputFormat.Image, reportName);

            Assert.IsNotNull(referenceTiffFilePath, string.Format("RefrenzAuswertungen {0} not found!", reportName));

            var differentReportResultsDirectory = Path.Combine(outPutDir, "DifferentReportResults");
            if (!Directory.Exists(differentReportResultsDirectory))
                Directory.CreateDirectory(differentReportResultsDirectory);

            var areEqual = ImageHelpers.AreEqualByPixel(referenceTiffFilePath, actualNamedTiffFilePath);
            if (!areEqual)
            {
                File.Copy(actualNamedTiffFilePath, Path.Combine(differentReportResultsDirectory, Path.GetFileName(actualNamedTiffFilePath)), true);
                File.Copy(actuallNamedPdfFilePath, Path.Combine(differentReportResultsDirectory, Path.GetFileName(actuallNamedPdfFilePath)), true);
            }

            Assert.IsTrue(areEqual, "Reports are different. Referenceze path: {0} - actual path: {1}", referenceTiffFilePath, actualNamedTiffFilePath);
        }

        public string GetActuallNamedFilePath(OutputFormat outputFormat, string reportName)
        {
            var outPutDir = TestDeploymentHelper.GetTestOutputDir();
            string extension = outputFormat.ToFileExtension();

            var actualReportFilePath = Path.Combine(outPutDir, string.Format("CurrentReport.{0}", extension));
            string actualNamedFilePath = Path.Combine(outPutDir, string.Format("{0}.{1}", reportName, extension));
            File.Copy(actualReportFilePath, actualNamedFilePath, true);

            return actualNamedFilePath;
        }
    }

    public class MengeProBelastungskategorieRow
    {
        public string BelastungskategorieTyp { get; set; }
        public string Menge { get; set; }
    }

    public class ReportFilters
    {
        public Guid? ErfassungsPeriodId { get; set; }
        public EigentuemerTyp? EigentuemerTyp { get; set; }
    }

    public class ReportFilterRow
    {
        public string Filter { get; set; }
        public string FilterWert { get; set; }
    }
}
