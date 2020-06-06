using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MenuSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await DeleteDatabase();
            //await CreateDatabase();
            //AddRecords();
            //ObjectTracking();
            //UpdateRecords();
            //ChangeUntracked();
            AddHundredRecords();
        }

        private static async Task DeleteDatabase()
        {
            Console.Write("Delete the database? ");
            string input = Console.ReadLine();
            if (input.ToLower() == "y")
            {
                using (var context = new MenusContext())
                {
                    await context.Database.EnsureDeletedAsync();
                }
            }
        }


        private static async Task CreateDatabase()
        {
            using (var context = new MenusContext())
            {
                bool created = await context.Database.EnsureCreatedAsync();
                string creationInfo = created ? "created" : "existed";
                Console.WriteLine($"database {creationInfo}");
            }
        }

        public static void ShowState(MenusContext context)
        {
            //使用ChangeTracker可以访问所有上下文相关的对象的状态
            //ChangeTracker.Entries()返回变更跟踪器了解的所有对象,将所有对象包括状态写入console
            foreach (EntityEntry entry in context.ChangeTracker.Entries())
            {
                Console.WriteLine($"type: {entry.Entity.GetType().Name}, state: {entry.State}," +
                $" {entry.Entity}");
            }
        }

        private static void AddRecords()
        {
            Console.WriteLine(nameof(AddRecords));
            try
            {
                using (var context = new MenusContext())
                {

                    var soupCard = new MenuCard();
                    Menu[] soups =
                    {
                        new Menu
                        {
                            Text = "Consommé Célestine (with shredded pancake)",
                            Price = 4.8m,
                            MenuCard =soupCard
                        },
                        new Menu
                        {
                            Text = "Baked Potato Soup",
                            Price = 4.8m,
                            MenuCard = soupCard
                        },
                        new Menu
                        {
                            Text = "Cheddar Broccoli Soup",
                            Price = 4.8m,
                            MenuCard = soupCard
                        },
                    };

                    soupCard.Title = "Soups";
                    //soups会插入到Menus表
                    soupCard.Menus.AddRange(soups);
                    //soupCard插入到MenuCards表
                    context.MenuCards.Add(soupCard);

                    ShowState(context);

                    int records = context.SaveChanges();
                    Console.WriteLine($"{records} added");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine();
        }

        static void ObjectTracking()
        {
            using(var context = new MenusContext())
            {
                //可以修改context的ChangeTracker行为
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                //这里m1和m2是相同对象, 因为查询到第二个对象和第一个对象具有相同主键，只实例化了第一个对象
                var m1 = (from m in context.Menus where m.Text.StartsWith("Con") select m).FirstOrDefault();
                var m2 = (from m in context.Menus where m.Text.Contains("(") select m).FirstOrDefault();

                //如果使用AsNoTracking方法的话，可以不跟踪在数据库中运行查询的对象
                //var m1 = (from m in context.Menus.AsNoTracking() where m.Text.StartsWith("Con") select m).FirstOrDefault();

                
                if (ReferenceEquals(m1, m2))
                {
                    Console.WriteLine("the same object");
                }
                else
                {
                    Console.WriteLine("not the same");
                }
                ShowState(context);
            }
        }

        private static void UpdateRecords()
        {
            Console.WriteLine(nameof(UpdateRecords));
            using (var context = new MenusContext())
            {
                Menu menu = context.Menus
                  .Skip(1)
                  .FirstOrDefault();
                ShowState(context); //Unchanged
                menu.Price += 0.2m;
                ShowState(context); //Modified
                int records = context.SaveChanges();
                Console.WriteLine($"{records} updated");
                ShowState(context); //Unchanded
            }
            Console.WriteLine();
        }

        private static void ChangeUntracked()
        {
            Menu GetMenu()
            {
                using (var context = new MenusContext())
                {
                    var menu = context.Menus.Skip(2).FirstOrDefault();
                    return menu;
                }
            }

            var m = GetMenu();
            m.Price += 0.7m;
            UpdateUntracked(m);
        }

        static void UpdateUntracked(Menu m)
        {
            using (var context = new MenusContext())
            {
                ShowState(context);
                //显示的关联对象到上下文
                //EntityEntry<Menu> entry = context.Menus.Attach(m);
                //设置对象状态是Modified
                //entry.State = EntityState.Modified;

                //使用Update方法可以完成关联到上下文并设置对象状态为Modified
                context.Menus.Update(m);
                ShowState(context);
                context.SaveChanges();
            }
        }

        static void AddHundredRecords()
        {
            Console.WriteLine(nameof(AddHundredRecords));
            using (var context = new MenusContext())
            {
                var card = context.MenuCards.FirstOrDefault();
                if(card != null)
                {
                    var menus = Enumerable.Range(1, 100).Select(x => new Menu
                    {
                        MenuCard = card,
                        Text = $"$menu {x}",
                        Price = 9.9m
                    });
                    context.Menus.AddRange(menus);
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    int record = context.SaveChanges();
                    stopwatch.Stop();
                    Console.WriteLine($"{record} records added after ${stopwatch.ElapsedMilliseconds} ms");
                }
            }
        }
    }
}
