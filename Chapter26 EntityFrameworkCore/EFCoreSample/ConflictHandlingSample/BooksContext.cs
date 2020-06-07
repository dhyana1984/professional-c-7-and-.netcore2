using System;
using Microsoft.EntityFrameworkCore;

namespace ConflictHandlingSample
{
    public class BooksContext : DbContext
    {
        const string ConnectionString =
              @"server=192.168.1.7\SQLEXPRESS; User Id=sa;Password=xiongyi1984; database=Books;";
        public DbSet<Book> Books { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var book = modelBuilder.Entity<Book>();
            book.HasKey(p => p.BookId);
            book.Property(p => p.Title).HasMaxLength(120).IsRequired();
            book.Property(p => p.Publisher).HasMaxLength(50);
            book.Property(p => p.TimeStamp)
                .HasColumnType("timestamp")     //设定在数据库中的类型是timestamp
                .ValueGeneratedOnAddOrUpdate()  //每一个insert和update改变这个属性
                .IsConcurrencyToken();          //必要属性，检查它在读取操作完以后是不是没有变化
        }
    }
}
