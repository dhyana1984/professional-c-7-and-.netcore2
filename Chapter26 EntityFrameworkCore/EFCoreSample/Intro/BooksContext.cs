using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Intro
{
    //通过创建BooksContext类，实现CoreBook表与数据库的关系
    public class BooksContext : DbContext
    {
        private const string ConnectionString = @"server=192.168.1.7\SQLEXPRESS; User Id=sa;Password=xiongyi1984; database=Books;";

        public DbSet<Book> Books { get; set; }


        //通过重写OnConfiguring方法将上下文映射到SQLServer数据库
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                //使用Logging
                optionsBuilder.UseLoggerFactory(MyLoggerFactory)
                    .UseSqlServer(ConnectionString);
            }
        }

        //EFCore 3.0+ 添加Logging的方式
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}
