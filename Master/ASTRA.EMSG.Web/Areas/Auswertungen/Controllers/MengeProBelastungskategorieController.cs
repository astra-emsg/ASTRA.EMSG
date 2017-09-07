using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.MengeProBelastungskategorie;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Summarisch, NetzErfassungsmodus.Tabellarisch, NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W1_7)]
    public class MengeProBelastungskategorieController : TabellarischReportControllerBase<MengeProBelastungskategorieParameter, MengeProBelastungskategoriePo, MengeProBelastungskategorieGridCommand>
    {
        public MengeProBelastungskategorieController(ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies) 
            : base(tabellarischeReportControllerBaseDependencies)
        {
        }

        protected override MengeProBelastungskategorieParameter GetReportParameter(MengeProBelastungskategorieGridCommand reportGridCommand)
        {
            return new MengeProBelastungskategorieParameter { Eigentuemer = reportGridCommand.Eigentuemer };
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
    }
}
