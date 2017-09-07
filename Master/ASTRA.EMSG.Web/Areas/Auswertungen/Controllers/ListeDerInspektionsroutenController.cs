using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.ListeDerInspektionsrouten;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using ASTRA.EMSG.Web.Areas.Common.Controllers;
using ASTRA.EMSG.Web.Areas.Common.DependencyPackages;
using System.Web.Mvc;
using System.Net;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.Web.Infrastructure.Security;
using ASTRA.EMSG.Business.Services.GIS;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.Controllers
{
    [AllowedModes(NetzErfassungsmodus.Gis, NetzErfassungsmodus.Summarisch, NetzErfassungsmodus.Tabellarisch)]
    [ReportInfo(AuswertungTyp.W1_3)]
    public class ListeDerInspektionsroutenController : GisReportControllerBase<ListeDerInspektionsroutenParameter, ListeDerInspektionsroutenMapParameter, ListeDerInspektionsroutenPo, ListeDerInspektionsroutenGridCommand>
    {
        private readonly IServerConfigurationProvider serverConfigurationProvider;
        private readonly ILegendService legendService;
        public ListeDerInspektionsroutenController(ITabellarischeReportControllerBaseDependencies tabellarischeReportControllerBaseDependencies, 
            IServerConfigurationProvider serverConfigurationProvider, ILegendService legendService)
            : base(tabellarischeReportControllerBaseDependencies)
        {
            this.serverConfigurationProvider = serverConfigurationProvider;
            this.legendService = legendService;
        }

        protected override void PrepareViewBagForPreview(Guid? erfassungsPeriodId)
        {
            ViewBag.NetzErfassungsmodus = reportControllerService.GetNetzErfassungsmodus(erfassungsPeriodId);
        }

        [HttpGet]
        [AccessRights(Rolle.Applikationsadministrator, Rolle.Applikationssupporter, Rolle.DataReader)]
        public ActionResult GetInspektionsRouteReportLegendImage(string query)
        {
            try
            {
                return this.legendService.GetInspektionsRouteLegendImage(int.Parse(query));
            }
            catch (System.Net.WebException ex)
            {
                return new HttpNotFoundResult(ex.Message.ToString());
            }
            catch (Exception ex)
            {
                return new HttpUnauthorizedResult(ex.Message.ToString());
            }

        }
        protected override void SetMapReportParameter(ListeDerInspektionsroutenParameter mapParameter, ListeDerInspektionsroutenGridCommand reportGridCommand)
        {
           mapParameter.Eigentuemer = reportGridCommand.EigentuemerTyp;
           mapParameter.Strassenname = reportGridCommand.Strassenname;
           mapParameter.Inspektionsroutename = reportGridCommand.Inspektionsroutename;
           mapParameter.InspektionsrouteInInspektionBei = reportGridCommand.InspektionsrouteInInspektionBei;
           mapParameter.InspektionsrouteInInspektionBisVon = reportGridCommand.InspektionsrouteInInspektionBisVon;
           mapParameter.InspektionsrouteInInspektionBisBis = reportGridCommand.InspektionsrouteInInspektionBisBis;
           mapParameter.LegendImageBaseUrl = string.Format(@"{0}://{1}{2}",HttpContext.Request.Url.Scheme, HttpContext.Request.Url.Authority,Url.RouteUrl("InspektionsrouteLegendLabel"));
           
        }
    }
}
