using Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Persistence;

public sealed class BankingDbContext(DbContextOptions<BankingDbContext> options) : DbContext(options)
{
    public DbSet<CheckingAccount> CheckingAccounts { get; set; }
    public DbSet<SavingsAccount> SavingsAccounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}
