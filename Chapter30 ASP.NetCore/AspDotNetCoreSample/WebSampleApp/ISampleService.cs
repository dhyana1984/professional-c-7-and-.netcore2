using System;
using System.Collections;
using System.Collections.Generic;

namespace WebSampleApp
{
    public interface ISampleService
    {
        IEnumerable<string> GetSampleStrings();
    }
}
