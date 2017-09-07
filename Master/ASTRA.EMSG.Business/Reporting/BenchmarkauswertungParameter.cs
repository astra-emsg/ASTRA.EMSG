using System;
using System.Collections.Generic;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reporting
{
    [Serializable]
    public class BenchmarkauswertungParameter : EmsgReportParameter
    {
        public BenchmarkauswertungParameter()
        {
            BenchmarkingGruppenTypList = new List<BenchmarkingGruppenTyp>();
        }

        public Guid JahrId { get; set; }
        public List<BenchmarkingGruppenTyp> BenchmarkingGruppenTypList { get; set; }
    }
}