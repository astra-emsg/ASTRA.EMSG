using System;

namespace ASTRA.EMSG.Business.Models.Common
{
    [Serializable]
    public class SplitStrassenabschnittBaseModel
    {
        public Guid StrassenabschnittId { get; set; }
        public int Count { get; set; }
    }
}