using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.Reports;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.AusgefuellteErfassungsformulareFuerOberflaechenschaeden;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using System.Linq;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Tabellarisch, NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W3_3)]
    public class AusgefuellteErfassungsformulareFuerOberflaechenschaedenController : TabellarischReportControllerBase<AusgefuellteErfassungsformulareFuerOberflaechenschaedenParameter, AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo, AusgefuellteErfassungsformulareFuerOberflaechenschaedenGridCommand>
    {
        public AusgefuellteErfassungsformulareFuerOberflaechenschaedenController(ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies)
            : base(tabellarischeReportControllerBaseDependencies)
        {
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
            PrepareViewBag(erfassungsPeriodId);
        }

        protected override void PrepareViewBagForIndex(Guid? erfassungsPeriodId)
        {
            base.PrepareViewBagForIndex(erfassungsPeriodId);
            PrepareViewBag(erfassungsPeriodId);
        }

        private void PrepareViewBag(Guid? erfassungsPeriodId)
        {
            ErfassungsPeriod erfassungsPeriod;
            if (erfassungsPeriodId.HasValue)
                erfassungsPeriod = erfassungsPeriodService.GetEntityById(erfassungsPeriodId.Value);
            else
                erfassungsPeriod = erfassungsPeriodService.GetCurrentErfassungsPeriod();

            ViewBag.NetzErfassungsmodus = erfassungsPeriod.NetzErfassungsmodus;
            ViewBag.Erfassungsjahr = erfassungsPeriod.IsClosed ? erfassungsPeriod.Erfassungsjahr : (DateTime?) null;
        }

        protected override AusgefuellteErfassungsformulareFuerOberflaechenschaedenParameter GetReportParameter(AusgefuellteErfassungsformulareFuerOberflaechenschaedenGridCommand reportGridCommand)
        {
            return new AusgefuellteErfassungsformulareFuerOberflaechenschaedenParameter
                       {
                           Strassenname = reportGridCommand.Strassenname,
                           Inspektionsroutename = reportGridCommand.Inspektionsroutename,
                           Eigentuemer = reportGridCommand.Eigentuemer,

                           AufnahmedatumVon = reportGridCommand.AufnahmedatumVon,
                           AufnahmedatumBis = reportGridCommand.AufnahmedatumBis,

                           ZustandsindexVon = reportGridCommand.ZustandsindexVon,
                           ZustandsindexBis = reportGridCommand.ZustandsindexBis
                       };
        }


        public override ActionResult GetAll([DataSourceRequest] DataSourceRequest dataSourceRequest, AusgefuellteErfassungsformulareFuerOberflaechenschaedenGridCommand command)
        {
            //NOTE: GetAllPreview is used istead of this method.
            return base.GetAll(dataSourceRequest, command);
        }

        public ActionResult GetAllPreview([DataSourceRequest] DataSourceRequest dataSourceRequest, AusgefuellteErfassungsformulareFuerOberflaechenschaedenGridCommand command)
        {
            var reportParameter = GetReportParameterInternal(command);
            var emsgPoProvider = (IAusgefuellteErfassungsformulareFuerOberflaechenschaedenPoProvider)CreateEmsgPoProvider(reportParameter);

            var previewModels = emsgPoProvider.GetPreviewModels(reportParameter, new PresentationObjectProcessor<AusgefuellteErfassungsformulareFuerOberflaechenschaeden>(this));
            var gridModel = previewModels.ToDataSourceResult(dataSourceRequest);
            
            return Json(new AusgefuellteErfassungsformulareResult()
            {
                CanGenerateReport = previewModels.Any(m => m.IstDetaillierteSchadenserfassungsformular),
                Data = gridModel.Data,
                Total = gridModel.Total,
                AggregateResults = gridModel.AggregateResults,
                Errors = gridModel.Errors
            });
        }
    }

    public class AusgefuellteErfassungsformulareResult
    {
        public bool CanGenerateReport { get; set; }

        public IEnumerable Data { get; set; }

        public int Total { get; set; }

        public IEnumerable<AggregateResult> AggregateResults { get; set; }

        public object Errors { get; set; }
    }
}
