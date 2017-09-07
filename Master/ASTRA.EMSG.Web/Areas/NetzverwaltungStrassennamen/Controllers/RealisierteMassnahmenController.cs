using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Common.GridCommands;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Resources;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungStrassennamen.Controllers
{
    public class RealisierteMassnahmenController : Controller
    {
        private readonly IRealisierteMassnahmeService realisierteMassnahmeService;
        private readonly IRealisierteMassnahmeOverviewService realisierteMassnahmeOverviewService;
        private readonly IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService;
        private readonly IBelastungskategorieService belastungskategorieService;
        private readonly ILocalizationService localizationService;

        public RealisierteMassnahmenController(
            IRealisierteMassnahmeService realisierteMassnahmeService, 
            IBelastungskategorieService belastungskategorieService, 
            ILocalizationService localizationService,
            IRealisierteMassnahmeOverviewService realisierteMassnahmeOverviewService,
            IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService)
        {
            this.realisierteMassnahmeService = realisierteMassnahmeService;
            this.belastungskategorieService = belastungskategorieService;
            this.localizationService = localizationService;
            this.realisierteMassnahmeOverviewService = realisierteMassnahmeOverviewService;
            this.massnahmenvorschlagKatalogService = massnahmenvorschlagKatalogService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest, RealisierteMassnahmenGridCommand command)
        {
            return Json(GetGridModel(dataSourceRequest, command));
        }

        public ActionResult Delete([DataSourceRequest] DataSourceRequest dataSourceRequest, Guid id)
        {
            realisierteMassnahmeService.DeleteEntity(id);
            return View(GetGridModel(dataSourceRequest, new RealisierteMassnahmenGridCommand()));
        }

        public ActionResult Create()
        {
            var defaultRealisierteMassnahme = CreateDefaultRealisierteMassnahme();
            PrepareViewBag(true, defaultRealisierteMassnahme);
            return PartialView("EditRealisierteMassnahme", defaultRealisierteMassnahme);
        }

        private void PrepareViewBag(bool isInCreateMode, RealisierteMassnahmeModel model)
        {
            ViewBag.IsNew = isInCreateMode;
            ViewBag.MassnahmenvorschlagFahrbahnKatalogen = GetMassnamenKatalogDropDownItems(model.MassnahmenvorschlagFahrbahn, MassnahmenvorschlagKatalogTyp.Fahrbahn);
            ViewBag.MassnahmenvorschlagTrottoirKatalogen = GetMassnamenKatalogDropDownItems(model.MassnahmenvorschlagTrottoir,MassnahmenvorschlagKatalogTyp.Trottoir);
            ViewBag.Belastungskategorien = belastungskategorieService.AllBelastungskategorieModel.ToDropDownItemList(tvp => localizationService.GetLocalizedBelastungskategorieTyp(tvp.Typ), bk => bk.Id, model.Belastungskategorie, TextLocalization.EmptyMessage);
        }

        private IEnumerable<DropDownListItem> GetMassnamenKatalogDropDownItems(Guid? selectedMassnahmenvorschlagKatalogId, MassnahmenvorschlagKatalogTyp fahrbahn)
        {
            IEnumerable<BLKIndependentMassnahmenvorschlagKatalogModel> massnahmenvorschlagKatalogModels;
                massnahmenvorschlagKatalogModels = massnahmenvorschlagKatalogService
                    .GetMassnahmenvorschlagKatalogModelList(fahrbahn);
            var selectedModel = massnahmenvorschlagKatalogModels.FirstOrDefault(m => m.Id == selectedMassnahmenvorschlagKatalogId);
            var massnahmenvorschlagKatalogen = massnahmenvorschlagKatalogModels
                .ToDropDownItemList(rmk => LookupLocalization.ResourceManager.GetString(rmk.Typ) ?? rmk.Typ, mk => mk.Id, selectedModel, TextLocalization.EmptyMessage)
                .OrderBy(ddi => ddi.Text);

            return massnahmenvorschlagKatalogen;
        }

        private RealisierteMassnahmeModel CreateDefaultRealisierteMassnahme()
        {
            return new RealisierteMassnahmeModel{Strasseneigentuemer = EigentuemerTyp.Gemeinde};
        }

        public ActionResult EditRealisierteMassnahme(Guid id)
        {
            RealisierteMassnahmeModel realisierteMassnahmeModel = realisierteMassnahmeService.GetById(id);
            PrepareViewBag(false, realisierteMassnahmeModel);
            return PartialView(realisierteMassnahmeModel);
        }

        public ActionResult Insert(RealisierteMassnahmeModel realisierteMassnahmeModel)
        {
            if (ModelState.IsValid)
            {
                realisierteMassnahmeService.CreateEntity(realisierteMassnahmeModel);
                return new EmsgEmptyResult();
            }

            PrepareViewBag(true, realisierteMassnahmeModel);
            return PartialView("EditRealisierteMassnahme", realisierteMassnahmeModel);
        }

        public ActionResult ApplyInsert(RealisierteMassnahmeModel realisierteMassnahmeModel)
        {
            var isInCreateMode = true;

            if (ModelState.IsValid)
            {
                realisierteMassnahmeModel = realisierteMassnahmeService.CreateEntity(realisierteMassnahmeModel);
                ModelState.Clear();
                isInCreateMode = false;
            }

            PrepareViewBag(isInCreateMode, realisierteMassnahmeModel);
            return PartialView("EditRealisierteMassnahme", realisierteMassnahmeModel);
        }

        public ActionResult Update(RealisierteMassnahmeModel realisierteMassnahmeModel)
        {
            if (ModelState.IsValid)
            {
                realisierteMassnahmeService.UpdateEntity(realisierteMassnahmeModel);
                return new EmsgEmptyResult();
            }

            PrepareViewBag(false, realisierteMassnahmeModel);
            return PartialView("EditRealisierteMassnahme", realisierteMassnahmeModel);
        }

        public ActionResult ApplyUpdate(RealisierteMassnahmeModel realisierteMassnahmeModel)
        {
            if (ModelState.IsValid)
            {
                realisierteMassnahmeModel = realisierteMassnahmeService.UpdateEntity(realisierteMassnahmeModel);
                ModelState.Clear();
            }

            PrepareViewBag(false, realisierteMassnahmeModel);
            return PartialView("EditRealisierteMassnahme", realisierteMassnahmeModel);
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest, RealisierteMassnahmenGridCommand command)
        {
            var realisierteMassnahmeSummarsichModels = realisierteMassnahmeOverviewService.GetCurrentModelsByProjektname(command.ProjektnameFilter)
                .OrderBy(m => m.Projektname);

            return new SerializableDataSourceResult(realisierteMassnahmeSummarsichModels.ToDataSourceResult(dataSourceRequest));
        }
    }
}
