using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Common.GridCommands;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Resources;
using ASTRA.EMSG.Common.Master.GeoJSON;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using ASTRA.EMSG.Business.Services;
using ASTRA.EMSG.Business.Entities.GIS;
using Kendo.Mvc.Extensions;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers
{
    public class KoordinierteMassnahmenController : Controller
    {
        private readonly IKoordinierteMassnahmeGISModelService koordinierteMassnahmeGISModelService;
        private readonly IKoordinierteMassnahmeGISOverviewModelService koordinierteMassnahmeGISOverviewModelService;
        private readonly IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService;
        private readonly IGeoJSONParseService geoJSONParseService;
        private readonly ILocalizationService localizationService;

        public KoordinierteMassnahmenController(
            IKoordinierteMassnahmeGISModelService koordinierteMassnahmeGISModelService, 
            IKoordinierteMassnahmeGISOverviewModelService koordinierteMassnahmeGISOverviewModelService, 
            IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService,
            IGeoJSONParseService geoJSONParseService,
            ILocalizationService localizationService)
        {
            this.localizationService = localizationService;
            this.geoJSONParseService = geoJSONParseService;
            this.massnahmenvorschlagKatalogService = massnahmenvorschlagKatalogService;
            this.koordinierteMassnahmeGISModelService = koordinierteMassnahmeGISModelService;
            this.koordinierteMassnahmeGISOverviewModelService = koordinierteMassnahmeGISOverviewModelService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest, KoordinierteMassnahmenGridCommand command)
        {
            return Json(GetGridModel(dataSourceRequest, command));
        }

        public ActionResult Delete([DataSourceRequest] DataSourceRequest dataSourceRequest, Guid id)
        {
            koordinierteMassnahmeGISModelService.DeleteEntity(id);
            return PartialView(GetGridModel(dataSourceRequest, new KoordinierteMassnahmenGridCommand()));
        }

        public ActionResult Create()
        {
            PrepareViewBag(true, null);
            return PartialView("EditKoordinierteMassnahmen", CreateDefaultKoordinierteMassnahmeGISModel());
        }

        public ActionResult Insert(KoordinierteMassnahmeGISModel koordinierteMassnahmeGISModel)
        {
            koordinierteMassnahmeGISModel = UpdateAndValidateGeometry(koordinierteMassnahmeGISModel);
            if (ModelState.IsValid)
            {
                koordinierteMassnahmeGISModelService.CreateEntity(koordinierteMassnahmeGISModel);
                return new EmsgEmptyResult();
            }
            PrepareViewBag(true, koordinierteMassnahmeGISModel.MassnahmenvorschlagFahrbahn);
            return PartialView("EditKoordinierteMassnahmen", koordinierteMassnahmeGISModel);
        }

        public ActionResult ApplyInsert(KoordinierteMassnahmeGISModel koordinierteMassnahmeGISModel)
        {
            koordinierteMassnahmeGISModel = UpdateAndValidateGeometry(koordinierteMassnahmeGISModel);
            var isInNewMode = true;
            if (ModelState.IsValid)
            {
                koordinierteMassnahmeGISModel = koordinierteMassnahmeGISModelService.CreateEntity(koordinierteMassnahmeGISModel);
                ModelState.Clear();
                isInNewMode = false;
            }

            PrepareViewBag(isInNewMode, koordinierteMassnahmeGISModel.MassnahmenvorschlagFahrbahn);
            return PartialView("EditKoordinierteMassnahmen", koordinierteMassnahmeGISModel);
        }

        public ActionResult Update(KoordinierteMassnahmeGISModel koordinierteMassnahmeGISModel)
        {
            koordinierteMassnahmeGISModel = UpdateAndValidateGeometry(koordinierteMassnahmeGISModel);
            if (ModelState.IsValid)
            {
                koordinierteMassnahmeGISModelService.UpdateEntity(koordinierteMassnahmeGISModel);
                return new EmsgEmptyResult();
            }
            PrepareViewBag(false, koordinierteMassnahmeGISModel.MassnahmenvorschlagFahrbahn);
            return PartialView("EditKoordinierteMassnahmen", koordinierteMassnahmeGISModel);
        }

        public ActionResult ApplyUpdate(KoordinierteMassnahmeGISModel koordinierteMassnahmeGISModel)
        {
            koordinierteMassnahmeGISModel = UpdateAndValidateGeometry(koordinierteMassnahmeGISModel);
            if (ModelState.IsValid)
            {
                koordinierteMassnahmeGISModel = koordinierteMassnahmeGISModelService.UpdateEntity(koordinierteMassnahmeGISModel);
                ModelState.Clear();
            }

            PrepareViewBag(false, koordinierteMassnahmeGISModel.MassnahmenvorschlagFahrbahn);

            return PartialView("EditKoordinierteMassnahmen", koordinierteMassnahmeGISModel);
        }


        private KoordinierteMassnahmeGISModel CreateDefaultKoordinierteMassnahmeGISModel()
        {
            return new KoordinierteMassnahmeGISModel();
        }

        public ActionResult Edit(Guid id)
        {
            var koordinierteMassnahmeGISModel = koordinierteMassnahmeGISModelService.GetById(id);
            PrepareViewBag(false, koordinierteMassnahmeGISModel.MassnahmenvorschlagFahrbahn);
            return PartialView("EditKoordinierteMassnahmen", koordinierteMassnahmeGISModel);
        }

        private void PrepareViewBag(bool isCreateMode, Guid? selectedMassnahmenbeschreibungFahrbahn)
        {
            ViewBag.MassnahmenbeschreibungFahrbahnen = GetMassnamenKatalogDropDownItems(selectedMassnahmenbeschreibungFahrbahn);
            ViewBag.IsNew = isCreateMode;
        }

        private IEnumerable<DropDownListItem> GetMassnamenKatalogDropDownItems(Guid? selectedMassnahmenbeschreibungFahrbahn)
        {
            var massnahmenvorschlagKatalogModels =
                massnahmenvorschlagKatalogService.GetMassnahmenvorschlagKatalogModelList(MassnahmenvorschlagKatalogTyp.Fahrbahn);
            var selectedRealisierteMassnahme = massnahmenvorschlagKatalogModels.FirstOrDefault(m => m.Id == selectedMassnahmenbeschreibungFahrbahn);
            var massnahmenvorschlagKatalogen = massnahmenvorschlagKatalogModels
                .ToDropDownItemList(rmk => LookupLocalization.ResourceManager.GetString(rmk.Typ) ?? rmk.Typ, mk => mk.Id, selectedRealisierteMassnahme, TextLocalization.EmptyMessage)
                .OrderBy(ddi => ddi.Text);

            return massnahmenvorschlagKatalogen;
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest, KoordinierteMassnahmenGridCommand command)
        {
            var koordinierteMassnahmeGISOverviewModels = koordinierteMassnahmeGISOverviewModelService.GetCurrentModelsByProjektname(command.ProjektnameFilter);
            return new SerializableDataSourceResult(
                koordinierteMassnahmeGISOverviewModels.OrderBy(m => m.Projektname)
                    .ToDataSourceResult(dataSourceRequest));
        }

        private KoordinierteMassnahmeGISModel UpdateAndValidateGeometry(KoordinierteMassnahmeGISModel model)
        {
            if (ModelState.IsValid)
            {
                model = (KoordinierteMassnahmeGISModel) geoJSONParseService.GenerateModelFromGeoJsonString(model);

                if (!koordinierteMassnahmeGISModelService.Validate(model))
                {
                    ModelState.AddModelError(
                        ASTRA.EMSG.Common.ExpressionHelper.GetPropertyName<IAbschnittGISModelBase, string>(m => m.FeatureGeoJSONString),
                        localizationService.GetLocalizedError(ValidationError.GeometryNotOnAchsenSegment));
                }

            }
            return model;
        }

        [HttpGet]
        public ActionResult GetKoordinierteMassnahmeAt(double x, double y, double tolerance)
        {
            try
            {
                KoordinierteMassnahmeGISModel koordinierteMassnahme = koordinierteMassnahmeGISModelService.GetKoordinierteMassnahmeAt(x, y, tolerance);
                return Content(koordinierteMassnahme.FeatureGeoJSONString, "application/json");
            }
            catch (Exception exc)
            {
                return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
            }
        }
        
        [HttpGet]
        public ActionResult GetAllKoordinierteMassnahmenAt(double x, double y, double tolerance)
        {
            try
            {
                IEnumerable<KoordinierteMassnahmeGIS> koordinierteMassnahmen = koordinierteMassnahmeGISModelService.GetAllKoordinierteMassnahmenAt(x, y, tolerance);
                return Content(geoJSONParseService.GenereateGeoJsonStringfromEntities(koordinierteMassnahmen), "application/json");
            }
            catch (Exception exc)
            {
                return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
            }
        }

        [HttpGet]
        public ActionResult GetKoordinierteMassnahmeByID(string id)
        {
            try
            {
                KoordinierteMassnahmeGISModel koordinierteMassnahme = koordinierteMassnahmeGISModelService.GetById(Guid.Parse(id));
                return Content(koordinierteMassnahme.FeatureGeoJSONString, "application/json");
            }
            catch (Exception exc)
            {
                return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
            }
        }
    }
}
