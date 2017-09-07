using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Administration.Controllers
{
    [AccessRights(Rolle.Benutzeradministrator)]
    public class MandantLogoController : Controller
    {
        private readonly ISessionService sessionService;
        private readonly IMandantLogoService mandantLogoService;
        private readonly ISecurityService securityService;

        public MandantLogoController(
            ISessionService sessionService, 
            IMandantLogoService mandantLogoService,
            ISecurityService securityService
            )
        {
            this.sessionService = sessionService;
            this.mandantLogoService = mandantLogoService;
            this.securityService = securityService;
        }

        public ActionResult Index()
        {
            ViewBag.MandantName = securityService.GetCurrentMandant().MandantDisplayName;
            return View();
        }

        public ActionResult Upload(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            sessionService.LastUploadErrorList = mandantLogoService.UploadMandantLogo(uploadFiles.First().InputStream);
            return new EmsgEmptyResult();
        }

        public ActionResult GetLastUploadResult()
        {
            return PartialView("UploadResult", sessionService.LastUploadErrorList);
        }

        public ActionResult CurrentMandantImage()
        {
            return File(mandantLogoService.GetMandantLogo().Logo, "image/png");
        }
    }
}
