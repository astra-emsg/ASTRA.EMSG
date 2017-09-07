using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.MengeProBelastungskategorieGrafische;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Summarisch, NetzErfassungsmodus.Tabellarisch, NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W1_1)]
    public class MengeProBelastungskategorieGrafischeController : GrafischeReportControllerBase<MengeProBelastungskategorieGrafischeParameter, MengeProBelastungskategorieGrafischePo>
    {
        public MengeProBelastungskategorieGrafischeController(IGrafischeReportControllerBaseDependencies grafischeReportControllerBaseDependencies)
            : base(grafischeReportControllerBaseDependencies)
        {
        }
    }
}
