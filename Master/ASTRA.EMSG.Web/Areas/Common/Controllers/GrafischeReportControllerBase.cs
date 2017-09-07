using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Master.Utils;
using ASTRA.EMSG.Web.Areas.Common.ControllerServices;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure;
using Kendo.Mvc.UI;


namespace ASTRA.EMSG.Web.Areas.Common.Controllers
{
    public abstract class GrafischeReportControllerBase<TReportParameter, TPresentationObject> : ReportControllerBase<TReportParameter, TPresentationObject>
        where TPresentationObject : new()
        where TReportParameter : EmsgReportParameter, new()
    {
        protected readonly IServerReportGenerator serverReportGenerator;
        protected readonly IReportControllerService reportControllerService;
        protected readonly IEmsgPoProviderService emsgPoProviderService;
        private readonly ISessionService sessionService;

        protected GrafischeReportControllerBase(IGrafischeReportControllerBaseDependencies grafischeReportControllerBaseDependencies)
        {
            serverReportGenerator = grafischeReportControllerBaseDependencies.ServerReportGenerator;
            reportControllerService = grafischeReportControllerBaseDependencies.ReportControllerService;
            emsgPoProviderService = grafischeReportControllerBaseDependencies.EmsgPoProviderService;
            sessionService = grafischeReportControllerBaseDependencies.SessionService;
        }

        public virtual ActionResult Index()
        {
            var parameter = PrepareViewBagForIndex();

            return View(parameter);
        }

        protected virtual TReportParameter PrepareViewBagForIndex()
        {
            var closedErfassungsperiodenDropDownItems = reportControllerService.GetClosedErfassungsperiodenDropDownItems<TReportParameter, TPresentationObject>().ToArray();
            ViewBag.SupportedYears = reportControllerService.GetSupportedErfassungsperioden<TReportParameter>().Select(e => e.Id);
            ViewBag.ClosedErfassungsperiods = closedErfassungsperiodenDropDownItems;

            DropDownListItem erfassungsPeriodDropDownItem = closedErfassungsperiodenDropDownItems.SingleOrDefault(ep => ep.Selected);
            Guid? erfassungsPeriodId = erfassungsPeriodDropDownItem == null
                                           ? (Guid?)null
                                           : new Guid(erfassungsPeriodDropDownItem.Value);

            var parameter = new TReportParameter { ErfassungsPeriodId = erfassungsPeriodId };
            ViewBag.NetzErfassungsmodus = reportControllerService.GetNetzErfassungsmodus(parameter.ErfassungsPeriodId);
            return parameter;
        }

        private List<Bitmap> PreviewPages
        {
            get
            {
                Report lastGeneratedReport = sessionService.LastGeneratedReport;

                if (lastGeneratedReport == null)
                    return new List<Bitmap>();

                Bitmap tiff = (Bitmap)Image.FromStream(new MemoryStream(lastGeneratedReport.ReportData));
                return ImageHelpers.GetAllPages(tiff).Select(p => TrimBitmap((Bitmap)p, lastGeneratedReport.MaxImagePreviewPageHeight, lastGeneratedReport.MaxImagePreviewPageWidth)).ToList();
            }
        }
        
        private Bitmap TrimBitmap(Bitmap bitmap, int? maxHeight, int? maxWidth)
        {
            if (!maxHeight.HasValue && !maxWidth.HasValue)
                return bitmap;

            var height = maxHeight.HasValue ? Math.Min(bitmap.Height, maxHeight.Value) : bitmap.Height;
            var width = maxWidth.HasValue ? Math.Min(bitmap.Width, maxWidth.Value) : bitmap.Width;

            return bitmap.Clone(new Rectangle(0, 0, width, height), bitmap.PixelFormat);
        }

        public ActionResult GetPreview(Guid? erfassungsPeriodId)
        {
            var parameter = new TReportParameter { ErfassungsPeriodId = erfassungsPeriodId };

            ViewBag.NetzErfassungsmodus = reportControllerService.GetNetzErfassungsmodus(erfassungsPeriodId);
            ViewBag.HasReportData = GetPoProviderForPreview(parameter).HasData;

            PrepareViewBag(erfassungsPeriodId);

            return PartialView("Preview", parameter);
        }

        public virtual ActionResult GetReport(TReportParameter parameter)
        {
            var emsgPoProvider = CreateEmsgPoProvider(parameter);
            var emsgReport = serverReportGenerator.GenerateReport(emsgPoProvider);

            return reportControllerService.GetReportFileResult(emsgReport);
        }

        public virtual ContentResult GenerateReport(TReportParameter parameter)
        {
            var emsgPoProvider = CreateEmsgPoProvider(parameter);
            var emsgReport = serverReportGenerator.GenerateReport(emsgPoProvider);

            sessionService.LastGeneratedReport = emsgReport;

            return new EmsgEmptyResult();
        }

        public virtual ActionResult GetLastGeneratedReport()
        {
            var lastGeneratedReport = sessionService.LastGeneratedReport;
            sessionService.LastGeneratedReport = null;
            return reportControllerService.GetReportFileResult(lastGeneratedReport);
        }

        public virtual ActionResult GetReportImagePreview(TReportParameter parameter)
        {
            parameter.IsPreview = true;

            var emsgPoProvider = GetPoProviderForPreview(parameter);

            ViewBag.HasReportData = emsgPoProvider.HasData;

            if (emsgPoProvider.HasData)
            {
                var emsgReport = serverReportGenerator.GenerateReport(emsgPoProvider);
                sessionService.LastGeneratedReport = emsgReport;
            }

            ViewBag.PageCount = PreviewPages.Count;

            return PartialView("ReportImagePreview", parameter);
        }

        public ActionResult GetPreviewImage(int page)
        {
            if (sessionService.LastGeneratedReport == null)
            {
                var closedErfassungsperiodenDropDownItems = reportControllerService.GetClosedErfassungsperiodenDropDownItems<TReportParameter, TPresentationObject>();
                DropDownListItem erfassungsPeriodDropDownItem = closedErfassungsperiodenDropDownItems.SingleOrDefault(ep => ep.Selected);

                var emsgPoProvider = GetPoProviderForPreview(new TReportParameter
                                                                 {
                                                                     ErfassungsPeriodId = erfassungsPeriodDropDownItem == null
                                                                                              ? (Guid?)null
                                                                                              : new Guid(erfassungsPeriodDropDownItem.Value),
                                                                     IsPreview = true
                                                                 });

                sessionService.LastGeneratedReport = serverReportGenerator.GenerateReport(emsgPoProvider);
            }

            var pngStream = new MemoryStream();
            PreviewPages[page].Save(pngStream, ImageFormat.Png);
            pngStream.Seek(0, 0);

            return File(pngStream.ToArray(), "image/png");
        }

        protected IEmsgPoProviderBase GetPoProviderForPreview(TReportParameter parameter)
        {
            parameter.IsPreview = true;
            parameter.OutputFormat = OutputFormat.Image;
            return CreateEmsgPoProvider(parameter);
        }

        protected override IEmsgPoProviderBase CreateEmsgPoProvider(TReportParameter parameter)
        {
            return emsgPoProviderService.CreateEmsgPoProvider<TReportParameter, TPresentationObject>(parameter);
        }

        protected virtual void PrepareViewBag(Guid? erfassungsPeriodId) { }
    }
}