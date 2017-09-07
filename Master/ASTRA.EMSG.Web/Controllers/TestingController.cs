using System;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Web.Infrastructure;
using ASTRA.EMSG.Web.Infrastructure.Security;
using NHibernate.Linq;
using System.Linq;

namespace ASTRA.EMSG.Web.Controllers
{
    public class TestingController : Controller
    {
        private readonly ISessionService sessionService;
        private readonly ICookieService cookieService;
        private readonly IBenchmarkingDataDetailCalculatorService benchmarkingDataDetailCalculatorService;
        private readonly ITransactionScopeProvider transactionScopeProvider;

        public TestingController(
            ISessionService sessionService, 
            ICookieService cookieService,
            IBenchmarkingDataDetailCalculatorService benchmarkingDataDetailCalculatorService,
            ITransactionScopeProvider transactionScopeProvider)
        {
            this.sessionService = sessionService;
            this.cookieService = cookieService;
            this.benchmarkingDataDetailCalculatorService = benchmarkingDataDetailCalculatorService;
            this.transactionScopeProvider = transactionScopeProvider;
        }

        [HttpGet]
        public ActionResult StartNewSession()
        {
            var testSessionService = sessionService as TestSessionService;
            if (testSessionService != null)
                testSessionService.StartNewSession();

            var testCookieService = cookieService as TestCookieService;
            if(testCookieService != null)
                testCookieService.ResetCookies();

            return new EmsgEmptyResult();
        }

        public ViewResult Index()
        {
            return View(new TestModel());
        }

        public ViewResult SimulateError()
        {
            throw new Exception();
        }

        public JsonResult CalculateBenchmarkingData()
        {
            var calculatorService = (ITestBenchmarkingDataDetailCalculatorService) benchmarkingDataDetailCalculatorService;

            var erfassungsPeriod = transactionScopeProvider.CurrentTransactionScope.Session
                .Query<ErfassungsPeriod>()
                .Single(ep => ep.Mandant.MandantName == "4271" && ep.Name == "2013");

            //var erfassungsPeriod = transactionScopeProvider.CurrentTransactionScope.Session
            //    .Query<ErfassungsPeriod>()
            //    .Single(ep => ep.Mandant.MandantName == "4001" && ep.Name == "2011");

            var mandantDetails = transactionScopeProvider.CurrentTransactionScope.Session
                .Query<MandantDetails>()
                .Single(md => md.ErfassungsPeriod == erfassungsPeriod);

            var benchmarkingData = calculatorService.CalculateBenchmarkingDataForTabellarischeModus(erfassungsPeriod, mandantDetails);

            return Json(benchmarkingData, "text/html", JsonRequestBehavior.AllowGet);
        }
    }

    public class TestModel
    {
        public TestModel()
        {
            String = "String";
            Text = "Multi\\nLine\\Text";
            ReadOnlyString = "ReadOnlyString";
            Number = 100;
            Dec = 100.123m;
            DateTime = DateTime.Now;
        }

        public string String { get; set; }
        public string Text { get; set; }
        public string ReadOnlyString { get; set; }
        public int Number { get; set; }
        public decimal Dec { get; set; }
        public DateTime DateTime { get; set; }
    }
}
