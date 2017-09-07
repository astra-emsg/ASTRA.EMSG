using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.GISExport;
using ASTRA.EMSG.Business.Reports.StrassenabschnitteListe;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Filters;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Business.Services.GIS.Shape;
using Resources;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W6_1)]
    public class GISExportController : TabellarischReportControllerBase<GISExportParameter, GISExportPo, GISExportGridCommand>
    {
        private readonly IErfassungsPeriodService erfassungsPeriodService;
        private readonly IShapeExportService shapeExportService;
        private readonly ILocalizationService localizationService;

        public GISExportController(ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies,
            IShapeExportService shapeExportService,
            IErfassungsPeriodService erfassungsPeriodService,
            ILocalizationService localizationService) : base(tabellarischeReportControllerBaseDependencies)
        {
            this.erfassungsPeriodService = erfassungsPeriodService;
            this.shapeExportService = shapeExportService;
            this.localizationService = localizationService;
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
            ViewBag.CurrentErfassungsPeriodId = erfassungsPeriodService.GetCurrentErfassungsPeriod().Id;
        }

        protected override GISExportParameter GetReportParameter(GISExportGridCommand reportGridCommand)
        {
            var reportParameter = new GISExportParameter
                {
                    Type = reportGridCommand.ExportType,
                    Periode = erfassungsPeriodService.GetEntityById(reportGridCommand.ErfassungsPeriodId.Value)
                };

            return reportParameter;
        }

        public ActionResult Export(GISExportGridCommand command)
        {
            var filestream = shapeExportService.Export(erfassungsPeriodService.GetEntityById(command.ErfassungsPeriodId.Value), command.ExportType);

            if (filestream == null)
            {
                ViewBag.ErrorMessage = ValidationErrorLocalization.NoGISDataForExportError;
                return View("Index");
            }
            var file = File(filestream, "application/zip");
            
            switch(command.ExportType)
            {
                case ShapeExportType.Strassenabschnitt:
                    file.FileDownloadName = String.Format(@"{0}.zip", TextLocalization.Strassenabschnitte);    
                    break;
                case ShapeExportType.Zustandsabschnitt:
                    file.FileDownloadName = String.Format(@"{0}.zip", TextLocalization.Zustandsabschnitte); 
                    break;
                case ShapeExportType.Trottoir:
                    file.FileDownloadName = String.Format(@"{0}.zip", TextLocalization.Trottoir); 
                    break;
                default:
                    file.FileDownloadName = "Export";
                    break;
            }
            return file;
        }
    }
}
