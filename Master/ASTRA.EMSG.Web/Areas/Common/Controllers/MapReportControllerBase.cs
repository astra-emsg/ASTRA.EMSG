using System.Web.Mvc;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Web.Areas.Common.ControllerServices;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using Kendo.Mvc;

namespace ASTRA.EMSG.Web.Areas.Common.Controllers
{
    public abstract class MapControllerBase<TReportParameter, TPresentationObject> : Controller
        where TPresentationObject : new()
        where TReportParameter : EmsgReportParameter
    {
        protected readonly IReportControllerService reportControllerService;

        protected MapControllerBase(IReportControllerService reportControllerService)
        {
            this.reportControllerService = reportControllerService;
        }
    }
}