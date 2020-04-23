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
        }
    }
}
