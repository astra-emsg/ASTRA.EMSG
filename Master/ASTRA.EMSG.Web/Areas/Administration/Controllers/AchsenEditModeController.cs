using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure.Security;
using Resources;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.Administration.Controllers
{
    [AccessRights(Rolle.Applikationsadministrator)]
    public class AchsenEditModeController : Controller
    {
        private readonly IMandantenService mandantenService;
        private readonly IMandantDetailsService mandantDetailsService;

        public AchsenEditModeController(
            IMandantenService mandantenService,
            IMandantDetailsService mandantDetailsService
            )
        {
            this.mandantenService = mandantenService;
            this.mandantDetailsService = mandantDetailsService;
        }

        public ActionResult Index(Guid? mandantId)
        {
            var dropDownItems = mandantenService.GetCurrentMandanten()
                .OrderBy(m => m.MandantDisplayName)
                .Select(m => new DropDownListItem { Text = m.MandantDisplayName, Value = m.Id.ToString(), Selected = m.Id == mandantId}).ToList();
            dropDownItems.Insert(0, new DropDownListItem { Text = TextLocalization.PleaseSelect, Value = ""} );
            ViewBag.Mandanten = dropDownItems;

            bool isAchsenEditEnabled = false;
            if (mandantId != null)
            {
                var mandantDetails = mandantDetailsService.GetEntityByMandant(mandantId.Value);
                isAchsenEditEnabled = mandantDetails.IsAchsenEditEnabled;
                ViewBag.MandantDetailsId = mandantDetails.Id;
            }
            ViewBag.IsAchsenEditEnabled = isAchsenEditEnabled;
            ViewBag.MandantId = mandantId;
            return View();
        }

        public ActionResult EnableAchsenEdit(Guid? selectedMandantDetailsId)
        {
            if (selectedMandantDetailsId.HasValue)
            {
                mandantDetailsService.EnableAchsenEdit(selectedMandantDetailsId.Value);
                return View("EnableAchsenEditSuccessfull");
            }

            return View("Index");
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
