using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustGrafische
{
    public class WiederbeschaffungswertUndWertverlustGrafischeParameter : EmsgReportParameter
    {
        public EigentuemerTyp? Eigentuemer { get; set; }
    }
}