using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestingSamples.MSTests
{
    [TestClass]
    public class StringSampleTests
    {
        [TestMethod]
        //使用这个Attribute，可以捕捉到ArgumentNullException这个异常,不加的话会报错
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorShouldThrowOnNull()
        {
            var sample = new StringSample(null);
        }

        [TestMethod]
        public void GetStringDemoBNotInA()
        {
            string expected = "b not found in a";
            var sample = new StringSample(String.Empty);
            string actual = sample.GetStringDemo("a", "b");
            Assert.AreEqual(expected, actual);
        }
    }
}
