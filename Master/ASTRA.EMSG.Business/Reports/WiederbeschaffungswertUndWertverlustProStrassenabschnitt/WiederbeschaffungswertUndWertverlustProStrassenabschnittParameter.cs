using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProStrassenabschnitt
{
    public class WiederbeschaffungswertUndWertverlustProStrassenabschnittParameter : EmsgTabellarischeReportParameter, IOrtsbezeichnungFilter, IStrassennameFilter, IEigentuemerFilter
    {
        public string Strassenname { get; set; }
        public string Ortsbezeichnung { get; set; }
        public EigentuemerTyp? Eigentuemer { get; set; }
    }
}