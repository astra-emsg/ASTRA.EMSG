using System.Collections.Generic;

namespace ASTRA.EMSG.Business.Services.Identity
{
    public interface IIdentityService
    {
        bool IsUserExists(string username);

        List<UserRole> GetRole(string username, string applicationname);
        List<UserRole> GetRoleMandator(string mandatorname, string username, string applicationname);
        List<IdentityMandatorShort> GetMandatorShort(string username, string applicationname);
    }
}