using System;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.Common
{
    [TableShortName("SCL")]
    public class ScriptLog : Entity
    {
        public virtual string ScriptName { get; set; }
        public virtual int Version { get; set; }
        public virtual DateTime ExecutionDateTime { get; set; }
    }
}
