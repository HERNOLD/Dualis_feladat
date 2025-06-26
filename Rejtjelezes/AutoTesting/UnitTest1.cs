using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rejtjelezes;
using System;

namespace AutoTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string message = "hello world";
            string key = "xmckl xmckl";

            string encoded = Task1.Encode(message, key);
            string decoded = Task1.Decode(encoded, key);

            Assert.AreEqual(message, decoded);
        }
    }
}
