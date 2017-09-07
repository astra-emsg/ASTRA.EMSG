using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.Services;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.Administration.Controllers
{
    public class MassnahmenvorschlagController : Controller
    {
        private readonly IGlobalMassnahmenvorschlagKatalogEditService globalMassnahmenvorschlagKatalogService;
        private readonly IMassnahmenvorschlagKatalogEditService massnahmenvorschlagKatalogEditService;
        private readonly IMassnahmenvorschlagKatalogOverviewService massnahmenvorschlagKatalogOverviewService;
        private readonly ISecurityService securityService;
        private readonly IBelastungskategorieService belastungskategorieService;
        private readonly ILocalizationService localizationService;

        public MassnahmenvorschlagController(IGlobalMassnahmenvorschlagKatalogEditService globalMassnahmenvorschlagKatalogService,
            IMassnahmenvorschlagKatalogEditService massnahmenvorschlagKatalogEditService,
            IMassnahmenvorschlagKatalogOverviewService massnahmenvorschlagKatalogOverviewService,
            ISecurityService securityService, 
            IBelastungskategorieService belastungskategorieService,
            ILocalizationService localizationService)
        {
            this.globalMassnahmenvorschlagKatalogService = globalMassnahmenvorschlagKatalogService;
            this.massnahmenvorschlagKatalogEditService = massnahmenvorschlagKatalogEditService;
            this.massnahmenvorschlagKatalogOverviewService = massnahmenvorschlagKatalogOverviewService;
            this.securityService = securityService;
            this.belastungskategorieService = belastungskategorieService;
            this.localizationService = localizationService;
        }

        public ActionResult Index()
        {
            PrepareViewBag();
            return View();
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            return Json(GetGridModel(dataSourceRequest));
        }
        
        public ActionResult CreateMassnahmenvorschlag()
        {
            var belas = belastungskategorieService.AllBelastungskategorieModel;
            var model = new MassnahmenvorschlagKatalogEditModel();
            model.KonstenModels = belas.Select(b => new MassnahmenvorschlagKatalogKonstenEditModel()
                                                        {
                                                            DefaultKosten = 0m,
                                                            Belastungskategorie = b.Id,
                                                            BelastungskategorieBezeichnung = localizationService.GetLocalizedBelastungskategorieTyp(b.Typ)
                                                        }).ToList();
            PrepareViewBag();
            return PartialView("CreateMassnahmenvorschlag", model);
        }

        public ActionResult EditMassnahmenvorschlag(string typ)
        {
            PrepareViewBag();
            return PartialView(IsForApplicationLevel ? globalMassnahmenvorschlagKatalogService.GetMassnahmenvorschlagKatalogModel(typ) : massnahmenvorschlagKatalogEditService.GetMassnahmenvorschlagKatalogModel(typ));
        }

        [AccessRights(Rolle.Benutzeradministrator)]
        public ActionResult LoadDefaultMassnahmenvorschlag(MassnahmenvorschlagKatalogEditModel editModel)
        {
            if (!string.IsNullOrWhiteSpace(editModel.Typ))
            {
                editModel = globalMassnahmenvorschlagKatalogService.GetDefaultMassnahmenvorschlagKatalogModel(editModel.Typ);
                ModelState.Clear();
                PrepareViewBag();
                return PartialView("CreateMassnahmenvorschlag", editModel);
            }
            return CreateMassnahmenvorschlag();
        }

        [HttpPost]
        [AccessRights(Rolle.Applikationsadministrator)]
        public ActionResult InsertMassnahmenvorschlag(MassnahmenvorschlagKatalogCreateModel editModel)
        {
            if (ModelState.IsValid)
            {
                globalMassnahmenvorschlagKatalogService.AddMassnahmenvorschlag(editModel);
                return new EmsgEmptyResult();
            }
            
            PrepareViewBag();
            return PartialView("CreateMassnahmenvorschlag", editModel);
        }

        [HttpPost]
        [AccessRights(Rolle.Benutzeradministrator)]
        public ActionResult CustomizeMassnahmenvorschlag(MassnahmenvorschlagKatalogEditModel editModel)
        {
            if (ModelState.IsValid)
            {
                massnahmenvorschlagKatalogEditService.Customize(editModel);
                return new EmsgEmptyResult();
            }

            PrepareViewBag();
            return PartialView("CreateMassnahmenvorschlag", editModel);
        }
        
        [HttpPost]
        public ActionResult UpdateMassnahmenvorschlag(MassnahmenvorschlagKatalogEditModel editModel)
        {
            if (ModelState.IsValid)
            {
                if (IsForApplicationLevel)
                    globalMassnahmenvorschlagKatalogService.UpdateMassnahmenvorschlag(editModel);
                else
                    massnahmenvorschlagKatalogEditService.UpdateMassnahmenvorschlag(editModel);
                return new EmsgEmptyResult();
            }
            
            PrepareViewBag();
            return PartialView("EditMassnahmenvorschlag", editModel);
        }

        public ActionResult Delete([DataSourceRequest] DataSourceRequest dataSourceRequest, string typ)
        {
            if (IsForApplicationLevel)
                globalMassnahmenvorschlagKatalogService.DeleteMassnahmenvorschlag(typ);
            else
                massnahmenvorschlagKatalogEditService.ResetToGlobal(typ);
            return View(GetGridModel(dataSourceRequest));
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            PrepareViewBag();

            var massnahmenvorschlagKatalogEditModels = IsForApplicationLevel
                ? massnahmenvorschlagKatalogOverviewService.GetCurrentGlobalModels()
                : massnahmenvorschlagKatalogOverviewService.GetCurrentModels();

            return new SerializableDataSourceResult(
                massnahmenvorschlagKatalogEditModels.OrderBy(mvk => mvk.KatalogTyp)
                    .ThenBy(mvk => mvk.TypBezeichnung)
                    .ToDataSourceResult(dataSourceRequest));
        }

        private void PrepareViewBag()
        {
            ViewBag.IsForApplicationLevel = IsForApplicationLevel;
            ViewBag.Belastungskategorien = belastungskategorieService.AllBelastungskategorieModel;
            ViewBag.PossibleMassnahmenvorschlagen = massnahmenvorschlagKatalogEditService.GetPossibleMassnahmenvorschlagen().OrderBy(mvk => mvk.KatalogTyp).ThenBy(mvk => mvk.TypBezeichnung)
                .ToDropDownItemList(m => string.Format("{0} ({1})", localizationService.GetLocalizedMassnahmenvorschlagTyp(m.Typ), localizationService.GetLocalizedEnum(m.KatalogTyp)), m => m.Typ, emptyItemText:"");
        }

        private bool IsForApplicationLevel
        {
            get { return securityService.GetCurrentRollen().Contains(Rolle.Applikationsadministrator); }
        }
    }
}
