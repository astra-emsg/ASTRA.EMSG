using System;
using System.Collections.Generic;
using ASTRA.EMSG.Localization;
using NUnit.Framework;

namespace ASTRA.EMSG.Tests.ResourceTests
{
    [TestFixture]
    public class ResourceLocalizationTest
    {

        [Test]
        public void ButtonLocalizationTest()
        {
            AssertKeyIdentity("ButtonLocalization");
        }

        [Test]
        public void MenuLocalizationTest()
        {
            AssertKeyIdentity("MenuLocalization");
        }

        [Test]
        public void EditorLocalizationTest()
        {
            AssertKeyIdentity("EditorLocalization");
        }

        [Test]
        public void EnumLocalizationTest()
        {
            AssertKeyIdentity("EnumLocalization");
        }

        [Test]
        public void GridHeaderFooterLocalizationTest()
        {
            AssertKeyIdentity("GridHeaderFooterLocalization");
        }

        [Test]
        public void GridLocalizationTest()
        {
            AssertKeyIdentity("GridLocalization");
        }

        [Test]
        public void LookupLocalizationTest()
        {
            AssertKeyIdentity("LookupLocalization");
        }

        [Test, Ignore]
        public void MapLocalizationTest()
        {
            AssertKeyIdentity("MapLocalization");
        }

        [Test]
        public void ModelLocalizationTest()
        {
            AssertKeyIdentity("ModelLocalization");
        }

        [Test]
        public void NotificationLocalizationTest()
        {
            AssertKeyIdentity("NotificationLocalization");
        }

        [Test]
        public void PageTitleLocalizationTest()
        {
            AssertKeyIdentity("PageTitleLocalization");
        }

        [Test]
        public void ReportLocalizationTest()
        {
            AssertKeyIdentity("ReportLocalization");
        }

        [Test]
        public void TextLocalizationTest()
        {
            AssertKeyIdentity("TextLocalization");
        }

        [Test]
        public void TitleLocalizationTest()
        {
            AssertKeyIdentity("TitleLocalization");
        }

        [Test]
        public void UploadLocalizationTest()
        {
            AssertKeyIdentity("UploadLocalization");
        }

        [Test]
        public void ValidationErrorLocalizationTest()
        {
            AssertKeyIdentity("ValidationErrorLocalization");
        }

        [Test]
        public void MobileLocalizationTest()
        {
            AssertKeyIdentity("MobileLocalization", true);
        }

        private void AssertKeyIdentity(string localizationName, bool forMobile = false)
        {
            var service = new ResourceComparingService(localizationName, forMobile);
            Assert.IsTrue(service.AreKeysIdentical(),service.GetDifferentKeysString());
        }

    }
}
