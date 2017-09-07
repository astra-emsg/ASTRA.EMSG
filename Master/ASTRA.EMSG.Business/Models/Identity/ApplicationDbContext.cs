using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA.EMSG.Business.Models.Identity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<MandantRole> MandantRoles { get; set; }

        public ApplicationDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

    }
}
