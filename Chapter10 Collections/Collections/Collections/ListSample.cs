using System;
using System.Collections.Generic;
using Lib.Racer;

namespace Collections
{
    public class ListSample
    {
        public ListSample()
        {

        }

        public void DisplaySample()
        {
            //设置容量为10个元素的List
            List<int> intList = new List<int>(10);
            intList.AddRange(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
            //如果添加的元素个数超过了10个，容量就会变为之前的两倍，也就是20
            Console.WriteLine(intList.Capacity); //20
            //使用TrimExcess()方法可以去掉多余的容量，但是当元素个数超过容量超过90%的时候，不会起作用，因为需要重新定位
            intList.TrimExcess();
            Console.WriteLine(intList.Capacity);//11

            var graham = new Racer(7, "Graham", "Hill", "UK", 14);
            var emerson = new Racer(13, "Emerson", "Fittipaldi", "Brazil", 14);
            var mario = new Racer(16, "Mario", "Andretti", "USA", 12);

            var racers = new List<Racer>(20) { graham, emerson, mario };

            racers.Add(new Racer(24, "Michael", "Schumacher", "Germany", 91));
            racers.Add(new Racer(27, "Mika", "Hakkinen", "Finland", 20));

            racers.AddRange(new Racer[] {
               new Racer(14, "Niki", "Lauda", "Austria", 25),
               new Racer(21, "Alain", "Prost", "France", 51)});

            // insert elements

            racers.Insert(3, new Racer(6, "Phil", "Hill", "USA", 3));

            //FindIndex传入的是一个Predicate委托参数
            //在本例中定义了FindCountry类中的FindCountryPredicate方法，符合Predicate委托签名，传入一个匹配元素，返回一个bool
            int index2 = racers.FindIndex(new FindCountry("Finland").FindCountryPredicate);
            //直接用Lambda表达式也可以
            int index3 = racers.FindIndex(r => r.Country == "Finland");
            Console.WriteLine("index2: " + index2);
            Console.WriteLine("index3: " + index3);
            var bigWinners = racers.FindAll(r => r.Wins > 20);
            Console.WriteLine("------bigWinners-----");
            bigWinners.ForEach(r => Console.WriteLine($"{r:A}"));
            //调用Sort重载，传入IComparer<Racer>的实现对象
            //CompareType是排序规则
            racers.Sort(new RacerComparer(CompareType.Country));
            Console.WriteLine("------RacerComparer Sort-----");
            racers.ForEach(r => Console.WriteLine($"{r:A}"));


            Console.WriteLine("------Comparison Sort-----");
            //此时Sort传入的是Comparison委托，该委托有2个参数，返回值为int类型。
            //相等返回0，第一个比第二个小返回-1，第一个比第二个大返回1
            //public delegate int Comparison<T>(T x, T y)
            racers.Sort((r1, r2) => r2.Wins.CompareTo(r1.Wins));
            racers.ForEach(r => Console.WriteLine($"{r:A}"));
        }

        
    }
}
