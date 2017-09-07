using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using NUnit.Framework;

namespace ASTRA.EMSG.IntegrationTests.ResourceTests
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

        [Test]        
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

        private void AssertKeyIdentity(string localizationName, IEnumerable<string> languageAbbreviations = null)
        {
            var service = new ResourceComparingService(localizationName,languageAbbreviations);
            Assert.IsTrue(service.AreKeysIdentical(),service.GetDifferentKeysString());
        }
    }
}
