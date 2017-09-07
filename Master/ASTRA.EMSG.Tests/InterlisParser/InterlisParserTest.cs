using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using ASTRA.EMSG.Business.Interlis.Parser;
using System.Diagnostics;

namespace ASTRA.EMSG.Tests.InterlisParser
{
    [TestFixture]
    public class InterlisParserTest
    {
        [Test]
        public void ParseAxisFileTest()
        {
            AchsenCollectedData dataIncr;
            
            {
                String filename = @"InterlisParser\TestData\2012_01_04_export_Achsen_CH_incr_1.09.2011_19.12.2011.xtf";

                AchsenCollectedData data = new AchsenCollectedData();

                AxisReader2 axisReader = new AxisReader2(filename, data);

                axisReader.Parse();

                dataIncr = data;
            }

            Debug.WriteLine("Statistics: " + dataIncr.Statistics());

            Assert.AreEqual(29, dataIncr.achsenDict.Count);
            Assert.AreEqual(57, dataIncr.achsenSegmentDict.Count);


            var ai = dataIncr.achsenToSegmentDict[new Guid("b96a78e8-2b24-6a43-b23b-55e2b6d269ca")];
        }

        [Test]
        [Ignore("Requires big Interlis file")]
        public void ParseFullAxisFileTestCHKT()
        {
            AchsenCollectedData dataIncr;

            {
                String filename = @"C:\temp\emsg_achsen\Achsdaten_full_2011_09_01\20111219110540_Axis_CH_Kt_Full_2011_09_01.xtf";

                AchsenCollectedData data = new AchsenCollectedData();

                AxisReader2 axisReader = new AxisReader2(filename, data);

                axisReader.Parse();

                dataIncr = data;
            }

            Debug.WriteLine("Statistics: " + dataIncr.Statistics());


        }

        [Test]
        [Ignore("Requires big Interlis file")]
        public void ParseFullAxisFileTestGS()
        {
            AchsenCollectedData dataIncr;

            {
                String filename = @"C:\temp\emsg_achsen\Achsdaten_full_2011_09_01\20111219135120_Axis_GS_Full_2011_09_01.xtf";

                AchsenCollectedData data = new AchsenCollectedData();

                AxisReader2 axisReader = new AxisReader2(filename, data);

                axisReader.Parse();

                dataIncr = data;
            }

            Debug.WriteLine("Statistics: " + dataIncr.Statistics());
        }


    }
}
