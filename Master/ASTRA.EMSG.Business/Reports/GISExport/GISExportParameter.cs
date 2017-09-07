using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.GIS.Shape;

namespace ASTRA.EMSG.Business.Reports.GISExport
{
    public class GISExportParameter : EmsgGisReportParameter
    {
        public ErfassungsPeriod Periode { get; set; }
        public ShapeExportType Type { get; set; }
    }
}
