using System;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.Common
{
    [TableShortName("ERL")]
    public class EreignisLog : Entity
    {
        public virtual string Benutzer { get; set; }

        public virtual string MandantName { get; set; }

        public virtual  DateTime Zeit { get; set; }

        public virtual EreignisTyp EreignisTyp { get; set; }

        public virtual string EreignisData { get; set; }
    }
}