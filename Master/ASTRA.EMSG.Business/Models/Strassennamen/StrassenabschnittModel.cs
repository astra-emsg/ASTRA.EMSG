using System;
using ASTRA.EMSG.Business.Models.Common;

namespace ASTRA.EMSG.Business.Models.Strassennamen
{
    [Serializable]
    public class StrassenabschnittModel : StrassenabschnittModelBase
    {
        public int? Abschnittsnummer { get; set; }
    }
}