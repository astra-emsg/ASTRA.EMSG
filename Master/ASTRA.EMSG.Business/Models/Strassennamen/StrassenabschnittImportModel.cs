using System;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Models.Common;

namespace ASTRA.EMSG.Business.Models.Strassennamen
{
    [Serializable]
    public class StrassenabschnittImportModel : StrassenabschnittModelBase, IExternalIdHolder
    {
        public int? Abschnittsnummer { get; set; }
    }
}