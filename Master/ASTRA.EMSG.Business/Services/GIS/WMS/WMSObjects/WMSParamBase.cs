using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Business.Services.GIS.WMS.WMSObjects
{
    public class WMSParamBase
    {
        public string LAYERS { get; set; }
        public string BBOX { get; set; }
        public string FORMAT { get; set; }
        public string REQUEST { get; set; }
    }
}
