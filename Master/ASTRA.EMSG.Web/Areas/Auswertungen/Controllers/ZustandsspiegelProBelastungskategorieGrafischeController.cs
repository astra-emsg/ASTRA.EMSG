using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.ZustandsspiegelProBelastungskategorieGrafische;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Tabellarisch, NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W3_2)]
    public class ZustandsspiegelProBelastungskategorieGrafischeController : GrafischeReportControllerBase<ZustandsspiegelProBelastungskategorieGrafischeParameter, ZustandsspiegelProBelastungskategorieGrafischePo>
    {
        public ZustandsspiegelProBelastungskategorieGrafischeController(IGrafischeReportControllerBaseDependencies grafischeReportControllerBaseDependencies)
            : base(grafischeReportControllerBaseDependencies)
        {
        }
    }
}
