using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.StrassenabschnitteListeOhneInspektionsroute
{
    public class StrassenabschnitteListeOhneInspektionsrouteParameter : EmsgTabellarischeReportParameter, ICurrentMandantFilter, IErfassungsPeriodFilter, IEigentuemerFilter, IStrassennameFilter
    {
        public string Strassenname { get; set; }
        public EigentuemerTyp? Eigentuemer { get; set; }
    }
}