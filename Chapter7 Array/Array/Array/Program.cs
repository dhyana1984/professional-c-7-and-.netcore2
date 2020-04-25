using System;
using System.Linq;
using Lib.Person;

namespace ArraySample
{
    class Program
    {
        static void Main(string[] args)
        {
            //定义数组
            int[] array = new int[4];
            //定义二维数组并初始化，定义后就不能修改其阶数了
            int[,] twodim = new int[3, 3]{
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            };

            //定义锯齿数组
            int[][] jagged = new int[3][];
            jagged[0] = new int[2] { 1, 2 };
            jagged[1] = new int[6] { 3, 4, 5, 6, 7, 8 };
            jagged[2] = new int[3] { 9, 10, 11 };


            ComparablePerson[] persons =
            {
                new ComparablePerson("Tom", "Jason", new DateTime(1992, 2, 3)),
                new ComparablePerson("Jim", "Lee", new DateTime(1993, 5, 6)),
                new ComparablePerson("Jack", "Wong", new DateTime(1997, 8, 9))
            };
            Array.Sort(persons);
            persons.ToList().ForEach(t => Console.WriteLine(t));
            Console.WriteLine("-------------");

            //排序条件设置为按照FirstName排序
            Array.Sort(persons, new PersonComparer(PersonCompareType.FirstName));
            persons.ToList().ForEach(t => Console.WriteLine(t));
            Console.WriteLine("-------------");
            HelloCollection hello = new HelloCollection();
            hello.HelloWorld();
            Console.WriteLine("-------------");
            var musicTitles = new MusicTitles();
            foreach (var item in musicTitles)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("-------Reverse------");
            foreach (var item in musicTitles.Reverse())
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("-------Subset------");
            foreach (var item in musicTitles.Subset(2,2))
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("-------IntroSpans------");
            var span1 = IntroSpans();
            Console.WriteLine("-------CreateSlices------");
            var span2 = CreateSlices(span1);
            Console.WriteLine("-------ChangeValues------");
            ChangeValues(span1, span2);

            //Span转Array
            int[] arr = span1.ToArray();

        }

        private static Span<int> IntroSpans()
        {
            int[] arr1 = { 1, 4, 5, 11, 13, 18 };
            //使用Span直接访问数组元素
            var span1 = new Span<int>(arr1);
            span1[1] = 1;
            Console.WriteLine($"arr1[1] is changed via span1[1]:{arr1[1]}");
            return span1;
        }

        //使用span创建数组切片
        private static Span<int> CreateSlices(Span<int> span1)
        {
            Console.WriteLine(nameof(CreateSlices));
            int[] arr2 = { 3, 5, 7, 9, 11, 13, 15 };
            var span2 = new Span<int>(arr2);
            //利用span创建数组切片，直接访问数组不会复制数组元素
            var span3 = new Span<int>(arr2, start: 3, length: 3);
            //直接从span1创造新的切片
            var span4 = span1.Slice(start: 2, length: 4);
            DisplaySpan("Content of Span3", span3);
            DisplaySpan("Content of Span4", span4);
            return span2;
        }

        private static void DisplaySpan(string title, ReadOnlySpan<int> span)
        {
            Console.WriteLine(title);
            for (int i = 0; i < span.Length; i++)
            {
                Console.Write($"{span[i]}.");
            }
            Console.WriteLine();
        }

        private static void ChangeValues(Span<int> span1, Span<int> span2)
        {
            Console.WriteLine(nameof(ChangeValues));
            Span<int> span4 = span1.Slice(start: 4);
            //Clear方法用0填充这个span4
            //同时也修改了span1
            span4.Clear();
            DisplaySpan("Content of span1", span1);
            Span<int> span5 = span2.Slice(start: 3, length: 3);
            //Fill方法填充Span5
            //同时也修改了span2
            span5.Fill(42);
            DisplaySpan("Content of span2", span2);
            span5.CopyTo(span1);
            DisplaySpan("Content of span1", span1);
            if (!span1.TryCopyTo(span4))
            {
                Console.WriteLine("Cannot copy span1 to span4 because span4 is too small");
                Console.WriteLine($"length of span4: {span4.Length}, length of span1: {span1.Length}");
            }

        }

    }
}
