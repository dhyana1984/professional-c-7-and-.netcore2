        using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RelationUsingFluentAPI
{
    public class BooksContext : DbContext
    {
        const string ConnectionString =
      @"server=192.168.1.7\SQLEXPRESS; User Id=sa;Password=xiongyi1984; database=Books;";
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .UseSqlServer(ConnectionString);
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Book和Chapter是一对多，所以是HasMany和WithOne
            modelBuilder.Entity<Book>().HasMany(b => b.Chapters).WithOne(c => c.Book);
            //Book和Author是多对一，所以是HasOne和WithMany
            modelBuilder.Entity<Book>().HasOne(b => b.Author).WithMany(a => a.WrittenBooks);
            modelBuilder.Entity<Book>().HasOne(b => b.Reviewer).WithMany(r => r.ReviewedBooks);
            modelBuilder.Entity<Book>().HasOne(b => b.Editor).WithMany(e => e.EditedBooks);

            //Chapter中有一个外键。所以使用HasForeignKey(c => c.BookId)
            modelBuilder.Entity<Chapter>().HasOne(c => c.Book).WithMany(b => b.Chapters).HasForeignKey(c => c.BookId);

            modelBuilder.Entity<User>().HasMany(a => a.WrittenBooks).WithOne(b => b.Author);
            modelBuilder.Entity<User>().HasMany(r => r.ReviewedBooks).WithOne(b => b.Reviewer);
            modelBuilder.Entity<User>().HasMany(e => e.EditedBooks).WithOne(b => b.Editor);
        }
    }
}
