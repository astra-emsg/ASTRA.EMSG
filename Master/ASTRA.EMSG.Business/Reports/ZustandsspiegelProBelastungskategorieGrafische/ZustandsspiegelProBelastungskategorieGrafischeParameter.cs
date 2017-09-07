using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.ZustandsspiegelProBelastungskategorieGrafische
{
    public class ZustandsspiegelProBelastungskategorieGrafischeParameter : EmsgReportParameter, IEigentuemerFilter, IStrassennameFilter, IErfassungsPeriodFilter, ICurrentMandantFilter
    {
        public string Strassenname { get; set; }

        public EigentuemerTyp? Eigentuemer { get; set; }
    }
}