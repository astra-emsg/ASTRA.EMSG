using System.Web.Mvc;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class EmsgEmptyResult : ContentResult
    {
        public EmsgEmptyResult()
        {
            ContentType = "text/plain";
        }
    }
}