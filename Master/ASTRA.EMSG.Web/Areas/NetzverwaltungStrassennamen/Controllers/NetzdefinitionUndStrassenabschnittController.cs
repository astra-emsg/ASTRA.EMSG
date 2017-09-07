using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASTRA.EMSG.Business.Infrastructure.Xlsx;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Common.GridCommands;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Resources;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungStrassennamen.Controllers
{
    public class NetzdefinitionUndStrassenabschnittController : Controller
    {
        private readonly IStrassenabschnittService strassenabschnittService;
        private readonly IBelastungskategorieService belastungskategorieService;
        private readonly ILocalizationService localizationService;

        public NetzdefinitionUndStrassenabschnittController(IStrassenabschnittService strassenabschnittService,
            ILocalizationService localizationService, IBelastungskategorieService belastungskategorieService)
        {
            this.strassenabschnittService = strassenabschnittService;
            this.belastungskategorieService = belastungskategorieService;
            this.localizationService = localizationService;
        }

        public ActionResult Index()
        {
            ViewBag.DefaultStrassenabschnittModel = strassenabschnittService.CreateDefaultStrassenabschnittModel();
            ViewBag.NetzErfassungsmodus = NetzErfassungsmodus.Tabellarisch;
            PrepareBelastungskategorien();
            return View();
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest, StrassenabschnittGridCommand command)
        {
            return Json(GetGridModel(dataSourceRequest, command));
        }

        public ActionResult Delete([DataSourceRequest] DataSourceRequest dataSourceRequest, Guid id)
        {
            strassenabschnittService.DeleteEntity(id);
            return View(GetGridModel(dataSourceRequest, new StrassenabschnittGridCommand()));
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest request, StrassenabschnittGridCommand command)
        {
            PrepareBelastungskategorien();

            var strassenabschnittModels = strassenabschnittService.GetCurrentModelsByStrassenname(command.StrassennameFilter, command.Ortsbezeichnung);
            return new SerializableDataSourceResult(strassenabschnittModels.OrderBy(m => m.Strassenname).ThenBy(m => m.Abschnittsnummer).ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            ViewBag.IsNew = true;
            PrepareBelastungskategorien();
            return PartialView("EditStrassenabschnitt", strassenabschnittService.CreateDefaultStrassenabschnittModel());
        }

        public ActionResult EditStrassenabschnitt(Guid id)
        {
            PrepareBelastungskategorien();
            return PartialView(strassenabschnittService.GetById(id));
        }

        public ActionResult Update(StrassenabschnittModel strassenabschnittModel)
        {
            if (ModelState.IsValid)
            {
                strassenabschnittService.UpdateEntity(strassenabschnittModel);
                return new EmsgEmptyResult();
            }

            PrepareBelastungskategorien();
            return PartialView("EditStrassenabschnitt", strassenabschnittModel);
        }

        public ActionResult ApplyUpdate(StrassenabschnittModel strassenabschnittModel)
        {
            if (ModelState.IsValid)
            {
                strassenabschnittModel = strassenabschnittService.UpdateEntity(strassenabschnittModel);
                ModelState.Clear();
            }
            
            ViewBag.IsNew = false;
            PrepareBelastungskategorien();

            return PartialView("EditStrassenabschnitt", strassenabschnittModel);
        }

        public ActionResult Insert(StrassenabschnittModel strassenabschnittModel)
        {
            if (ModelState.IsValid)
            {
                strassenabschnittService.CreateEntity(strassenabschnittModel);
                return new EmsgEmptyResult();
            }

            ViewBag.IsNew = true;
            PrepareBelastungskategorien();

            return PartialView("EditStrassenabschnitt", strassenabschnittModel);
        }

        public ActionResult ApplyInsert(StrassenabschnittModel strassenabschnittModel)
        {
            ViewBag.IsNew = true;
            
            if (ModelState.IsValid)
            {
                strassenabschnittModel = strassenabschnittService.CreateEntity(strassenabschnittModel);
                ModelState.Clear();
                ViewBag.IsNew = false;
            }
            
            PrepareBelastungskategorien();

            return PartialView("EditStrassenabschnitt", strassenabschnittModel);
        }

        public ActionResult SplitStrassenabschnitt(Guid id)
        {
            return PartialView(new SplitStrassenabschnittModel { StrassenabschnittId = id, Count = 2 });
        }

        public ActionResult Split(SplitStrassenabschnittModel splitStrassenabschnittModel)
        {
            if (ModelState.IsValid)
            {
                ViewBag.IsNew = true;
                ViewBag.OriginalLaenge = strassenabschnittService.GetStrassenabschnittOriginalLaenge(splitStrassenabschnittModel.StrassenabschnittId);
                PrepareBelastungskategorien();
                return PartialView("GenerateStrassenabschnitten", strassenabschnittService.GetSplittedStrassenabschnittModels(splitStrassenabschnittModel));
            }

            return PartialView("SplitStrassenabschnitt", splitStrassenabschnittModel);
        }

        public ActionResult InsertStrassenabschnitten(List<StrassenabschnittSplitModel> strassenabschnittModels)
        {
            List<StrassenabschnittSplitModel> models = null;

            if (ModelState.IsValid)
                strassenabschnittService.InsertSplittedStrassenabschnittModels(strassenabschnittModels);
            else
                models = strassenabschnittModels;

            ViewBag.IsNew = true;
            ViewBag.OriginalLaenge = strassenabschnittService.GetStrassenabschnittOriginalLaenge(strassenabschnittModels.First().Id);
            PrepareBelastungskategorien();
            return PartialView("GenerateStrassenabschnitten", models);
        }

        private void PrepareBelastungskategorien()
        {
            var allBelastungskategorieModel = belastungskategorieService.AllBelastungskategorieModel;
            ViewBag.Belastungskategorien = allBelastungskategorieModel;
            
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

        [HttpGet]
        public FileStreamResult DownloadExcelReport(StrassenabschnittGridCommand command)
        {
            var data = strassenabschnittService.GetAllStrassenabschnittImportModels(command.StrassennameFilter);
            var doc = new XlsxDoc(14);
            for (var j = 1; j < (data.Count + 2) ; j++)
            {
                var i = 0;
                if (j == 1)
                {
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.Strassenname;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.BezeichnungVon;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.BezeichnungBis;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.StrassenabschnittImportModel_ExternalId;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.StrassenabschnittImportModel_Abschnittsnummer;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.Strasseneigentuemer;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.Ortsbezeichnung;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.Belastungskategorie;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.Belag;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.BreiteFahrbahn;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.Laenge;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.Trottoir;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.BreiteTrottoirLinks;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.BreiteTrottoirRechts;
                }
                else
                {
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].Strassenname;
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].BezeichnungVon;
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].BezeichnungBis;
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].ExternalId;
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].Abschnittsnummer;
                    doc.worksheet.Row(j).Cell(++i).Value = localizationService.GetLocalizedEnum(data[j - 2].Strasseneigentuemer);
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].Ortsbezeichnung;
                    doc.worksheet.Row(j).Cell(++i).Value = localizationService.GetLocalizedBelastungskategorieTyp(data[j - 2].BelastungskategorieTyp);
                    doc.worksheet.Row(j).Cell(++i).Value = localizationService.GetLocalizedEnum(data[j - 2].Belag);
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].BreiteFahrbahn;
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].Laenge;
                    doc.worksheet.Row(j).Cell(++i).Value = localizationService.GetLocalizedEnum(data[j - 2].Trottoir);
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].BreiteTrottoirLinks;
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].BreiteTrottoirRechts;
                }
            }
            var fileSteam = doc.Save();
            fileSteam.Seek(0, SeekOrigin.Begin);
            var file = File(fileSteam, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            file.FileDownloadName = TextLocalization.Strassenabschnitte + ".xlsx"; 
            return file;
        }
    }
}
