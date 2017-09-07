using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Text.RegularExpressions;
using ASTRA.EMSG.Business.Interlis.AxisImportScanner;

namespace ASTRA.EMSG.Tests.InterlisParser
{
    [TestFixture]
    class InterlisFilenameTest
    {
        [Test]
        public void TestSenderDate()
        {

            string file = @"AxisImportDbTests\TestData\36b9a4a6-5a25-4e40-b8e4-c3ebb427943b_20120301080529_Axis.xtf";

            DateTime? date = ImportFolders.ParseSenderDate(file);

            Assert.AreEqual(new DateTime(2012, 3, 1, 8, 5, 29), date);
        }


        //[Test]
        public void TestGetOldestFile()
        {
            
            List<string> files = new List<string>() {
                @"AxisImportDbTests\TestData\36b9a4a6-5a25-4e40-b8e4-c3ebb427943b_20120401172551_Axis.xtf",
                @"AxisImportDbTests\TestData\36b9a4a6-5a25-4e40-b8e4-c3ebb427943b_20120301080529_Axis.xtf",
                @"AxisImportDbTests\TestData\36b9a4a6-5a25-4e40-b8e4-c3ebb427943b_20120201110217_MbsInventory.xtf"
            };


            string oldest = ImportFolders.GetOldestFile(files);

            //Assert.AreEqual(oldest, @"AxisImportDbTests\TestData\36b9a4a6-5a25-4e40-b8e4-c3ebb427943b_20120301080529_Axis.xtf");
        }


    }
}
