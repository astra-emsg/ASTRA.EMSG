using System;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Resources;
using System.Linq;
using ASTRA.EMSG.Web.Infrastructure;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.Administration.Controllers
{
    public class WiederbeschaffungswertAndAlterungsbeiwertController : Controller
    {
        private readonly IWiederbeschaffungswertKatalogEditService wiederbeschaffungswertKatalogEditService;
        private readonly IGlobalWiederbeschaffungswertKatalogEditService globalWiederbeschaffungswertKatalogEditService;
        private readonly ISecurityService securityService;

        public WiederbeschaffungswertAndAlterungsbeiwertController(IWiederbeschaffungswertKatalogEditService wiederbeschaffungswertKatalogEditService, ISecurityService securityService, IGlobalWiederbeschaffungswertKatalogEditService globalWiederbeschaffungswertKatalogEditService)
        {
            this.wiederbeschaffungswertKatalogEditService = wiederbeschaffungswertKatalogEditService;
            this.securityService = securityService;
            this.globalWiederbeschaffungswertKatalogEditService = globalWiederbeschaffungswertKatalogEditService;
        }

        public ActionResult Index()
        {
            ViewBag.IsForApplicationLevel = IsForApplicationLevel;
            return View();
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            return Json(GetGridModel(dataSourceRequest));
        }

        [AccessRights(Rolle.Benutzeradministrator)]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest dataSourceRequest, Guid id)
        {
            wiederbeschaffungswertKatalogEditService.ResetToGlobal(id);
            return View(GetGridModel(dataSourceRequest));
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            ViewBag.IsForApplicationLevel = IsForApplicationLevel;
            var wiederbeschaffungswertKatalogEditModels = IsForApplicationLevel
                ? globalWiederbeschaffungswertKatalogEditService.GetCurrentModels()
                : wiederbeschaffungswertKatalogEditService.GetCustomizedModels();

            return new SerializableDataSourceResult(
                wiederbeschaffungswertKatalogEditModels.OrderBy(m => m.BelastungskategorieReihenfolge)
                    .ToDataSourceResult(dataSourceRequest));
        }

        [AccessRights(Rolle.Benutzeradministrator)]
        public ActionResult Create()
        {
            ViewBag.IsNew = true;
            PrepareViewBag();

            var wiederbeschaffungswertKatalogEditModel = new WiederbeschaffungswertKatalogEditModel();          
            
            var belastungskategorie = wiederbeschaffungswertKatalogEditService.GetBelastungskategorienWithoutWiederbeschaffungswertOverride().OrderBy(o => o.Reihenfolge).FirstOrDefault();
            if(belastungskategorie != null)
            {
                wiederbeschaffungswertKatalogEditModel.Belastungskategorie = belastungskategorie.Id;
                globalWiederbeschaffungswertKatalogEditService.LoadGlobalValues(wiederbeschaffungswertKatalogEditModel);
            }

            return PartialView("EditWiederbeschaffungswertAndAlterungsbeiwert", wiederbeschaffungswertKatalogEditModel);
        }

        [AccessRights(Rolle.Benutzeradministrator)]
        public ActionResult LoadDefaultWiederbeschaffungswert(WiederbeschaffungswertKatalogEditModel wiederbeschaffungswertKatalogEditModel)
        {
            if (wiederbeschaffungswertKatalogEditModel.Belastungskategorie.HasValue)
            {
                globalWiederbeschaffungswertKatalogEditService.LoadGlobalValues(wiederbeschaffungswertKatalogEditModel);
                ModelState.Clear();
            }

            ViewBag.IsNew = true;
            PrepareViewBag();
            return PartialView("EditWiederbeschaffungswertAndAlterungsbeiwert", wiederbeschaffungswertKatalogEditModel);
        }

        public ActionResult EditWiederbeschaffungswertAndAlterungsbeiwert(Guid id)
        {
            PrepareViewBag();
            var wiederbeschaffungswertKatalogEditModel = IsForApplicationLevel 
                ? globalWiederbeschaffungswertKatalogEditService.GetById(id) 
                : wiederbeschaffungswertKatalogEditService.GetById(id);

            return PartialView(wiederbeschaffungswertKatalogEditModel);
        }

        public ActionResult Update(WiederbeschaffungswertKatalogEditModel wiederbeschaffungswertKatalogEditModel)
        {
            if (ModelState.IsValid)
            {
                if(IsForApplicationLevel)
                    globalWiederbeschaffungswertKatalogEditService.UpdateEntity(wiederbeschaffungswertKatalogEditModel);
                else
                    wiederbeschaffungswertKatalogEditService.UpdateEntity(wiederbeschaffungswertKatalogEditModel);

                return new EmsgEmptyResult();
            }

            PrepareViewBag();
            return PartialView("EditWiederbeschaffungswertAndAlterungsbeiwert", wiederbeschaffungswertKatalogEditModel);
        }

        [AccessRights(Rolle.Benutzeradministrator)]
        public ActionResult Insert(WiederbeschaffungswertKatalogEditModel wiederbeschaffungswertKatalogEditModel)
        {
            if (ModelState.IsValid)
            {
                wiederbeschaffungswertKatalogEditService.Customize(wiederbeschaffungswertKatalogEditModel);
                return new EmsgEmptyResult();
            }

            ViewBag.IsNew = true;
            PrepareViewBag();

            return PartialView("EditWiederbeschaffungswertAndAlterungsbeiwert", wiederbeschaffungswertKatalogEditModel);
        }

        private void PrepareViewBag()
        {
            ViewBag.IsForApplicationLevel = IsForApplicationLevel;

            if(!IsForApplicationLevel)
                ViewBag.Belastungskategorien = wiederbeschaffungswertKatalogEditService
                    .GetBelastungskategorienWithoutWiederbeschaffungswertOverride()
                    .OrderBy(o => o.Reihenfolge)
                    .ToDropDownItemList(bk => LookupLocalization.ResourceManager.GetString(bk.Typ), bk => bk.Id, string.Empty);
        }

        private bool IsForApplicationLevel
        {
            get { return securityService.GetCurrentRollen().Contains(Rolle.Applikationsadministrator); }
        }
    }
}
