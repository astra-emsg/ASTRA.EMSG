using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Security;
using Resources;

namespace ASTRA.EMSG.Web.Areas.Administration.Controllers
{
    [AccessRights(Rolle.Applikationsadministrator)]
    public class AnwendungsLoggingsController : Controller
    {
        private readonly ILogHandlerService logHandlerService;

        public AnwendungsLoggingsController(ILogHandlerService logHandlerService)
        {
            this.logHandlerService = logHandlerService;
        }

        public ActionResult Index()
        {
            var logLevel = logHandlerService.GetLogLevel();
            SetLogLevels(logLevel);
            return View(new LogLevelModel { LogLevel = logLevel });
        }

        public ActionResult SelectLogLevel(LogLevelModel logLevelModel)
        {
            if (ModelState.IsValid)
                logHandlerService.SetLogLevel(logLevelModel.LogLevel);

            SetLogLevels(logLevelModel.LogLevel);

            return View("Index", logLevelModel);
        }

        public ActionResult Cancel()
        {
            SetLogLevels();
            return View("Index");
        }

        private void SetLogLevels(string logLevel = null)
        {
            if (logLevel == null)
                logLevel = logHandlerService.GetLogLevel();

            ViewBag.LogLevels = logHandlerService
                .GetAllLogLevel()
                .ToDropDownItemList(ll => LookupLocalization.ResourceManager.GetString(ll), ll => ll, logLevel);
        }
    }
}
