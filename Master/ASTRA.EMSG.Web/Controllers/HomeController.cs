using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Security;
using System.Linq;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Exceptions;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.Web.Infrastructure.Security;

namespace ASTRA.EMSG.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISecurityService securityService;
        private readonly ILocalizationService localizationService;
        private readonly IErfassungsPeriodService erfassungsPeriodService;
        private readonly IServerConfigurationProvider serverConfigurationProvider;

        public HomeController(
            ISecurityService securityService,
            ILocalizationService localizationService,
            IErfassungsPeriodService erfassungsPeriodService,
            IServerConfigurationProvider serverConfigurationProvider)
        {
            this.securityService = securityService;
            this.localizationService = localizationService;
            this.erfassungsPeriodService = erfassungsPeriodService;
            this.serverConfigurationProvider = serverConfigurationProvider;
        }

        public ActionResult Index()
        {
            List<Rolle> currentRollen = securityService.GetCurrentRollen();
            ApplicationMode currentApplicationMode = securityService.GetCurrentApplicationMode();
            
            NetzErfassungsmodus? netzErfassungsmodus = null;
            Mandant currentMandant = null;
            if (currentApplicationMode == ApplicationMode.Mandant)
            {
                currentMandant = securityService.GetCurrentMandant();
                netzErfassungsmodus = erfassungsPeriodService.GetCurrentErfassungsPeriod().NetzErfassungsmodus;
            }
            
            List<string> notInitializedMandanten = securityService.GetNotInitializedMandanten();
            var homeModel = new HomeModel
                                {
                                    Rollen = currentRollen,
                                    RolleBezeichnungen = string.Join(", ", currentRollen.Select(r => localizationService.GetLocalizedEnum(r))),
                                    MandantName = currentMandant == null ? "-" : currentMandant.MandantName,
                                    MandantBezeichnung = currentMandant == null ? "-" : currentMandant.MandantBezeichnung,
                                    NetzErfassungsmodus = netzErfassungsmodus,
                                    NetzErfassungsmodusBezeichnung = netzErfassungsmodus.HasValue ? localizationService.GetLocalizedEnum(netzErfassungsmodus.Value) : "-",
                                    NotInitialisedMandanten = notInitializedMandanten,
                                    NotInitialisedMandantenBezeichnung = string.Join(", ", notInitializedMandanten),
                                    AppMode = currentApplicationMode
            };
            
            return View(homeModel);
        }

        public FileResult GetMobileClientInstaller()
        {
            return File(Path.Combine(serverConfigurationProvider.ClientFilesFolderPath, FileNameConstants.ClientInstallerFileName), "application/octet-stream", FileNameConstants.ClientInstallerFileName);
        }
    }
}
