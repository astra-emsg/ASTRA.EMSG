using System;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.Administration.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Gis)]
    [AccessRights(Rolle.Benutzeradministrator)]
    public class CheckOutRueckgaengigController : Controller
    {
        private readonly IInspektionsRouteGISOverviewService inspektionsRouteGISOverviewService;

        public CheckOutRueckgaengigController(IInspektionsRouteGISOverviewService inspektionsRouteGISOverviewService)
        {
            this.inspektionsRouteGISOverviewService = inspektionsRouteGISOverviewService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            return Json(GetGridModel(dataSourceRequest));
        }

        public EmsgEmptyResult CancellExport(Guid id)
        {
            inspektionsRouteGISOverviewService.CancellExport(id);
            return new EmsgEmptyResult();
        }

        private SerializableDataSourceResult GetGridModel([DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            return new SerializableDataSourceResult(inspektionsRouteGISOverviewService.GetCurrentModels().ToDataSourceResult(dataSourceRequest));
        }
    }
}
