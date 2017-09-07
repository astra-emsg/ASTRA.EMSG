using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.GIS.WMS;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Utils;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure.Filters;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using ASTRA.EMSG.Web.Infrastructure;
using System.Linq;
using ASTRA.EMSG.Business.Services.GIS.WMS.WMSObjects;
using System.IO;
using Kendo.Mvc;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.Common.Controllers
{
    public abstract class GisReportControllerBase<TReportParameter, TMapReportParameter, TPresentationObject, TReportGridCommand> : TabellarischReportControllerBase<TReportParameter, TPresentationObject, TReportGridCommand>
        where TPresentationObject : new()
        where TReportGridCommand : GisReportGridCommand, new() where TReportParameter : EmsgGisReportParameter, new()
        where TMapReportParameter : TReportParameter, new()
    {
        public GisReportControllerBase(ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies)
            : base(tabellarischeReportControllerBaseDependencies)
        {
        }

        private static void CopyMapParameters(TReportParameter reportParameter, TReportGridCommand reportGridCommand)
        {
            reportParameter.BoundingBox  = reportGridCommand.BoundingBox;
            reportParameter.MapSize  = reportGridCommand.MapSize;
            reportParameter.BackgroundLayers  = reportGridCommand.BackgroundLayers;
            reportParameter.LayersAVBackground = reportGridCommand.LayersAVBackground;
            reportParameter.Layers  = reportGridCommand.Layers;
            reportParameter.LayersAV  = reportGridCommand.LayersAV;
            reportParameter.LayerDefs  = reportGridCommand.LayerDefs;
            reportParameter.ScaleWidth = reportGridCommand.ScaleWidth;
            reportParameter.ScaleText  = reportGridCommand.ScaleText;
            reportParameter.IsPreview = reportGridCommand.isPreview;
        }

        protected override TReportParameter GetReportParameterInternal(TReportGridCommand command)
        {
            var parameter = base.GetReportParameterInternal(command);
            if (command.MapSize.HasText())
                CopyMapParameters(parameter, command);

            return parameter;
        }

        [HttpPost]
        public ActionResult Undock(TReportGridCommand command)
        {
            PrepareViewBagForPreview(command.ErfassungsPeriodId);
            ViewBag.DisableMenu = true;
            ViewBag.IsInPopup = true;
            return View(command);
        }

        [HttpPost]
        public ActionResult Dock(TReportGridCommand command)
        {
            PrepareViewBagForPreview(command.ErfassungsPeriodId);
            return PartialView("Undock", command);
        }

        public override ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest, TReportGridCommand command)
        {
            if (erfassungsPeriodService.GetCurrentErfassungsPeriod().NetzErfassungsmodus == NetzErfassungsmodus.Gis &&
                (string.IsNullOrEmpty(command.BoundingBox) || command.BoundingBox.Split(',').All(c => c.Trim() == "0")))
            {
                GetReportParameterInternal(command);
                sessionService["LastReportGridCommand"] = command;
                //TODO: check
                return Json(new SerializableGridModel<TPresentationObject> { Data = new List<TPresentationObject>() });
            }

            return base.GetAll(dataSourceRequest, command);
        }

        [GridCommandCreator]
        public EmsgEmptyResult GenerateMapReport(TReportGridCommand command)
        {
            var reportParameter = new TMapReportParameter();

            SetMapReportParameter(reportParameter, command);

            reportParameter.ErfassungsPeriodId = command.ErfassungsPeriodId;
            reportParameter.OutputFormat = command.OutputFormat ?? OutputFormat.Excel;

            if (command.MapSize.HasText())
                CopyMapParameters(reportParameter, command);

            var emsgPoProvider = CreateEmsgPoProvider(reportParameter);

            var emsgReport = serverReportGenerator.GenerateReport(emsgPoProvider);

            sessionService.LastGeneratedMapReport = emsgReport;

            if (command.MapSize.HasText())
                DeleteMapReport(reportParameter.ReportImagePath);

            return new EmsgEmptyResult();
        }

        [GridCommandCreator]
        public ActionResult GenerateMapPreview(TReportGridCommand command)
        {
            command.isPreview = true;
            var reportParameter = new TMapReportParameter();

            SetMapReportParameter(reportParameter, command);

            reportParameter.ErfassungsPeriodId = command.ErfassungsPeriodId;
            reportParameter.OutputFormat = command.OutputFormat ?? OutputFormat.Excel;

            if (command.MapSize.HasText())
                CopyMapParameters(reportParameter, command);

            var emsgPoProvider = CreateEmsgPoProvider(reportParameter);
            Stream ms = new MemoryStream();
            using(Stream stream = new FileStream(reportParameter.ReportImagePath, FileMode.Open)){
                stream.CopyTo(ms);
            }
            if (command.MapSize.HasText())
                DeleteMapReport(reportParameter.ReportImagePath);


            ms.Seek(0, 0);
            return new FileStreamResult(ms, "image/png");            
        }

        public ActionResult GetLastGeneratedMapReport(TReportGridCommand command)
        {
            if (sessionService.LastGeneratedMapReport == null)
                GenerateMapReport(command);

            var lastGeneratedReport = sessionService.LastGeneratedMapReport;
            sessionService.LastGeneratedMapReport = null;
            return reportControllerService.GetReportFileResult(lastGeneratedReport);
        }

        protected override TReportParameter GetReportParameter(TReportGridCommand reportGridCommand)
        {
            var reportParameter = new TReportParameter();
            SetMapReportParameter(reportParameter, reportGridCommand);
            return reportParameter;
        }

        protected abstract void SetMapReportParameter(TReportParameter parameter, TReportGridCommand reportGridCommand);

        public void DeleteMapReport(string reportImagePath)
        {
            try
            {
                if (System.IO.File.Exists(reportImagePath))
                {
                    System.IO.File.Delete(reportImagePath);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        protected override void PrepareViewBagForIndex(Guid? erfassungsPeriodId)
        {
            ViewBag.SupportedYears = reportControllerService.GetSupportedErfassungsperioden<TMapReportParameter>().Select(e => e.Id).ToArray();
        }
    }
}