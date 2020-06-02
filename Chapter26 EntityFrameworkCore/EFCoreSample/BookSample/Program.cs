using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var context = new BooksContext())
            {
                //await DeleteDatabaseAsync();
                //await CreateTheDatabaseAsync();
                //await AddBooksAsync();
                //await DeleteBookAsync(2);
                //await QueryDeletedBooksAsync();

                //await QuerySample.QueryAllBooksAsync();
                //await QuerySample.RawSqlQuery("Wrox Press");
                //await QuerySample.CompileQueryAsync();
                await QuerySample.UseEFFunctions("java");
            }
        }

        private static async Task DeleteDatabaseAsync()
        {
            Console.WriteLine(nameof(DeleteDatabaseAsync));
            Console.Write("Delete the database? ");
            string input = Console.ReadLine();
            if (input.ToLower() == "y")
            {
                using (var context = new BooksContext())
                {
                    bool deleted = await context.Database.EnsureDeletedAsync();
                    string deletionInfo = deleted ? "deleted" : "not deleted";
                    Console.WriteLine($"database {deletionInfo}");
                }
            }
            Console.WriteLine();
        }

        private static async Task CreateTheDatabaseAsync()
        {
            Console.WriteLine(nameof(CreateTheDatabaseAsync));
            using (var context = new BooksContext())
            {
                bool created = await context.Database.EnsureCreatedAsync();
                string creationInfo = created ? "created" : "exists";
                Console.WriteLine($"database {creationInfo}");
            }
            Console.WriteLine();
        }

        private static async Task AddBookAsync(string title, string publisher)
        {
            Console.WriteLine(nameof(AddBookAsync));
            using (var context = new BooksContext())
            {
                var book = new Book(title, publisher);
                await context.Books.AddAsync(book);
                int records = await context.SaveChangesAsync();

                Console.WriteLine($"{records} record added");
            }
            Console.WriteLine();
        }

        private static async Task AddBooksAsync()
        {
            Console.WriteLine(nameof(AddBooksAsync));
            using (var context = new BooksContext())
            {
                var b1 = new Book("Professional C# 6 and .NET Core 1.0", "Wrox Press");
                var b2 = new Book("Professional C# 5 and .NET 4.5.1", "Wrox Press");
                var b3 = new Book("JavaScript for Kids", "Wrox Press");
                var b4 = new Book("HTML and CSS", "John Wiley");
                await context.Books.AddRangeAsync(b1, b2, b3, b4);

                int records = await context.SaveChangesAsync();

                Console.WriteLine($"{records} records added");
            }
            Console.WriteLine();
        }

        private static async Task DeleteBookAsync(int id)
        {
            using (var context = new BooksContext())
            {
                Book b = await context.Books.FindAsync(id);
                if (b == null) return;

                context.Books.Remove(b);
                int records = await context.SaveChangesAsync();
                Console.WriteLine($"{records} books deleted");
            }
        }

        private static async Task QueryDeletedBooksAsync()
        {
            using (var context = new BooksContext())
            {
                //读取shadow property, 需要使用EF.Property<bool>(b, "IsDeleted")
                IEnumerable<Book> deletedBooks = await context.Books.Where(b => EF.Property<bool>(b, "IsDeleted")).ToListAsync();

                foreach (var item in deletedBooks)
                {
                    Console.WriteLine($"deleted: {item}");
                }
            }
        }
    }
}
