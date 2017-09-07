using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.Utils;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using ASTRA.EMSG.Tests.Common.Utils;
using NHibernate.Linq;
using NUnit.Framework;

namespace ASTRA.EMSG.IntegrationTests.Common
{
    public abstract class ReportTestBase : IntegrationTestBase
    {
        private ErfassungsPeriod currentErfassungsPeriod;
        private ErfassungsPeriod closedErfassungPeriod;
        private ErfassungsPeriod otherErfassungPeriod;

        protected List<TPresentationObject> GetPos<TPresentationObject>()
        {
            var xmlSerializer = new XmlSerializer(typeof(TPresentationObject[]));

            IEnumerable<TPresentationObject> pos;
            string poXmlPath = Path.Combine(TestDeploymentHelper.GetTestOutputDir(), string.Format("CurrentReport.xml"));
            using (var sr = new StreamReader(poXmlPath))
            {
                pos = (IEnumerable<TPresentationObject>)xmlSerializer.Deserialize(sr);
            }

            return pos.ToList();
        }

        public void AssertReportToReferenzReport(string referenzeReportName)
        {
            var outPutDir = TestDeploymentHelper.GetTestOutputDir();

            var referenceTiffFilePath = Directory.GetFiles(Path.Combine(outPutDir, "RefrenzAuswertungen"), string.Format("{0}.tiff", referenzeReportName), SearchOption.AllDirectories).FirstOrDefault();

            var actuallNamedPdfFilePath = GetActualNamedFilePath(OutputFormat.Pdf, referenzeReportName);
            GetActualNamedFilePath(OutputFormat.Excel, referenzeReportName);
            GetActualNamedFilePath(OutputFormat.Xml, referenzeReportName);

            string actualNamedTiffFilePath = GetActualNamedFilePath(OutputFormat.Image, referenzeReportName);

            Assert.IsNotNull(referenceTiffFilePath, string.Format("RefrenzAuswertungen {0} not found!", referenzeReportName));

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

        private string GetActualNamedFilePath(OutputFormat outputFormat, string reportName)
        {
            var outPutDir = TestDeploymentHelper.GetTestOutputDir();
            string extension = outputFormat.ToFileExtension();

            var actualReportFilePath = Path.Combine(outPutDir, string.Format("CurrentReport.{0}", extension));
            string actualNamedFilePath = Path.Combine(outPutDir, string.Format("{0}.{1}", reportName, extension));
            File.Copy(actualReportFilePath, actualNamedFilePath, true);

            return actualNamedFilePath;
        }
        
        public ErfassungsPeriod GetClosedErfassungPeriod(NHibernateTestScope scope)
        {
            return LookupPeriod(scope, closedErfassungPeriod);
        }

        public ErfassungsPeriod GetCurrentErfassungsPeriod(NHibernateTestScope scope)
        {
            return LookupPeriod(scope, currentErfassungsPeriod);
        }

        public ErfassungsPeriod GetOtherErfassungPeriod(NHibernateTestScope scope)
        {
            return LookupPeriod(scope, otherErfassungPeriod); ;
        }

        private static ErfassungsPeriod LookupPeriod(NHibernateTestScope scope, ErfassungsPeriod periodToLookup)
        {
            return scope.Session.Query<ErfassungsPeriod>().Single(ep => ep.Id == periodToLookup.Id);
        }

        public Guid GetClosedErfassungPeriodId()
        {
            return closedErfassungPeriod.Id;
        }

        public Guid GetCurrentErfassungsPeriodId()
        {
            return currentErfassungsPeriod.Id;
        }

        public Guid GetOtherErfassungPeriodId()
        {
            return otherErfassungPeriod.Id;
        }

        protected override void DbInit()
        {
            using (var scope = new NHibernateTestScope())
            {
                var mandant = DbHandlerUtils.CreateMandant(scope.Session, TestMandantName, "0", Erfassungmodus);
                
                closedErfassungPeriod = DbHandlerUtils.CreateErfassungsPeriod(scope.Session, mandant, Erfassungmodus);
                closedErfassungPeriod.IsClosed = true;
                closedErfassungPeriod.Erfassungsjahr = new DateTime(2010, 1, 1);

                DbHandlerUtils.CreateMandant(scope.Session, OtherTestMandantName, "0", Erfassungmodus);

                DbHandlerUtils.CreateTestUser(scope.Session, TestUserName, new[] { mandant }, new List<Rolle> { Rolle.DataManager, Rolle.DataReader, Rolle.Benutzeradministrator, Rolle.Benchmarkteilnehmer });
            }

            using (var scope = new NHibernateTestScope())
            {
                currentErfassungsPeriod = scope.Session.Query<ErfassungsPeriod>().Single(m => !m.IsClosed && m.Mandant.MandantName == TestMandantName);
                currentErfassungsPeriod.Erfassungsjahr = new DateTime(2012, 1, 1);
                otherErfassungPeriod = scope.Session.Query<ErfassungsPeriod>().Single(m => !m.IsClosed && m.Mandant.MandantName == OtherTestMandantName);
                otherErfassungPeriod.Erfassungsjahr = new DateTime(2012, 1, 1);
            }

            Init();
        }

        protected virtual void Init() { }

        protected abstract override NetzErfassungsmodus Erfassungmodus { get;  }
    }
}
