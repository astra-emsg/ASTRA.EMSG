using System.IO;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using ASTRA.EMSG.Tests.Common.Utils;
using ASTRA.EMSG.Web.Controllers;
using ASTRA.EMSG.Web.Infrastructure;
using TechTalk.SpecFlow;
using System.Linq;

namespace ASTRA.EMSG.IntegrationTests.Support
{
    [Binding]
    public class EventDefinition
    {
        [BeforeTestRun]
        public static void BeforeFeature()
        {
            //Initialize the MvcTestFramework
            AppHostBuilder.BuildHost();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            if (ScenarioContext.Current.ScenarioInfo.Tags.Contains("Manuell"))
                return;

            //Initialize Db
            if (File.Exists("emsg.sdf"))
                File.Delete("emsg.sdf");
            
            var dbHandlerUtils = new DbHandlerUtils(NHibernateDb.ConfigurationProvider.Value.Configuration);
            dbHandlerUtils.ReCreateDbSchema();

            using (var nHScope = new NHibernateSpecflowScope())
            {
                dbHandlerUtils.GenerateStammDaten(nHScope.Session);
                var transactionScopeProvider = new TransactionScopeProvider(new TestHttpRequestService(), new TransactionScopeFactory(NHibernateDb.ConfigurationProvider.Value));
                transactionScopeProvider.SetCurrentTransactionScope(nHScope.Scope);
            }

            var browserDriver = new BrowserDriver();
            browserDriver.InvokeGetAction<TestingController, ActionResult>((c, r) => c.StartNewSession(), (ActionResult)null);
        }
    }
}
