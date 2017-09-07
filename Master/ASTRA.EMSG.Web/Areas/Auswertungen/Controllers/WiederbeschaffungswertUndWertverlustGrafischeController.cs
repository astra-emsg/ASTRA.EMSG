using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustGrafische;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Summarisch, NetzErfassungsmodus.Tabellarisch, NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W2_3)]
    public class WiederbeschaffungswertUndWertverlustGrafischeController : GrafischeReportControllerBase<WiederbeschaffungswertUndWertverlustGrafischeParameter, WiederbeschaffungswertUndWertverlustGrafischePo>
    {
        public WiederbeschaffungswertUndWertverlustGrafischeController(IGrafischeReportControllerBaseDependencies grafischeReportControllerBaseDependencies)
            : base(grafischeReportControllerBaseDependencies)
        {
        }
    }
}
