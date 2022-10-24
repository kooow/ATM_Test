using ATM_Test.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace ATM_Test.Services
{
    public class APIDbContext : DbContext
    {
        public DbSet<DepositModel> DepositModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var apiDbContextConnectionString = configuration.GetConnectionString("ApiDbContext");
            optionsBuilder.UseSqlite(apiDbContextConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DepositModel>();

            base.OnModelCreating(modelBuilder);
        }

    }
}
