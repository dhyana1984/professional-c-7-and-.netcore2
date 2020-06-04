using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RelationUsingConventions
{
    class Program
    {
        static void Main(string[] args)
        {
            //DeleteDatabase();
            //CreateDatabase();
            //AddBooks();
            //ExplicitLoading();
            EagerLoading();
        }

        private static void DeleteDatabase()
        {
            Console.Write("Delete the database? ");
            string input = Console.ReadLine();
            if (input.ToLower() == "y")
            {
                using (var context = new BooksContext())
                {
                    context.Database.EnsureDeleted();
                }
            }
        }

        private static void CreateDatabase()
        {
            using (var context = new BooksContext())
            {
                bool created = context.Database.EnsureCreated();
                Console.WriteLine($"database created: {created}");
            }
        }

        //显示加载，每次调用Load会访问一次数据库加载数据
        private static void ExplicitLoading()
        {
            Console.WriteLine(nameof(ExplicitLoading));
            using (var context = new BooksContext())
            {
                var book = context.Books.Where(b => b.Title.StartsWith("Professional")).FirstOrDefault();
                if(book != null)
                {
                    Console.WriteLine(book.Title);
                    //直接访问book的Author和Chapters是null，必须通过context.Entry(book)来显示加载
                    //Entry(book)的Collection加载一对多的关系
                    //Entry(book)的Reference加载一对一的关系
                    context.Entry(book).Collection(b => b.Chapters).Load(); //一对多
                    context.Entry(book).Reference(b => b.Author).Load();    //一对一
                    Console.WriteLine(book.Author.Name);
                    foreach (var chapter in book.Chapters)
                    {
                        Console.WriteLine($"{chapter.Number}. {chapter.Title}");
                    }
                }
            }
            Console.WriteLine();
        }

        //即使加载
        private static void EagerLoading()
        {
            Console.WriteLine(nameof(EagerLoading));
            using (var context = new BooksContext())
            {
                //通过使用Include，sql会使用join一次性加载数据
                var book = context.Books
                    .Include(b => b.Chapters)
                    .Include(b => b.Author)
                    .Where(b => b.Title.StartsWith("Professional"))
                    .FirstOrDefault();
                if(book != null)
                {
                    Console.WriteLine(book.Title);
                    foreach (var chapter in book.Chapters)
                    {
                        Console.WriteLine($"{chapter.Number}. {chapter.Title}");
                    }
                }
            }
        }

        private static void AddBooks()
        {
            Console.WriteLine(nameof(AddBooks));
            using (var context = new BooksContext())
            {
                var author = new User
                {
                    Name = "Christian Nagel"
                };
                var b1 = new Book
                {
                    Title = "Professional C# 7 and .NET Core 2.0",
                    Author = author
                };

                var c1 = new Chapter
                {
                    Title = ".NET Applications and Tools",
                    Number = 1,
                    Book = b1,
                };
                var c2 = new Chapter
                {
                    Title = "Core C#",
                    Number = 2,
                    Book = b1
                };
                var c3 = new Chapter
                {
                    Title = "Entity Framework Core",
                    Number = 28,
                    Book = b1
                };

                context.Books.Add(b1);

                context.Users.Add(author);
                context.Chapters.AddRange(c1, c2, c3);

                int records = context.SaveChanges();
                b1.Chapters.AddRange(new[] { c1, c2, c3 });

                Console.WriteLine($"{records} records added");
            }
            Console.WriteLine();
        }
    }
}
