using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using BruTile;
using System.Globalization;
using BruTile.Web;

namespace ASTRA.EMSG.Common.EMSGBruTile
{
    public class TileFetcher
    {
        private string userName;
        private string password;
        public TileFetcher(string userName = null, string password = null)
        {
            this.userName = userName;
            this.password = password;
        }
        public Byte[] fetchTile(Uri uri)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Referer = "http://astra.admin.ch";

            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
            {
                webRequest.Credentials = new NetworkCredential(userName, password);
            }

            WebResponse webResponse = webRequest.GetResponse();
            if (webResponse == null) throw (new WebException("An error occurred while fetching tile", null));
            if (webResponse.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {
                using (Stream responseStream = webResponse.GetResponseStream())
                {
                    return Utilities.ReadFully(responseStream);
                }
            }
            string message = String.Format(
                CultureInfo.InvariantCulture,
                "Failed to retrieve tile from this uri:\n{0}\n.An image was expected but the received type was '{1}'.",
                uri,
                webResponse.ContentType
                );

            if (webResponse.ContentType.StartsWith("text", StringComparison.OrdinalIgnoreCase))
            {
                HttpWebResponse httpResponse = webResponse as HttpWebResponse;
                if (httpResponse != null && httpResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    message += "\nNo content Returned";
                }
                else
                {
                    string content = String.Empty;
                    using (Stream stream = webResponse.GetResponseStream())
                    {
                        using (var streamReader = new StreamReader(stream, true))
                        {
                            content = streamReader.ReadToEnd();
                        }
                        message += String.Format(CultureInfo.InvariantCulture,
                          "\nThis was returned:\n{0}", content);
                    }
                }
            }

            throw (new WebResponseFormatException(message, null));
        }
    }
}
