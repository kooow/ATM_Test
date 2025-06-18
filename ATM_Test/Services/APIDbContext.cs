using ATM_Test.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace ATM_Test.Services;

/// <summary>
/// Database context for the ATM API, managing bank notes.
/// </summary>
public class APIDbContext : DbContext
{
    private const string ConnectionStringKey = "ApiDbContext";

    /// <summary>
    /// Banknotes entities in the database.
    /// </summary>
    public DbSet<BankNote> BankNotes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();

        var apiDbContextConnectionString = configuration.GetConnectionString(ConnectionStringKey);
        optionsBuilder.UseSqlite(apiDbContextConnectionString);
    }

    private static BankNote[] GetDefaultBankNotes()
    {
        var bankNotes = Denomation.GetAll<Denomation>().Select(d => new BankNote() { Value = d.Unit, Quantity = 0 }).ToArray();
        return bankNotes;
    }

    /// <summary>
    /// Model configuration for the database context.
    /// </summary>
    /// <param name="modelBuilder">Model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankNote>().HasData(GetDefaultBankNotes());
        base.OnModelCreating(modelBuilder);
    }
}
