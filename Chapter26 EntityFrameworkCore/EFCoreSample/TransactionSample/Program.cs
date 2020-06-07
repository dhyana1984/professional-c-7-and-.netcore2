using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace TransactionSample
{
    class Program
    {
        static void Main(string[] args)
        {

            //Setup();
            //AddTwoRecordsWithOneTx();
            //AddTwoRecordsWithTwoTx();
            TwoSaveChangesWithOneTx();
        }

        public static void Setup()
        {
            using (var context = new MenusContext())
            {
                bool deleted = context.Database.EnsureDeleted();
                string deletedText = deleted ? "deleted" : "does not exist";
                Console.WriteLine($"database {deletedText}");

                bool created = context.Database.EnsureCreated();

                string createdText = created ? "created" : "already exists";
                Console.WriteLine($"database {createdText}");

                var card = new MenuCard() { Title = "Meat" };
                var m1 = new Menu { MenuCard = card, Text = "Wiener Schnitzel", Price = 12.90m };
                var m2 = new Menu { MenuCard = card, Text = "Goulash", Price = 8.80m };
                card.Menus.AddRange(new Menu[] { m1, m2 });
                context.MenuCards.Add(card);

                int records = context.SaveChanges();
                Console.WriteLine($"{records} records added");
            }
        }

        
        private static void AddTwoRecordsWithOneTx()
        {
            Console.WriteLine(nameof(AddTwoRecordsWithOneTx));
            try
            {
                using (var context = new MenusContext())
                {
                    var card = context.MenuCards.First();
                    var m1 = new Menu { MenuCardId = card.MenuCardId, Text = "added", Price = 99.99m };

                    int hightestCardId = context.MenuCards.Max(c => c.MenuCardId);
                    var mInvalid = new Menu { MenuCardId = ++hightestCardId, Text = "invalid", Price = 999.99m };
                    context.Menus.AddRange(m1, mInvalid);

                    Console.WriteLine("trying to add one invalid record to the database, this should fail...");
                    //同一个savechanges,所有操作只要报错会回滚
                    int records = context.SaveChanges();
                    Console.WriteLine($"{records} records added");
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"{ex.Message}");
                Console.WriteLine($"{ex?.InnerException.Message}");
            }
            Console.WriteLine();
        }

        private static void AddTwoRecordsWithTwoTx()
        {
            Console.WriteLine(nameof(AddTwoRecordsWithTwoTx));
            try
            {
                using (var context = new MenusContext())
                {
                    Console.WriteLine("adding two records with two transactions to the database. One record should be written, the other not....");
                    var card = context.MenuCards.First();
                    var m1 = new Menu { MenuCardId = card.MenuCardId, Text = "added", Price = 99.99m };

                    context.Menus.Add(m1);
                    //这里插入成功
                    int records = context.SaveChanges();
                    Console.WriteLine($"{records} records added");

                    int hightestCardId = context.MenuCards.Max(c => c.MenuCardId);
                    var mInvalid = new Menu { MenuCardId = ++hightestCardId, Text = "invalid", Price = 999.99m };
                    context.Menus.Add(mInvalid);
                    //插入失败
                    records = context.SaveChanges();
                    Console.WriteLine($"{records} records added");
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"{ex.Message}");
                Console.WriteLine($"{ex?.InnerException.Message}");
            }
            Console.WriteLine();
        }

        private static void TwoSaveChangesWithOneTx()
        {
            Console.WriteLine(nameof(TwoSaveChangesWithOneTx));
            //使用IDContextTransaction开启事务
            IDbContextTransaction tx = null;
            try
            {
                using (var context = new MenusContext())
                //context.Database.BeginTransaction()返回一个IDbContextTransaction的事务
                //BeginTransaction这里可以提供隔离级别
                using (tx = context.Database.BeginTransaction())
                {

                    Console.WriteLine("using one explicit transaction, writing should roll back...");
                    var card = context.MenuCards.First();
                    var m1 = new Menu { MenuCardId = card.MenuCardId, Text = "added with explicit tx", Price = 99.99m };

                    context.Menus.Add(m1);
                    int records = context.SaveChanges();
                    Console.WriteLine($"{records} records added");

                    int hightestCardId = context.MenuCards.Max(c => c.MenuCardId);
                    var mInvalid = new Menu { MenuCardId = ++hightestCardId, Text = "invalid", Price = 999.99m };
                    context.Menus.Add(mInvalid);

                    records = context.SaveChanges();
                    Console.WriteLine($"{records} records added");

                    //使用Commit()提交，即使多个SaveChanges()，如果有失败，也会不会提交
                    tx.Commit();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"{ex.Message}");
                Console.WriteLine($"{ex?.InnerException.Message}");

                Console.WriteLine("rolling back...");
                tx.Rollback();
            }
            Console.WriteLine();
        }
    }
}

