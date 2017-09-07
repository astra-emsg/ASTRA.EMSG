using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.StrassenabschnitteListe;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Security;
using Resources;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W1_6, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class StrassenabschnitteListeGISController : GisReportControllerBase<StrassenabschnitteListeParameter, StrassenabschnitteListeMapParameter, StrassenabschnitteListePo, StrassenabschnitteListeGridCommand>
    {
        private readonly IBelastungskategorieService belastungskategorieService;

        public StrassenabschnitteListeGISController(
            ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies,
            IBelastungskategorieService belastungskategorieService
            )
            : base(tabellarischeReportControllerBaseDependencies)
        {
            this.belastungskategorieService = belastungskategorieService;
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
            ViewBag.Belastungskategorien = GetBelastungskategorieDropDownItemList();
            ViewBag.NetzErfassungsmodus = reportControllerService.GetNetzErfassungsmodus(erfassungsPeriodId);
        }

        protected override void PrepareViewBagForIndex(Guid? erfassungsPeriodId)
        {
            base.PrepareViewBagForIndex(erfassungsPeriodId);
            ViewBag.Belastungskategorien = GetBelastungskategorieDropDownItemList();
        }

        private IEnumerable<DropDownListItem> GetBelastungskategorieDropDownItemList()
        {
            return belastungskategorieService.AllBelastungskategorieModel
                .ToDropDownItemList(bk => LookupLocalization.ResourceManager.GetString(bk.Typ), bk => bk.Id, null, TextLocalization.All);
        }

        protected override void SetMapReportParameter(StrassenabschnitteListeParameter parameter, StrassenabschnitteListeGridCommand reportGridCommand)
        {
            parameter.Belastungskategorie = reportGridCommand.Belastungskategorie;
            parameter.Eigentuemer = reportGridCommand.EigentuemerTyp;
            parameter.Ortsbezeichnung = reportGridCommand.Ortsbezeichnung;
        }
    }
}