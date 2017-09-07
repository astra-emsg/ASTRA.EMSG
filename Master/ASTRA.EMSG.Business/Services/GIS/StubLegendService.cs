using System.IO;
using ASTRA.EMSG.Common.Utils;
using System.Drawing;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Reporting;
using System;
using System.Web.Mvc;

namespace ASTRA.EMSG.Business.Services.GIS
{
    public class StubLegendService : ILegendService
    {
        public Stream GetLegendStream(string layer, int dpi = 90, int size = 30, string fontName = "Arial", bool fontAntiAliasing = true, string fontColor = "0x000000", string bgColor = "0xFFFFFF", int fontSize = 10, EmsgGisReportParameter reportParameter = null)
        {
            return null;
        }

        public string GetInspektionsRouteLegendImageUrl(int legendNumber, string baseUrl)
        {
            return "favicon.ico";
        }
       
        public Bitmap FormatLegendForReport(string layerList, EmsgGisReportParameter reportparams, ReportDefintion reportDefinition)
        {
            return null;
        }

        public ActionResult GetInspektionsRouteLegendImage(int LegendNumber, int dpi = 90, int size = 20, string bgColor = "0xFFFFFF")
        {
            return null;
        }
    }
}