using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA.EMSG.Business.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            MandantRoles = new List<MandantRole>();
        }
        public virtual ICollection<MandantRole> MandantRoles { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
