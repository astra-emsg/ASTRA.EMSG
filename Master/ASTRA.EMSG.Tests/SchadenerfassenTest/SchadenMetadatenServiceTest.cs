using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Services.SchadenMetadaten;
using NUnit.Framework;

namespace ASTRA.EMSG.Tests.SchadenerfassenTest
{
    [TestFixture]
    public class SchadenMetadatenServiceTest
    {
        [Test]
        public void SchadenMetadatenShouldBeInitialized()
        {
            var schadenMetadatenService = new SchadenMetadatenService();
            Assert.AreEqual(5, schadenMetadatenService.GetSchadengruppeMetadaten(BelagsTyp.Asphalt).Count);
            Assert.AreEqual(6, schadenMetadatenService.GetSchadengruppeMetadaten(BelagsTyp.Beton).Count);
        }
    }
}
