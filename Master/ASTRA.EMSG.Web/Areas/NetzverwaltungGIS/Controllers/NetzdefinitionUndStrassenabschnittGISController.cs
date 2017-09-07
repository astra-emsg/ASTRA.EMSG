using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.GeoJSON;
using ASTRA.EMSG.Web.Areas.Common.GridCommands;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Resources;
using System.Linq;
using ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers.Common;
using ASTRA.EMSG.Business.Services;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using ExpressionHelper = ASTRA.EMSG.Common.ExpressionHelper;


namespace ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers
{
    public class NetzdefinitionUndStrassenabschnittGISController : NetzverwaltungGISController
    {
        private readonly IStrassenabschnittGISService strassenabschnittGISService;
        private readonly IAchsenSegmentService achsensegmentService;
        private readonly IBelastungskategorieService belastungskategorieService;
        private readonly IGeoJSONParseService geoJSONParseService;
        private readonly ILocalizationService localizationService;
        private readonly IInspektionsRouteGISService inspektionsRouteGISService;
        private readonly IAbschnittGisValidationService abschnittGisValidationService;

        public NetzdefinitionUndStrassenabschnittGISController(IStrassenabschnittGISService strassenabschnittGISService,
            IAchsenSegmentService achsensegmentService, 
            IBelastungskategorieService belastungskategorieService, 
            IGeoJSONParseService geoJSONParseService, 
            ILocalizationService localizationService, 
            IInspektionsRouteGISService inspektionsRouteGISService,
            IAbschnittGisValidationService abschnittGisValidationService) :
            base(strassenabschnittGISService)
        {
            
            this.localizationService = localizationService;
            this.geoJSONParseService = geoJSONParseService;
            this.strassenabschnittGISService = strassenabschnittGISService;
            this.achsensegmentService = achsensegmentService;
            this.belastungskategorieService = belastungskategorieService;
            this.inspektionsRouteGISService = inspektionsRouteGISService;
            this.abschnittGisValidationService = abschnittGisValidationService;
        }

        public ActionResult Index()
        {
            return View();
        }
                
        [HttpGet]
        public ActionResult GetAxisAt(double? x, double? y, double tolerance)
        {
            if (x == null && y == null)
            {
                return Content(GeoJSONStrings.GeoJSONFailure("Keine Koordinaten angegeben"), "application/json");
            }
            else 
            {
                try
                {
                    return Content(achsensegmentService.GetNearestAchsenSegment((double)x,(double)y, tolerance), "application/json");
                }
                catch (Exception exc)
                {
                    return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
                }
                
            }
        }

        [HttpGet]
        public ActionResult GetAxisCollection(string ids)
        {
            string achsensegmentCollection = achsensegmentService.GetAchsensegmentCollection(ids);

            return Content(achsensegmentCollection, "application/json");
        }



        public ActionResult Update(StrassenabschnittGISModel strassenabschnittGisModel)
        {
            strassenabschnittGisModel = ValidateStrassenabschnittGisModelGeometry(strassenabschnittGisModel);

            if (ModelState.IsValid)
            {
                strassenabschnittGISService.UpdateEntity(strassenabschnittGisModel);
                inspektionsRouteGISService.UpdateInspektionsroutenGeometry(strassenabschnittGisModel);
                return new EmsgEmptyResult();
            }

            PrepareViewBag();
            return PartialView("EditStrassenabschnitt", strassenabschnittGisModel);
        }

        public ActionResult ApplyUpdate(StrassenabschnittGISModel strassenabschnittGisModel)
        {
            strassenabschnittGisModel = ValidateStrassenabschnittGisModelGeometry(strassenabschnittGisModel);

            if (ModelState.IsValid)
            {
                strassenabschnittGisModel = strassenabschnittGISService.UpdateEntity(strassenabschnittGisModel);
                inspektionsRouteGISService.UpdateInspektionsroutenGeometry(strassenabschnittGisModel);
                ModelState.Clear();
            }

            PrepareViewBag();

            return PartialView("EditStrassenabschnitt", strassenabschnittGisModel);
        }

        public ActionResult Insert(StrassenabschnittGISModel strassenabschnittGisModel)
        {
            strassenabschnittGisModel = ValidateStrassenabschnittGisModelGeometry(strassenabschnittGisModel);

            if (ModelState.IsValid)
            {
                strassenabschnittGISService.CreateEntity(strassenabschnittGisModel);
                return new EmsgEmptyResult();
            }

            PrepareViewBag(true);

            return PartialView("EditStrassenabschnitt", strassenabschnittGisModel);
        }

        public ActionResult ApplyInsert(StrassenabschnittGISModel strassenabschnittGisModel)
        {
            var isInNewMode = true;
            strassenabschnittGisModel = ValidateStrassenabschnittGisModelGeometry(strassenabschnittGisModel);

            if (ModelState.IsValid)
            {
                strassenabschnittGisModel = strassenabschnittGISService.CreateEntity(strassenabschnittGisModel);
                ModelState.Clear();
                isInNewMode = false;
            }

            PrepareViewBag(isInNewMode);

            return PartialView("EditStrassenabschnitt", strassenabschnittGisModel);
        }

        private StrassenabschnittGISModel ValidateStrassenabschnittGisModelGeometry(StrassenabschnittGISModel strassenabschnittGisModel)
        {
            if (ModelState.IsValid)
            {
                strassenabschnittGisModel =
                    (StrassenabschnittGISModel)geoJSONParseService.GenerateModelFromGeoJsonString(strassenabschnittGisModel);

                if (!abschnittGisValidationService.ValidateOverlap(strassenabschnittGisModel))
                    ModelState.AddModelError(
                        ExpressionHelper.GetPropertyName<IAbschnittGISModelBase, string>(m => m.FeatureGeoJSONString),
                        localizationService.GetLocalizedError(ValidationError.GeometryOverlaps));

                if (!strassenabschnittGISService.ZustandsabschnittWithinStrassenabschnitt(strassenabschnittGisModel))
                    ModelState.AddModelError(
                        ExpressionHelper.GetPropertyName<IAbschnittGISModelBase, string>(m => m.FeatureGeoJSONString),
                        localizationService.GetLocalizedError(ValidationError.ZustandAbschnittNotWithin));
                if (!strassenabschnittGISService.IsStrassenabschnittOnAchsensegment(strassenabschnittGisModel))
                    ModelState.AddModelError(
                        ExpressionHelper.GetPropertyName<IAbschnittGISModelBase, string>(m => m.FeatureGeoJSONString),
                        localizationService.GetLocalizedError(ValidationError.GeometryNotOnAchsenSegment));
            }
            return strassenabschnittGisModel;
        }

        public ActionResult EditStrassenabschnitt(Guid id)
        {
            PrepareViewBag();
            return PartialView("EditStrassenabschnitt", strassenabschnittGISService.GetById(id));
        }        
        
        public ActionResult CreateStrassenabschnitt()
        {
            PrepareViewBag(true);
            return PartialView("EditStrassenabschnitt", CreateDefaultStrassenabschnittGISModel());
        }

        public ActionResult SplitStrassenabschnitt(Guid id)
        {
            return PartialView(new SplitStrassenabschnittGISModel { StrassenabschnittId = id, Count = 2 });
        }

        [HttpGet]
        public ActionResult SplitStrassenabschnittAt(Guid strassenabschnitId, string x, string y)
        {
            strassenabschnittGISService.SplitStrassenabschnittGISAtXY(strassenabschnitId, x, y);
            return Content(GeoJSONStrings.GeoJSONSuccess("success"), "application/json");
        }

        public ActionResult Split(SplitStrassenabschnittGISModel splitStrassenabschnittModel)
        {
            if (ModelState.IsValid)
            {
                //strassenabschnittGISService.SplitStrassenabschnittGIS(splitStrassenabschnittModel);
                return new EmsgEmptyResult();
            }

            return PartialView("SplitStrassenabschnitt", splitStrassenabschnittModel);
        }

        private static StrassenabschnittGISModel CreateDefaultStrassenabschnittGISModel()
        {
            return new StrassenabschnittGISModel
                       {
                           Trottoir = TrottoirTyp.NochNichtErfasst,
                           Belag = BelagsTyp.Asphalt,
                           Strasseneigentuemer = EigentuemerTyp.Gemeinde
                       };
        }

        [HttpPost]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest dataSourceRequest, Guid id)
        {
            strassenabschnittGISService.DeleteEntity(id);
            return PartialView(GetGridModel(dataSourceRequest, new StrassenabschnittGISGridCommand()));
        }

        private void PrepareViewBag(bool isCreateMode = false)
        {
            ViewBag.Belastungskategorien = belastungskategorieService.AllBelastungskategorieModel
                .ToDropDownItemList(bk => LookupLocalization.ResourceManager.GetString(bk.Typ), bk => bk.Id.ToString(), emptyItemText: string.Empty);

            ViewBag.IsNew = isCreateMode;

            var belastungskategorieModels = belastungskategorieService.AllBelastungskategorieModel
                .ToDictionary(bkm => bkm.Id.ToString(), bkm => new
                    {
                        Id = bkm.Id.ToString(),
                        bkm.Typ,
                        bkm.DefaultBreiteFahrbahn,
                        bkm.DefaultBreiteTrottoirLinks,
                        bkm.DefaultBreiteTrottoirRechts,
                        AllowedBelagList = bkm.AllowedBelagList.Select(bt => bt.ToString()).ToList()
                    });
            var javaScriptSerializer = new JavaScriptSerializer();
            ViewBag.BelastungskategorienDictionary = javaScriptSerializer.Serialize(belastungskategorieModels);
        }

        public ActionResult PopupForm()
        {
            ViewBag.DisableMenu = true;
            return View("Form");
        }

        public ActionResult Overview()
        {
            return PartialView();
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest, StrassenabschnittGISGridCommand gridCommand)
        {
            return Json(GetGridModel(dataSourceRequest, gridCommand));
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest, StrassenabschnittGISGridCommand gridCommand)
        {
            var strassenabschnittModels = strassenabschnittGISService.GetCurrentModelsByStrassenname(gridCommand.StrassennameFilter, gridCommand.Ortsbezeichnung);

            return new SerializableDataSourceResult(
                strassenabschnittModels.OrderBy(m => m.Strassenname)
                    .ThenBy(m => m.Abschnittsnummer)
                    .ToDataSourceResult(dataSourceRequest));
        }
    }   
}
