using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProJahrGrafische
{
    public class WiederbeschaffungswertUndWertverlustProJahrGrafischeParameter : EmsgReportParameter, IEigentuemerFilter, ICurrentMandantFilter
    {
        public Guid JahrIdVon { get; set; }
        public Guid JahrIdBis { get; set; }

        public EigentuemerTyp? Eigentuemer { get; set; }
    }
}
