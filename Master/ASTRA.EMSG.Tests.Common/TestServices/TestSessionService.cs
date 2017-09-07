using System.Collections.Generic;
using ASTRA.EMSG.Web.Infrastructure;

namespace ASTRA.EMSG.Tests.Common.TestServices
{
    public class TestSessionService : SessionService
    {
        private readonly Dictionary<string, object> sessionDictionary = new Dictionary<string, object>();

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