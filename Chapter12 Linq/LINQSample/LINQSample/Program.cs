using System;

namespace LINQSample
{
    class Program
    {
        static void Main(string[] args)
        {
            LinqQuerySample.LINQQuery();
            Console.WriteLine("----CompoundFromWithMethod---------");
            LinqQuerySample.CompoundFromWithMethod();
            Console.WriteLine("----Grouping---------");
            LinqQuerySample.Grouping();
            Console.WriteLine("----GroupingAndNestedObject---------");
            LinqQuerySample.GroupingAndNestedObject();
            Console.WriteLine("----InnerJoin---------");
            LinqQuerySample.InnerJoin();
            Console.WriteLine("----LeftOuyJoin---------");
            LinqQuerySample.LeftOutJoin();
            Console.WriteLine("----Partition---------");
            LinqQuerySample.Partitioning();
            Console.WriteLine("----RangeSample---------");
            RangeSample.GetRange();
            Console.WriteLine("----Expression---------");
            ExpressionSample.DisplaySample();
        }
    }
}
