using System.Collections.Generic;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class TestHttpRequestService : HttpRequestService
    {
        private Dictionary<string, object> sessionDictionary = new Dictionary<string, object>();

        public void StartNewRequest()
        {
            sessionDictionary = new Dictionary<string, object>();
        }

        public override object this[string key]
        {
            get
            {
                object obj;
                sessionDictionary.TryGetValue(key, out obj);
                return obj;
            }
            set
            {
                if (!sessionDictionary.ContainsKey(key))
                    sessionDictionary.Add(key, value);
                sessionDictionary[key] = value;
            }
        }
    }
}