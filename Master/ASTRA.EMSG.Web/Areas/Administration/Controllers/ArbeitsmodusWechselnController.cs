using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Administration;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Areas.Administration.Controllers
{
    [AccessRights(Rolle.Benutzeradministrator)]
    public class ArbeitsmodusWechselnController : Controller
    {
        private readonly IArbeitsmodusService arbeitsmodusService;
        private readonly IMandantDetailsService mandantDetailsService;

        public ArbeitsmodusWechselnController(IArbeitsmodusService arbeitsmodusService, IMandantDetailsService mandantDetailsService)
        {
            this.arbeitsmodusService = arbeitsmodusService;
            this.mandantDetailsService = mandantDetailsService;
        }

        public ActionResult Index()
        {
            return View(arbeitsmodusService.GetArbeitsmodusModel());
        }

        public ActionResult ArbeitsmodusWechseln(ArbeitsmodusModel arbeitsmodusModel)
        {
            if(ModelState.IsValid)
            {
                arbeitsmodusService.SaveArbeitsmodusModel(arbeitsmodusModel);
                ViewBag.IsAchsenEditEnabled = mandantDetailsService.GetCurrentMandantDetails().IsAchsenEditEnabled;
                return View("ArbeitsmodusWechselnSuccessfull", arbeitsmodusModel);
            }

            return View("Index", arbeitsmodusModel);
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("Index", "Home", new {area = ""});
        }
    }
}
