using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Common.Master.HttpRequest
{
    public class HttpRequestObject
    {
        public string requestURI { get; set; }
        public string contentType { get; set; }
        public string referer { get; set; }
        public string userName { get; set; }
        public string password { get; set; }

        public HttpRequestObject() { }

        public HttpRequestObject(string domain, string query, string contentType = "", string referer = "", string userName = "", string password = "")
        {

            this.requestURI = domain+query;
            this.contentType = contentType;
            this.referer = referer;
            this.userName = userName;
            this.password = password;
        }
    }
}
