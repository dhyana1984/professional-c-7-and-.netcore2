using System;
using Xunit;

namespace UnitTestingSamples.xUnit.Tests
{
    public class DeepThoughtTests
    {
        [Fact]
        public void ResultOfTheAnswerToTheUltimateQuestionOfLifeTheUniverseAndEverything()
        {
            // arrange
            int expected = 42;
            var dt = new DeepThought();

            // act
            int actual =
              dt.TheAnswerToTheUltimateQuestionOfLifeTheUniverseAndEverything();

            // assert
            Assert.Equal(expected, actual);
        }
    }
}
