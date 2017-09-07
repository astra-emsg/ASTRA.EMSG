using System;
using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using ASTRA.EMSG.Web.Infrastructure;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.Common.Controllers
{
    public class KenngroessenFruehererJahreControllerBase : Controller
    {
        private readonly ILocalizationService localizationService;
        private readonly IBelastungskategorieService belastungskategorieService;
        private readonly IKenngroessenFruehererJahreOverviewService kenngroessenFruehererJahreOverviewService;
        private readonly IKenngroessenFruehererJahreService kenngroessenFruehererJahreService;
        private readonly IErfassungsPeriodService erfassungsPeriodService;


        public KenngroessenFruehererJahreControllerBase(
            ILocalizationService localizationService,
            IBelastungskategorieService belastungskategorieService, 
            IKenngroessenFruehererJahreOverviewService kenngroessenFruehererJahreOverviewService,
            IKenngroessenFruehererJahreService kenngroessenFruehererJahreService,
            IErfassungsPeriodService erfassungsPeriodService
            )
        {
            this.localizationService = localizationService;
            this.belastungskategorieService = belastungskategorieService;
            this.kenngroessenFruehererJahreOverviewService = kenngroessenFruehererJahreOverviewService;
            this.kenngroessenFruehererJahreService = kenngroessenFruehererJahreService;
            this.erfassungsPeriodService = erfassungsPeriodService;
        }

        public ActionResult Index()
        {
            ViewBag.WasThereJahresabschluss = erfassungsPeriodService.WasThereJahresabschluss();
            PrepareViewBag(false);
            return View("KenngroessenFruehererJahre/Index");
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            PrepareViewBag(false);
            return Json(GetGridModel(dataSourceRequest));
        }

        public ActionResult Delete([DataSourceRequest] DataSourceRequest dataSourceRequest, Guid id)
        {
            kenngroessenFruehererJahreService.DeleteEntity(id);
            return View(GetGridModel(dataSourceRequest));
        }

        public ActionResult Create()
        {
            PrepareViewBag(true);
            return PartialView("KenngroessenFruehererJahre/EditKenngroessenFruehererJahre", CreateDefaultKenngroessenFruehererJahreModel());
        }

        private void PrepareViewBag(bool isInCreateMode)
        {
            ViewBag.Belastungskategorien = belastungskategorieService.AllBelastungskategorieModel;
            ViewBag.IsNew = isInCreateMode;
        }

        private KenngroessenFruehererJahreModel CreateDefaultKenngroessenFruehererJahreModel()
        {
            var belastungskategorieModels = belastungskategorieService.AllBelastungskategorieModel;
            PrepareViewBag(true);
            return new KenngroessenFruehererJahreModel
                       {
                           KenngroesseFruehereJahrDetailModels = belastungskategorieModels
                           .Select(b => new KenngroessenFruehererJahreDetailModel
                                            {
                                                Belastungskategorie = b.Id,
                                                BelastungskategorieBezeichnung = localizationService.GetLocalizedBelastungskategorieTyp(b.Typ)
                                            }).ToList()
                       };
        }

        public ActionResult EditKenngroessenFruehererJahre(Guid id)
        {
            var kenngroessenFruehererJahreModel = kenngroessenFruehererJahreService.GetById(id);
            foreach (var detailModel in kenngroessenFruehererJahreModel.KenngroesseFruehereJahrDetailModels)
                detailModel.BelastungskategorieBezeichnung = localizationService.GetLocalizedBelastungskategorieTyp(detailModel.BelastungskategorieTyp);

            PrepareViewBag(false);
            return PartialView("KenngroessenFruehererJahre/EditKenngroessenFruehererJahre", kenngroessenFruehererJahreModel);    
        }

        public ActionResult Insert(KenngroessenFruehererJahreModel kenngroessenFruehererJahreModel)
        {
            if (ModelState.IsValid)
            {
                kenngroessenFruehererJahreService.CreateEntity(kenngroessenFruehererJahreModel);
                return new EmsgEmptyResult();
            }

            PrepareViewBag(true);
            return PartialView("KenngroessenFruehererJahre/EditKenngroessenFruehererJahre", kenngroessenFruehererJahreModel);
        }

        public ActionResult Update(KenngroessenFruehererJahreModel kenngroessenFruehererJahreModel)
        {
            if (ModelState.IsValid)
            {
                kenngroessenFruehererJahreService.UpdateEntity(kenngroessenFruehererJahreModel);
                return new EmsgEmptyResult();
            }

            PrepareViewBag(false);
            return PartialView("KenngroessenFruehererJahre/EditKenngroessenFruehererJahre", kenngroessenFruehererJahreModel);
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            var kenngroessenFruehererJahreOverviewModels = kenngroessenFruehererJahreOverviewService.GetCurrentModels().OrderBy(m => m.Jahr).ToList();
            return new SerializableDataSourceResult(kenngroessenFruehererJahreOverviewModels.ToDataSourceResult(dataSourceRequest));
        }
    }
}