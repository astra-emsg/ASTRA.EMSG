using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using ASTRA.EMSG.Web.Areas.Administration.Controllers;
using TechTalk.SpecFlow;

namespace ASTRA.EMSG.IntegrationTests.StepDefinitions
{
    [Binding]
    public class AdministrationSteps : StepsBase
    {
        public AdministrationSteps(BrowserDriver browserDriver) : base(browserDriver)
        {
        }

        [Given(@"ich einen Jahresabschluss für das Jahr '(.+)' durchführe")]
        public void AngenommenIchEinenJahresabschlussFurDasJahrDurchfuhre(string year)
        {
            BrowserDriver.InvokePostAction<ErfassungsPeriodAbschlussController, ErfassungsabschlussModel>(
                (c, r) => c.ErfassungsPeriodAbschluss(r),
                new ErfassungsabschlussModel() {AbschlussDate = new DateTime(int.Parse(year), 1, 1)}, false);
            BrowserDriver.GetRequestResult<TestPartialViewResult>();
        }

    }
}
