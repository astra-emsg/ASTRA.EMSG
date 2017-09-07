using System;
using System.Collections.Generic;
using System.IO;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;

namespace ASTRA.EMSG.IntegrationTests.Common
{
    public static class BrowserDriverExtensions
    {
        public static void GeneratReports(this BrowserDriver browser, EmsgReportParameter emsgReportParameter, Action<EmsgReportParameter> generateReport)
        {
            GenerateReportForAllOutputFormat(browser, outputFormat => { emsgReportParameter.IsPreview = false; emsgReportParameter.OutputFormat = outputFormat; generateReport(emsgReportParameter); });
        }

        public static void GeneratReports<T>(this BrowserDriver browser, T emsgReportParameter, Action<T> generateReport) where T : ReportGridCommand
        {
            GenerateReportForAllOutputFormat(browser, outputFormat => { emsgReportParameter.OutputFormat = outputFormat; generateReport(emsgReportParameter); });
        }

        public static void GenerateReportForAllOutputFormat(this BrowserDriver browser, Action<OutputFormat> action)
        {
            var outputFormatList = new List<OutputFormat> { OutputFormat.Xml, OutputFormat.Excel, OutputFormat.Pdf, OutputFormat.Image };

            foreach (var outputFormat in outputFormatList)
            {
                action(outputFormat);

                var result = browser.GetRequestResult<TestFileContentResult>();
                File.WriteAllBytes(Path.Combine(TestDeploymentHelper.GetTestOutputDir(), string.Format("CurrentReport.{0}", outputFormat.ToFileExtension())), result.Content);
            }
        }
    }
}