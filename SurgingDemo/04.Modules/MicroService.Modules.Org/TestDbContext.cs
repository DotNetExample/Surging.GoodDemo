using MicroService.Data.Configuration;
using MicroService.Modules.Org.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroService.Modules.Org
{
    public class TestDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public TestDbContext()
        {

        }

        public TestDbContext(DbContextOptions<DbContext> dbContextOptions) : base(dbContextOptions)
        {

            //dbContextOptions.
        }
        public override void Dispose()
        {
            base.Dispose();
        }
        protected string GetConnetciton()
        {
            var connectionString = ConfigManager.GetValue<string>("SqlConfig:connectionString");
            return connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseMySQL(GetConnetciton());

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");

        }
    }
}
