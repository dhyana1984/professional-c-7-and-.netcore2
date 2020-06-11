using System;
using System.Collections.Generic;

namespace WebSampleApp
{
    public class DefaultSampleService : ISampleService
    {
        private IList<string> _strings = new List<string> { "one", "two", "three" };
        public IEnumerable<string> GetSampleStrings() => _strings;

    }
}
