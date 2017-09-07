using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Import;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Resources;
using Kendo.Mvc;
using ASTRA.EMSG.Business.Utils;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.NetzverwaltungStrassennamen.Controllers
{
    public class ZustandsabschnittImportController : Controller
    {
        private readonly IZustandsabschnittXlsxImportService zustandsabschnittXlsxImportService;
        private readonly ISessionService sessionService;

        protected ImportResultModel<ZustandsabschnittImportModel, ZustandsabschnittImportOverviewModel> LastZustandsabschnittImportResult
        {
            get { return sessionService.LastZustandsabschnittImportResult ?? new ImportResultModel<ZustandsabschnittImportModel, ZustandsabschnittImportOverviewModel>(); }
        }

        public ZustandsabschnittImportController(IZustandsabschnittXlsxImportService zustandsabschnittXlsxImportService, ISessionService sessionService)
        {
            this.zustandsabschnittXlsxImportService = zustandsabschnittXlsxImportService;
            this.sessionService = sessionService;
        }

        public ActionResult Import()
        {
            return PartialView(null);
        }

        public ActionResult Upload(IEnumerable<HttpPostedFileBase> importFiles)
        {
            zustandsabschnittXlsxImportService.ImportZustandsabschnitte(importFiles.First().InputStream);

            return Content("");
        }

        public ActionResult GetLastImportResult()
        {
            if (LastZustandsabschnittImportResult.Errors.Any())
                return PartialView("Import", LastZustandsabschnittImportResult);

            return PartialView("ImportResult", LastZustandsabschnittImportResult);
        }

        public ActionResult CommitLastImportResult()
        {
            zustandsabschnittXlsxImportService.CommitZustandsabschnittImport();
            return PartialView("CommitResult", LastZustandsabschnittImportResult);
        }

        public ActionResult GetAllCreateImportOverviewModel([DataSourceRequest] DataSourceRequest dataSourceRequest, GridCommand command)
        {
            return Json(GetGridModel(dataSourceRequest, LastZustandsabschnittImportResult.CreateImportOverviewModels));
        }

        public ActionResult GetAllUpdateImportOverviewModel([DataSourceRequest] DataSourceRequest dataSourceRequest, GridCommand command)
        {
            return Json(GetGridModel(dataSourceRequest, LastZustandsabschnittImportResult.UpdateImportOverviewModels));
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest, List<ZustandsabschnittImportOverviewModel> strassenabschnittImportOverviewModels)
        {
            var models = strassenabschnittImportOverviewModels.OrderBy(m => m.Strassenname).ThenBy(m => m.StrassennameBezeichnungVon).ThenBy(m => m.BezeichnungVon);
            return new SerializableDataSourceResult(models.ToDataSourceResult(dataSourceRequest));
        }

        public ActionResult GetXlsxImportTemplate()
        {
            return File(zustandsabschnittXlsxImportService.GetImportTemplateForZustandsabschnittImportModels(), "application/ms-excel", TextLocalization.ZustandsabschnittImportTemplateFileName.ToASCIIString() + ".xlsx");
        }
    }
}