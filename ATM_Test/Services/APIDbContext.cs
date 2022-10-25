using ATM_Test.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace ATM_Test.Services
{
    public class APIDbContext : DbContext
    {
        public DbSet<BankNote> BankNotes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var apiDbContextConnectionString = configuration.GetConnectionString("ApiDbContext");
            optionsBuilder.UseSqlite(apiDbContextConnectionString);
        }

        private static BankNote[] GetDefaultBankNotes()
        {
            List<BankNote> bankNotes = new();

            var denomations = Denomation.GetAll<Denomation>();

            foreach (var denom in denomations)
            {
                BankNote bankNote = new BankNote()
                {
                    Value = denom.Unit,
                    Quantity = 0
                };
                bankNotes.Add(bankNote);
            }

            return bankNotes.ToArray();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankNote>().HasData(GetDefaultBankNotes());

            base.OnModelCreating(modelBuilder);
        }
    }
}
