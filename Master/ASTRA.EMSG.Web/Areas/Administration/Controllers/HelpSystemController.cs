using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Services.Administration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure.Help;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Administration.Controllers
{
    [AccessRights(Rolle.Applikationsadministrator)]
    public class HelpSystemController : Controller
    {
        private readonly ISessionService sessionService;
        private readonly IHelpSystemService helpSystemService;
        private readonly IHilfeSucheService hilfeSucheService;

        public HelpSystemController(
            ISessionService sessionService, 
            IHelpSystemService helpSystemService,
            IHilfeSucheService hilfeSucheService)
        {
            this.sessionService = sessionService;
            this.helpSystemService = helpSystemService;
            this.hilfeSucheService = hilfeSucheService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadMasterHelpSystem(IEnumerable<HttpPostedFileBase> uploadMasterHelpSystemFiles)
        {
            var httpPostedFileBase = uploadMasterHelpSystemFiles.First();
            sessionService.LastUploadErrorList = helpSystemService.UploadMasterHelpSystem(httpPostedFileBase.InputStream);
            HelpFileMappingStore.RefreshMappingDictionary();
            hilfeSucheService.ReloadHelp();
            return Content("");
        }

        public ActionResult UploadMobileHelpSystem(IEnumerable<HttpPostedFileBase> uploadMobileHelpSystemFiles)
        {
            var httpPostedFileBase = uploadMobileHelpSystemFiles.First();
            sessionService.LastUploadErrorList = helpSystemService.UploadMobileHelpSystem(httpPostedFileBase.InputStream);
            return Content("");
        }

        public ActionResult GetLastUploadResult()
        {
            return PartialView("UploadResult", sessionService.LastUploadErrorList);
        }

        public ActionResult GetMasterHelpSystem()
        {
            return File(helpSystemService.DownloadMasterHelpSystem(), "application/x-compressed", "MasterHelpSystem.zip");
        }

        public ActionResult GetMobileHelpSystem()
        {
            return File(helpSystemService.DownloadMobileHelpSystem(), "application/x-compressed", "MobileHelpSystem.zip");
        }
    }
}
