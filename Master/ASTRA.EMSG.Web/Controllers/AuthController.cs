using ASTRA.EMSG.Business.Models.Identity;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Master.Logging;

namespace ASTRA.EMSG.Web.Controllers
{
    public class 
        AuthController : Controller
    {
        private readonly ISessionService sessionService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEreignisLogService ereignisLogService;
        private readonly ILocalizationService localizationService;
        private readonly ISecurityService securityService;

        public AuthController(IServerConfigurationProvider serverConfigurationProvider, ISessionService sessionService, IEreignisLogService ereignisLogService,
            ILocalizationService localizationService, ISecurityService securityService)
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext(serverConfigurationProvider.ConnectionString)));
            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };
            this.sessionService = sessionService;
            this.ereignisLogService = ereignisLogService;
            this.localizationService = localizationService;
            this.securityService = securityService;
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            var model = new ChangePasswordModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (string.IsNullOrWhiteSpace(model.OldPassword))
                ModelState.AddModelError("", string.Format(ValidationErrorLocalization.ShouldNotBeEmpty, ModelLocalization.AltesPasswort));
            if (string.IsNullOrWhiteSpace(model.NewPassword))
                ModelState.AddModelError("", string.Format(ValidationErrorLocalization.ShouldNotBeEmpty, ModelLocalization.NeuesPasswort));
            if (string.IsNullOrWhiteSpace(model.NewPasswordCopy))
                ModelState.AddModelError("", string.Format(ValidationErrorLocalization.ShouldNotBeEmpty, ModelLocalization.NeuesPasswortNochmal));

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (model.NewPassword != model.NewPasswordCopy)
            {
                ModelState.AddModelError("", ValidationErrorLocalization.NewPasswordShouldBeSame);
                return View();
            }

            var userId = User.Identity.GetUserId();
            var result = userManager.ChangePassword(userId, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
                return RedirectToAction("index", "home");

            ModelState.AddModelError("", string.Join(",", result.Errors));
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = userManager.Find(model.Email, model.Password);
            if (user != null)
            {
                var identity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                GetAuthenticationManager().SignIn(identity);

                return RedirectToAction("LogUserLogin", new { returnUrl = model.ReturnUrl });
            }

            ModelState.AddModelError("", localizationService.GetLocalizedModelPropertyText<ModelLocalization>("InvalidEmailOrPassword")); //TODO: localize
            return View(model);
        }

        public ActionResult LogUserLogin(string returnUrl)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                ereignisLogService.LogEreignis(EreignisTyp.AnmeldenimSystem, new Dictionary<string, object>());
                var notInitializedMandanten = securityService.GetNotInitializedMandanten();
                if (notInitializedMandanten.Any())
                    Loggers.ApplicationLogger.Warn(string.Format(" User: {1} - Nicht initializierte Mandanten: {0}", 
                        string.Join(", ", notInitializedMandanten), System.Web.HttpContext.Current.User.Identity.Name));
            }
            return Redirect(GetRedirectUrl(returnUrl));
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            sessionService.Abandon();
            GetAuthenticationManager().SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("index", "home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && userManager != null)
            {
                userManager.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }

        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }
    }


}