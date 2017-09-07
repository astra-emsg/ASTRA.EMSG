using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Master;
using ASTRA.EMSG.Web.Areas.Common.GridCommands;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Help;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Kendo.Mvc;


namespace ASTRA.EMSG.Web.Controllers
{
    public class HilfeSucheController : Controller
    {
        private readonly IHilfeSucheService hilfeSucheService;
        private readonly ICookieService cookieService;
        

        public HilfeSucheController(IHilfeSucheService hilfeSucheService, ICookieService cookieService)
        {
            this.hilfeSucheService = hilfeSucheService;
            this.cookieService = cookieService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAll(HilfeSucheGridCommand command)
        {
            return Json(GetGridModel(command));
        }

        private SerializableGridModel<HilfeSucheModel> GetGridModel(HilfeSucheGridCommand command)
        {
            var result = hilfeSucheService.Suche(cookieService.EmsgLanguage ?? EmsgLanguage.Ch, command.HilfeFilter);
            return new SerializableGridModel<HilfeSucheModel>(result.OrderByDescending(r => r.MatchCount));
        }

        public ActionResult Hilfe(string actionName, string controllerName, string areaName)
        {
            string cultureCode = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

            var languageIndependentHelpFilePath = HelpFileMappingStore.GetHelpFilePath(GetHelpFileKey(actionName, controllerName, areaName), cultureCode);

            string helpFilePath = null;
            if (!string.IsNullOrEmpty(languageIndependentHelpFilePath))
                helpFilePath = string.Format("~/Help/{0}/{1}", cultureCode, languageIndependentHelpFilePath);

            if (helpFilePath == null)
                helpFilePath = GetHelpFilePath(cultureCode, actionName, controllerName, areaName);

            if (!System.IO.File.Exists(Server.MapPath(helpFilePath)))
                helpFilePath = string.Format("~/Help/{0}/{1}", cultureCode, HelpFileMappingStore.GetContentsPath(cultureCode));

            ViewBag.HelpUrl = new UrlHelper(ControllerContext.RequestContext).Content(helpFilePath);
            return View();
        }

        public ActionResult HilfeSucheResult(string helpFilePath, string culture)
        {
            string cultureCode = string.IsNullOrWhiteSpace(culture) ? Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName : culture;

            helpFilePath = string.Format("~/Help/{0}/{1}", cultureCode, helpFilePath);

            ViewBag.HelpUrl = new UrlHelper(ControllerContext.RequestContext).Content(helpFilePath);
            return View("Hilfe");
        }

        private static string GetHelpFilePath(string cultureCode, object action, object controller, object area)
        {
            string helpFile;
            if (string.IsNullOrEmpty(area as string))
                helpFile = string.Format("~/Help/{0}/{1}_{2}.html", cultureCode, controller, action);
            else
                helpFile = string.Format("~/Help/{0}/{1}/{2}_{3}.html", cultureCode, area, controller, action);
            return helpFile;
        }

        private static string GetHelpFileKey(object action, object controller, object area)
        {
            string helpFileKey;
            if (string.IsNullOrEmpty(area as string))
                helpFileKey = string.Format("{0}_{1}", controller, action);
            else
                helpFileKey = string.Format("{0}_{1}_{2}", area, controller, action);
            return helpFileKey;
        }
    }
}
