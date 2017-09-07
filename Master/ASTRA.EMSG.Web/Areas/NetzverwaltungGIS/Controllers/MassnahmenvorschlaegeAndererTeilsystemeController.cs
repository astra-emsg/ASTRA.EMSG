using System;
using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Common.GridCommands;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Common.Master.GeoJSON;
using ASTRA.EMSG.Business.Services;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.GIS;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers
{
    public class MassnahmenvorschlaegeAndererTeilsystemeController : Controller
    {
        private readonly IMassnahmenvorschlagTeilsystemeGISModelService massnahmenvorschlagTeilsystemeGISModelService;
        private readonly IMassnahmenvorschlagTeilsystemeGISOverviewModelService massnahmenvorschlagTeilsystemeGISOverviewModelService;
        private readonly IGeoJSONParseService geoJSONParseService;
        private readonly ILocalizationService localizationService;

        public MassnahmenvorschlaegeAndererTeilsystemeController(
            IMassnahmenvorschlagTeilsystemeGISModelService massnahmenvorschlagTeilsystemeGISModelService,
            IMassnahmenvorschlagTeilsystemeGISOverviewModelService massnahmenvorschlagTeilsystemeGISOverviewModelService, 
            IGeoJSONParseService geoJSONParseService,
            ILocalizationService localizationService)
        {
            this.localizationService = localizationService;
            this.geoJSONParseService = geoJSONParseService;
            this.massnahmenvorschlagTeilsystemeGISModelService = massnahmenvorschlagTeilsystemeGISModelService;
            this.massnahmenvorschlagTeilsystemeGISOverviewModelService = massnahmenvorschlagTeilsystemeGISOverviewModelService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest, MassnahmenvorschlagTeilsystemeGridCommand command)
        {
            return Json(GetGridModel(dataSourceRequest, command));
        }

        public ActionResult Delete([DataSourceRequest] DataSourceRequest dataSourceRequest, Guid id)
        {
            massnahmenvorschlagTeilsystemeGISModelService.DeleteEntity(id);
            return PartialView(GetGridModel(dataSourceRequest, new MassnahmenvorschlagTeilsystemeGridCommand()));
        }

        public ActionResult Create()
        {
            PrepareViewBag(true);
            return PartialView("EditMassnahmenvorschlagTeilsysteme", CreateMassnahmenvorschlagTeilsystemeGISModel());
        }

        public ActionResult Insert(MassnahmenvorschlagTeilsystemeGISModel massnahmenvorschlagTeilsystemeGISModel)
        {
            massnahmenvorschlagTeilsystemeGISModel = UpdateAndValidateGeometry(massnahmenvorschlagTeilsystemeGISModel);
            if (ModelState.IsValid)
            {
                massnahmenvorschlagTeilsystemeGISModelService.CreateEntity(massnahmenvorschlagTeilsystemeGISModel);
                return new EmsgEmptyResult();
            }

            PrepareViewBag(true);
            return PartialView("EditMassnahmenvorschlagTeilsysteme", massnahmenvorschlagTeilsystemeGISModel);
        }

        public ActionResult ApplyInsert(MassnahmenvorschlagTeilsystemeGISModel massnahmenvorschlagTeilsystemeGISModel)
        {
            massnahmenvorschlagTeilsystemeGISModel = UpdateAndValidateGeometry(massnahmenvorschlagTeilsystemeGISModel);
            var isInNewMode = true;
            if (ModelState.IsValid)
            {
                massnahmenvorschlagTeilsystemeGISModel = massnahmenvorschlagTeilsystemeGISModelService.CreateEntity(massnahmenvorschlagTeilsystemeGISModel);
                ModelState.Clear();
                isInNewMode = false;
            }

            PrepareViewBag(isInNewMode);
            return PartialView("EditMassnahmenvorschlagTeilsysteme", massnahmenvorschlagTeilsystemeGISModel);
        }

        public ActionResult Edit(Guid id)
        {
            PrepareViewBag(false);
            return PartialView("EditMassnahmenvorschlagTeilsysteme", massnahmenvorschlagTeilsystemeGISModelService.GetById(id));
        }

        public ActionResult Update(MassnahmenvorschlagTeilsystemeGISModel massnahmenvorschlagTeilsystemeGISModel)
        {
            massnahmenvorschlagTeilsystemeGISModel = UpdateAndValidateGeometry(massnahmenvorschlagTeilsystemeGISModel);
            
            if (ModelState.IsValid)
            {
                massnahmenvorschlagTeilsystemeGISModelService.UpdateEntity(massnahmenvorschlagTeilsystemeGISModel);
                return new EmsgEmptyResult();
            }

            PrepareViewBag(false);
            return PartialView("EditMassnahmenvorschlagTeilsysteme", massnahmenvorschlagTeilsystemeGISModel);
        }

        public ActionResult ApplyUpdate(MassnahmenvorschlagTeilsystemeGISModel massnahmenvorschlagTeilsystemeGISModel)
        {
            massnahmenvorschlagTeilsystemeGISModel = UpdateAndValidateGeometry(massnahmenvorschlagTeilsystemeGISModel);
            if (ModelState.IsValid)
            {
                massnahmenvorschlagTeilsystemeGISModel = massnahmenvorschlagTeilsystemeGISModelService.UpdateEntity(massnahmenvorschlagTeilsystemeGISModel);
                ModelState.Clear();
            }

            PrepareViewBag(false);

            return PartialView("EditMassnahmenvorschlagTeilsysteme", massnahmenvorschlagTeilsystemeGISModel);
        }
        
        private MassnahmenvorschlagTeilsystemeGISModel UpdateAndValidateGeometry(MassnahmenvorschlagTeilsystemeGISModel model)
        {
            if (ModelState.IsValid)
            {
                model = (MassnahmenvorschlagTeilsystemeGISModel) geoJSONParseService.GenerateModelFromGeoJsonString(model);

                if (!massnahmenvorschlagTeilsystemeGISModelService.Validate(model))
                {
                    ModelState.AddModelError(
                        ASTRA.EMSG.Common.ExpressionHelper.GetPropertyName<IAbschnittGISModelBase, string>(m => m.FeatureGeoJSONString),
                        localizationService.GetLocalizedError(ValidationError.GeometryNotOnAchsenSegment));
                }

            }
            return model;
        }
        private MassnahmenvorschlagTeilsystemeGISModel CreateMassnahmenvorschlagTeilsystemeGISModel()
        {
            return new MassnahmenvorschlagTeilsystemeGISModel { Status = StatusTyp.Vorgeschlagen };
        }

        private void PrepareViewBag(bool isCreateMode)
        {
            ViewBag.IsNew = isCreateMode;
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest, MassnahmenvorschlagTeilsystemeGridCommand command)
        {
            var realisierteMassnahmeSummarsichModels = massnahmenvorschlagTeilsystemeGISOverviewModelService.GetCurrentModelsByProjektname(command.ProjektnameFilter);
            return new SerializableDataSourceResult(realisierteMassnahmeSummarsichModels.OrderBy(m => m.Projektname)
                .ToDataSourceResult(dataSourceRequest));
        }

        [HttpGet]
        public ActionResult GetMassnahmenvorschlagTeilsystemeGISAt(double x, double y, double tolerance)
        {
            try
            {
                MassnahmenvorschlagTeilsystemeGISModel massnahmenvorschlagTeilsystemeGIS = massnahmenvorschlagTeilsystemeGISModelService.GetKoordinierteMassnahmeAt(x, y, tolerance);
                return Content(massnahmenvorschlagTeilsystemeGIS.FeatureGeoJSONString, "application/json");
            }
            catch (Exception exc)
            {
                return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
            }
        }

        [HttpGet]
        public ActionResult GetAllMassnahmenvorschlagTeilsystemeGISAt(double x, double y, double tolerance)
        {
            try
            {
                IList<MassnahmenvorschlagTeilsystemeGIS> massnahmenvorschlagTeilsystemeGIS = massnahmenvorschlagTeilsystemeGISModelService.GetAllKoordinierteMassnahmenAt(x, y, tolerance);
                return Content(geoJSONParseService.GenereateGeoJsonStringfromEntities(massnahmenvorschlagTeilsystemeGIS), "application/json");
            }
            catch (Exception exc)
            {
                return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
            }
        }

        [HttpGet]
        public ActionResult GetMassnahmenvorschlagTeilsystemeGISByID(string id)
        {
            try
            {
                MassnahmenvorschlagTeilsystemeGISModel massnahmenvorschlagTeilsystemeGIS = massnahmenvorschlagTeilsystemeGISModelService.GetById(Guid.Parse(id));
                return Content(massnahmenvorschlagTeilsystemeGIS.FeatureGeoJSONString, "application/json");
            }
            catch (Exception exc)
            {
                return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
            }
        }
    }
}
