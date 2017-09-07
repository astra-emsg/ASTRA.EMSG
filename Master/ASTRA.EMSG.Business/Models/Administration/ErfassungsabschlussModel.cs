using System;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.Administration
{
    [Serializable]
    public class ErfassungsabschlussModel : IModel
    {
        public DateTime AbschlussDate { get; set; }
    }
}