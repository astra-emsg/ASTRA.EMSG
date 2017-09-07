using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.ZustandsspiegelProJahrGrafische
{
    public class ZustandsspiegelProJahrGrafischeParameter : EmsgReportParameter, IEigentuemerFilter, ICurrentMandantFilter
    {
        public Guid ErfassungsPeriodIdVon { get; set; }
        public Guid ErfassungsPeriodIdBis { get; set; }

        public EigentuemerTyp? Eigentuemer { get; set; }
    }
}