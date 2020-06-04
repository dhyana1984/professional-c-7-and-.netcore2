using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using static BookSample.ColumnNames;

namespace BookSample
{

    internal class ColumnNames
    {
        public const string LastUpdated = nameof(LastUpdated);
        public const string IsDeleted = nameof(IsDeleted);
        public const string BookId = nameof(BookId);
        public const string AuthorId = nameof(AuthorId);
    }

    public class BooksContext : DbContext
    {
        const string ConnectionString =
              @"server=192.168.1.7\SQLEXPRESS; User Id=sa;Password=xiongyi1984; database=Books;";

        //EFCore 3.0+ 添加Logging的方式
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .UseSqlServer(ConnectionString);
               

        }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("BookSample");

            modelBuilder.Entity<Book>().Property(b => b.Title).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Book>().Property(b => b.Publisher).IsRequired(false).HasMaxLength(30);
            //使用HasField方法,将数据库的BookId映射到类的字段_bookId
            //EFCore 3.1中Property必须和filed一样，但是在数据库中字段名可以用HasColumnName来定义
            modelBuilder.Entity<Book>().Property<int>("_bookId").HasColumnName(BookId).HasField("_bookId").IsRequired();
            modelBuilder.Entity<Book>().HasKey("_bookId");

            //定义shadow properties
            modelBuilder.Entity<Book>().Property<bool>(IsDeleted);
            modelBuilder.Entity<Book>().Property<DateTime>(LastUpdated);
            //定义global filter，检查IsDeleted，只返回isdelete = false的值，此时所有查询都会带上where IsDeleted!=0
            modelBuilder.Entity<Book>().HasQueryFilter(b => !EF.Property<bool>(b, IsDeleted));
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();
            //如果是新增，删除和修改状态下更新LastUpdated字段为当前时间
            foreach (var item in ChangeTracker.Entries<Book>().Where(t=>t.State == EntityState.Added ||
                                                                        t.State == EntityState.Modified ||
                                                                        t.State == EntityState.Deleted))
            {
                //读取shadow property，使用EntityEntry的CurrentValues索引器
                item.CurrentValues[LastUpdated] = DateTime.Now;
                if(item.State == EntityState.Deleted)
                {
                    //如果是删除状态，把删除改变为修改
                    item.State = EntityState.Modified;
                    //把IsDeleted置为true
                    item.CurrentValues[IsDeleted] = true;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }

}
