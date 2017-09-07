using System;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.Administration
{
    [Serializable]
    public class BenchmarkingGruppenConfigurationModel : Model
    {
        public string EigenschaftTyp { get; set; }
        public decimal Grenzwert { get; set; }
    }
}