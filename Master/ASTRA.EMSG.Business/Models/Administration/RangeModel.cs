using System;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Administration
{
    [Serializable]
    public class RangeModel : Model
    {
        public BenchmarkingGruppenTyp BenchmarkingGruppen { get; set; }
        public decimal? ObereExclusiveGrenzwert { get; set; }
        public decimal? UntereInclusieveGrenzwert { get; set; }
    }
}