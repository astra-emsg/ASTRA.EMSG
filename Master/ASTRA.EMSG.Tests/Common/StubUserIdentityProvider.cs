using ASTRA.EMSG.Business.Services.Security;

namespace ASTRA.EMSG.Tests.Common
{
    public class StubUserIdentityProvider : IUserIdentityProvider
    {
        public const string TestUsername = "TestUser";

        public string UserName { get { return TestUsername; } }
        public bool IsAuthenticated { get { return true; } }
    }
}