using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Services.Security
{
    public class TestUserIdentityProvider : IUserIdentityProvider
    {
        public string UserName { get { return TestUserInfo.IntegrationTestUserName; } }
        public bool IsAuthenticated { get { return true; } }
    }
}