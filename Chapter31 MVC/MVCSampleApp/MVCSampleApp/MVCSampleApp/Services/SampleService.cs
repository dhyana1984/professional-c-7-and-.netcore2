using System;
using System.Collections.Generic;

namespace MVCSampleApp.Services
{
    public interface ISampleService
    {
        IEnumerable<string> GetSampleStrings();
    }

    public class SampleService : ISampleService
    {
        public IEnumerable<string> GetSampleStrings() => new[] { "one", "two", "three", "four" };
    }
}