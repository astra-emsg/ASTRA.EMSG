using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Master.Logging;
using System.Net;
using System.IO;
using System.Web;

namespace ASTRA.EMSG.Common.Master.HttpRequest
{
    public class SendHttpRequest
    {
        public static HttpResponseObject WmsRequest(HttpRequestObject requestObject)
        {
            Loggers.WMSRedirectLogger.Debug("wms request url: " + requestObject.requestURI);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(requestObject.requestURI);
            myReq.Timeout = 200000;
            NetworkCredential credentials;
            if (TryGetCreadentials(requestObject.requestURI, out credentials)) {
                myReq.Credentials = credentials;
            }

            if (!String.IsNullOrEmpty(requestObject.referer))
            {
                myReq.Referer = requestObject.referer;
            }
            if (!String.IsNullOrEmpty(requestObject.userName) && !String.IsNullOrEmpty(requestObject.password))
            {
                myReq.Credentials = new NetworkCredential(requestObject.userName, requestObject.password);
            }
            myReq.ServicePoint.ConnectionLimit = 1000;

            return SendRequest(myReq, contentType: requestObject.contentType);
        }

        private static bool TryGetCreadentials(string requestURI, out NetworkCredential credentials)
        {
            if (requestURI.Contains("@"))
            {
                string[] credAndUrl = requestURI.Split('@');
                if (credAndUrl.Count() == 2)
                {
                    string credsString = credAndUrl[0];
                    credsString = credsString.Replace("http://", string.Empty);
                    credsString = credsString.Replace("https://", string.Empty);
                    string[] creds = credsString.Split(':');
                    if (creds.Count() == 2)
                    {
                        credentials = new NetworkCredential(WebUtility.UrlDecode(creds[0]), WebUtility.UrlDecode(creds[1]));
                        return true;
                    }
                }
            }
            credentials = null;
            return false;
        }

        private static HttpResponseObject SendRequest(HttpWebRequest myReq, string contentType = "")
        {
            HttpWebResponse myResp = null;
            try
            {
                myResp = (HttpWebResponse)myReq.GetResponse();
                WebHeaderCollection headers = myResp.Headers;
                if (HttpStatusCode.OK != myResp.StatusCode)
                    Loggers.WMSRedirectLogger.Error(string.Format("Call WmsRequest failure: {0}, {1}, requested url: {2}", myResp.StatusCode, myResp.StatusDescription, myReq.RequestUri));

                string cTypeTemp = myResp.ContentType;
                if (String.IsNullOrEmpty(cTypeTemp))
                    cTypeTemp = contentType;

                return new HttpResponseObject
                {
                    responseStream = myResp.GetResponseStream(),
                    contentType = cTypeTemp
                };
            }
            catch (System.Net.WebException ex)
            {
                Loggers.WMSRedirectLogger.Error(string.Format("Call WmsRequest exception: {0}, {1}, requested url: {2}", ex.Message, ex.StackTrace.ToString(), myReq.RequestUri));
                return null;
            }
            catch (Exception ex)
            {
                Loggers.WMSRedirectLogger.Error(string.Format("Call WmsRequest exception: {0}, {1}, requested url: {2}", ex.Message, ex.StackTrace.ToString(), myReq.RequestUri));
                return null;
            }
        }
    }
}
