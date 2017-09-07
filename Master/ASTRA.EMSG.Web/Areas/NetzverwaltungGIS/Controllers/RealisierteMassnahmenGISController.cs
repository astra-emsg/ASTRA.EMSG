using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Common.GridCommands;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Resources;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Master.GeoJSON;
using Kendo.Mvc.Extensions;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers
{
    public class RealisierteMassnahmenGISController : Controller
    {
        private readonly IRealisierteMassnahmeGISModelService realisierteMassnahmeGISModelService;
        private readonly IRealisierteMassnahmeGISOverviewModelService realisierteMassnahmeGISOverviewModelService;
        private readonly IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService;
        private readonly IGeoJSONParseService geoJSONParseService;
        private readonly ILocalizationService localizationService;
        private readonly IBelastungskategorieService belastungskategorieService;
        
        public RealisierteMassnahmenGISController(
            IRealisierteMassnahmeGISModelService realisierteMassnahmeGISModelService, 
            IRealisierteMassnahmeGISOverviewModelService realisierteMassnahmeGISOverviewModelService, 
            IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService,
            IGeoJSONParseService geoJSONParseService,
            ILocalizationService localizationService,
            IBelastungskategorieService belastungskategorieService)
        {
            this.realisierteMassnahmeGISModelService = realisierteMassnahmeGISModelService;
            this.realisierteMassnahmeGISOverviewModelService = realisierteMassnahmeGISOverviewModelService;
            this.massnahmenvorschlagKatalogService = massnahmenvorschlagKatalogService;
            this.geoJSONParseService = geoJSONParseService;
            this.localizationService = localizationService;
            this.belastungskategorieService = belastungskategorieService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DataSaved()
        {

            return PartialView();
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest, RealisierteMassnahmenGISGridCommand command)
        {
            return Json(GetGridModel(dataSourceRequest, command));
        }

        public ActionResult Delete([DataSourceRequest] DataSourceRequest dataSourceRequest, Guid id)
        {
            realisierteMassnahmeGISModelService.DeleteEntity(id);
            return PartialView(GetGridModel(dataSourceRequest, new RealisierteMassnahmenGISGridCommand()));
        }

        public ActionResult Create()
        {
            var defaultRealisierteMassnahmeGISModel = CreateDefaultRealisierteMassnahmeGISModel();
            PrepareViewBag(true, defaultRealisierteMassnahmeGISModel);
            return PartialView("EditRealisierteMassnahmen", defaultRealisierteMassnahmeGISModel);
        }

        public ActionResult Insert(RealisierteMassnahmeGISModel realisierteMassnahmeGISModel)
        {
            realisierteMassnahmeGISModel = UpdateAndValidateGeometry(realisierteMassnahmeGISModel);
            if (ModelState.IsValid)
            {
                realisierteMassnahmeGISModelService.CreateEntity(realisierteMassnahmeGISModel);
                return new EmsgEmptyResult();
            }
            PrepareViewBag(true, realisierteMassnahmeGISModel);
            return PartialView("EditRealisierteMassnahmen", realisierteMassnahmeGISModel);
        }

        public ActionResult ApplyInsert(RealisierteMassnahmeGISModel realisierteMassnahmeGISModel)
        {
            realisierteMassnahmeGISModel = UpdateAndValidateGeometry(realisierteMassnahmeGISModel);
            var isInNewMode = true;
            if (ModelState.IsValid)
            {
                realisierteMassnahmeGISModel = realisierteMassnahmeGISModelService.CreateEntity(realisierteMassnahmeGISModel);
                ModelState.Clear();
                isInNewMode = false;
            }

            PrepareViewBag(isInNewMode, realisierteMassnahmeGISModel);
            return PartialView("EditRealisierteMassnahmen", realisierteMassnahmeGISModel);
        }

        public ActionResult Update(RealisierteMassnahmeGISModel realisierteMassnahmeGISModel)
        {
            realisierteMassnahmeGISModel = UpdateAndValidateGeometry(realisierteMassnahmeGISModel);
            if (ModelState.IsValid)
            {
                realisierteMassnahmeGISModelService.UpdateEntity(realisierteMassnahmeGISModel);
                return new EmsgEmptyResult();
            }
            PrepareViewBag(false, realisierteMassnahmeGISModel);
            return PartialView("EditRealisierteMassnahmen", realisierteMassnahmeGISModel);
        }

        public ActionResult ApplyUpdate(RealisierteMassnahmeGISModel realisierteMassnahmeGISModel)
        {
            realisierteMassnahmeGISModel = UpdateAndValidateGeometry(realisierteMassnahmeGISModel);
            if (ModelState.IsValid)
            {
                realisierteMassnahmeGISModel = realisierteMassnahmeGISModelService.UpdateEntity(realisierteMassnahmeGISModel);
                ModelState.Clear();
            }

            PrepareViewBag(false, realisierteMassnahmeGISModel);

            return PartialView("EditRealisierteMassnahmen", realisierteMassnahmeGISModel);
        }


        private RealisierteMassnahmeGISModel CreateDefaultRealisierteMassnahmeGISModel()
        {
            return new RealisierteMassnahmeGISModel
                {
                    Strasseneigentuemer = EigentuemerTyp.Gemeinde
                };
        }

        public ActionResult Edit(Guid id)
        {
            var realisierteMassnahmeGISModel = realisierteMassnahmeGISModelService.GetById(id);
            PrepareViewBag(false, realisierteMassnahmeGISModel);
            return PartialView("EditRealisierteMassnahmen", realisierteMassnahmeGISModel);
        }

        private void PrepareViewBag(bool isCreateMode, RealisierteMassnahmeGISModel model)
        {
            ViewBag.MassnahmenvorschlagFahrbahnKatalogen = GetMassnamenKatalogDropDownItems(model.MassnahmenvorschlagFahrbahn, MassnahmenvorschlagKatalogTyp.Fahrbahn);
            ViewBag.MassnahmenvorschlagTrottoirKatalogen = GetMassnamenKatalogDropDownItems(model.MassnahmenvorschlagTrottoir, MassnahmenvorschlagKatalogTyp.Trottoir);
            ViewBag.Belastungskategorien = belastungskategorieService.AllBelastungskategorieModel.ToDropDownItemList(tvp => localizationService.GetLocalizedBelastungskategorieTyp(tvp.Typ), bk => bk.Id, model.Belastungskategorie, TextLocalization.EmptyMessage);
            ViewBag.Strasseneigentuemer = belastungskategorieService.AllBelastungskategorieModel.ToDropDownItemList(tvp => localizationService.GetLocalizedBelastungskategorieTyp(tvp.Typ), bk => bk.Id, model.Belastungskategorie, TextLocalization.EmptyMessage);
            ViewBag.IsNew = isCreateMode;
        }

        private IEnumerable<DropDownListItem> GetMassnamenKatalogDropDownItems(Guid? selectedMassnahmenvorschlagKatalogId, MassnahmenvorschlagKatalogTyp fahrbahn)
        {
            var massnahmenvorschlagKatalogModels = massnahmenvorschlagKatalogService
                    .GetMassnahmenvorschlagKatalogModelList(fahrbahn);
            var selectedRealisierteMassnahme = massnahmenvorschlagKatalogModels.FirstOrDefault(m => m.Id == selectedMassnahmenvorschlagKatalogId);
            var massnahmenvorschlagKatalogen = massnahmenvorschlagKatalogModels
                .ToDropDownItemList(rmk => LookupLocalization.ResourceManager.GetString(rmk.Typ) ?? rmk.Typ, mk => mk.Id, selectedRealisierteMassnahme, TextLocalization.EmptyMessage)
                .OrderBy(ddi => ddi.Text);

            return massnahmenvorschlagKatalogen;
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest, RealisierteMassnahmenGISGridCommand command)
        {
            var realisierteMassnahmeGISOverviewModels = realisierteMassnahmeGISOverviewModelService
                .GetCurrentModelsByProjektname(new RealisierteMassnahmeGISOverviewFilter
                                                   {
                                                       Projektname = command.ProjektnameFilter
                                                   });
            return new SerializableDataSourceResult(realisierteMassnahmeGISOverviewModels
                .OrderBy(m => m.Projektname).ToDataSourceResult(dataSourceRequest));
        }

        private RealisierteMassnahmeGISModel UpdateAndValidateGeometry(RealisierteMassnahmeGISModel model)
        {
            if (ModelState.IsValid)
            {
                model = (RealisierteMassnahmeGISModel)geoJSONParseService.GenerateModelFromGeoJsonString(model);

                if (!realisierteMassnahmeGISModelService.Validate(model))
                {
                    ModelState.AddModelError(
                        ASTRA.EMSG.Common.ExpressionHelper.GetPropertyName<IAbschnittGISModelBase, string>(m => m.FeatureGeoJSONString),
                        localizationService.GetLocalizedError(ValidationError.GeometryNotOnAchsenSegment));
                }
            }
            return model;
        }

        [HttpGet]
        public ActionResult GetRealisierteMassnahmeAt(double x, double y,  double tolerance)
        {
            try
            {
                RealisierteMassnahmeGISModel realisierteMassnahme = realisierteMassnahmeGISModelService.GetRealisierteMassnahmeAt(x, y, tolerance);
                return Content(realisierteMassnahme.FeatureGeoJSONString, "application/json");
            }
            catch (Exception exc)
            {
                return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
            }
        }

        [HttpGet]
        public ActionResult GetRealisierteMassnahmeByID(string id)
        {
            try
            {
                RealisierteMassnahmeGISModel realisierteMassnahme = realisierteMassnahmeGISModelService.GetById(Guid.Parse(id));
                return Content(realisierteMassnahme.FeatureGeoJSONString, "application/json");
            }
            catch (Exception exc)
            {
                return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
            }
        }
    
    }
}