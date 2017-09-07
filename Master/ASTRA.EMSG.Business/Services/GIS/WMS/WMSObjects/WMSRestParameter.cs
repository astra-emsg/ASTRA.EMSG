using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Services.Security;

namespace ASTRA.EMSG.Business.Services.GIS.WMS.WMSObjects
{
    public class WMSRestParameter : WMSParamBase
    {
        public string F { get; set; }
        public string TRANSPARENT { get; set; }
        public string SIZE { get; set; }
        public string BBOXSR { get; set; }
        public string IMAGESR { get; set; }
        public string LAYERDEFS { get; set; }
        public string DPI { get; set; }

        public WMSRestParameter(
         string layers,
         string bbox,
         string size,
         string transparent="true",
         string format="png",
         string bboxsr="EPSG:21781",
         string imagesr= "EPSG:21781",
         string layerdefs="",
         string f="image",
         string dpi = "96")
        { 
            this.F=f;
            LAYERS = layers;
            this.TRANSPARENT=transparent;
            FORMAT=format;
            BBOX=bbox;
            this.SIZE=size;
            this.BBOXSR=bboxsr;
            this.IMAGESR=imagesr;
            this.LAYERDEFS = layerdefs;
            this.DPI = dpi;
        }
    }
}
