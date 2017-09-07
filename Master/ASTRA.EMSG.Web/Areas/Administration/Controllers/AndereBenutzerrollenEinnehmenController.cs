using System.Web.Mvc;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Administration.Controllers
{
    [AccessRights(Rolle.Applikationssupporter)]
    public class AndereBenutzerrollenEinnehmenController : Controller
    {
        private readonly IApplicationSupporterService applicationSupporterService;

        public AndereBenutzerrollenEinnehmenController(IApplicationSupporterService applicationSupporterService)
        {
            this.applicationSupporterService = applicationSupporterService;
        }

        public ActionResult Index()
        {
            return View(applicationSupporterService.GetCurrentSupportedUserInfo());
        }

        public ActionResult AndereBenutzerrollenEinnehmen(AndereBenutzerrollenEinnehmenModel andereBenutzerrollenEinnehmenModel)
        {
            if (ModelState.IsValid)
            {
                applicationSupporterService.StartSupporting(andereBenutzerrollenEinnehmenModel);
                return RedirectToAction("AndereBenutzerrollenEinnehmenSuccessfull");
            }

            return View("Index", andereBenutzerrollenEinnehmenModel);
        }

        public ActionResult StopSupporting()
        {
            applicationSupporterService.StopSupporting();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public ActionResult AndereBenutzerrollenEinnehmenSuccessfull()
        {
            return View(applicationSupporterService.GetCurrentSupportedUserInfo());
        }
    }
}
