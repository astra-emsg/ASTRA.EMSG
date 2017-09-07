using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProBelastungskategorie;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Summarisch, NetzErfassungsmodus.Tabellarisch, NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W2_1)]
    public class WiederbeschaffungswertUndWertverlustProBelastungskategorieController : TabellarischReportControllerBase<WiederbeschaffungswertUndWertverlustProBelastungskategorieParameter, WiederbeschaffungswertUndWertverlustProBelastungskategoriePo, WiederbeschaffungswertUndWertverlustProBelastungskategorieGridCommand>
    {
        public WiederbeschaffungswertUndWertverlustProBelastungskategorieController(
            ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies
            ) : base(tabellarischeReportControllerBaseDependencies)
        {
        }

        protected override void PrepareViewBagForIndex(Guid? erfassungsPeriodId)
        {
            base.PrepareViewBagForIndex(erfassungsPeriodId);
            PrepareViewBagForPreview(erfassungsPeriodId);
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
            ViewBag.NetzErfassungsmodus = reportControllerService.GetNetzErfassungsmodus(erfassungsPeriodId);
        }

        protected override WiederbeschaffungswertUndWertverlustProBelastungskategorieParameter GetReportParameter(WiederbeschaffungswertUndWertverlustProBelastungskategorieGridCommand reportGridCommand)
        {
            return new WiederbeschaffungswertUndWertverlustProBelastungskategorieParameter
                       {
                           Eigentuemer = reportGridCommand.Eigentuemer
                       };
        }
    }
}
