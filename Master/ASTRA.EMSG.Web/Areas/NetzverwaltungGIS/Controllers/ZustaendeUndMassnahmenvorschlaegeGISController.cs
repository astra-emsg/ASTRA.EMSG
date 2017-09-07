using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.GeoJSON;
using ASTRA.EMSG.Web.Areas.Common.GridCommands;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Resources;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using System.Linq.Expressions;
using Kendo.Mvc.Extensions;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers
{
    public class ZustaendeUndMassnahmenvorschlaegeGISController : Controller
    {
        private readonly IZustandsabschnittGISService zustandsabschnittGISService;
        private readonly IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService;
        private readonly IStrassenabschnittGISService strassenabschnittGISService;
        private readonly IAchsenSegmentService achsensegmentService;
        private readonly IGeoJSONParseService geoJSONParseService;
        private readonly ILocalizationService localizationService;
        private readonly IAbschnittGisValidationService abschnittGisValidationService;
        private readonly IFahrbahnZustandServiceBase fahrbahnZustandServiceBase;
        private readonly ITrottoirZustandServiceBase trottoirZustandServiceBase;

        public ZustaendeUndMassnahmenvorschlaegeGISController
            (
            IZustandsabschnittGISService zustandsabschnittGISService,
            IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService,
            IFahrbahnZustandGISService fahrbahnZustandServiceBase,
            ITrottoirZustandGISService trottoirZustandServiceBase, 
            IStrassenabschnittGISService strassenabschnittGISService, 
            IAchsenSegmentService achsenSegmentService, 
            IGeoJSONParseService geoJSONParseService, 
            ILocalizationService localizationService,
            IAbschnittGisValidationService abschnittGisValidationService
            ) 
        {
            this.zustandsabschnittGISService = zustandsabschnittGISService;
            this.massnahmenvorschlagKatalogService = massnahmenvorschlagKatalogService;
            this.strassenabschnittGISService = strassenabschnittGISService;
            this.achsensegmentService = achsenSegmentService;
            this.geoJSONParseService = geoJSONParseService;
            this.localizationService = localizationService;
            this.abschnittGisValidationService = abschnittGisValidationService;
            this.trottoirZustandServiceBase = trottoirZustandServiceBase;
            this.fahrbahnZustandServiceBase = fahrbahnZustandServiceBase;
        }
        
        [HttpGet]
        public ActionResult GetZustandsabschnittAt(double x, double y, double tolerance)
        {
            try
            {
                ZustandsabschnittGISModel zustandsabschnitt = zustandsabschnittGISService.GetZustandsabschnittAt(x, y,tolerance);
                return Content(zustandsabschnitt.FeatureGeoJSONString, "application/json");
            }
            catch (Exception exc)
            {
                return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
            }
        }

        [HttpGet]
        public ActionResult GetZustandsabschnittByID(string id)
        {
          try
          {
            ZustandsabschnittGISModel zustandsabschnitt = zustandsabschnittGISService.GetById(Guid.Parse(id));
            return Content(zustandsabschnitt.FeatureGeoJSONString, "application/json");
          }
          catch (Exception exc)
          {
            return Content(GeoJSONStrings.GeoJSONFailure(exc.Message), "application/json");
          }
        }

        [HttpPost]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest dataSourceRequest, Guid id)
        {
            zustandsabschnittGISService.DeleteEntity(id);
            return PartialView(GetGridModel(dataSourceRequest, new ZustandsabschnittGISGridCommand()));
        }

        public ActionResult Insert(ZustandsabschnittGISMonsterModel zustandsabschnittGISModel)
        {
            zustandsabschnittGISModel.Stammdaten = ValidateZustandsabschnittGISModel(zustandsabschnittGISModel.Stammdaten);

            if (ModelState.IsValid)
            {
                var createdZustandabschnitt = zustandsabschnittGISService.CreateEntity(zustandsabschnittGISModel.Stammdaten);
                zustandsabschnittGISModel.Fahrbahn.Id = createdZustandabschnitt.Id;
                fahrbahnZustandServiceBase.UpdateZustandsabschnittdetails(zustandsabschnittGISModel.Fahrbahn);
                if (zustandsabschnittGISModel.Trottoir != null)
                {
                    zustandsabschnittGISModel.Trottoir.Id = createdZustandabschnitt.Id;
                    trottoirZustandServiceBase.UpdateZustandsabschnittTrottoirModel(zustandsabschnittGISModel.Trottoir);
                }
                return new EmsgEmptyResult();
            }

            PrepareViewBag(true);
            PrepareKatalogs(zustandsabschnittGISModel);
            return PartialView("EditZustandsabschnitt", zustandsabschnittGISModel);
        }

        public ActionResult ApplyInsert(ZustandsabschnittGISMonsterModel zustandsabschnittGISModel)
        {
            var isInNewMode = true;
            zustandsabschnittGISModel.Stammdaten = ValidateZustandsabschnittGISModel(zustandsabschnittGISModel.Stammdaten);

            if (ModelState.IsValid)
            {
                zustandsabschnittGISModel.Stammdaten = zustandsabschnittGISService.CreateEntity(zustandsabschnittGISModel.Stammdaten);
                zustandsabschnittGISModel.Fahrbahn.Id = zustandsabschnittGISModel.Stammdaten.Id;
                fahrbahnZustandServiceBase.UpdateZustandsabschnittdetails(zustandsabschnittGISModel.Fahrbahn);
                if (zustandsabschnittGISModel.Trottoir != null)
                {
                    zustandsabschnittGISModel.Trottoir.Id = zustandsabschnittGISModel.Stammdaten.Id;
                    trottoirZustandServiceBase.UpdateZustandsabschnittTrottoirModel(zustandsabschnittGISModel.Trottoir);
                }
                ModelState.Clear();
                isInNewMode = false;
            }

            PrepareViewBag(isInNewMode);
            PrepareKatalogs(zustandsabschnittGISModel);
            return PartialView("EditZustandsabschnitt", zustandsabschnittGISModel);
        }

        public ActionResult GetErfassungForm(ZustandsErfassungsmodus zustandsErfassungsmodus, Guid id)
        {
            var model = fahrbahnZustandServiceBase.GetZustandsabschnittdetailsModel(id, zustandsErfassungsmodus);
            return PartialView("ErfassungForm", new ZustandsabschnittGISMonsterModel() { Fahrbahn = model });
        }

        public ActionResult GetCreateErfassungForm(ZustandsErfassungsmodus zustandsErfassungsmodus, Guid id)
        {
            var model = fahrbahnZustandServiceBase.GetDefaultZustandsabschnittdetailsModel(id, zustandsErfassungsmodus);
            return PartialView("ErfassungForm", new ZustandsabschnittGISMonsterModel() { Fahrbahn = model });
        }

        public string GetMassnahmenvorschlagKosten(Guid? massnahmenvorschlagKatalogId)
        {
            return string.Format(FormatStrings.LongDecimalFormat, massnahmenvorschlagKatalogService.GetMassnahmenvorschlagKosten(massnahmenvorschlagKatalogId));
        }

        public ActionResult Update(ZustandsabschnittGISMonsterModel zustandsabschnittGISModel)
        {
            zustandsabschnittGISModel.Stammdaten = ValidateZustandsabschnittGISModel(zustandsabschnittGISModel.Stammdaten);

            if (ModelState.IsValid)
            {
                zustandsabschnittGISService.UpdateEntity(zustandsabschnittGISModel.Stammdaten);
                fahrbahnZustandServiceBase.UpdateZustandsabschnittdetails(zustandsabschnittGISModel.Fahrbahn);
                zustandsabschnittGISModel.Fahrbahn.Zustandsindex = zustandsabschnittGISModel.Fahrbahn.ZustandsindexCalculated;
                if (zustandsabschnittGISModel.Trottoir != null)
                    trottoirZustandServiceBase.UpdateZustandsabschnittTrottoirModel(zustandsabschnittGISModel.Trottoir);
                return new EmsgEmptyResult();
            }

            PrepareViewBag();
            PrepareKatalogs(zustandsabschnittGISModel);
            return PartialView("EditZustandsabschnitt", zustandsabschnittGISModel);
        }

        public ActionResult ApplyUpdate(ZustandsabschnittGISMonsterModel zustandsabschnittGISModel)
        {
            zustandsabschnittGISModel.Stammdaten = ValidateZustandsabschnittGISModel(zustandsabschnittGISModel.Stammdaten);

            if (ModelState.IsValid)
            {
                zustandsabschnittGISModel.Stammdaten = zustandsabschnittGISService.UpdateEntity(zustandsabschnittGISModel.Stammdaten);
                fahrbahnZustandServiceBase.UpdateZustandsabschnittdetails(zustandsabschnittGISModel.Fahrbahn);
                if (zustandsabschnittGISModel.Trottoir != null)
                    trottoirZustandServiceBase.UpdateZustandsabschnittTrottoirModel(zustandsabschnittGISModel.Trottoir);
                ModelState.Clear();
            }

            PrepareViewBag();
            PrepareKatalogs(zustandsabschnittGISModel);
            return PartialView("EditZustandsabschnitt", zustandsabschnittGISModel);
        }

        private string GetValdiationErrors()
        {
            string errors = "";
            foreach (var ms in ModelState)
            {
                if(ms.Value.Errors.Any())
                    errors += ms.Key + ": " + string.Join(" ++ ", ms.Value.Errors.Select(e => e.ErrorMessage)) +  ", ";
            }

            return errors;
        }

        private ZustandsabschnittGISModel ValidateZustandsabschnittGISModel(ZustandsabschnittGISModel zustandsabschnittGISModel)
        {
            if (ModelState.IsValid)
            {
                zustandsabschnittGISModel =
                    (ZustandsabschnittGISModel) geoJSONParseService.GenerateModelFromGeoJsonString(zustandsabschnittGISModel);

                Expression<Func<ZustandsabschnittGISMonsterModel, string>> expression = m => m.Stammdaten.FeatureGeoJSONString;
                if (! abschnittGisValidationService.ValidateOverlap(zustandsabschnittGISModel))
                    ModelState.AddModelError(
                        System.Web.Mvc.ExpressionHelper.GetExpressionText(expression),
                        localizationService.GetLocalizedError(ValidationError.GeometryOverlaps));

                if (!zustandsabschnittGISService.IsZustandsabschnittWithinStrassenabschnitt(zustandsabschnittGISModel))
                    ModelState.AddModelError(
                        System.Web.Mvc.ExpressionHelper.GetExpressionText(expression),
                        localizationService.GetLocalizedError(ValidationError.ZustandAbschnittNotWithin));
            }
            return zustandsabschnittGISModel;
        }

        public ActionResult EditZustandsabschnitt(Guid id)
        {
            var result = new ZustandsabschnittGISMonsterModel();
            result.Stammdaten = zustandsabschnittGISService.GetById(id);
            result.Fahrbahn = fahrbahnZustandServiceBase.GetZustandsabschnittdetailsModel(id);
            result.Trottoir = trottoirZustandServiceBase.GetZustandsabschnittTrottoirModel(id);
            PrepareViewBag();
            PrepareKatalogs(result);
            return PartialView("EditZustandsabschnitt", result);
        }

        public ActionResult CalculateZustandIndex(ZustandsabschnittGISMonsterModel model)
        {
            if (model == null)
                return new EmsgEmptyResult();

            if (model.Fahrbahn == null)
                return new EmsgEmptyResult();

            var zi = string.Format(FormatStrings.ShortDecimalFormat, model.Fahrbahn.ZustandsindexCalculated);

            if (model.Fahrbahn.Erfassungsmodus == ZustandsErfassungsmodus.Grob && !model.Fahrbahn.IsGrobInitializiert)
                zi = string.Empty;

            else if (model.Fahrbahn.Erfassungsmodus == ZustandsErfassungsmodus.Detail && !model.Fahrbahn.IsDetailInitializiert)
                zi = string.Empty;

            return new ContentResult
                {
                    Content = zi
                };
        }

        public ActionResult GetZustandsIndex(Guid id)
        {
            var zustandsabschnittdetailsModel = fahrbahnZustandServiceBase.GetZustandsabschnittdetailsModel(id);

            if (zustandsabschnittdetailsModel == null)
                return new EmsgEmptyResult();

            return new ContentResult
                {
                    Content = string.Format(FormatStrings.ShortDecimalFormat, zustandsabschnittdetailsModel.Zustandsindex)
                };
        }

        public ActionResult CalculateSchadensumme(ZustandsabschnittGISMonsterModel model)
        {
            if (model == null)
                return new EmsgEmptyResult();

            if (model.Fahrbahn == null)
                return new EmsgEmptyResult();

            return new ContentResult
                {
                    Content = string.Format(FormatStrings.NoDecimalFormat, model.Fahrbahn.Schadensumme)
                };
        }

        public ActionResult RefreshErfassungform(ZustandsabschnittGISMonsterModel model)
        {
            return PartialView("ErfassungForm", new ZustandsabschnittGISMonsterModel { Fahrbahn = model.Fahrbahn });
        }

        public ActionResult CreateZustandsabschnitt(Guid id)
        {
            PrepareViewBag(true);
            var model = CreateDefaultZustandsabschnittGISModel(id);
            PrepareKatalogs(model);
            return PartialView("EditZustandsabschnitt", model);
        }

        private ZustandsabschnittGISMonsterModel CreateDefaultZustandsabschnittGISModel(Guid id)
        {
            var strassenabschnittGISModel = strassenabschnittGISService.GetById(id);
            return new ZustandsabschnittGISMonsterModel
                {
                    Stammdaten =
                        new ZustandsabschnittGISModel
                            {
                                StrassenabschnittGIS = id,
                                Strassenname = strassenabschnittGISModel.Strassenname,
                                StrasseBezeichnungVon = strassenabschnittGISModel.BezeichnungVon,
                                StrasseBezeichnungBis = strassenabschnittGISModel.BezeichnungBis,
                                IsLocked = strassenabschnittGISModel.IsLocked,
                                HasTrottoir =
                                    strassenabschnittGISModel.Trottoir != TrottoirTyp.NochNichtErfasst &&
                                    strassenabschnittGISModel.Trottoir != TrottoirTyp.KeinTrottoir
                            },
                    Fahrbahn = new ZustandsabschnittdetailsModel
                        {
                            IsLocked = strassenabschnittGISModel.IsLocked,
                            Erfassungsmodus = ZustandsErfassungsmodus.Manuel,
                            BelastungskategorieTyp = strassenabschnittGISModel.BelastungskategorieTyp,
                            Belag = strassenabschnittGISModel.Belag
                        },
                    Trottoir = new ZustandsabschnittdetailsTrottoirModel
                        {
                            Trottoir = strassenabschnittGISModel.Trottoir,
                            BelastungskategorieTyp = strassenabschnittGISModel.BelastungskategorieTyp
                        }
                };
        }


        private void PrepareViewBag(bool isCreateMode = false)
        {
            ViewBag.IsNew = isCreateMode;
        }

        private void PrepareKatalogs(ZustandsabschnittGISMonsterModel result)
        {
            PrepareViewBagForFahrbahn(result.Fahrbahn.MassnahmenvorschlagKatalog, result.Fahrbahn.BelastungskategorieTyp);
            if (result.Trottoir != null)
                PrepareViewBagForTrottoir(result.Trottoir.LinkeTrottoirMassnahmenvorschlagKatalogId,
                                          result.Trottoir.RechteTrottoirMassnahmenvorschlagKatalogId,
                                          result.Trottoir.BelastungskategorieTyp);
        }

        protected void PrepareViewBagForTrottoir(Guid? selectedTrottoirLinksMassnahmenvorschlagKatalogId, Guid? selectedTrottoirRechtsMassnahmenvorschlagKatalogId, string belastungsKategorieTyp)
        {
            var massnahmenvorschlagKatalogModelList = massnahmenvorschlagKatalogService.GetMassnahmenvorschlagKatalogModelList(MassnahmenvorschlagKatalogTyp.Trottoir, belastungsKategorieTyp);
            ViewBag.TrottoirLinksMassnahmenvorschlagKatalogen = GetMassnamenKatalogDropDownItems(massnahmenvorschlagKatalogModelList, selectedTrottoirLinksMassnahmenvorschlagKatalogId, TextLocalization.EmptyMessage);
            ViewBag.TrottoirRechtsMassnahmenvorschlagKatalogen = GetMassnamenKatalogDropDownItems(massnahmenvorschlagKatalogModelList, selectedTrottoirRechtsMassnahmenvorschlagKatalogId, TextLocalization.EmptyMessage);
        }

        protected void PrepareViewBagForFahrbahn(Guid? selectedTrottoirRechtsMassnahmenvorschlagKatalogId, string belastungsKategorieTyp)
        {
            var massnahmenvorschlagKatalogModelList = massnahmenvorschlagKatalogService.GetMassnahmenvorschlagKatalogModelList(MassnahmenvorschlagKatalogTyp.Fahrbahn, belastungsKategorieTyp);
            ViewBag.MassnahmenvorschlagKatalogen = GetMassnamenKatalogDropDownItems(massnahmenvorschlagKatalogModelList, selectedTrottoirRechtsMassnahmenvorschlagKatalogId, TextLocalization.EmptyMessage);
        }

        private static IEnumerable<DropDownListItem> GetMassnamenKatalogDropDownItems(List<MassnahmenvorschlagKatalogModel> massnahmenvorschlagKatalogModelList, Guid? selectedTrottoirRechtsMassnahmenvorschlagKatalogId, string emptyMessageText = null)
        {
            var selectedTrottoirRechtsModel = massnahmenvorschlagKatalogModelList.FirstOrDefault(m => m.Id == selectedTrottoirRechtsMassnahmenvorschlagKatalogId);
            var trottoirRechtsMassnahmenvorschlagKatalogen = massnahmenvorschlagKatalogModelList
                .ToDropDownItemList(mk => LookupLocalization.ResourceManager.GetString(mk.Typ) ?? mk.Typ, mk => mk.Id, selectedTrottoirRechtsModel, emptyMessageText)
                .OrderBy(ddi => ddi.Text);
            return trottoirRechtsMassnahmenvorschlagKatalogen;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Overview()
        {
            return PartialView();
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest, ZustandsabschnittGISGridCommand command)
        {
            if (dataSourceRequest.Sorts == null)
                dataSourceRequest.Sorts = new List<SortDescriptor>();

            dataSourceRequest.Sorts.Add(new SortDescriptor { Member = "Strassenname" });
            dataSourceRequest.Sorts.Add(new SortDescriptor { Member = "Sreassenabschnittsnummer" });
            dataSourceRequest.Sorts.Add(new SortDescriptor { Member = "StrasseBezeichnungBis" });
            dataSourceRequest.Sorts.Add(new SortDescriptor { Member = "Abschnittsnummer" });
            dataSourceRequest.Sorts.Add(new SortDescriptor { Member = "BezeichnungVon" });
            
            return Json(GetGridModel(dataSourceRequest, command));
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest, ZustandsabschnittGISGridCommand command)
        {
            return new SerializableDataSourceResult(
                zustandsabschnittGISService.GetOverviewList(new ZustandsabschnittGISOverviewFilter
                {
                    Strassenname = command.StrassennameFilter
                }).ToDataSourceResult(dataSourceRequest));
        }

        private void PrepareViewBag()
        {
            ViewBag.IsNew = false;
        }
    }

    [Serializable]
    public class ZustandsabschnittGISMonsterModel
    {
        public ZustandsabschnittGISModel Stammdaten { get; set; }

        public ZustandsabschnittdetailsModel Fahrbahn { get; set; }

        public ZustandsabschnittdetailsTrottoirModel Trottoir { get; set; }

        public int SelectedTabIndex { get; set; }
    }
}
