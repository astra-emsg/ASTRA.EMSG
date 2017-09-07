using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.EineListeVonMassnahmenGegliedertNachTeilsystemen
{
    public class EineListeVonMassnahmenGegliedertNachTeilsystemenParameter : EmsgGisReportParameter, IProjektnameFilter, IStatusFilter, ICurrentMandantFilter, IDringlichkeitFilter, ITeilsystemFilter
    {
        public string Projektname { get; set; }
        public StatusTyp? Status { get; set; }
        public DringlichkeitTyp? Dringlichkeit { get; set; }
        public TeilsystemTyp? Teilsystem { get; set; }
    }
}
