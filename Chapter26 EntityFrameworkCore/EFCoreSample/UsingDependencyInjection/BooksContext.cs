using System;
using Microsoft.EntityFrameworkCore;

namespace UsingDependencyInjection
{
    public class BooksContext : DbContext
    {
        //通过构造函数注入连接和SQlServer选项
        public BooksContext(DbContextOptions<BooksContext> options) : base(options) { }
     
        public DbSet<Book> Books { get; set; }

    }
}
