using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Business.Services.GIS.WMS.WMSObjects
{
    public class WMSParameter : WMSParamBase
    {
        public string CRS { get; set; }
        public string SRS { get; set; }
        public string WIDTH { get; set; }
        public string HEIGHT { get; set; }
        public string VERSION { get; set; }
        public string SERVICE { get; set; }

        public WMSParameter(string LAYERS,
            string BBOX,
            string FORMAT = "png32",
            string SRS = "EPSG:21781",
            string CRS = "EPSG:21781",
            string WIDTH = "256",
            string HEIGHT = "256",
            string REQUEST = "GetMap",
            string VERSION = "1.3.0",
            string SERVICE = "WMS")
        {
            this.LAYERS = LAYERS;
            this.BBOX = BBOX;
            this.FORMAT = FORMAT;
            this.CRS = CRS;
            this.SRS = SRS;
            this.WIDTH = WIDTH;
            this.HEIGHT = HEIGHT;
            this.REQUEST = REQUEST;
            this.VERSION = VERSION;
            this.SERVICE = SERVICE;
        }
    }
}
