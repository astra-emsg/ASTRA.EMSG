using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Utils;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure;
using Resources;

namespace ASTRA.EMSG.Web.Areas.Administration.Controllers
{
    
    public class ErfassungsPeriodCheckerController : Controller
    {
        private readonly IErfassungsPeriodService erfassungsPeriodService;
        public ErfassungsPeriodCheckerController(IErfassungsPeriodService erfassungsPeriodService)
        {
            this.erfassungsPeriodService = erfassungsPeriodService;
        }

        [HttpPost]
        public ActionResult GetErfassungsPeriodReady(Guid erfassungsPeriodId)
        {
            // if the erfassungsPeriodId not equal to currentErfassungsPeriodId then it is ready
            bool isReady = erfassungsPeriodId != erfassungsPeriodService.GetCurrentErfassungsPeriod().Id;

            return Json(new
            {
                ready = isReady
            });
        }
    }
}