using System;
using System.Collections.Generic;
using System.Linq;
using DataLib;

namespace LINQSample
{
    public static class LinqQuerySample
    {


        public static void LINQQuery()
        {
            //使用linq语句
            var query = from r in Formula1.GetChampions()
                        where r.Country == "Brazil"
                        orderby r.Wins descending
                        select r;
            //使用linq扩展方法
            var champions = new List<Racer>(Formula1.GetChampions());
            IEnumerable<Racer> brizilChampions = champions.Where(t => t.Country == "Brazil").OrderBy(t => t.Wins);

            //foreach (var r in query)
            //{
            //    Console.WriteLine($"{r:A}");
            //}

            foreach (var r in brizilChampions)
            {
                Console.WriteLine($"{r:A}");
            }
        }

        public static void FilteringWithIndex()
        {
            //Where的方法重载可以传第二个参数，index索引
            var racers = Formula1.GetChampions().Where((r, index) => r.LastName.StartsWith("A") && index % 2 == 0);
        }

        public static void TypeFiltering()
        {
            object[] data = { "one", 2, 3, "four", "five", 6 };
            //data.OfType<string>()是根据类型筛选集合的内容
            //OfType<string>表示只要string类型
            var query = data.OfType<string>();
            foreach (var item in query)
            {
                Console.WriteLine(item);
            }
        }

        public static void CompoundFromWithMethod()
        {
            var ferrariDrivers = from r in Formula1.GetChampions()
                                 from c in r.Cars  //Cars 是Formula1.GetChampions()的一个子集合
                                 where c == "Ferrari"
                                 orderby r.LastName
                                 select r.FirstName + " " + r.LastName;
            //等同于上面的复杂Linq查询
            //SelectMany就是把集合中的集合展开,即查询子集合并且和父集合一起展开
            ferrariDrivers = Formula1.GetChampions()
                .SelectMany(r => r.Cars, (r, c) => new { Racer = r, Car = c })
                .Where(r=>r.Car =="Ferrari")
                .OrderBy(r => r.Racer.LastName)
                .Select(r => r.Racer.FirstName + " " + r.Racer.LastName);
            foreach (var item in ferrariDrivers)
            {
                Console.WriteLine(item);
            }
        }

        //分组
        public static void Grouping()
        {

            //g.group r by r.Country和.GroupBy(r => r.Country)的Country关键字
            var countries = from r in Formula1.GetChampions()
                            group r by r.Country into g
                            let count = g.Count()   //可以在查询中使用let创建变量以便在以后的查询中使用
                            orderby count descending, g.Key
                            where count >= 2
                            select new
                            {
                                Country = g.Key,
                                Count = count
                            };

            countries = Formula1.GetChampions()
                        .GroupBy(r => r.Country)
                        .Select(g => new { Group = g, Count = g.Count() }) //这里可以用Select创建一个临时匿名类型
                        .OrderByDescending(g => g.Count) //这里就可以使用上面临时匿名类型的属性而不是方法了
                        .ThenBy(g => g.Group.Key)   //但是这里要调用Key的话就需要调用Group属性的Key
                        .Where(g => g.Count >= 2)
                        .Select(g => new
                        {
                            Country = g.Group.Key,
                            Count = g.Count
                        });


            foreach (var item in countries)
            {
                Console.WriteLine($"Country: {item.Country, -10} Count: {item.Count}");
            }
        }

        //对嵌套的对象分组，即分组后子查询生成复杂类型
        public static void GroupingAndNestedObject()
        {
            //var countries = from r in Formula1.GetChampions()
            //                group r by r.Country into g
            //                let count = g.Count()
            //                orderby count descending, g.Key
            //                select new
            //                {
            //                    Country = g.Key,
            //                    Count = count,
            //                    Racers = from r1 in g //从g中获得所有车手
            //                            orderby r1.LastName
            //                            select $"{r1.FirstName,-10}{r1.LastName}"
            //                };


            var countries = Formula1.GetChampions()
                .GroupBy(r => r.Country)
                .Select(g => new
                {
                    Group = g,
                    Key = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ThenBy(g => g.Key)
                .Where(g => g.Count >= 2)
                .Select(g => new
                {
                    Country = g.Key,
                    Count = g.Count,
                    Racers = g.Group.OrderBy(r => r.LastName)       //通过Group by的g访问组内项从而生成子查询
                    .Select(r => $"{r.FirstName,-10}{r.LastName}")
                });
                                
            foreach (var item in countries)
            {
                Console.WriteLine($"{item.Country, -10} {item.Count}");
                Console.WriteLine("------Racers------");
                foreach (var name in item.Racers)
                {
                    Console.WriteLine($"{name}");
                }
                Console.WriteLine("------End------");
                Console.WriteLine("");
            }
        }

        //内连接
        public static void InnerJoin()
        {
            var racers = from r in Formula1.GetChampions()
                         from y in r.Years
                         select new
                         {
                             Year = y,
                             Name = r.FirstName + " " + r.LastName
                         };
            var teams = from t in Formula1.GetConstructorChampions()
                        from y in t.Years
                        select new
                        {
                            Year = y,
                            Name = t.Name
                        };
            var racersAndTeams = (from r in racers
                                  join t in teams on r.Year equals t.Year
                                  select new
                                  {
                                      r.Year,
                                      Champion = r.Name,
                                      Constructor = t.Name
                                  }).Take(10);

            racers = Formula1.GetChampions()
            .SelectMany(r => r.Years, (r1, year) =>
            new //这里的r1就是Formula1.GetChampions()的项，year就是r.Years的每一项
            {
                Year = year,
                Name = r1.FirstName + " " + r1.LastName
            });
            teams = Formula1.GetConstructorChampions()
            .SelectMany(t => t.Years, (t, year) =>
            new
            {
                Year = year,
                t.Name
            });
            racersAndTeams = racers.Join(
                teams,          //第一个参数传车队
                r => r.Year,    //外部选择器(racers的选择器)
                t => t.Year,    //内部选择器(teams的选择器)
                (r, t) =>       //定义结果选择器
                new
                {
                    Year = r.Year,
                    Champion = r.Name,
                    Constructor = t.Name
                }).OrderBy(item => item.Year).Take(10);
                
            Console.WriteLine("Year  World Champion\t   Constructor Title");
            foreach (var item in racersAndTeams)
            {
                Console.WriteLine($"{item.Year}: {item.Champion,-20} {item.Constructor}");
            }


        }
    }

    


}
