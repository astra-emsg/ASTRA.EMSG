using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Infrastructure.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AccessRightsAttribute : FilterAttribute
    {
        public List<Rolle> Rollen { get; set; }

        public AccessRightsAttribute(Rolle rolle, params Rolle[] rollen)
        {
            Rollen = new List<Rolle> {rolle};
            Rollen.AddRange(rollen);
            Order = 1;
        }
    }
}