using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ASTRA.EMSG.Common.Master.Logging;
using ASTRA.EMSG.Common.Master.Utils;
using Microsoft.Reporting.WebForms;

namespace ASTRA.EMSG.Business.Infrastructure.Reporting
{
    public interface IServerReportGenerator
    {
        Report GenerateReport(IPoProvider poProvider);
    }

    public class ServerReportGenerator : IServerReportGenerator
    {
        public Report GenerateReport(IPoProvider poProvider)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            LocalReport localReport = new LocalReport();

            using (Stream reportDefinitionStreams = poProvider.OpenReportDefinitionStreams())
            {
                var subReportDefinitionStreams = poProvider.OpenSubReportDefinitionStreams();

                InitializeLocalReport(localReport, poProvider, reportDefinitionStreams, subReportDefinitionStreams);

                string mimeType = null;
                string fileNameExtension = String.Empty;
                Warning[] warnings = null;
                byte[] renderedBytes = new byte[0];

                if (poProvider.HasData)
                {
                    if (poProvider.OutputFormat == OutputFormat.Xml)
                        renderedBytes = GenerateXmlOutput(poProvider, out fileNameExtension, out mimeType);
                    else
                        renderedBytes = GenerateReportingServicesOutput(poProvider, localReport, out warnings, out fileNameExtension, out mimeType);
                }

                LogWarnings(warnings);

                if (poProvider.OutputFormat == OutputFormat.Png)
                    renderedBytes = GeneratePngOutput(renderedBytes, out mimeType);

                var report = new Report(poProvider.HasData, renderedBytes, poProvider.ReportFileName, fileNameExtension, mimeType, poProvider.MaxImagePreviewPageHeight, poProvider.MaxImagePreviewPageWidth);

                foreach (var subReportStream in subReportDefinitionStreams.Values)
                    subReportStream.Dispose();

                stopwatch.Stop();
                Loggers.PeformanceLogger.DebugFormat("Generated report: {0}({1}) in {2}", poProvider.ReportFileName, poProvider.OutputFormat, stopwatch.Elapsed);
                return report;
            }
        }

        private static byte[] GeneratePngOutput(byte[] renderedBytes, out string mimeType)
        {
            Bitmap tiff = (Bitmap) Image.FromStream(new MemoryStream(renderedBytes));
            List<Bitmap> pages = ImageHelpers.GetAllPages(tiff).Select(p => (Bitmap)p).ToList();

            var mergedBitmap = new Bitmap(pages.Max(p => p.Width), pages.Sum(p => p.Height));

            int heigthShift = 0;
            foreach (var image in pages)
            {
                for (int x = 0; x < image.Width; x++)
                    for (int y = 0; y < image.Height; y++)
                        mergedBitmap.SetPixel(x, y + heigthShift, image.GetPixel(x,y));

                heigthShift += image.Height;
            }

            var pngStream = new MemoryStream();
            mergedBitmap.Save(pngStream, ImageFormat.Png);
            pngStream.Seek(0, 0);

            mimeType = "image/png";

            return pngStream.ToArray();
        }

        private static byte[] GenerateReportingServicesOutput(IPoProvider poProvider, LocalReport localReport, out Warning[] warnings, out string fileNameExtension, out string mimeType)
        {
            string[] streams;
            string encoding;
            return localReport.Render(poProvider.OutputFormat.ToFormatName(), null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
        }

        private static byte[] GenerateXmlOutput(IPoProvider poProvider, out string fileNameExtension, out string mimeType)
        {
            var xmlSerializer = new XmlSerializer(poProvider.DataSources.First().Value.GetType());
            using (var steam = new MemoryStream())
            {
                xmlSerializer.Serialize(steam, poProvider.DataSources.First().Value);
                mimeType = "text/xml";
                fileNameExtension = "xml";
                return steam.GetBuffer();
            }
        }

        private static void InitializeLocalReport(LocalReport localReport, IPoProvider poProvider, Stream reportDefinitionStream, Dictionary<string, Stream> subReportDefinitionStreams)
        {
            localReport.SubreportProcessing += (sender, args) => poProvider.SubReportProcessingEventHandler(sender, new ServerSubReportProcessingEventArgs(args));

            localReport.LoadReportDefinition(reportDefinitionStream);
            localReport.EnableExternalImages = true;

            foreach (var subReportKeyValuePair in subReportDefinitionStreams)
                localReport.LoadSubreportDefinition(subReportKeyValuePair.Key, subReportKeyValuePair.Value);

            ReportDataSource.SetDataSources(new ServerReportDataSourceCollection(localReport.DataSources), poProvider.DataSources);
            localReport.SetParameters(poProvider.ReportParameters.Select(svp => svp.ReportParameter));
        }

        private void LogWarnings(Warning[] warnings)
        {
            if (warnings == null || warnings.Length == 0)
                return;

            var reportErrors = warnings.Where(error => error.Severity == Severity.Error);
            var reportWarnings = warnings.Where(warning => warning.Severity == Severity.Warning);

            DumpToLog(reportErrors, Loggers.ReportingLogger.ErrorFormat);
            DumpToLog(reportWarnings, Loggers.ReportingLogger.WarnFormat);
        }

        private void DumpToLog(IEnumerable<Warning> warnings, Action<string, object[]> loggingAction)
        {
            if (warnings == null)
                return;

            foreach (var warning in warnings)
            {
                loggingAction("Code: {0}, ObjectType: {1}, ObjectName: {2}, Message: {3}",
                              new object[] { warning.Code, warning.ObjectType, warning.ObjectName, warning.Message });
            }
        }
    }
}