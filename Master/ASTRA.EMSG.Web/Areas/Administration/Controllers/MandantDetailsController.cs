using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Web.Infrastructure;

namespace ASTRA.EMSG.Web.Areas.Administration.Controllers
{
    [AccessRights(Rolle.Benutzeradministrator)]
    public class MandantDetailsController : Controller
    {
        private readonly IMandantDetailsService mandantDetailsService;
        private readonly IGemeindeKatalogService gemeindeKatalogService;
        private readonly IOeffentlicheVerkehrsmittelKatalogService oeffentlicheVerkehrsmittelKatalogService;
        private readonly ILocalizationService localizationService;

        public MandantDetailsController(
            IMandantDetailsService mandantDetailsService,
            IGemeindeKatalogService gemeindeKatalogService,
            IOeffentlicheVerkehrsmittelKatalogService oeffentlicheVerkehrsmittelKatalogService,
            ILocalizationService localizationService)
        {
            this.mandantDetailsService = mandantDetailsService;
            this.gemeindeKatalogService = gemeindeKatalogService;
            this.oeffentlicheVerkehrsmittelKatalogService = oeffentlicheVerkehrsmittelKatalogService;
            this.localizationService = localizationService;
        }

        public ActionResult Index()
        {
            var mandantDetailsModel = mandantDetailsService.GetCurrentMandantDetailsModel();
            PrepareViewBag(mandantDetailsModel);
            return View(mandantDetailsModel);
        }

        public ActionResult Update(MandantDetailsModel mandantDetailsModel)
        {
            if (ModelState.IsValid)
            {
                mandantDetailsService.UpdateEntity(mandantDetailsModel);
                return View("MandantDetailsSpeichernSuccesfull", mandantDetailsModel);
            }

            PrepareViewBag(mandantDetailsModel);
            return View("Index", mandantDetailsModel);
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        private void PrepareViewBag(MandantDetailsModel mandantDetailsModel)
        {
            ViewBag.GemeindetypDropDownItems = gemeindeKatalogService.GetGemeindeKatalogModels().ToDropDownItemList(m => localizationService.GetLocalizedBelastungskategorieTyp(m.Typ), m => m.Id, mandantDetailsModel.Gemeindetyp, mandantDetailsModel.Gemeindetyp == null ? string.Empty : null);
            ViewBag.OeffentlicheVerkehrsmittelDropDownItems = oeffentlicheVerkehrsmittelKatalogService.GetOeffentlicheVerkehrsmittelKatalogModels().ToDropDownItemList(m => localizationService.GetLocalizedBelastungskategorieTyp(m.Typ), m => m.Id, mandantDetailsModel.OeffentlicheVerkehrsmittel, mandantDetailsModel.OeffentlicheVerkehrsmittel == null ? string.Empty : null);
        }
    }
}
