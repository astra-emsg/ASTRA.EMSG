using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ASTRA.EMSG.Common.Enums;
using Microsoft.AspNet.Identity;
using ASTRA.EMSG.Business.Models.Identity;
using ASTRA.EMSG.Business.Services.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;

namespace ASTRA.EMSG.Business.Services.Security
{
    public class IdentityRoleService : IIdentityCacheService
    {
        public const string ApplicationName = "EMSG";
        private string connectionString;

        public IdentityRoleService(IServerConfigurationProvider serverConfigurationProvider)
        {
            connectionString = serverConfigurationProvider.ConnectionString;
        }

        public List<IdentityMandatorShort> GetMandatorShort(string username)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext(connectionString)));

            var user = userManager.FindByName(username);
            if (user == null)
                return new List<IdentityMandatorShort>();

            var result = user.MandantRoles.Select(e => e.MandantName)
                .Distinct()
                .OrderBy(e => e)
                .Select(e => new IdentityMandatorShort
                {
                    Applicationname = ApplicationName,
                    Mandatorname = e
                })
                .ToList();
            return result;
        }

        public List<UserRole> GetRole(string username)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext(connectionString)));

            var user = userManager.FindByName(username);
            if (user == null)
                return new List<UserRole>();

            var result = userManager.GetRoles(user.Id)
                .Select(e => new UserRole
                {
                    Rolle = (Rolle)Enum.Parse(typeof(Rolle), e)
                })
                .Union(user.MandantRoles
                    .Select(e => new UserRole
                    {
                        MandatorName = e.MandantName,
                        Rolle = (Rolle)Enum.Parse(typeof(Rolle), e.RoleName)
                    })
                )
                .ToList();

            return result;
        }

        public List<UserRole> GetRoleMandator(string mandantname, string username)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext(connectionString)));

            var user = userManager.FindByName(username);
            if (user == null)
                return new List<UserRole>();

            var result = user.MandantRoles
                .Where(e => e.MandantName == mandantname)
                .Select(e => new UserRole
                {
                    MandatorName = e.MandantName,
                    Rolle = (Rolle)Enum.Parse(typeof(Rolle), e.RoleName)
                })
                .ToList();

            return result;
        }

        public bool IsUserExists(string username)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext(connectionString)));

            bool result = userManager.FindByName(username) != null;
            return result;
        }
    }
}
