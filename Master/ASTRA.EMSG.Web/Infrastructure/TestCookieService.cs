using System.Collections.Generic;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class TestCookieService : CookieService
    {
        private Dictionary<string, string> sessionDictionary = new Dictionary<string, string>();

        public void ResetCookies()
        {
            sessionDictionary = new Dictionary<string, string>();
        }

        public override string this[string key]
        {
            get
            {
                string str;
                sessionDictionary.TryGetValue(key, out str);
                return str;
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