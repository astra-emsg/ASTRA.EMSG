using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Infrastructure.Xlsx;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.GridCommands;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Resources;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungStrassennamen.Controllers
{
    
    public class ZustaendeUndMassnahmenvorschlaegeController : ZustandsabschnittControllerBase
    {
        private readonly IZustandsabschnittService zustandsabschnittService;
        private readonly ILocalizationService localizationService;

        public ZustaendeUndMassnahmenvorschlaegeController(IZustandsabschnittService zustandsabschnittService,
            IMassnahmenvorschlagKatalogService massnahmenvorschlagKatalogService, ILocalizationService localizationService, IFahrbahnZustandService fahrbahnZustandService,
            ITrottoirZustandService trottoirZustandService)
            : base(massnahmenvorschlagKatalogService, fahrbahnZustandService, trottoirZustandService)
        {
            this.localizationService = localizationService;
            this.zustandsabschnittService = zustandsabschnittService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest, ZustandsabschnittGridCommand command)
        {
            if (dataSourceRequest.Sorts == null)
                dataSourceRequest.Sorts = new List<SortDescriptor>();

            dataSourceRequest.Sorts.Add(new SortDescriptor { Member = "Strassenname" });
            dataSourceRequest.Sorts.Add(new SortDescriptor { Member = "Sreassenabschnittsnummer" });
            dataSourceRequest.Sorts.Add(new SortDescriptor { Member = "StrasseBezeichnungBis" });
            dataSourceRequest.Sorts.Add(new SortDescriptor { Member = "Abschnittsnummer" });
            dataSourceRequest.Sorts.Add(new SortDescriptor { Member = "BezeichnungVon" });

           ((ValueProviderCollection)ValueProvider).Insert(0, new SortValueProvider(command.SortDescriptors));
            return Json(GetGridModel(dataSourceRequest, command));
        }

        public ActionResult Delete([DataSourceRequest] DataSourceRequest dataSourceRequest, Guid id, ZustandsabschnittGridCommand command)
        {
            zustandsabschnittService.DeleteEntity(id);
            return View(GetGridModel(dataSourceRequest, command));
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest, ZustandsabschnittGridCommand command)
        {
            var zustandsabschnittModels = zustandsabschnittService.GetCurrentModelsByOrtsbezeichnung(command.Ortsbezeichnung)
                .OrderBy(m => m.Strassenname).ThenBy(m => m.Abschnittsnummer);
            return new SerializableDataSourceResult(zustandsabschnittModels.ToDataSourceResult(dataSourceRequest));
        }

        public ActionResult EditZustandsabschnitt(Guid id)
        {
            var model = InitializeZustandsabschnittMonsterModel(zustandsabschnittService.GetById(id));

            return PartialView("EditZustandsabschnitt", model);
        }

        public ActionResult Update(ZustandsabschnittModel zustandsabschnittModel)
        {
            if (ModelState.IsValid)
            {
                zustandsabschnittService.UpdateEntity(zustandsabschnittModel);
                return new EmsgEmptyResult();
            }

            var model = InitializeZustandsabschnittMonsterModel(zustandsabschnittModel);

            return PartialView("EditZustandsabschnitt", model);
        }

        public ActionResult ApplyUpdate(ZustandsabschnittModel zustandsabschnittModel)
        {
            if (ModelState.IsValid)
            {
                zustandsabschnittService.UpdateEntity(zustandsabschnittModel);
                ModelState.Clear();
            }

            var model = InitializeZustandsabschnittMonsterModel(zustandsabschnittModel);

            return PartialView("EditZustandsabschnitt", model);
        }

        private ZustandsabschnittMonsterModel InitializeZustandsabschnittMonsterModel(
            ZustandsabschnittModel zustandsabschnittModel)
        {
            var model = new ZustandsabschnittMonsterModel
                {
                    Stammdaten = zustandsabschnittModel
                };

            return model;
        }


        [HttpGet]
        public FileStreamResult DownloadExcelReport()
        {
            var data = zustandsabschnittService.GetAllZustandsabschnittModels();
            var doc = new XlsxDoc(20);
            for (var j = 1; j < (data.Count + 2); j++)
            {
                var i = 0;
                if (j == 1)
                {
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.Strassenname;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.ZustandsabschnittImportModel_StrassennameBezeichnungVon;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.ZustandsabschnittImportModel_StrassennameBezeichnungBis;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.ZustandsabschnittImportModel_ExternalId;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.ZustandsabschnittImportModel_Abschnittsnummer;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.BezeichnungVon;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.BezeichnungBis;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.Laenge;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.Zustandsindex;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.ZustandsabschnittImportModel_ZustandsindexTrottoirLinks;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.ZustandsabschnittImportModel_ZustandsindexTrottoirRechts;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.Aufnahmedatum;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.Aufnahmeteam;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.Wetter;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.Bemerkung;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.ZustandsabschnittImportModel_MassnahmenvorschlagFahrbahnId;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.ZustandsabschnittImportModel_DringlichkeitFahrbahn;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.ZustandsabschnittImportModel_MassnahmenvorschlagTrottoirLinksId;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.ZustandsabschnittImportModel_DringlichkeitTrottoirLinks;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.ZustandsabschnittImportModel_MassnahmenvorschlagTrottoirRechtsId;
                    doc.worksheet.Row(j).Cell(++i).Value = ModelLocalization.ZustandsabschnittImportModel_DringlichkeitTrottoirRechts;
                }
                else
                {
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].Strassenname;
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].StrassennameBezeichnungVon;
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].StrassennameBezeichnungBis;
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].ExternalId;
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].Abschnittsnummer;
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].BezeichnungVon;
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].BezeichnungBis;
                    doc.worksheet.Row(j).Cell(++i).Value = decimal.Round((decimal) data[j - 2].Laenge, 1, MidpointRounding.AwayFromZero);
                    doc.worksheet.Row(j).Cell(++i).Value = decimal.Round(data[j - 2].Zustandsindex, 1, MidpointRounding.AwayFromZero);
                    doc.worksheet.Row(j).Cell(++i).Value = localizationService.GetLocalizedEnum(data[j - 2].ZustandsindexTrottoirLinks);
                    doc.worksheet.Row(j).Cell(++i).Value = localizationService.GetLocalizedEnum(data[j - 2].ZustandsindexTrottoirRechts);
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].Aufnahmedatum;
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].Aufnahmeteam;
                    doc.worksheet.Row(j).Cell(++i).Value = localizationService.GetLocalizedEnum(data[j - 2].Wetter);
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].Bemerkung;
                    doc.worksheet.Row(j).Cell(++i).Value = localizationService.GetLocalizedMassnahmenvorschlagTyp(data[j - 2].MassnahmenvorschlagKatalogTypFahrbahn);
                    doc.worksheet.Row(j).Cell(++i).Value = localizationService.GetLocalizedEnum(data[j - 2].DringlichkeitFahrbahn);
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].MassnahmenvorschlagKatalogTypTrottoirLinks;
                    doc.worksheet.Row(j).Cell(++i).Value = localizationService.GetLocalizedEnum(data[j - 2].DringlichkeitTrottoirLinks);
                    doc.worksheet.Row(j).Cell(++i).Value = data[j - 2].MassnahmenvorschlagKatalogTypTrottoirRechts;
                    doc.worksheet.Row(j).Cell(++i).Value = localizationService.GetLocalizedEnum(data[j - 2].DringlichkeitTrottoirRechts);
                }
            }
            var fileSteam = doc.Save();
            fileSteam.Seek(0, SeekOrigin.Begin);
            var file = File(fileSteam, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            file.FileDownloadName = TextLocalization.Zustandsabschnitte + ".xlsx";
            return file;
        }
    }
}
