using System;
using ASTRA.EMSG.Business.Services.GIS.WMS;
using ASTRA.EMSG.Business.Services.GIS.WMS.WMSObjects;
using System.Collections.Generic;

namespace ASTRA.EMSG.Business.Reporting
{
    [Serializable]
    public class EmsgGisReportParameter : EmsgTabellarischeReportParameter
    {
        public string BoundingBox { get; set; }
        public string BoundingBoxFilter { get; set; }
        public string MapSize { get; set; }
        public string BackgroundLayers { get; set; }
        public string Layers { get; set; }
        public string LayersAV { get; set; }
        public string LayersAVBackground { get; set; }
        public string LayerDefs { get; set; }
        public string ScaleWidth { get; set; }
        public string ScaleText { get; set; }

        public List<FilterListPo> Filterparams { get; set; }
        public string ReportImagePath { get; set; }
    }
}