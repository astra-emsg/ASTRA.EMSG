using NUnit.Framework;

namespace ASTRA.EMSG.IntegrationTests.ResourceTests
{
    [TestFixture]
    public class ResourceLocalizationEmptinessTest
    {

        [Test]
        public void ButtonLocalizationTest()
        {
            AssertMissingValues("ButtonLocalization");
        }

        [Test]
        public void EditorLocalizationTest()
        {
            AssertMissingValues("EditorLocalization");
        }

        [Test]
        public void EnumLocalizationTest()
        {
            AssertMissingValues("EnumLocalization", "ZustandsindexTyp_Unbekannt");
        }

        [Test]
        public void GridHeaderFooterLocalizationTest()
        {
            AssertMissingValues("GridHeaderFooterLocalization");
        }

        [Test]
        public void GridLocalizationTest()
        {
            AssertMissingValues("GridLocalization");
        }

        [Test]
        public void LookupLocalizationTest()
        {
            AssertMissingValues("LookupLocalization");
        }

        [Test]
        public void MapLocalizationTest()
        {
            AssertMissingValues("MapLocalization");
        }

        [Test]
        public void ModelLocalizationTest()
        {
            AssertMissingValues("ModelLocalization");
        }

        [Test]
        public void NotificationLocalizationTest()
        {
            AssertMissingValues("NotificationLocalization");
        }

        [Test]
        public void PageTitleLocalizationTest()
        {
            AssertMissingValues("PageTitleLocalization");
        }

        [Test]
        public void ReportLocalizationTest()
        {
            AssertMissingValues("ReportLocalization");
        }

        [Test]
        public void TextLocalizationTest()
        {
            AssertMissingValues("TextLocalization", "EmptyMessage");
        }

        [Test]
        public void TitleLocalizationTest()
        {
            AssertMissingValues("TitleLocalization");
        }

        [Test]
        public void UploadLocalizationTest()
        {
            AssertMissingValues("UploadLocalization");
        }

        [Test]
        public void ValidationErrorLocalizationTest()
        {
            AssertMissingValues("ValidationErrorLocalization");
        }

        private void AssertMissingValues(string localizationName, params string[] ignoredKeys)
        {
            var service = new ResourceComparingService(localizationName);
            Assert.IsTrue(service.AllValuesExist(ignoredKeys), service.GetMissingsKeysString(ignoredKeys));
        }
    }
}