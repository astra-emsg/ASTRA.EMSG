using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ASTRA.EMSG.Business.Models.Identity
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class AllowAnonymousAttribute : Attribute { }
}
