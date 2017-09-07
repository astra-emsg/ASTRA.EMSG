using System.Web.Mvc;
using ASTRA.EMSG.Business.Reporting;

namespace ASTRA.EMSG.Web.Areas.Common.Controllers
{
    public abstract class ReportControllerBase<TReportParameter, TPresentationObject> : Controller
        where TPresentationObject : new()
        where TReportParameter : EmsgReportParameter
    {
        protected abstract IEmsgPoProviderBase CreateEmsgPoProvider(TReportParameter parameter);
    }
}