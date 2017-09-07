using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.NochNichtInspizierteStrassenabschnitte
{
    public class NochNichtInspizierteStrassenabschnitteParameter : EmsgTabellarischeReportParameter
    {
        public string Strassenname { get; set; }
        public EigentuemerTyp? Eigentuemer { get; set; }
    }
}
