using System;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Administration
{
    [Serializable]
    public class ErfassungsPeriodModel : Model
    {
        public string Name { get; set; }
        public bool IsClosed { get; set; }
        public NetzErfassungsmodus NetzErfassungsmodus { get; set; }
        public DateTime Erfassungsjahr { get; set; }

        public Guid Mandant { get; set; }
    }
}