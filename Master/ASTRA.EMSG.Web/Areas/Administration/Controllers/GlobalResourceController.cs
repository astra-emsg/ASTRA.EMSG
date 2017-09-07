using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Services.Administration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Administration.Controllers
{
    [AccessRights(Rolle.Applikationsadministrator)]
    public class GlobalResourceController : Controller
    {
        private readonly ISessionService sessionService;
        private readonly IGlobalResourceHandlerService globalResourceHandlerService;

        public GlobalResourceController(
            ISessionService sessionService, 
            IGlobalResourceHandlerService globalResourceHandlerService)
        {
            this.sessionService = sessionService;
            this.globalResourceHandlerService = globalResourceHandlerService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            var httpPostedFileBase = uploadFiles.First();
            sessionService.LastUploadErrorList = globalResourceHandlerService.UploadResource(httpPostedFileBase.InputStream, httpPostedFileBase.FileName);
            return Content("");
        }

        public ActionResult GetLastUploadResult()
        {
            return PartialView("UploadResult", sessionService.LastUploadErrorList);
        }

        public ActionResult GetGlobalResources()
        {
            return File(globalResourceHandlerService.DownloadResources(), "application/x-compressed", "GetGlobalResources.zip");
        }
    }
}
