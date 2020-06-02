using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookSample
{
    public class QuerySample
    {
        public static async Task QueryAllBooksAsync()
        {
            Console.WriteLine(nameof(QueryAllBooksAsync));
            using (var context = new BooksContext())
            {
                List<Book> books = await context.Books.ToListAsync();
                foreach (var b in books)
                {
                    Console.WriteLine(b);
                }
            }
            Console.WriteLine();
        }

        public static async Task RawSqlQuery(string publisher)
        {
            Console.WriteLine(nameof(RawSqlQuery));
            using (var context = new BooksContext())
            {
                //FromSqlRaw执行原生sql语句
                IList<Book> books = await context.Books.FromSqlRaw($"SELECT * FROM BookSample.Books WHERE Publisher = '{publisher}'").ToListAsync();

                foreach (var b in books)
                {
                    Console.WriteLine($"{b.Title} {b.Publisher}");
                }
            }
            Console.WriteLine();
        }

        //已编译查询
        public static async Task CompileQueryAsync()
        {
            Console.WriteLine(nameof(CompileQueryAsync));
            //定义已编译查询
            Func<BooksContext, string, IAsyncEnumerable<Book>> query =
                EF.CompileAsyncQuery<BooksContext, string, Book>((context, publisher) => context.Books.Where(b => b.Publisher == publisher));

            using (var context = new BooksContext())
            {
                //使用IAsyncEnumerable接口类型，直接使用await foreach(var item in collection)
                IAsyncEnumerable<Book> books = query(context, "Wrox Press");
                await foreach (var b in books)
                {
                    Console.WriteLine($"{b.Title} {b.Publisher}");
                };
            }

            Console.WriteLine();
        }

        //使用EF.Function
        public static async Task UseEFFunctions(string titleSegment)
        {
            Console.WriteLine(nameof(UseEFFunctions));
            using (var context = new BooksContext())
            {
                string likeExpression = $"%{titleSegment}%";
                //EF.Functions.Like(b.Title, likeExpression) 就是DB的like关键字
                //相当于 title like '%titleSegment%'
                IList<Book> books = await context.Books.Where(b => EF.Functions.Like(b.Title, likeExpression)).ToListAsync();
                foreach (var b in books)
                {
                    Console.WriteLine($"{b.Title} {b.Publisher}");
                }
            }
            Console.WriteLine();
        }
    }
}
