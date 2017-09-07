using System;
using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.Summarisch;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Summarisch;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Common.GridCommands;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using ASTRA.EMSG.Web.Infrastructure;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungSummarisch.Controllers
{
    public class RealisierteMassnahmenSummarischController : Controller
    {
        private readonly IRealisierteMassnahmeSummarsichService realisierteMassnahmeSummarsichService;
        private readonly IRealisierteMassnahmeSummarsichOverviewService realisierteMassnahmeSummarsichOverviewService;
        private readonly IBelastungskategorieService belastungskategorieService;

        public RealisierteMassnahmenSummarischController(
            IRealisierteMassnahmeSummarsichService realisierteMassnahmeSummarsichService,
            IBelastungskategorieService belastungskategorieService,
            IRealisierteMassnahmeSummarsichOverviewService realisierteMassnahmeSummarsichOverviewService)
        {
            this.realisierteMassnahmeSummarsichService = realisierteMassnahmeSummarsichService;
            this.belastungskategorieService = belastungskategorieService;
            this.realisierteMassnahmeSummarsichOverviewService = realisierteMassnahmeSummarsichOverviewService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest, RealisierteMassnahmenSummarischGridCommand command)
        {
            return Json(GetGridModel(dataSourceRequest, command));
        }

        public ActionResult Delete([DataSourceRequest] DataSourceRequest dataSourceRequest, Guid id)
        {
            realisierteMassnahmeSummarsichService.DeleteEntity(id);
            return View(GetGridModel(dataSourceRequest, new RealisierteMassnahmenSummarischGridCommand()));
        }

        public ActionResult Create()
        {
            ViewBag.IsNew = true;
            PrepareBelastungskategorien();
            return PartialView("EditRealisierteMassnahmeSummarsich", new RealisierteMassnahmeSummarsichModel{Strasseneigentuemer = EigentuemerTyp.Gemeinde});
        }

        public ActionResult EditRealisierteMassnahmeSummarsich(Guid id)
        {
            PrepareBelastungskategorien();
            return PartialView(realisierteMassnahmeSummarsichService.GetById(id));
        }

        public ActionResult Insert(RealisierteMassnahmeSummarsichModel realisierteMassnahmeSummarsichModel)
        {
            if (ModelState.IsValid)
            {
                realisierteMassnahmeSummarsichService.CreateEntity(realisierteMassnahmeSummarsichModel);
                return new EmsgEmptyResult();
            }

            ViewBag.IsNew = true;
            PrepareBelastungskategorien();

            return PartialView("EditRealisierteMassnahmeSummarsich", realisierteMassnahmeSummarsichModel);
        }

        public ActionResult Update(RealisierteMassnahmeSummarsichModel realisierteMassnahmeSummarsichModel)
        {
            if (ModelState.IsValid)
            {
                realisierteMassnahmeSummarsichService.UpdateEntity(realisierteMassnahmeSummarsichModel);
                return new EmsgEmptyResult();
            }

            PrepareBelastungskategorien();
            return PartialView("EditRealisierteMassnahmeSummarsich", realisierteMassnahmeSummarsichModel);
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest, RealisierteMassnahmenSummarischGridCommand command)
        {
            var realisierteMassnahmeSummarsichModels = realisierteMassnahmeSummarsichOverviewService.GetCurrentModelsByProjektname(command.Projektname);
            return new SerializableDataSourceResult(realisierteMassnahmeSummarsichModels.OrderBy(m => m.Projektname).ToDataSourceResult(dataSourceRequest));
        }

        private void PrepareBelastungskategorien()
        {
            ViewBag.Belastungskategorien = belastungskategorieService.AllBelastungskategorieModel;
        }
    }
}
