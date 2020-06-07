using System;
using Microsoft.EntityFrameworkCore;

namespace MenuSample
{
    public class MenusContext : DbContext
    {
        const string ConnectionString =
              @"server=192.168.1.7\SQLEXPRESS; User Id=sa;Password=xiongyi1984; database=Books;";

        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuCard> MenuCards { get; set; }

        public MenusContext(DbContextOptions<MenusContext> options): base(options)
        {

        }

        public MenusContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseSqlServer(ConnectionString, b=>b.MigrationsAssembly("EFCoreLib"));
                //, options => options.MaxBatchSize(1)); //将MaxBatchSize设置成1来禁用批处理
                //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);//全局定义跟踪行为
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("mc");

            modelBuilder.ApplyConfiguration(new MenuCardConfiguration());
            modelBuilder.ApplyConfiguration(new MenuConfiguration());
        }
    }
}
