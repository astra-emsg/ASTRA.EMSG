using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using ASTRA.EMSG.Web.Areas.NetzverwaltungStrassennamen.Controllers;
using ASTRA.EMSG.Web.Areas.NetzverwaltungSummarisch.Controllers;
using TechTalk.SpecFlow;

namespace ASTRA.EMSG.IntegrationTests.StepDefinitions
{
    [Binding]
    public class CommonSteps : StepsBase
    {
        public CommonSteps(BrowserDriver browserDriver) : base(browserDriver)
        {
        }

        [Given(@"ich öffne die Seite '(.+)'")]
        public void AngenommenIchOffneDieSeite(string seiteName)
        {
            switch (seiteName)
            {
                case "Netzdefinition und Hauptabschnitt mit Strassennamen":
                    BrowserDriver.InvokeGetAction<NetzdefinitionUndStrassenabschnittController>(c => c.Index());
                    break;
                case "Zustands- und Netzinformationen im Summarischen Modus":
                    BrowserDriver.InvokeGetAction<StrassenmengeUndZustandController>(c => c.Index());
                    break;
            }
        }

        [Then(@"liefert Feldbezeichnung '(.+)' einen Validationsfehler '(.+)'")]
        public void DannLiefertFeldbezeichnungEinenValidationsfehlerValidationfehler(string propertyName, string hasValidationError)
        {
            if (!propertyName.IsNull())
                AssertValidationFehler(hasValidationError.ParseBool(), GetObjectReaderConfiguration().GetPropertyName(propertyName));
            else
                AssertValidationFehler(hasValidationError.ParseBool());
        }
    }
}
