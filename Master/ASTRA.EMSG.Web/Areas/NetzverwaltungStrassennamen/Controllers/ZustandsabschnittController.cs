using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Resources;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using ASTRA.EMSG.Web.Infrastructure.Security;
using Kendo.Mvc.Extensions;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungStrassennamen.Controllers
{
    public class ZustandsabschnittController : Controller
    {
        private readonly IZustandsabschnittService zustandsabschnittService;
        private readonly IStrassenabschnittService strassenabschnittService;
        private readonly IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService;
        private readonly IFahrbahnZustandServiceBase fahrbahnZustandServiceBase;
        private readonly ITrottoirZustandServiceBase trottoirZustandServiceBase;

        public ZustandsabschnittController(
            IZustandsabschnittService zustandsabschnittService,
            IStrassenabschnittService strassenabschnittService,
            IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService,
            ITrottoirZustandService trottoirZustandServiceBase,
            IFahrbahnZustandService fahrbahnZustandServiceBase
            )
        {
            this.zustandsabschnittService = zustandsabschnittService;
            this.strassenabschnittService = strassenabschnittService;
            this.massnahmenvorschlagKatalogService = massnahmenvorschlagKatalogService;
            this.trottoirZustandServiceBase = trottoirZustandServiceBase;
            this.fahrbahnZustandServiceBase = fahrbahnZustandServiceBase;
        }

        public ActionResult Index(Guid id)
        {
            ViewBag.DefaultZustandsabschnittModel = CreateDefaultZustandsabschnittModel(id);

            ViewBag.ActionInfo = new ActionInfo("NetzverwaltungStrassennamen", "NetzdefinitionUndStrassenabschnitt", "Index");
            var menuItemModel = new MenuItemModel("NetzverwaltungStrassennamen", "Zustandsabschnitt", "Index");
            menuItemModel.Id = id;
            menuItemModel.Text = MenuLocalization.ResourceManager.GetString(menuItemModel.ResourceKey);

            ViewBag.MenuItemModels = new List<MenuItemModel> { menuItemModel };
            ViewBag.StrassenabschnittId = id;
            return PartialView();
        }

        private ZustandsabschnittMonsterModel CreateDefaultZustandsabschnittModel(Guid id)
        {
            var strassenabschnittModel = strassenabschnittService.GetById(id);
            return new ZustandsabschnittMonsterModel
            {
                Stammdaten = new ZustandsabschnittModel()
                {
                    Strassenabschnitt = id,
                    Strassenname = strassenabschnittModel.Strassenname,
                    StrasseLaenge = strassenabschnittModel.Laenge ?? 0,
                    StrasseBezeichnungBis = strassenabschnittModel.BezeichnungBis,
                    StrasseBezeichnungVon = strassenabschnittModel.BezeichnungVon,
                    Sreassenabschnittsnummer = strassenabschnittModel.Abschnittsnummer,
                    Erfassungsmodus = ZustandsErfassungsmodus.Manuel,
                    HasTrottoir = strassenabschnittModel.Trottoir != TrottoirTyp.NochNichtErfasst && strassenabschnittModel.Trottoir != TrottoirTyp.KeinTrottoir,
                },
                Fahrbahn = new ZustandsabschnittdetailsModel
                              {
                                  Erfassungsmodus = ZustandsErfassungsmodus.Manuel,
                                  BelastungskategorieTyp = strassenabschnittModel.BelastungskategorieTyp,
                                  Belag = strassenabschnittModel.Belag
                              },
                Trottoir = new ZustandsabschnittdetailsTrottoirModel
                               {
                                   Trottoir = strassenabschnittModel.Trottoir,
                                   BelastungskategorieTyp = strassenabschnittModel.BelastungskategorieTyp
                               }
            };
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest, Guid strassenabschnittId, GridCommand command)
        {
            if (dataSourceRequest.Sorts == null)
                dataSourceRequest.Sorts = new List<SortDescriptor>();

            dataSourceRequest.Sorts.Add(new SortDescriptor { Member = "Strassenname" });
            dataSourceRequest.Sorts.Add(new SortDescriptor { Member = "Sreassenabschnittsnummer" });
            dataSourceRequest.Sorts.Add(new SortDescriptor { Member = "StrasseBezeichnungBis" });
            dataSourceRequest.Sorts.Add(new SortDescriptor { Member = "Abschnittsnummer" });
            dataSourceRequest.Sorts.Add(new SortDescriptor { Member = "BezeichnungVon" });

            ((ValueProviderCollection)ValueProvider).Insert(0, new SortValueProvider(command.SortDescriptors));
            return Json(GetGridModel(dataSourceRequest, strassenabschnittId));
        }

        public ActionResult Delete([DataSourceRequest] DataSourceRequest dataSourceRequest, Guid id, Guid strassenabschnittId)
        {
            zustandsabschnittService.DeleteEntity(id);
            return View(GetGridModel(dataSourceRequest, strassenabschnittId));
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest, Guid strassenabschnittId)
        {
            ViewBag.StrassenabschnittId = strassenabschnittId;
            var zustandsabschnittModel = zustandsabschnittService.GetAllZustandsabschnittModel(strassenabschnittId);
            
            return new SerializableDataSourceResult(zustandsabschnittModel.ToDataSourceResult(dataSourceRequest));
        }

        public ActionResult CreateZustandsabschnitt(Guid id)
        {
            ViewBag.IsNew = true;
            var model = CreateDefaultZustandsabschnittModel(id);
            PrepareKatalogs(model);

            var zustandsabschnitte = zustandsabschnittService.GetZustandsabschnitteByStrassenabschnittId(id);
            decimal? currendUsedStreetLength = 0;
            foreach (var za in zustandsabschnitte)
            {
                currendUsedStreetLength += za.Laenge;
            }

            model.Stammdaten.Laenge = model.Stammdaten.StrasseLaenge - currendUsedStreetLength;
            
            return PartialView("EditZustandsabschnitt", model);
        }

        public ActionResult EditZustandsabschnitt(Guid id)
        {

            var result = new ZustandsabschnittMonsterModel();
            result.Stammdaten = zustandsabschnittService.GetById(id);
            result.Fahrbahn = fahrbahnZustandServiceBase.GetZustandsabschnittdetailsModel(id);
            result.Trottoir = trottoirZustandServiceBase.GetZustandsabschnittTrottoirModel(id);
            PrepareKatalogs(result);
            return PartialView(result);
        }

        private void PrepareKatalogs(ZustandsabschnittMonsterModel result)
        {
            PrepareViewBagForFahrbahn(result.Fahrbahn.MassnahmenvorschlagKatalog, result.Fahrbahn.BelastungskategorieTyp);
            if (result.Trottoir != null)
                PrepareViewBagForTrottoir(result.Trottoir.LinkeTrottoirMassnahmenvorschlagKatalogId,
                                          result.Trottoir.RechteTrottoirMassnahmenvorschlagKatalogId,
                                          result.Trottoir.BelastungskategorieTyp);
        }

        public ActionResult Update(ZustandsabschnittMonsterModel zustandsabschnittModel)
        {
            if (ModelState.IsValid)
            {
                zustandsabschnittService.UpdateEntity(zustandsabschnittModel.Stammdaten);
                fahrbahnZustandServiceBase.UpdateZustandsabschnittdetails(zustandsabschnittModel.Fahrbahn);
                if (zustandsabschnittModel.Trottoir != null)
                    trottoirZustandServiceBase.UpdateZustandsabschnittTrottoirModel(zustandsabschnittModel.Trottoir);
                return new EmsgEmptyResult();
            }

            PrepareKatalogs(zustandsabschnittModel);
            return PartialView("EditZustandsabschnitt", zustandsabschnittModel);
        }

        public ActionResult ApplyUpdate(ZustandsabschnittMonsterModel zustandsabschnittModel)
        {
            if (ModelState.IsValid)
            {
                zustandsabschnittModel.Stammdaten = zustandsabschnittService.UpdateEntity(zustandsabschnittModel.Stammdaten);
                fahrbahnZustandServiceBase.UpdateZustandsabschnittdetails(zustandsabschnittModel.Fahrbahn);
                if (zustandsabschnittModel.Trottoir != null)
                    trottoirZustandServiceBase.UpdateZustandsabschnittTrottoirModel(zustandsabschnittModel.Trottoir);
                ModelState.Clear();
            }

            ViewBag.IsNew = false;

            PrepareKatalogs(zustandsabschnittModel);
            return PartialView("EditZustandsabschnitt", zustandsabschnittModel);
        }

        public ActionResult Insert(ZustandsabschnittMonsterModel zustandsabschnittModel)
        {
            if (ModelState.IsValid)
            {
                var createdZustandabschnitt = zustandsabschnittService.CreateEntity(zustandsabschnittModel.Stammdaten);
                zustandsabschnittModel.Fahrbahn.Id = createdZustandabschnitt.Id;
                fahrbahnZustandServiceBase.UpdateZustandsabschnittdetails(zustandsabschnittModel.Fahrbahn);
                if (zustandsabschnittModel.Trottoir != null)
                {
                    zustandsabschnittModel.Trottoir.Id = createdZustandabschnitt.Id;
                    trottoirZustandServiceBase.UpdateZustandsabschnittTrottoirModel(zustandsabschnittModel.Trottoir);
                }
                return new EmsgEmptyResult();
            }

            ViewBag.IsNew = true;
            PrepareKatalogs(zustandsabschnittModel);
            return PartialView("EditZustandsabschnitt", zustandsabschnittModel);
        }

        public ActionResult ApplyInsert(ZustandsabschnittMonsterModel zustandsabschnittModel)
        {
            ViewBag.IsNew = true;

            if (ModelState.IsValid)
            {
                zustandsabschnittModel.Stammdaten = zustandsabschnittService.CreateEntity(zustandsabschnittModel.Stammdaten);
                zustandsabschnittModel.Fahrbahn.Id = zustandsabschnittModel.Stammdaten.Id;
                fahrbahnZustandServiceBase.UpdateZustandsabschnittdetails(zustandsabschnittModel.Fahrbahn);
                if (zustandsabschnittModel.Trottoir != null)
                {
                    zustandsabschnittModel.Trottoir.Id = zustandsabschnittModel.Stammdaten.Id;
                    trottoirZustandServiceBase.UpdateZustandsabschnittTrottoirModel(zustandsabschnittModel.Trottoir);
                }
                ModelState.Clear();
                ViewBag.IsNew = false;
            }

            PrepareKatalogs(zustandsabschnittModel);
            return PartialView("EditZustandsabschnitt", zustandsabschnittModel);
        }

        public ActionResult GetErfassungForm(ZustandsErfassungsmodus zustandsErfassungsmodus, Guid id)
        {
            var model = fahrbahnZustandServiceBase.GetZustandsabschnittdetailsModel(id, zustandsErfassungsmodus);
            return PartialView("ErfassungForm", new ZustandsabschnittMonsterModel() { Fahrbahn = model });
        }

        public ActionResult GetCreateErfassungForm(ZustandsErfassungsmodus zustandsErfassungsmodus, Guid id)
        {
            var model = fahrbahnZustandServiceBase.GetDefaultZustandsabschnittdetailsModel(id, zustandsErfassungsmodus);
            return PartialView("ErfassungForm", new ZustandsabschnittMonsterModel() { Fahrbahn = model });
        }

        public string GetMassnahmenvorschlagKosten(Guid? massnahmenvorschlagKatalogId)
        {
            return string.Format(FormatStrings.LongDecimalFormat, massnahmenvorschlagKatalogService.GetMassnahmenvorschlagKosten(massnahmenvorschlagKatalogId));
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
    }

    [Serializable]
    public class ZustandsabschnittMonsterModel
    {
        public ZustandsabschnittModel Stammdaten { get; set; }

        public ZustandsabschnittdetailsModel Fahrbahn { get; set; }

        public ZustandsabschnittdetailsTrottoirModel Trottoir { get; set; }

        public int SelectedTabIndex { get; set; }
    }
}
