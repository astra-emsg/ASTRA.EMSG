using System;
using System.Collections;
using System.Web;
using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class HttpRequestService : IHttpRequestService
    {
        public const string LastErrorTrackIdKey = "LastErrorTrackId";
        public const string LastExceptionKey = "LastException";

        public virtual object this[string key]
        {
            get
            {
                if (HttpRequestItems.Contains(key))
                    return HttpRequestItems[key];
                return null;
            }
            set
            {
                if(!HttpRequestItems.Contains(key))
                    HttpRequestItems.Add(key, value);
                else
                    HttpRequestItems[key] = value;
            }
        }

        protected virtual IDictionary HttpRequestItems { get { return HttpContext.Current.Items; } }

        public Guid? LastErrorTrackId { get { return (Guid?)this[LastErrorTrackIdKey]; } set { this[LastErrorTrackIdKey] = value; } }
        public Exception LastException { get { return (Exception)this[LastExceptionKey]; } set { this[LastExceptionKey] = value; } }
    }
}