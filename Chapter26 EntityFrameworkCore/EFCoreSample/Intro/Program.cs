using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intro
{
    class Program
    {
        static async Task Main(string[] args)
        {
        
            //await DeleteDatabaseAsync();
            //await CreateTheDatabaseAsync();
            //await AddBookAsync("TestTitle", "TestPublisher");
            //await AddBooksAsync();
            await ReadBooksAsync();
            //await UpdateBookAsync();
        }

        //创建数据库
        private static async Task CreateTheDatabaseAsync()
        {
            using (var context = new BooksContext())
            {
                bool created = await context.Database.EnsureCreatedAsync();
                string creationInfo = created ? "created" : "existed";
                Console.WriteLine($"database {creationInfo}");
            }
        }

        //删除数据库
        static async Task DeleteDatabaseAsync()
        {
            Console.WriteLine("Delete the Database");
            string input = Console.ReadLine();
            if (input.ToLower() == "y")
            {
                using (var context = new BooksContext())
                {
                    bool deleted = await context.Database.EnsureDeletedAsync();
                    string deleteInfo = deleted ? "deleted" : "not deleted";
                    Console.WriteLine($"database {deleteInfo}");
                }
            }
        }

        static async Task AddBookAsync(string title, string publisher)
        {
            using (var context = new BooksContext())
            {
                var book = new Book
                {
                    Title = title,
                    Publisher = publisher
                };
                //AddAsync方法仅仅把对象添加到上下文中，不写入数据库。
                await context.Books.AddAsync(book);
                //调用SaveChangesAsync把Book写入数据库
                int records = await context.SaveChangesAsync();
                Console.WriteLine($"{records} record added");
            }
            Console.WriteLine();
        }

        private static async Task AddBooksAsync()
        {
            using (var context = new BooksContext())
            {
                var b1 = new Book { Title = "Professional C# 6 and .NET Core 1.0", Publisher = "Wrox Press" };
                var b2 = new Book { Title = "Professional C# 5 and .NET 4.5.1", Publisher = "Wrox Press" };
                var b3 = new Book { Title = "JavaScript for Kids", Publisher = "Wrox Press" };
                var b4 = new Book { Title = "Web Design with HTML and CSS", Publisher = "For Dummies" };
                //批量插入
                await context.Books.AddRangeAsync(b1, b2, b3, b4);
                int records = await context.SaveChangesAsync();

                Console.WriteLine($"{records} records added");
            }
            Console.WriteLine();
        }

        //读取
        private static async Task ReadBooksAsync()
        {
            using (var context = new BooksContext())
            {
                List<Book> books = await context.Books.ToListAsync();
                foreach (var b in books)
                {
                    Console.WriteLine($"{b.Title} {b.Publisher}");
                }
            }
            Console.WriteLine();
        }

        //更新
        private static async Task UpdateBookAsync()
        {
            using (var context = new BooksContext())
            {
                int records = 0;
                //先查出来
                Book book = await context.Books
                    .Where(b => b.Title == "TestTitle")
                    .FirstOrDefaultAsync();
                if (book != null)
                {
                    //再更新
                    book.Title = "Professional C# 7 and .NET Core 2.0";
                    records = await context.SaveChangesAsync();
                }

                Console.WriteLine($"{records} record updated");
            }
            Console.WriteLine();
        }

        //EFCore 2.0的添加Logging的方式
        //private void AddLogging()
        //{
        //    using (var context = new BooksContext())
        //    {
        //        IServiceProvider provider = context.GetInfrastructure<IServiceProvider>();
        //        ILoggerFactory loggerFactory = provider.GetService<ILoggerFactory>();
        //        loggerFactory.AddConsole(LogLevel.Information);
        //    }
        //}

    }
}
