using System;
using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.IntegrationTests.Support.TestServices
{
    public class TestTimeService : ITimeService
    {
        private TestTimeService() { }
        private static TestTimeService instance;
        public static TestTimeService Instance { get { return instance ?? (instance = new TestTimeService()); } }

        public DateTime Now { get { return DateTime.Now + shift; } }

        TimeSpan shift = TimeSpan.Zero;
        public void SetTime(DateTime newTime) { shift = newTime - DateTime.Now; }
        public void ResetTime() { shift = TimeSpan.Zero; }
    }
}
