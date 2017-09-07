using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ASTRA.EMSG.Tests.ResourceTests
{
    [TestFixture]
    public class ResourceLocalizationValueConsistencyTest
    {
        [Test]
        public void ButtonLocalizationTest()
        {
            AssertValueConsistency("ButtonLocalization");
        }

        [Test]
        public void EditorLocalizationTest()
        {
            AssertValueConsistency("EditorLocalization");
        }

        [Test]
        public void EnumLocalizationTest()
        {
            AssertValueConsistency("EnumLocalization");
        }

        [Test]
        public void GridHeaderFooterLocalizationTest()
        {
            AssertValueConsistency("GridHeaderFooterLocalization");
        }

        [Test]
        public void GridLocalizationTest()
        {
            AssertValueConsistency("GridLocalization");
        }

        [Test]
        public void LookupLocalizationTest()
        {
            AssertValueConsistency("LookupLocalization");
        }

        [Test, Ignore]
        public void MapLocalizationTest()
        {
            AssertValueConsistency("MapLocalization");
        }

        [Test]
        public void ModelLocalizationTest()
        {
            AssertValueConsistency("ModelLocalization");
        }

        [Test]
        public void NotificationLocalizationTest()
        {
            AssertValueConsistency("NotificationLocalization");
        }

        [Test]
        public void PageTitleLocalizationTest()
        {
            AssertValueConsistency("PageTitleLocalization");
        }

        [Test]
        public void ReportLocalizationTest()
        {
            AssertValueConsistency("ReportLocalization");
        }

        [Test]
        public void TextLocalizationTest()
        {
            AssertValueConsistency("TextLocalization");
        }

        [Test]
        public void TitleLocalizationTest()
        {
            AssertValueConsistency("TitleLocalization");
        }

        [Test]
        public void UploadLocalizationTest()
        {
            AssertValueConsistency("UploadLocalization");
        }

        [Test]
        public void ValidationErrorLocalizationTest()
        {
            AssertValueConsistency("ValidationErrorLocalization");
        }

        [Test]
        public void MobileLocalizationTest()
        {
            AssertValueConsistency("MobileLocalization", true);
        }

        private void AssertValueConsistency(string localizationName, bool forMobile = false)
        {
            var service = new ResourceComparingService(localizationName, forMobile);
            Assert.IsTrue(service.AreValuesConsistent(), service.GetDifferentValueString());
        }
    }
}
