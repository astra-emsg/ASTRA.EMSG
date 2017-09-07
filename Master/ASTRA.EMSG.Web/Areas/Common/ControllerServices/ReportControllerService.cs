using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Web.Infrastructure;
using Resources;
using Kendo.Mvc.UI;
using ASTRA.EMSG.Business.Utils;

namespace ASTRA.EMSG.Web.Areas.Common.ControllerServices
{
    public interface IReportControllerService
    {
        IEnumerable<DropDownListItem> GetClosedErfassungsperiodenDropDownItems<TReportParameter, TPresentationObject>()
            where TReportParameter : EmsgReportParameter
            where TPresentationObject : new();

        IEnumerable<ErfassungsPeriodModel> GetSupportedErfassungsperioden<TReportParameter>()
             where TReportParameter : EmsgReportParameter;

        ActionResult GetReportFileResult(Report report);
        NetzErfassungsmodus GetNetzErfassungsmodus(Guid? erfassungsPeriodId);
    }

    public class ReportControllerService : IReportControllerService
    {
        protected readonly IEmsgPoProviderService emsgPoProviderService;
        private readonly IHistorizationService historizationService;
        private readonly ILocalizationService localizationService;
        private readonly IErfassungsPeriodService erfassungsPeriodService;

        public ReportControllerService(
            IEmsgPoProviderService emsgPoProviderService,
            IHistorizationService historizationService,
            ILocalizationService localizationService,
            IErfassungsPeriodService erfassungsPeriodService)
        {
            this.emsgPoProviderService = emsgPoProviderService;
            this.historizationService = historizationService;
            this.localizationService = localizationService;
            this.erfassungsPeriodService = erfassungsPeriodService;
        }

        public IEnumerable<DropDownListItem> GetClosedErfassungsperiodenDropDownItems<TReportParameter, TPresentationObject>()
            where TReportParameter : EmsgReportParameter
            where TPresentationObject : new()
        {
            var erfassungsPeriodModels = historizationService
                .GetClosedErfassungsperiods()
                .OrderByDescending(ep => ep.Erfassungsjahr.Year)
                .ToList();

            ErfassungsPeriodModel selected = erfassungsPeriodModels.FirstOrDefault();
            bool isReportAvailableInCurrentErfassungPeriod = emsgPoProviderService.IsReportAvailableInCurrentErfassungPeriod<TReportParameter, TPresentationObject>();
            if (!isReportAvailableInCurrentErfassungPeriod && selected == null)
                selected = erfassungsPeriodModels.FirstOrDefault();

            if (isReportAvailableInCurrentErfassungPeriod)
            {
                erfassungsPeriodModels.Insert(0, historizationService.GetCurrentErfassungsperiodModel());
                selected = null;
            }
            return erfassungsPeriodModels.ToDropDownItemList(GetErfassungsPeriodName, ep => ep.Id, selected);
        }

        public IEnumerable<ErfassungsPeriodModel> GetSupportedErfassungsperioden<TReportParameter>()
            where TReportParameter : EmsgReportParameter
        {
            var erfassungsPeriodModels = historizationService
                .GetClosedErfassungsperiods()
                .OrderByDescending(ep => ep.Erfassungsjahr.Year)
                .ToList();
            var supportedNetzErfassungsmodus = emsgPoProviderService.GetSupportedNetzErfassungsmodus<TReportParameter>();
            erfassungsPeriodModels = erfassungsPeriodModels.Where(e => supportedNetzErfassungsmodus.Contains(e.NetzErfassungsmodus)).ToList();
            return erfassungsPeriodModels;
        }

        private string GetErfassungsPeriodName(ErfassungsPeriodModel ep)
        {
            if (!ep.IsClosed)
                return TextLocalization.Current;

            return string.Format("{0} ({1})", ep.Erfassungsjahr.Year, localizationService.GetLocalizedEnum(ep.NetzErfassungsmodus));
        }

        public ActionResult GetReportFileResult(Report report)
        {
            if (report == null)
                return new EmsgEmptyResult();

            if (report.HasData)
            {
                return new FileContentResult(report.ReportData, report.MimeType)
                           {FileDownloadName = string.Format("{0}.{1}", report.FileName.ToASCIIString(), report.FileExtension)};
            }
            return new FileContentResult(Encoding.UTF8.GetBytes("NoData"), "NoData");
        }

        public NetzErfassungsmodus GetNetzErfassungsmodus(Guid? erfassungsPeriodId)
        {
            NetzErfassungsmodus netzErfassungsmodus;
            if (erfassungsPeriodId.HasValue)
                netzErfassungsmodus = erfassungsPeriodService.GetEntityById(erfassungsPeriodId.Value).NetzErfassungsmodus;
            else
                netzErfassungsmodus = historizationService.GetCurrentErfassungsperiod().NetzErfassungsmodus;

            return netzErfassungsmodus;
        }
    }
}
