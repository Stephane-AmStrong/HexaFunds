using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;

namespace Persistence;

public sealed class BankingDbContext(DbContextOptions<BankingDbContext> options) : DbContext(options)
{
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var accountBuilder = modelBuilder.Entity<BankAccount>();

        AccountBehaviorConfiguration.Configure(accountBuilder);
    }
}
