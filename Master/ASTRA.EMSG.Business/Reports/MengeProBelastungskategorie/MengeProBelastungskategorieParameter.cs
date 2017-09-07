using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.MengeProBelastungskategorie
{
    public class MengeProBelastungskategorieParameter : EmsgTabellarischeReportParameter
    {
        public EigentuemerTyp? Eigentuemer { get; set; }
    }
}