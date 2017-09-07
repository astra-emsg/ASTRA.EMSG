using System.Web;

namespace ASTRA.EMSG.Business.Services.Security
{
    public class UserIdentityProvider : IUserIdentityProvider
    {
        public string UserName
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }

        }
    }
}