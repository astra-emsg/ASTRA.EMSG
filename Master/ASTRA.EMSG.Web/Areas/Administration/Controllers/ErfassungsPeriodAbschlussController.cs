using System;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure;
using System.Linq;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Business.Utils;
using Microsoft.ReportingServices.Diagnostics.Utilities;
using Resources;

namespace ASTRA.EMSG.Web.Areas.Administration.Controllers
{
    [AccessRights(Rolle.Benutzeradministrator)]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ErfassungsPeriodAbschlussController : Controller
    {
        private readonly IHistorizationService historizationService;
        private readonly IJahresabschlussService jahresabschlussService;
        private readonly IStrassenabschnittService strassenabschnittService;
        private readonly IStrassenabschnittGISService strassenabschnittGisService;
        private readonly IMandantDetailsService mandantDetailsService;
        private readonly IAchsenSegmentService achsenSegmentService;
        private readonly IErfassungsPeriodService erfassungsPeriodService;

        public ErfassungsPeriodAbschlussController(
            IHistorizationService historizationService,
            IJahresabschlussService jahresabschlussService,
            IStrassenabschnittService strassenabschnittService,
            IStrassenabschnittGISService strassenabschnittGisService,
            IMandantDetailsService mandantDetailsService,
            IAchsenSegmentService achsenSegmentService,
            IErfassungsPeriodService erfassungsPeriodService
            )
        {
            this.historizationService = historizationService;
            this.jahresabschlussService = jahresabschlussService;
            this.strassenabschnittService = strassenabschnittService;
            this.strassenabschnittGisService = strassenabschnittGisService;
            this.mandantDetailsService = mandantDetailsService;
            this.achsenSegmentService = achsenSegmentService;
            this.erfassungsPeriodService = erfassungsPeriodService;
        }

        public ActionResult Index()
        {
            ViewBag.CurrentPeriodId = erfassungsPeriodService.GetCurrentErfassungsPeriod().Id;
            return View(CreateErfassungsabschlussModel());
        }

        public ActionResult ErfassungsPeriodAbschluss(ErfassungsabschlussModel erfassungsabschlussModel)
        {
            ErfassungsabschlussModel model = null;
            if (ModelState.IsValid) 
                jahresabschlussService.CloseCurrentErfassungsperiod(erfassungsabschlussModel);
            else
                model = erfassungsabschlussModel;

            CreateErfassungsabschlussModel();

            return PartialView("ErfassungsPeriodAbschluss", model);
        }

        [HttpPost]
        public ActionResult RevertLastErfassungsPeriod()
        {
            jahresabschlussService.RevertLastErfassungsperiod();

            return new ContentResult() { Content = NotificationLocalization.JahresAbschlussRueckgaengingMachenWasSuccessfull };
        }

        private ErfassungsabschlussModel CreateErfassungsabschlussModel()
        {
            var availableErfassungsabschlussen = historizationService.GetAvailableErfassungsabschlussen().OrderBy(ea => ea.AbschlussDate);
            var erfassungsabschlussModel = availableErfassungsabschlussen.FirstOrDefault();
            var dropDownItemList = availableErfassungsabschlussen
                .ToDropDownItemList(ea => ea.AbschlussDate.Year, ea => ea.AbschlussDate, erfassungsabschlussModel);

            ViewBag.ErfassungsabschlussModellen = dropDownItemList;

            ErfassungsPeriod currentErfassungsperiod = historizationService.GetCurrentErfassungsperiod();

            bool isThereMissingZustandsabschnitte;
            switch (currentErfassungsperiod.NetzErfassungsmodus)
            {
                case NetzErfassungsmodus.Summarisch:
                    isThereMissingZustandsabschnitte = false;
                    break;
                case NetzErfassungsmodus.Tabellarisch:
                    isThereMissingZustandsabschnitte = strassenabschnittService.IsThereMissingZustandsabschnitte();
                    break;
                case NetzErfassungsmodus.Gis:
                    isThereMissingZustandsabschnitte = strassenabschnittGisService.IsThereMissingZustandsabschnitte();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("NetzErfassungsmodus");
            }

            ViewBag.IsThereMissingZustandsabschnitte = isThereMissingZustandsabschnitte;
            ViewBag.IsMandantenDetailsCompleted = mandantDetailsService.GetCurrentMandantDetails().IsCompleted;
            ViewBag.AreThereLockedStrassenabschnitte = strassenabschnittGisService.AreThereLockedStrassenabschnitte();
            ViewBag.NetzErfassungsmodus = currentErfassungsperiod.NetzErfassungsmodus;
            ViewBag.LastClosedErfassungsPeriod = erfassungsPeriodService.GetNewestClosedErfassungsPeriod();
            ViewBag.IsLastClosedYear = erfassungsPeriodService.GetClosedErfassungsPeriodModels().Count == 1;

            if (currentErfassungsperiod.NetzErfassungsmodus == NetzErfassungsmodus.Gis)
            {
                ViewBag.ApproxWaitTime = GisHistorizationTimeEstimate.GetApproxHistorizationTime(achsenSegmentService.GetCurrentEntities().Count());
            }

            return erfassungsabschlussModel;
        }
    }
}
