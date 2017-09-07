using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Common.GridCommands;
using ASTRA.EMSG.Web.Infrastructure.Security;
using System.Linq;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.Administration.Controllers
{
    [AccessRights(Rolle.Applikationsadministrator)]
    public class EreignisLogController : Controller
    {
        private readonly IEreignisLogService ereignisLogService;
        private readonly ILogHandlerService logHandlerService;

        public EreignisLogController(IEreignisLogService ereignisLogService, ILogHandlerService logHandlerService)
        {
            this.ereignisLogService = ereignisLogService;
            this.logHandlerService = logHandlerService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest, EreignisLogOverviewGridCommand command)
        {
            var ereignisLogOverviewParameter = new EreignisLogOverviewParameter
                                                   {
                                                       Benutzer = command.Benutzer,
                                                       EreignisTyp = command.EreignisTyp,
                                                       Mandant = command.Mandant,
                                                       ZeitVon = command.ZeitVon,
                                                       ZeitBis = command.ZeitBis
                                                   };

            var ereignisLogOverviewModels = ereignisLogService.GetModelsByFilter(ereignisLogOverviewParameter).OrderByDescending(m => m.Zeit);
            var result = new SerializableDataSourceResult(ereignisLogOverviewModels.ToDataSourceResult(dataSourceRequest));
            return Json(result);
        }

        public ActionResult ClearLog()
        {
            ereignisLogService.ClearLog();
            return new EmsgEmptyResult();
        }

        public ActionResult DownloadApplicationLog()
        {
            return File(logHandlerService.DownloadApplicationLog(), "application/x-compressed", "ApplicationLog.zip");
        }
    }
}
