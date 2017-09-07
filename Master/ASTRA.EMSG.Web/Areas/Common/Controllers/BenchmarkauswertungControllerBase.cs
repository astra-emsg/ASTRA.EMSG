using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure;
using Resources;

namespace ASTRA.EMSG.Web.Areas.Common.Controllers
{
    public abstract class BenchmarkauswertungControllerBase<TParamter, TPo> : GrafischeReportControllerBase<TParamter, TPo> where TParamter : EmsgReportParameter, new() where TPo : new()
    {
        private readonly IErfassungsPeriodService erfassungsPeriodService;
        private readonly IKenngroessenFruehererJahreOverviewService kenngroessenFruehererJahreOverviewService;
        private readonly ILocalizationService localizationService;

        public BenchmarkauswertungControllerBase(
            IGrafischeReportControllerBaseDependencies grafischeReportControllerBaseDependencies, 
            IErfassungsPeriodService erfassungsPeriodService, 
            IKenngroessenFruehererJahreOverviewService kenngroessenFruehererJahreOverviewService,
            ILocalizationService localizationService) 
            : base(grafischeReportControllerBaseDependencies)
        {
            this.erfassungsPeriodService = erfassungsPeriodService;
            this.kenngroessenFruehererJahreOverviewService = kenngroessenFruehererJahreOverviewService;
            this.localizationService = localizationService;
        }

        protected override TParamter PrepareViewBagForIndex()
        {
            PrepareViewBag();
            return new TParamter();
        }

        private void PrepareViewBag()
        {
            var erfassugnsPeriodList = erfassungsPeriodService.GetClosedErfassungsPeriodModels()
                .OrderByDescending(ep => ep.Erfassungsjahr)
                .ToDropDownItemList(
                    ep =>
                    string.Format("{0} ({1})", ep.Erfassungsjahr.Year,
                                  localizationService.GetLocalizedEnum<NetzErfassungsmodus>(ep.NetzErfassungsmodus)), ep => ep.Id);
            var kengrossen = kenngroessenFruehererJahreOverviewService.GetCurrentModels().OrderByDescending(s => s.Jahr).
                ToDropDownItemList(
                    kg => string.Format("{0} ({1})", kg.Jahr, TextLocalization.FruehererJahre),
                    kg => kg.Id);


            ViewBag.ErfassugnsPeriodList = erfassugnsPeriodList.Concat(kengrossen).ToList();
        }

        public ActionResult GetBenchmarkauswertungPreview(TParamter parameter)
        {
            Session["BenchmarkauswertungParameter"] = parameter;
            if (ModelState.IsValid)
            {
                ViewBag.IsValid = true;
            }
            PrepareViewBag();
            return PartialView("Filter", parameter);
        }

        public override ActionResult GetReportImagePreview(TParamter parameter)
        {
            return base.GetReportImagePreview((TParamter)Session["BenchmarkauswertungParameter"]);
        }

        public override ActionResult GetReport(TParamter parameter)
        {
            var benchmarkauswertungParameter = (TParamter)Session["BenchmarkauswertungParameter"];
            benchmarkauswertungParameter.OutputFormat = parameter.OutputFormat;
            benchmarkauswertungParameter.IsPreview = false;
            return base.GetReport(benchmarkauswertungParameter);
        }

        public override ContentResult GenerateReport(TParamter parameter)
        {
            var benchmarkauswertungParameter = (TParamter)Session["BenchmarkauswertungParameter"];
            benchmarkauswertungParameter.OutputFormat = parameter.OutputFormat;
            benchmarkauswertungParameter.IsPreview = false; 
            return base.GenerateReport(benchmarkauswertungParameter);
        }
    }
}