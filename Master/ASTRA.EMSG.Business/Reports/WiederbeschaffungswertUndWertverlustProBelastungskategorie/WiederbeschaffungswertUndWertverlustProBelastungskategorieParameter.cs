using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProBelastungskategorie
{
    public class WiederbeschaffungswertUndWertverlustProBelastungskategorieParameter : EmsgTabellarischeReportParameter, ICurrentMandantFilter, IErfassungsPeriodFilter, IEigentuemerFilter
    {
        public EigentuemerTyp? Eigentuemer { get; set; }
    }
}