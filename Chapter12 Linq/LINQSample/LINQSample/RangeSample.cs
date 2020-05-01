using System;
using System.Linq;

namespace LINQSample
{
    public static class RangeSample
    {
        public static void GetRange()
        {
            var ranges = Enumerable.Range(1, 20); //生成一个1到20的IEnumerable的集合
            foreach (var item in ranges)
            {
                Console.Write($"{item} ", item);
            }
            Console.WriteLine();

            ranges = Enumerable.Range(1, 20).Select(n => n * 3);
            foreach (var item in ranges)
            {
                Console.Write($"{item} ", item);
            }
        }
    }
}