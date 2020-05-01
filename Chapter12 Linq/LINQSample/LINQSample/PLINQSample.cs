using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LINQSample
{
    public static class PLINQSample
    {
        static void LinqQuery()
        {
            IEnumerable<int> data = SampleData();
            var res = (from x in data.AsParallel()
                       where Math.Log(x) < 4
                       select x).Average();
            Console.WriteLine(res);
        }

        private static IEnumerable<int> SampleData()
        {
            const int arraySize = 50000000;
            var r = new Random();
            return Enumerable.Range(0, arraySize).Select(x => r.Next(140)).ToList();
        }

    }


}
