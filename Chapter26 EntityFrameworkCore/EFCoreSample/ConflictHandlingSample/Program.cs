using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ConflictHandlingSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //DeleteDatabase();
            //CreateDatabase();
            AddBook();
            ConflictHandling();
        }

        private const string BookTitle = "sample book";

        private static void ConflictHandling()
        {
            // user 1
            var tuple1 = PrepareUpdate();
            tuple1.book.Title = "user 1 wins";

            // user 2
            var tuple2 = PrepareUpdate();
            tuple2.book.Title = "user 2 wins";

            Update(tuple1.context, tuple1.book, "user 1");
            //在user2更新的时候，timestamp已经被user1更新过了，所以user2更新失败
            Update(tuple2.context, tuple2.book, "user 2");

            tuple1.context.Dispose();
            tuple2.context.Dispose();

            CheckUpdate(tuple1.book.BookId);
        }

        private static void Update(BooksContext context, Book book, string user)
        {
            try
            {
                Console.WriteLine($"{user}: updating id {book.BookId}, timestamp {book.TimeStamp.StringOutput()}");
                ShowChanges(book.BookId, context.Entry(book));

                int records = context.SaveChanges();
                Console.WriteLine($"{user}: updated {book.TimeStamp.StringOutput()}");
                Console.WriteLine($"{user}: {records} record(s) updated while updating {book.Title}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"{user} update failed with {book.Title}");
                Console.WriteLine($"{user} error: {ex.Message}");
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is Book b)
                    {
                        Console.WriteLine($"{b.Title} {b.TimeStamp.StringOutput()}");
                        ShowChanges(book.BookId, context.Entry(book));
                    }
                }
            }
        }

        private static void ShowChanges(int id, EntityEntry entity)
        {
            //对于与上下文相关的对象，使用PropertyEntry对象可以访问原始值和当前值
            //从数据库获取的原始值用OriginalValue属性访问，当前值可以用CurrentValue访问
            void ShowChange(PropertyEntry propertyEntry) =>
                Console.WriteLine($"id: {id}, current: {propertyEntry.CurrentValue}, original: {propertyEntry.OriginalValue}, modified: {propertyEntry.IsModified}");

            ShowChange(entity.Property("Title"));
            ShowChange(entity.Property("Publisher"));
        }

        private static (BooksContext context, Book book) PrepareUpdate()
        {
            var context = new BooksContext();
            Book book = context.Books.Where(b => b.Title == BookTitle).FirstOrDefault();
            return (context, book);
        }

        private static void CheckUpdate(int id)
        {
            using (var context = new BooksContext())
            {
                Book book = context.Books.Find(id);
                Console.WriteLine($"this is the updated state: {book.Title}");
            }
        }

        private static void AddBook()
        {
            using (var context = new BooksContext())
            {
                var b = new Book { Title = BookTitle, Publisher = "Sample" };

                context.Add(b);
                int records = context.SaveChanges();

                Console.WriteLine($"{records} record(s) added");
            }
            Console.WriteLine();
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
                bool created =  context.Database.EnsureCreated();
                string creationInfo = created ? "created" : "existed";
                Console.WriteLine($"database {creationInfo}");
            }
        }
    }
}
