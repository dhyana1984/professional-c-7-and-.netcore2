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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(ConnectionString);
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
