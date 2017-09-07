using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using ASTRA.EMSG.Tests.Common.Utils;
using ASTRA.EMSG.Web.Controllers;
using ASTRA.EMSG.Web.Infrastructure;
using NHibernate.Linq;
using NUnit.Framework;

namespace ASTRA.EMSG.IntegrationTests.Common
{
    public abstract class IntegrationTestBase
    {
        protected const string TestUserName = "Test";
        protected const string TestMandantName = "TestMandant";
        protected const string OtherTestMandantName = "OtherTestMandantName";

        protected BrowserDriver BrowserDriver { get; private set; }
        protected ILocalizationService LocalizationService { get; private set; }

        [SetUp]
        protected void Initialize()
        {
            var appHost = AppHostBuilder.AppHost;

            //Initialize Db
            if (File.Exists("emsg.sdf"))
                File.Delete("emsg.sdf");
            
            var dbHandlerUtils = new DbHandlerUtils(NHibernateDb.ConfigurationProvider.Value.Configuration);
            dbHandlerUtils.ReCreateDbSchema();
            

            using (var nHScope = new NHibernateTestScope())
            {
                dbHandlerUtils.GenerateStammDaten(nHScope.Session);
                var transactionScopeProvider = new TransactionScopeProvider(new TestHttpRequestService(), new TransactionScopeFactory(NHibernateDb.ConfigurationProvider.Value));
                transactionScopeProvider.SetCurrentTransactionScope(nHScope.Scope);
            }

            BrowserDriver = new BrowserDriver();
            BrowserDriver.InvokeGetAction<TestingController, ActionResult>((c, r) => c.StartNewSession(), (ActionResult)null);

            var cultureInfo = CultureInfo.CreateSpecificCulture("de-at");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            LocalizationService = new StubLocalizationService();

            DbInit();
        }

        protected virtual void DbInit() { }

        protected virtual NetzErfassungsmodus Erfassungmodus { get { return NetzErfassungsmodus.Gis; } }
    }
}