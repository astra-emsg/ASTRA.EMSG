using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASTRA.EMSG.Business.Services.GIS.Shape;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands
{
    public class GISExportGridCommand : GisReportGridCommand
    {
        public int? ExType { get; set; }

        public ShapeExportType ExportType
        {
            get { return (ShapeExportType) ExType.Value; }
        }

    }
}