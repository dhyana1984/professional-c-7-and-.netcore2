using System;
using Xunit;

namespace UnitTestingSamples.xUnit.Tests
{
    
    public class StringSampleTest
    {
        [Fact]//xUnit测试方法只用带[Fact]Attribute就可以了
        public void GetStringSampleExceptions()
        {
            var sample = new StringSample(String.Empty);
            Assert.Throws<ArgumentNullException>(() => sample.GetStringDemo(null, "a"));
            Assert.Throws<ArgumentNullException>(() => sample.GetStringDemo("a", null));
            Assert.Throws<ArgumentException>(() => sample.GetStringDemo(string.Empty, "a"));
        }
    }
}
