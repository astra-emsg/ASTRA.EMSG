using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Caching;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.Web.Areas.Administration.Controllers;
using ASTRA.EMSG.Web.Areas.Auswertungen.Controllers;
using ASTRA.EMSG.Web.Areas.Benchmarking.Controllers;
using ASTRA.EMSG.Web.Areas.NetzverwaltungGIS.Controllers;
using ASTRA.EMSG.Web.Areas.NetzverwaltungStrassennamen.Controllers;
using ASTRA.EMSG.Web.Areas.NetzverwaltungSummarisch.Controllers;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Security;
using Resources;
using Kendo.Mvc.UI;
using System.Linq;

namespace ASTRA.EMSG.Web.Controllers
{

    public class HeaderController : Controller
    {
        private readonly IMandantDetailsService mandantDetailsService;
        private readonly ISecurityService securityService;
        private readonly IPermissionService permissionService;
        private readonly IErfassungsPeriodService erfassungsPeriodService;
        private readonly ICookieService cookieService;
        private readonly IHttpRequestCacheService httpRequestCacheService;
        private readonly IApplicationSupporterService applicationSupporterService;
        private readonly IServerConfigurationProvider serverConfigurationProvider;

        public HeaderController(
            IMandantDetailsService mandantDetailsService,
            ISecurityService securityService,
            IPermissionService permissionService,
            IErfassungsPeriodService erfassungsPeriodService,
            ICookieService cookieService,
            IHttpRequestCacheService httpRequestCacheService,
            IApplicationSupporterService applicationSupporterService,
            IServerConfigurationProvider serverConfigurationProvider)
        {
            this.mandantDetailsService = mandantDetailsService;
            this.securityService = securityService;
            this.permissionService = permissionService;
            this.erfassungsPeriodService = erfassungsPeriodService;
            this.cookieService = cookieService;
            this.httpRequestCacheService = httpRequestCacheService;
            this.applicationSupporterService = applicationSupporterService;
            this.serverConfigurationProvider = serverConfigurationProvider;
        }

        public ActionResult MandantSelector()
        {
            ApplicationMode currentApplicationMode = securityService.GetCurrentApplicationMode();

            Mandant currentMandant = null;
            if (currentApplicationMode == ApplicationMode.Mandant)
                currentMandant = securityService.GetCurrentMandant();

            var dropDownItems = new List<DropDownListItem>();
            var supportedApplicationModes = securityService.GetSupportedApplicationModes();

            if (supportedApplicationModes.Contains(ApplicationMode.Application))
                dropDownItems.Add(new DropDownListItem { Text = EnumLocalization.ApplicationMode_Application, Value = null, Selected = (currentApplicationMode == ApplicationMode.Application) });

            if (supportedApplicationModes.Contains(ApplicationMode.Mandant))
                dropDownItems.AddRange(securityService.GetCurrentUserMandanten()
                    .OrderBy(m => m.MandantDisplayName)
                    .Select(m => new DropDownListItem { Text = m.MandantDisplayName + GetApplicationMode(m), Value = m.Id.ToString(), Selected = (m == currentMandant) }));

            return PartialView(dropDownItems);
        }

        private string GetApplicationMode(Mandant m)
        {
            return "";// (serverConfigurationProvider.Environment == ApplicationEnvironment.Development ? String.Format(" {0}", erfassungsPeriodService.GetCurrentErfassungsPeriod(m).NetzErfassungsmodus) : String.Empty);
        }

        public ActionResult Menu()
        {
            return PartialView(GetMenuItemModels());
        }

        public ActionResult ErrorMenu()
        {
            return PartialView("Menu", GetErrorMenuItemModels());
        }

        public ActionResult ErrorUserInfo()
        {
            ViewBag.UserName = "-";
            ViewBag.Mandant = "-";
            ViewBag.IsInSupportMode = false;

            try
            {
                //Note: If there was an Error at the UserInfo collectiong (Eg.: LDAP connection error, etc.) We just disply default Placeholders.
                ViewBag.UserName = securityService.GetCurrentUserName();
                if (securityService.GetCurrentApplicationMode() != ApplicationMode.NoMandants)
                    ViewBag.Mandant = securityService.GetCurrentApplicationMode() == ApplicationMode.Application ? "-" : securityService.GetCurrentMandant().MandantName;
                else
                    ViewBag.Mandant = "-";
                ViewBag.IsInSupportMode = applicationSupporterService.IsInSupportMode();
            }
            catch
            {
            }
            
            return PartialView("UserInfo");
        }

        public ActionResult UserInfo()
        {
            ViewBag.UserName = securityService.GetCurrentUserName();
            if(securityService.GetCurrentApplicationMode() != ApplicationMode.NoMandants)
                ViewBag.Mandant = securityService.GetCurrentApplicationMode() == ApplicationMode.Application ? "-" : securityService.GetCurrentMandant().MandantName;
            else
                ViewBag.Mandant = "-";
            ViewBag.IsInSupportMode = applicationSupporterService.IsInSupportMode();

            return PartialView();
        }

        public ActionResult Breadcrumb()
        {
            var menuItemModels = GetMenuItemModels();
            var parentActionViewContext = ControllerContext.ParentActionViewContext;

            var breadcrumbModel = MenuItemHelpers.GetBreadcrumbModel(new ActionInfo(parentActionViewContext.RouteData), menuItemModels);

            var parentActionInfo = parentActionViewContext.ViewData["ActionInfo"] as ActionInfo;
            var additionalMenuItemModels = parentActionViewContext.ViewData["MenuItemModels"] as List<MenuItemModel>;

            if (parentActionInfo != null)
                breadcrumbModel = MenuItemHelpers.GetBreadcrumbModel(parentActionInfo, menuItemModels);

            if (additionalMenuItemModels != null)
                breadcrumbModel.AddRange(additionalMenuItemModels);

            return PartialView(breadcrumbModel);
        }

        public ActionResult ErrorBreadcrumb()
        {
            return PartialView("Breadcrumb", new List<MenuItemModel> {new MenuItemModel {IsPlaceHolder = true, Text = MenuLocalization.Error}});
        }

        public ActionResult LanguageSelector()
        {
            return PartialView(cookieService.EmsgLanguage ?? EmsgLanguage.Ch);
        }

        public ActionResult SetLanguage(EmsgLanguage emsgLanguage, bool? lastMethodPost)
        {
            cookieService.EmsgLanguage = emsgLanguage;

            var urlReferrer = ControllerContext.HttpContext.Request.UrlReferrer;
            if (urlReferrer != null && !(lastMethodPost ?? false))
                    return Redirect(urlReferrer.ToString());
            
            return RedirectToAction("Index", "Home", new {area = ""});
        }

        public ActionResult SetMandant(Guid? mandantId)
        {
            if(mandantId.HasValue)
            {
                securityService.SetCurrentApplicationMode(ApplicationMode.Mandant);
                securityService.SetCurrentMandant(mandantId.Value);
            }
            else
            {
                securityService.SetCurrentApplicationMode(ApplicationMode.Application);
            }

            return new EmsgEmptyResult();
        }

        private List<MenuItemModel> GetErrorMenuItemModels()
        {
            Type homeControllerType = typeof(HomeController);
            var menuItemModels = new List<MenuItemModel> { new MenuItemModel(homeControllerType.GetAreaName(), homeControllerType.GetControllerName(), "Index") { IsPlaceHolder = true, Text = MenuLocalization._Home_Index } };

            try
            {
                //Note: If there was an Error at the MenuBuilding (Eg.: LDAP connection error, etc.) We just disply the Startseite Menu item.
                menuItemModels = GetMenuItemModels();
            }
            catch
            {
            }

            foreach (var menuItemModel in menuItemModels)
            {
                menuItemModel.SubMenuItemModels.Clear();

                if(menuItemModel.Area == homeControllerType.GetAreaName() && menuItemModel.Controller == homeControllerType.GetControllerName() && menuItemModel.Action == "Index")
                    menuItemModel.IsPlaceHolder = false;
                else
                    menuItemModel.IsPlaceHolder = true;
            }

            return menuItemModels;
        }

        private List<MenuItemModel> GetMenuItemModels()
        {
            if (httpRequestCacheService.MenuItemModels != null)
                return httpRequestCacheService.MenuItemModels;

            var applicationMode = securityService.GetCurrentApplicationMode();
            NetzErfassungsmodus netzErfassungsmodus = NetzErfassungsmodus.Summarisch;
            if (applicationMode != ApplicationMode.NoMandants)
                netzErfassungsmodus = applicationMode == ApplicationMode.Application ?
                    NetzErfassungsmodus.Tabellarisch : erfassungsPeriodService.GetCurrentErfassungsPeriod().NetzErfassungsmodus;

            var isAchenEditEnabled = false;
            if (applicationMode == ApplicationMode.Mandant)
            {
                isAchenEditEnabled = mandantDetailsService.GetCurrentMandantDetails().IsAchsenEditEnabled;
            }

            var menuBuilder = new MenuItemModelBuilder(permissionService, serverConfigurationProvider, netzErfassungsmodus);

            var allModus = new[] { NetzErfassungsmodus.Gis, NetzErfassungsmodus.Summarisch, NetzErfassungsmodus.Tabellarisch };
            var strassenModus = new[] { NetzErfassungsmodus.Gis, NetzErfassungsmodus.Tabellarisch };

            menuBuilder.AddMenuItem<HomeController>(allModus);

            menuBuilder.AddMenuItem<StrassenmengeUndZustandController>(NetzErfassungsmodus.Summarisch);
            menuBuilder.AddMenuItem<NetzdefinitionUndStrassenabschnittController>(NetzErfassungsmodus.Tabellarisch);


            if (netzErfassungsmodus == NetzErfassungsmodus.Gis)
            {
                menuBuilder.AddPlaceHolderMenuItems(MenuLocalization.Strassennetz,
                                    builder =>
                                    {
                                        builder.AddMenuItem<NetzdefinitionUndStrassenabschnittGISController>(NetzErfassungsmodus.Gis);
                                        if (isAchenEditEnabled)
                                            builder.AddMenuItem<AchsenController>(NetzErfassungsmodus.Gis);
                                    });
            }


            menuBuilder.AddPlaceHolderMenuItems(MenuLocalization.Zustand,
                                    builder =>
                                    {
                                        builder.AddMenuItem<ZustaendeUndMassnahmenvorschlaegeController>(NetzErfassungsmodus.Tabellarisch);
                                        builder.AddMenuItem<ZustaendeUndMassnahmenvorschlaegeGISController>(NetzErfassungsmodus.Gis);
                                        builder.AddMenuItem<InspektionsroutenGISController>(NetzErfassungsmodus.Gis);
                                    });

            menuBuilder.AddPlaceHolderMenuItems(MenuLocalization.Massnahmen,
                                    builder =>
                                    {
                                        builder.AddMenuItem<MassnahmenvorschlaegeAndererTeilsystemeController>(NetzErfassungsmodus.Gis);
                                        builder.AddMenuItem<KoordinierteMassnahmenController>(NetzErfassungsmodus.Gis);
                                        builder.AddMenuItem<RealisierteMassnahmenSummarischController>(NetzErfassungsmodus.Summarisch);
                                        builder.AddMenuItem<RealisierteMassnahmenGISController>(NetzErfassungsmodus.Gis);
                                        builder.AddMenuItem<RealisierteMassnahmenController>(NetzErfassungsmodus.Tabellarisch);
                                    });

            menuBuilder.AddPlaceHolderMenuItems(MenuLocalization.Auswertungen,
                                                builder =>
                                                {
                                                    builder.AddPlaceHolderMenuItems(MenuLocalization.Auswertungen_Invertar,
                                                        subBuilder =>
                                                        {
                                                            subBuilder.AddMenuItem<MengeProBelastungskategorieController>(allModus);
                                                            subBuilder.AddMenuItem<MengeProBelastungskategorieGrafischeController>(allModus);
                                                            subBuilder.AddMenuItem<StrassenabschnitteListeController>(NetzErfassungsmodus.Tabellarisch);
                                                            subBuilder.AddMenuItem<StrassenabschnitteListeGISController>(NetzErfassungsmodus.Gis);
                                                        });

                                                    builder.AddPlaceHolderMenuItems(MenuLocalization.Auswertungen_WiederbeschaffungswertUndWertverlust,
                                                        subBuilder =>
                                                        {
                                                            subBuilder.AddMenuItem<WiederbeschaffungswertUndWertverlustProBelastungskategorieController>(allModus);
                                                            subBuilder.AddMenuItem<WiederbeschaffungswertUndWertverlustGrafischeController>(allModus);
                                                            subBuilder.AddMenuItem<WiederbeschaffungswertUndWertverlustProStrassenabschnittController>(strassenModus);
                                                            subBuilder.AddMenuItem<WiederbeschaffungswertUndWertverlustProJahrGrafischeController>(allModus);
                                                        });

                                                    builder.AddPlaceHolderMenuItems(MenuLocalization.Auswertungen_ZustandUndMassnahmenvorschläge,
                                                        subBuilder =>
                                                        {
                                                            subBuilder.AddMenuItem<ZustandsspiegelProBelastungskategorieGrafischeController>(strassenModus);
                                                            subBuilder.AddMenuItem<ZustandsspiegelProJahrGrafischeController>(strassenModus);
                                                            subBuilder.AddMenuItem<ZustandProZustandsabschnittController>(NetzErfassungsmodus.Tabellarisch);
                                                            subBuilder.AddMenuItem<ZustandProZustandsabschnittGISController>(NetzErfassungsmodus.Gis);
                                                            subBuilder.AddMenuItem<MassnahmenvorschlagProZustandsabschnittController>(NetzErfassungsmodus.Tabellarisch);
                                                            subBuilder.AddMenuItem<MassnahmenvorschlagProZustandsabschnittGISController>(NetzErfassungsmodus.Gis);
                                                            subBuilder.AddMenuItem<AusgefuellteErfassungsformulareFuerOberflaechenschaedenController>(strassenModus);
                                                            subBuilder.AddMenuItem<ErfassungsformulareFuerOberflaechenschaedenController>(allModus);                                                          
                                                            subBuilder.AddMenuItem<ListeDerInspektionsroutenController>(NetzErfassungsmodus.Gis);
                                                            subBuilder.AddMenuItem<StrassenabschnitteListeOhneInspektionsrouteController>(NetzErfassungsmodus.Gis);
                                                            subBuilder.AddMenuItem<NochNichtInspizierteStrassenabschnitteController>(strassenModus);
                                                        });

                                                    builder.AddPlaceHolderMenuItems(MenuLocalization.Auswertungen_MassnahmenDerTeilsystemeUndKoordinierteMassnahmen,
                                                        subBuilder =>
                                                        {
                                                            subBuilder.AddMenuItem<EineListeVonMassnahmenGegliedertNachTeilsystemenController>(NetzErfassungsmodus.Gis);
                                                            subBuilder.AddMenuItem<EineListeVonKoordiniertenMassnahmenController>(NetzErfassungsmodus.Gis);
                                                        });

                                                    builder.AddPlaceHolderMenuItems(MenuLocalization.Auswertungen_Fortschreibung,
                                                        subBuilder =>
                                                        {
                                                            subBuilder.AddMenuItem<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischController>(NetzErfassungsmodus.Summarisch);
                                                            subBuilder.AddMenuItem<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenController>(NetzErfassungsmodus.Tabellarisch);
                                                            subBuilder.AddMenuItem<EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISController>(NetzErfassungsmodus.Gis);
                                                            subBuilder.AddMenuItem<RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeController>(allModus);
                                                        });
                                                    builder.AddMenuItem<GISExportController>(NetzErfassungsmodus.Gis);
                                                });

            menuBuilder.AddPlaceHolderMenuItems(MenuLocalization.Benchmarking,
                                                builder =>
                                                    {
                                                        builder.AddMenuItem<BenchmarkauswertungInventarkennwertenController>(allModus);
                                                        builder.AddMenuItem<BenchmarkauswertungZustandskennwertenController>(allModus);
                                                        builder.AddMenuItem<BenchmarkauswertungKennwertenRealisiertenMassnahmenController>(allModus);
                                                    });

            menuBuilder.AddPlaceHolderMenuItems(MenuLocalization.Administration,
                                                builder =>
                                                {
                                                    builder.AddMenuItem<KenngroessenFruehererJahreSummarischController>(NetzErfassungsmodus.Summarisch);
                                                    builder.AddMenuItem<KenngroessenFruehererJahreController>(NetzErfassungsmodus.Tabellarisch);
                                                    builder.AddMenuItem<KenngroessenFruehererJahreGISController>(NetzErfassungsmodus.Gis);
                                                    if (!isAchenEditEnabled)
                                                        builder.AddMenuItem<AchsenupdateController>(NetzErfassungsmodus.Gis);
                                                    builder.AddMenuItem<AchsenEditModeController>(allModus);
                                                    builder.AddMenuItem<CheckOutRueckgaengigController>(NetzErfassungsmodus.Gis);
                                                    builder.AddMenuItem<ErfassungsPeriodAbschlussController>(allModus);
                                                    builder.AddPlaceHolderMenuItems(MenuLocalization.Administration_SystemparameterPflegen,
                                                        subBuilder =>
                                                        {
                                                            subBuilder.AddMenuItem<AnwendungsLoggingsController>(allModus);
                                                            subBuilder.AddMenuItem<WiederbeschaffungswertAndAlterungsbeiwertController>(allModus);
                                                            subBuilder.AddMenuItem<MassnahmenvorschlagController>(allModus);
                                                        }); 
                                                    builder.AddMenuItem<ArbeitsmodusWechselnController>(allModus);
                                                    builder.AddMenuItem<MandantDetailsController>(allModus);
                                                    builder.AddMenuItem<MandantLogoController>(allModus);
                                                    builder.AddMenuItem<AndereBenutzerrollenEinnehmenController>(allModus);
                                                    builder.AddMenuItem<EreignisLogController>(allModus);
                                                    builder.AddMenuItem<GlobalResourceController>(allModus);
                                                    builder.AddMenuItem<HelpSystemController>(allModus);
                                                });



            httpRequestCacheService.MenuItemModels = menuBuilder.MenuItemModels;

            return menuBuilder.MenuItemModels;
        }
    }
}
