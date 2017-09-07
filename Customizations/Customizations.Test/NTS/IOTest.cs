using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NetTopologySuite.IO;

namespace Customizations.Test.NTS
{
     [TestFixture]
    public class IOTest
    {
         [Test]
         public void TestBinarySubstring()
         {
             DbaseMemoryStreamWriter writer = new DbaseMemoryStreamWriter(Encoding.UTF8);

             string test1 = "asdfasdf";
             string test2 = "äsdfasdf";
             string test3 = "asdfäsdf";
             string test4 = "asäääsdf";
             string test5 = "ääääääää";

             Assert.AreEqual("asdf", writer.BinarySubstring(test1, 4));
             Assert.AreEqual("äsd", writer.BinarySubstring(test2, 4));
             Assert.AreEqual("asdf", writer.BinarySubstring(test3, 4));
             Assert.AreEqual("asä", writer.BinarySubstring(test4, 4));
             Assert.AreEqual("ää", writer.BinarySubstring(test5, 4));

             Assert.AreEqual("", writer.BinarySubstring(test5, 1));
             Assert.AreEqual("ä", writer.BinarySubstring(test5, 3));
         }
    }
}
