using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Infrastructure.Security
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AllowedModesAttribute : FilterAttribute
    {
        public List<NetzErfassungsmodus> NetzErfassungsmoduses { get; set; }
        
        public AllowedModesAttribute(NetzErfassungsmodus netzErfassungsmodus, params NetzErfassungsmodus[] others)
        {
            NetzErfassungsmoduses = new List<NetzErfassungsmodus>{netzErfassungsmodus};
            NetzErfassungsmoduses.AddRange(others);
            Order = 2;
        }
    }
}