using System.Web.Mvc;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Common.Master.GeoJSON;
using System;
using GeoAPI.Geometries;
using ASTRA.EMSG.Business.Entities.GIS;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Models.GIS;
using ExpressionHelper = ASTRA.EMSG.Common.ExpressionHelper;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Common.GridCommands;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Implementation;
using ASTRA.EMSG.Common.Utils;
using Resources;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers
{
    [AccessRights(Rolle.Benutzeradministrator)]
    public class AchsenController : Controller
    {
        private readonly IAchsenSegmentService achsensegmentService;
        private readonly IGeoJSONParseService geoJSONParseService;
        private readonly ILocalizationService localizationService;
        private readonly IAchsenReferenzUpdateService achsenReferenzUpdateService;

        public AchsenController(IAchsenSegmentService achsensegmentService, IGeoJSONParseService geoJSONParseService, ILocalizationService localizationService, IAchsenReferenzUpdateService achsenReferenzUpdateService)
        {
            this.achsensegmentService = achsensegmentService;
            this.geoJSONParseService = geoJSONParseService;
            this.localizationService = localizationService;
            this.achsenReferenzUpdateService = achsenReferenzUpdateService;
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest, AchsenGridCommand command)
        {
            return Json(GetGridModel(dataSourceRequest, command));
        }

        public ActionResult Create()
        {
            var model = new AchsenSegmentModel();
            PrepareViewBag(true, model);
            return PartialView("EditAchse", model);
        }

        public ActionResult Edit(Guid id)
        {
            var model = achsensegmentService.GetById(id);
            PrepareViewBag(false, model);
            return PartialView("EditAchse", model);
        }

        public ActionResult Update(AchsenSegmentModel achsenSegmentModel)
        {
            if (ModelState.IsValid)
            {
                UpdateAchsenSegmentModel(achsenSegmentModel);
                return new EmsgEmptyResult();
            }
            PrepareViewBag(false, achsenSegmentModel);
            return PartialView("EditAchse", achsenSegmentModel);
        }

        public ActionResult ApplyUpdate(AchsenSegmentModel achsenSegmentModel)
        {
            if (ModelState.IsValid)
            {
                UpdateAchsenSegmentModel(achsenSegmentModel);
                ModelState.Clear();
            }
            PrepareViewBag(false, achsenSegmentModel);
            return PartialView("EditAchse", achsenSegmentModel);
        }

        public ActionResult Delete([DataSourceRequest] DataSourceRequest dataSourceRequest, Guid id)
        {
            achsenReferenzUpdateService.DeleteAchsenReferenzen(id);
            achsensegmentService.DeleteEntity(id);
            return PartialView(GetGridModel(dataSourceRequest, new AchsenGridCommand()));
        }

        [HttpPost]
        public ActionResult SearchModifiedAchen(string geoJson)
        {
            return Json(achsenReferenzUpdateService.GetModifiedEntities(geoJson));
        }

        public ActionResult Insert(AchsenSegmentModel achsenSegmentModel)
        {
            if (ModelState.IsValid)
            {
                CreateAchsenSegmentModel(achsenSegmentModel);
                return new EmsgEmptyResult();
            }
            PrepareViewBag(true, achsenSegmentModel);
            return PartialView("EditAchse", achsenSegmentModel);
        }

        public ActionResult ApplyInsert(AchsenSegmentModel achsenSegmentModel)
        {
            var isInNewMode = true;
            if (ModelState.IsValid)
            {
                CreateAchsenSegmentModel(achsenSegmentModel);
                ModelState.Clear();
                isInNewMode = false;
            }
            PrepareViewBag(isInNewMode, achsenSegmentModel);
            return PartialView("EditAchse", achsenSegmentModel);
        }

        private AchsenSegmentModel PrepareAchsenSegmentModel(AchsenSegmentModel achsenSegmentModel)
        {
            achsenSegmentModel = this.geoJSONParseService.GenerateAchsenSegmentModelFromGeoJsonString(achsenSegmentModel);

            //Snapping allows to easily create duplicates (by clicking the EXACT same spot twice)
            //this prevents duplicate coordinates (this should be prevented by the create tool but since duplicate Coordinates can cause problems another check here)
            CoordinateList list = new CoordinateList(achsenSegmentModel.Shape.Coordinates, false);
            if (list.Count > 1)
            {
                achsenSegmentModel.Shape = new LineString(new CoordinateArraySequence(list.ToCoordinateArray()), achsenSegmentModel.Shape.Factory);
            }
            
            return achsenSegmentModel;
        }
        private void UpdateAchsenSegmentModel(AchsenSegmentModel achsenSegmentModel)
        {
            achsenSegmentModel = PrepareAchsenSegmentModel(achsenSegmentModel);

            achsenReferenzUpdateService.UpdateAchsenReferenzen(achsenSegmentModel, true);
            achsensegmentService.UpdateEntity(achsenSegmentModel);
        }

        private void CreateAchsenSegmentModel(AchsenSegmentModel achsenSegmentModel)
        {
            achsenSegmentModel = PrepareAchsenSegmentModel(achsenSegmentModel);

            achsensegmentService.CreateEntity(achsenSegmentModel);
        }
        
        public ActionResult GetAchseAt(double? x, double? y, double tolerance)
        {
            if (x == null && y == null)
            {
                return Content(GeoJSONStrings.GeoJSONFailure("Keine Koordinaten angegeben"), "application/json");
            }
            else
            {
                try
                {
                    return Content(achsensegmentService.GetNearestAchsenSegment((double)x, (double)y, tolerance), "application/json");
                }
                catch (Exception exc)
                {
                    return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
                }

            }
        }

        public ActionResult GetAchseById(string id)
        {
            string achsensegmentCollection = achsensegmentService.GetAchsensegmentCollection(id);

            return Content(achsensegmentCollection, "application/json");
        }


        public ActionResult GetAchseByBbox(double minX, double minY, double maxX, double maxY)
        {
            try
            {
                Coordinate bottomLeft = new Coordinate(minX, minY);
                Coordinate topRight = new Coordinate(maxX, maxY);
                Coordinate bottomRight = new Coordinate(maxX, minY);
                Coordinate topLeft = new Coordinate(minX, maxY);

                ILinearRing linearRing = new LinearRing(new Coordinate[] { topLeft, topRight, bottomRight, bottomLeft, topLeft });

                IGeometry filterGeom = new NetTopologySuite.Geometries.Polygon(linearRing, GISService.CreateGeometryFactory());

                IList<AchsenSegment> achsseg = achsensegmentService.GetCurrentBySpatialFilter(filterGeom);

                achsseg = achsseg.Where(s => s.Shape.Intersects(filterGeom)).ToList();
                return Content(geoJSONParseService.GenereateGeoJsonStringfromAchsenSegment(achsseg), "application/json");
            }
            catch (Exception exc)
            {
                return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
            }
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest, AchsenGridCommand command)
        {
            var models = achsensegmentService.GetCurrentModelsByName(command.AchsenName);
            return new SerializableDataSourceResult(models.OrderBy(m => m.AchsenName).ToDataSourceResult(dataSourceRequest));
        }

        public ActionResult Index()
        {
            return View();
        }

        private void PrepareViewBag(bool isCreateMode, AchsenSegmentModel model)
        {
            ViewBag.IsNew = isCreateMode;
            ViewBag.IsLocked = false;
            if (!isCreateMode)
            {
                var segment = achsensegmentService.GetCurrentEntities().FirstOrDefault(s => s.Id == model.Id);
                if (segment != null)
                {
                    ViewBag.IsLocked = segment.AchsenReferenzen.Any(r => 
                        r.ReferenzGruppe != null && 
                        r.ReferenzGruppe.StrassenabschnittGIS != null &&
                        r.ReferenzGruppe.StrassenabschnittGIS.IsLocked);
                }
            }
        }
    }
}