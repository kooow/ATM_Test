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

        private static DepositModel[] GetDefaultDepositModels()
        {
            List<DepositModel> depositModels = new();

            var denomations = Denomation.GetAll<Denomation>();

            foreach (var denom in denomations)
            {
                DepositModel depositModel = new DepositModel()
                {
                    Unit = denom.Unit,
                    Quantity = 0
                };
                depositModels.Add(depositModel);
            }

            return depositModels.ToArray();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DepositModel>().HasData(GetDefaultDepositModels());
            base.OnModelCreating(modelBuilder);
        }
    }
}
