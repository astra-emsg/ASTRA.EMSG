using System.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecRun;

namespace ASTRA.EMSG.IntegrationTests.StepDefinitions
{
    [Binding]
    public class ManualSteps
    {
        [Given(".*"), When(".*"), Then(".*"), Scope(Tag = "Manuell")]
        public void EmptyStep()
        {
            HandleManualStep();
        }

        [Given(".*"), When(".*"), Then(".*"), Scope(Tag = "Manuell")]
        public void EmptyStep(string param)
        {
            HandleManualStep();
        }

        [Given(".*"), When(".*"), Then(".*"), Scope(Tag = "Manuell")]
        public void EmptyStep(Table param)
        {
            HandleManualStep();
        }

        private void HandleManualStep()
        {
            //NOP: Always green
        }

        [BeforeScenario("Manuell")]
        public void BeforeManualScenario()
        {
            HandleManualStep();
        }

        [Given("(.*)\\(manuell\\)(.*)"), When("(.*)\\(manuell\\)(.*)"), Then("(.*)\\(manuell\\)(.*)")]
        public void ManuellStep(string prefix, string postfix, Table param)
        {
            HandleManualStep();
        }
    }

}
