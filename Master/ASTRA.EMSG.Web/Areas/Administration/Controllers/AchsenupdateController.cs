using System;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Services.Administration;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using ASTRA.EMSG.Common.Master.Logging;
using Kendo.Mvc.UI;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using Kendo.Mvc.Extensions;

namespace ASTRA.EMSG.Web.Areas.Administration.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Gis)]
    [AccessRights(Rolle.Benutzeradministrator)]
    public class AchsenupdateController : Controller
    {
        private readonly IAchsenUpdateService achsenupdateService;
        private readonly IAchsenUpdateLogService achsenUpdateLogService;
        private readonly ITransactionScopeProvider transactionScopeProvider;

        public AchsenupdateController(
            IAchsenUpdateService achsenupdateService,
            ITransactionScopeProvider transactionScopeProvider,
            IAchsenUpdateLogService achsenUpdateLogService
            )
        {
            this.achsenupdateService = achsenupdateService;
            this.achsenUpdateLogService = achsenUpdateLogService;
            this.transactionScopeProvider = transactionScopeProvider;
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            var result = new SerializableDataSourceResult(achsenUpdateLogService.GetCurrentModels().ToDataSourceResult(dataSourceRequest));
            return Json(result);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StartAxisUpdate()
        {
            try
            {
                achsenupdateService.StartAchsenUpdate();
                return View("AchsenUpdateSuccessful");
                
            }
            catch (Exception ex)
            {
                //since the Exception is caught in order to display an ErrorMessage there is no reason to for the Application_EndRequest to rollback the session, so we rollback manually
                transactionScopeProvider.CurrentTransactionScope.Rollback();
                transactionScopeProvider.ResetCurrentTransactionScope();
                Loggers.ApplicationLogger.Error(string.Format("msg: {0}, stacktrace: {1}",ex.Message.ToString(),ex.StackTrace.ToString()));
                ViewBag.Message = ex.Message.ToString();
                return View("AchsenUpdateFailed");
            }
        }

    }
}
