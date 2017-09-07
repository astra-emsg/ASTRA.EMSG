using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ASTRA.EMSG.Common.Master.HttpRequest
{
    public class HttpResponseObject
    {
        public Stream responseStream { get; set; }
        public string contentType { get; set; }
    }
}
