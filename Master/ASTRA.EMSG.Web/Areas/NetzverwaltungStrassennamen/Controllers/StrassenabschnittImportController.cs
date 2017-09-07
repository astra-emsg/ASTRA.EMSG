using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.Strassennamen;
using System.Linq;
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
    public class StrassenabschnittImportController : Controller
    {
        private readonly IStrassenabschnittXlsxImportService strassenabschnittXlsxImportService;
        private readonly ISessionService sessionService;

        protected ImportResultModel<StrassenabschnittImportModel, StrassenabschnittImportOverviewModel> LastStrassenabschnittImportResult
        {
            get { return sessionService.LastStrassenabschnittImportResult ?? new ImportResultModel<StrassenabschnittImportModel, StrassenabschnittImportOverviewModel>(); }
        }

        public StrassenabschnittImportController(IStrassenabschnittXlsxImportService strassenabschnittXlsxImportService, ISessionService sessionService)
        {
            this.strassenabschnittXlsxImportService = strassenabschnittXlsxImportService;
            this.sessionService = sessionService;
        }

        public ActionResult Import()
        {
            return PartialView(null);
        }

        public ActionResult Upload(IEnumerable<HttpPostedFileBase> importFiles)
        {
            strassenabschnittXlsxImportService.ImportStrassenabschnitte(importFiles.First().InputStream);

            return Content("");
        }

        public ActionResult GetLastImportResult()
        {
            if (LastStrassenabschnittImportResult.Errors.Any())
                return PartialView("Import", LastStrassenabschnittImportResult);

            return PartialView("ImportResult", LastStrassenabschnittImportResult);
        }

        public ActionResult CommitLastImportResult()
        {
            strassenabschnittXlsxImportService.CommitStrassenabschnittImport();
            return PartialView("CommitResult", LastStrassenabschnittImportResult);
        }

        public ActionResult GetAllCreateImportOverviewModel([DataSourceRequest] DataSourceRequest dataSourceRequest, GridCommand command)
        {
            return Json(GetGridModel(dataSourceRequest, LastStrassenabschnittImportResult.CreateImportOverviewModels));
        }

        public ActionResult GetAllUpdateImportOverviewModel([DataSourceRequest] DataSourceRequest dataSourceRequest, GridCommand command)
        {
            return Json(GetGridModel(dataSourceRequest, LastStrassenabschnittImportResult.UpdateImportOverviewModels));
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest, List<StrassenabschnittImportOverviewModel> strassenabschnittImportOverviewModels)
        {
            var models = strassenabschnittImportOverviewModels.OrderBy(m => m.Strassenname).ThenBy(m => m.Abschnittsnummer).ThenBy(m => m.BezeichnungVon);
            return new SerializableDataSourceResult(models.ToDataSourceResult(dataSourceRequest));
        }

        public ActionResult GetXlsxImportTemplate()
        {
            return File(strassenabschnittXlsxImportService.GetImportTemplateForStrassenabschnittImportModels(), "application/ms-excel", TextLocalization.StrassenabschnittImportTemplateFileName.ToASCIIString() + ".xlsx");
        }
    }
}
