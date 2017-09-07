using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.MengeProBelastungskategorie;
using ASTRA.EMSG.Business.Reports.MengeProBelastungskategorieGrafische;
using ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProStrassenabschnitt;
using ASTRA.EMSG.Common.Master.Utils;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using ASTRA.EMSG.IntegrationTests.Support.ObjectReader;
using ASTRA.EMSG.Web.Areas.Auswertungen.Controllers;
using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using NHibernate.Linq;

namespace ASTRA.EMSG.IntegrationTests.StepDefinitions
{
    [Binding]
    public class WiederbeschaffungswertKatalogSteps : StepsBase
    {
        public WiederbeschaffungswertKatalogSteps(BrowserDriver browserDriver)
            : base(browserDriver)
        {
        }

        [Given(@"es wurden vom Benutzeradministrator keine mandantenspezifische Wiederbeschaffungswerte definiert")]
        public void AngenommenEsWurdenVomBenutzeradministratorKeineMandantenspezifischeWiederbeschaffungswerteDefiniert()
        {
            //SKIP
        }

        [Given(@"es wurden vom Benutzeradministrator keine mandantenspezifische Alterungsbeiwerte definiert")]
        public void AngenommenEsWurdenVomBenutzeradministratorKeineMandantenspezifischeAlterungsbeiwerteDefiniert()
        {
            //SKIP
        }
    }
}
