using System;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.Summarisch;
using ASTRA.EMSG.Business.Services.EntityServices.Summarisch;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using System.Linq;
using ASTRA.EMSG.Web.Areas.Auswertungen.Controllers;
using ASTRA.EMSG.Web.Infrastructure;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungSummarisch.Controllers
{
    public class StrassenmengeUndZustandController : Controller
    {
        private readonly INetzSummarischService netzSummarischService;
        private readonly INetzSummarischDetailService netzSummarischDetailService;

        public StrassenmengeUndZustandController(
            INetzSummarischService netzSummarischService, 
            INetzSummarischDetailService netzSummarischDetailService)
        {
            this.netzSummarischService = netzSummarischService;
            this.netzSummarischDetailService = netzSummarischDetailService;
        }

        public ActionResult Index()
        {
            return View(netzSummarischService.GetCurrentNetzSummarischModel());
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            return Json(GetGridModel(dataSourceRequest));
        }

        public ActionResult EditNetzSummarischDetailModel(Guid id)
        {
            return PartialView(netzSummarischDetailService.GetById(id));
        }

        public ActionResult Update(NetzSummarischDetailModel netzSummarischDetailModel)
        {
            if (ModelState.IsValid)
            {
                netzSummarischDetailService.UpdateEntity(netzSummarischDetailModel);
                return new EmsgEmptyResult();
            }

            return PartialView("EditNetzSummarischDetailModel", netzSummarischDetailModel);
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            return new SerializableDataSourceResult(netzSummarischDetailService.GetCurrentModels().ToDataSourceResult(dataSourceRequest));
        }

        public string GetMittleresErhebungsjahr()
        {
            DateTime? mittleresErhebungsJahr = netzSummarischService.GetCurrentNetzSummarischModel().MittleresErhebungsJahr;
            return mittleresErhebungsJahr.HasValue ? mittleresErhebungsJahr.Value.ToShortDateString() : " ";
        }

        public ActionResult EditMittleresErhebungsjahr(Guid id)
        {
            return PartialView(netzSummarischService.GetById(id));
        }

        public ActionResult SaveMittleresErhebungsjahr(NetzSummarischModel netzSummarischModel)
        {
            if (ModelState.IsValid)
            {
                netzSummarischService.UpdateEntity(netzSummarischModel);
                return new EmsgEmptyResult();
            }

            return PartialView("EditMittleresErhebungsjahr", netzSummarischModel);
        }
    }
}
