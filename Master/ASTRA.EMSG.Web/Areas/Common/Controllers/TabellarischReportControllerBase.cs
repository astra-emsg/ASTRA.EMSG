using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Master.Logging;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.ControllerServices;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Filters;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.Common.Controllers
{
    public abstract class TabellarischReportControllerBase<TReportParameter, TPresentationObject, TReportGridCommand> : ReportControllerBase<TReportParameter, TPresentationObject>
        where TPresentationObject : new()
        where TReportParameter : EmsgTabellarischeReportParameter
        where TReportGridCommand : ReportGridCommand
    {
        protected readonly IServerReportGenerator serverReportGenerator;
        protected readonly IReportControllerService reportControllerService;
        protected readonly IEmsgPoProviderService emsgPoProviderService;
        protected readonly IErfassungsPeriodService erfassungsPeriodService;
        protected readonly ISessionService sessionService;

        protected TabellarischReportControllerBase(ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies)
        {
            serverReportGenerator = tabellarischeReportControllerBaseDependencies.ServerReportGenerator;
            reportControllerService = tabellarischeReportControllerBaseDependencies.ReportControllerService;
            emsgPoProviderService = tabellarischeReportControllerBaseDependencies.EmsgPoProviderService;
            erfassungsPeriodService = tabellarischeReportControllerBaseDependencies.ErfassungsPeriodService;
            sessionService = tabellarischeReportControllerBaseDependencies.SessionService;
        }

        public virtual ActionResult Index()
        {
            var closedErfassungsperiodenDropDownItems = reportControllerService.GetClosedErfassungsperiodenDropDownItems<TReportParameter, TPresentationObject>();
            ViewBag.ClosedErfassungsperiods = closedErfassungsperiodenDropDownItems;

            DropDownListItem erfassungsPeriodDropDownItem = closedErfassungsperiodenDropDownItems.SingleOrDefault(ep => ep.Selected);
            Guid? erfassungsPeriodId = erfassungsPeriodDropDownItem == null ? (Guid?)null : new Guid(erfassungsPeriodDropDownItem.Value);

            if (!erfassungsPeriodId.HasValue)
            {
                ViewBag.NetzErfassungsmodus = erfassungsPeriodService.GetCurrentErfassungsPeriod().NetzErfassungsmodus;
            }
            else
            {
                var erfassungsPeriod = erfassungsPeriodService.GetEntityById(erfassungsPeriodId.Value);
                ViewBag.NetzErfassungsmodus = erfassungsPeriod.NetzErfassungsmodus;
            }

            PrepareViewBagForIndex(erfassungsPeriodId);
            return View();
        }

        public virtual ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest, TReportGridCommand command)
        {
            var reportParameter = GetReportParameterInternal(command);
            reportParameter.IsPreview = true;
            var emsgPoProvider = CreateEmsgPoProvider(reportParameter);

            var presentationObjectList = ((IEmsgModeDependentPoProviderBase<TReportParameter, TPresentationObject>) emsgPoProvider).PresentationObjectList;
            
            sessionService["LastReportGridCommand"] = command;
            var result = new SerializableDataSourceResult(presentationObjectList.ToDataSourceResult(dataSourceRequest));
            return Json(result);
        }

        [GridCommandCreator]
        public ActionResult GetReport(TReportGridCommand command)
        {
            var reportParameter = GetReportParameterInternal(command);

            var emsgPoProvider = CreateEmsgPoProvider(reportParameter);
            var emsgReport = serverReportGenerator.GenerateReport(emsgPoProvider);

            return reportControllerService.GetReportFileResult(emsgReport);
        }

        [GridCommandCreator]
        public EmsgEmptyResult GenerateReport(TReportGridCommand command)
        {
            var reportParameter = GetReportParameterInternal(command);

            var emsgPoProvider = CreateEmsgPoProvider(reportParameter);
            var session = HttpContext.Session;
            Task.Run(() =>
            {
                try
                {
                    var emsgReport = serverReportGenerator.GenerateReport(emsgPoProvider);
                    session[SessionService.LastGeneratedReportKey] = emsgReport;
                }
                catch (Exception ex)
                {
                    Loggers.ApplicationLogger.Error(string.Format("Error during GenerateReport: {0}", ex));
                }
            });

            return new EmsgEmptyResult();
        }

        [HttpPost]
        public ActionResult GetLastGeneratedReportReady()
        {
            var lastGeneratedReport = sessionService.LastGeneratedReport;
            return Json(new {
                    ready = lastGeneratedReport != null
                });
        }

        public ActionResult GetLastGeneratedReport()
        {
            var lastGeneratedReport = sessionService.LastGeneratedReport;
            sessionService.LastGeneratedReport = null;
            return reportControllerService.GetReportFileResult(lastGeneratedReport);
        }

        public ActionResult GetPreview(Guid? erfassungsPeriodId)
        {
            PrepareViewBagForPreview(erfassungsPeriodId);
            return PartialView("Preview");
        }

        protected virtual TReportParameter GetReportParameterInternal(TReportGridCommand command)
        {
            var reportParameter = GetReportParameter(command);
            reportParameter.ErfassungsPeriodId = command.ErfassungsPeriodId;
            reportParameter.OutputFormat = command.OutputFormat ?? OutputFormat.Excel;
            reportParameter.GridFilterDescriptors = command.GetGridFilterDescriptors();
            return reportParameter;
        }

        protected abstract void PrepareViewBagForPreview(Guid? erfassungsPeriodId);

        protected virtual void PrepareViewBagForIndex(Guid? erfassungsPeriodId)
        {
            ViewBag.SupportedYears = reportControllerService.GetSupportedErfassungsperioden<TReportParameter>().Select(e => e.Id).ToArray();
        }

        protected abstract TReportParameter GetReportParameter(TReportGridCommand reportGridCommand);

        protected override IEmsgPoProviderBase CreateEmsgPoProvider(TReportParameter parameter)
        {
            return emsgPoProviderService.CreateEmsgPoProvider(parameter, new PresentationObjectProcessor<TPresentationObject>(this));
        }
    }
}