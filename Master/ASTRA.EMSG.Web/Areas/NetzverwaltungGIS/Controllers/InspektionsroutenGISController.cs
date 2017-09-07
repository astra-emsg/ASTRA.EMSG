using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Common.Master.GeoJSON;
using System;
using ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers.Common;
using System.Linq;
using ASTRA.EMSG.Business.Services.PackageService;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Business.BusinessExceptions;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Common.Master.Logging;
using ASTRA.EMSG.Business.Services.GIS;
using NetTopologySuite.Geometries;
using GeoAPI.Geometries;
using ASTRA.EMSG.Business.Entities.GIS;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Web.SessionState;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class InspektionsroutenGISController : NetzverwaltungGISController
    {
        private readonly IStrassenabschnittGISService strassenabschnittGISService;
        private readonly IInspektionsRouteGISOverviewService inspektionsRouteGISOverviewService;
        private readonly IInspektionsRouteGISService inspektionsRouteGISService;
        private readonly IPackageService packageService;
        private readonly ISessionService sessionService;
        private readonly IInspektionsRtStrAbschnitteService inspektionsRtStrAbschnitteService;
        private readonly IInspektionsRouteStatusverlaufService inspektionsRouteStatusverlaufService;
        private readonly IInspektionsRouteLockingService inspektionsRouteLockingService;
        private readonly IEreignisLogService ereignisLogService;
        private readonly ILocalizationService localizationService;
        private readonly ICheckInService checkInService;
        private readonly ICheckOutService checkOutService;
        private readonly IGeoJSONParseService geoJSONParseService;

        private readonly string exportFilePattern = "~/App_Data/ExportPackages/{0}.emsge";

        public InspektionsroutenGISController(IStrassenabschnittGISService strassenabschnittGISService,
            IInspektionsRouteGISService inspektionsRouteGISService,
            IPackageService packageService,
            IInspektionsRouteGISOverviewService inspektionsRouteGISOverviewService,
            ISessionService sessionService,
            IInspektionsRtStrAbschnitteService inspektionsRtStrAbschnitteService,
            IInspektionsRouteStatusverlaufService inspektionsRouteStatusverlaufService,
            IInspektionsRouteLockingService inspektionsRouteLockingService,
            IEreignisLogService ereignisLogService,
            ILocalizationService localizationService,
            ICheckInService checkInService,
            ICheckOutService checkOutService,
            IGeoJSONParseService geoJSONParseService)
            : base(strassenabschnittGISService)
        {
            this.localizationService = localizationService;
            this.strassenabschnittGISService = strassenabschnittGISService;
            this.inspektionsRouteGISService = inspektionsRouteGISService;
            this.packageService = packageService;
            this.inspektionsRouteGISOverviewService = inspektionsRouteGISOverviewService;
            this.sessionService = sessionService;
            this.inspektionsRtStrAbschnitteService = inspektionsRtStrAbschnitteService;
            this.inspektionsRouteStatusverlaufService = inspektionsRouteStatusverlaufService;
            this.inspektionsRouteLockingService = inspektionsRouteLockingService;
            this.ereignisLogService = ereignisLogService;
            this.checkInService = checkInService;
            this.checkOutService = checkOutService;
            this.geoJSONParseService = geoJSONParseService;
        }
        public ActionResult Index()
        {

            return View();
        }


        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            return Json(GetGridModel(dataSourceRequest));
        }

        //Make the Details Grid happy
        public ActionResult GetDetails()
        {
            return Json(new SerializableGridModel<InspektionsRouteGISOverviewModel>());
        }

        public ActionResult Delete([DataSourceRequest] DataSourceRequest dataSourceRequest, Guid id)
        {
            if (!inspektionsRouteGISService.GetById(id).IsLocked)
            {
                inspektionsRouteGISService.DeleteEntity(id);
            }
            return PartialView(GetGridModel(dataSourceRequest));
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            return new SerializableDataSourceResult(
                inspektionsRouteGISOverviewService.GetCurrentModels()
                    .OrderBy(m => m.Bezeichnung)
                    .ToDataSourceResult(dataSourceRequest));
        }

        public ActionResult Overview()
        {
            return PartialView();
        }

        public ActionResult Export(List<Guid> ids, bool exportBackground, string tempFileName)
        {
            if (ids != null)
            {
                RemoveOldPackages();

                var inspektionsrouten = inspektionsRouteGISService.GetCurrentEntities().Where(ir => ids.Contains(ir.Id));
                ereignisLogService.LogEreignis(EreignisTyp.InspektionsRoutenExport, new Dictionary<string, object>() { { "Inspektionsrouten", string.Join(", ", inspektionsrouten.Select(ir => ir.Bezeichnung)) } });

                var cogModelList = new List<CheckOutsGISModel>();
                foreach (var id in ids)
                {
                    var cogModel = new CheckOutsGISModel();
                    cogModel.InspektionsRouteGIS = id;
                    cogModel.Mandant = sessionService.SelectedMandantId;
                    cogModel.CheckOutDatum = DateTime.Now;
                    cogModelList.Add(cogModel);
                }

                var memoryStream = packageService.Export(checkOutService.CheckOutData(ids, exportBackground), cogModelList).Stream as MemoryStream;
                var filePath = Server.MapPath(String.Format(exportFilePattern, tempFileName));
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    memoryStream.WriteTo(fs);
                    fs.Flush();
                }
                inspektionsRouteGISService.LockInspektionsRouten(ids);

                foreach (var id in ids)
                {
                    var inspektionsroute = inspektionsRouteGISService.GetInspektionsRouteById(id);
                    inspektionsRouteStatusverlaufService.HistorizeRouteExportiert(inspektionsroute);
                }
            }
            return new EmsgEmptyResult();
        }

        public ActionResult GetStrassenabschnitteFromInspektionsroute(Guid id)
        {
            if (id != Guid.Empty)
            {
                string geoJSONstring = inspektionsRouteGISService.GetStrassenabschnitteFromInspektionsroute(id);
                return Content(geoJSONstring, "application/json");
            }
            else
            {
                return Content(GeoJSONStrings.GeoJSONFailure("Empty ID!"), "application/json");
            }
        }

        [HttpPost]
        public ActionResult IsInspektionsroutenExportReady(List<Guid> ids, bool exportBackground)
        {
            bool isReady = true;
            foreach (var id in ids)
            {
                var inspektionsroute = inspektionsRouteGISService.GetInspektionsRouteById(id);
                if (inspektionsroute.Status != InspektionsRouteStatus.RouteExportiert)
                {
                    isReady = false;
                    break;
                }
            }

            return Json(new
            {
                ready = isReady
            });
        }

        public ActionResult GetLastExport(string tempFileName)
        {
            var filePath = Server.MapPath(String.Format(exportFilePattern, tempFileName));

            string fileDownloadName = "Route.emsge";
            return File(filePath, "application/x-compressed", fileDownloadName);
        }

        private void RemoveOldPackages()
        {
            string exportFolderPath = Server.MapPath(exportFilePattern.Substring(0, exportFilePattern.LastIndexOf("/")));

            var exportFolder = new DirectoryInfo(exportFolderPath);
            foreach (var package in exportFolder.GetFiles("*.emsge"))
            {
                if (package.CreationTimeUtc < DateTime.UtcNow.AddDays(-2) && System.IO.File.Exists(package.FullName))
                {
                    System.IO.File.Delete(package.FullName);
                }
            }
        }

        public ActionResult ImportInspektionsrouten()
        {
            return PartialView();
        }

        public ActionResult Upload(IEnumerable<HttpPostedFileBase> importFiles)
        {
            sessionService.LastImportErrorList = new List<string>();
            ImportResult importResult = new ImportResult();
            try
            {
                importResult = packageService.ImportZip(importFiles.Single().InputStream);
                if (!importResult.Errors.Any())
                {
                    importResult = checkInService.CheckInData(importResult);
                }
            }
            catch (ImportException e)
            {
                Loggers.ApplicationLogger.Error(e.Message, e);
                importResult.Errors.Add(e.Message);
            }
            catch (Exception e)
            {
                Loggers.ApplicationLogger.Error(e.Message, e);
                importResult.Errors.Add(localizationService.GetLocalizedError(ValidationError.InvalidPackage));
                //sessionService.LastImportErrorList.Add("Sample Error! (Das soll auch lokalisiert sein!)");
            }
            foreach (string error in importResult.Errors)
            {
                //Generate error list if any (Send back empty list if there was no error)
                sessionService.LastImportErrorList.Add(error);
            }
            if (importResult.Errors.Count > 0)
                return PartialView("ImportResult", importResult.Errors);
            return Content("");
        }

        public ActionResult Create()
        {
            PrepareViewBag(true);
            return PartialView("EditInspektionsroute", CreateDefaultInspektionsRouteGISModel());
        }

        public ActionResult Insert(InspektionsRouteGISModel inspektionsRouteGISModel)
        {
            if (ModelState.IsValid)
            {
                inspektionsRouteGISService.CreateEntity(inspektionsRouteGISModel);
                return new EmsgEmptyResult();
            }
            PrepareViewBag(true);
            return PartialView("EditInspektionsroute", inspektionsRouteGISModel);
        }

        public ActionResult Update(InspektionsRouteGISModel inspektionsRouteGISModel)
        {
            if (ModelState.IsValid)
            {
                inspektionsRouteGISService.UpdateEntity(inspektionsRouteGISModel);
                return new EmsgEmptyResult();
            }
            PrepareViewBag(false);
            return PartialView("EditInspektionsroute", inspektionsRouteGISModel);
        }

        public ActionResult Edit(Guid id)
        {
            PrepareViewBag(false);
            return PartialView("EditInspektionsroute", inspektionsRouteGISService.GetById(id));
        }

        private InspektionsRouteGISModel CreateDefaultInspektionsRouteGISModel()
        {
            return new InspektionsRouteGISModel();
        }

        public ActionResult GetInspektionsRtStrAbschnitte(Guid id)
        {
            return Json(inspektionsRtStrAbschnitteService.GetByStrassenabschnittId(id));
        }

        public ActionResult ShowStatusverlauf(Guid id)
        {
            var inspektionsRouteGISOverviewModel = inspektionsRouteGISOverviewService.GetById(id);

            return PartialView(new InspektionsRouteStatusverlaufOverviewModel
            {
                Id = id,
                Bezeichnung = inspektionsRouteGISOverviewModel.Bezeichnung,
                InspektionsRouteStatusverlaufModels = inspektionsRouteStatusverlaufService.GetInspektionsRouteStatusverlaufModels(id)
            });
        }

        private void PrepareViewBag(bool isCreateMode)
        {
            ViewBag.IsNew = isCreateMode;
        }

        [HttpGet]
        public ActionResult GetInspektionsRouteGISAt(double x, double y, double tolerance)
        {
            try
            {
                InspektionsRouteGISModel inspektionsRoute = inspektionsRouteGISService.GetInspektionsRouteGISAt(x, y, tolerance);
                return Content(inspektionsRoute.FeatureGeoJSONString, "application/json");
            }
            catch (Exception exc)
            {
                return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
            }
        }

        public ActionResult GetStrassenabschnittByBbox(double minX, double minY, double maxX, double maxY)
        {
            try
            {
                Coordinate bottomLeft = new Coordinate(minX, minY);
                Coordinate topRight = new Coordinate(maxX, maxY);
                Coordinate bottomRight = new Coordinate(maxX, minY);
                Coordinate topLeft = new Coordinate(minX, maxY);

                ILinearRing linearRing = new LinearRing(new Coordinate[] { topLeft, topRight, bottomRight, bottomLeft, topLeft });

                IGeometry filterGeom = new NetTopologySuite.Geometries.Polygon(linearRing, GISService.CreateGeometryFactory());

                IList<StrassenabschnittGIS> strassenabschnitte = strassenabschnittGISService.GetCurrentBySpatialFilter(filterGeom);

                strassenabschnitte = strassenabschnitte.Where(s => s.Shape.Intersects(filterGeom)).ToList();
                return Content(geoJSONParseService.GenereateGeoJsonStringfromEntities(strassenabschnitte), "application/json");
            }
            catch (Exception exc)
            {
                return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
            }
        }

        [HttpGet]
        public ActionResult GetAvailableInspektionsRouteGISAt(double x, double y, double tolerance)
        {
            try
            {
                InspektionsRouteGISModel inspektionsRoute = inspektionsRouteGISService.GetInspektionsRouteGISAt(x, y, tolerance);
                var inspektionsRouteGISOverviewModel = inspektionsRouteGISOverviewService.GetById(inspektionsRoute.Id);
                if (inspektionsRouteGISOverviewModel.Status != Business.Entities.GIS.InspektionsRouteStatus.RouteExportiert)
                {
                    return Content(inspektionsRoute.FeatureGeoJSONString, "application/json");
                }
                else
                {
                    return Content(GeoJSONStrings.GeoJSONFailure(String.Format(@"Inspektionsroute: {0} {1},  is already exported", inspektionsRoute.Id, inspektionsRoute.Bezeichnung)), "application/json");
                }
            }
            catch (Exception exc)
            {
                return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
            }
        }
    }
}
