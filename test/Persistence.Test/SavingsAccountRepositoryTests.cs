using Domain.Entities;

using Microsoft.EntityFrameworkCore;

using Persistence.Repository;

namespace Persistence.Test;

public class SavingsAccountRepositoryTests : IDisposable
{
    private readonly DbContextOptions<BankingDbContext> _dbContextOptions;
    private readonly SavingsAccountRepository _savingsAccountRepository;
    private readonly BankingDbContext _context;
    private readonly SavingsAccount _savingsAccount1;
    private readonly SavingsAccount _savingsAccount2;

    public SavingsAccountRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<BankingDbContext>()
            .UseInMemoryDatabase(databaseName: $"BankingDb_{Guid.NewGuid()}")
            .Options;
        _context = new BankingDbContext(_dbContextOptions);

        _savingsAccountRepository = new SavingsAccountRepository(_context);

        _savingsAccount1 = new SavingsAccount { Id = Guid.NewGuid(), AccountNumber = "FR_87654321", Balance = 1000 };
        _savingsAccount2 = new SavingsAccount { Id = Guid.NewGuid(), AccountNumber = "EN_39065432", Balance = 2000 };
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task CreateAsync_ShouldAddSavingsAccount()
    {
        await _savingsAccountRepository.CreateAsync(_savingsAccount1, CancellationToken.None);
        await _context.SaveChangesAsync();

        var accountInDb = await _context.SavingsAccounts.FindAsync(_savingsAccount1.Id);

        Assert.NotNull(accountInDb);
        Assert.Equal(_savingsAccount1.AccountNumber, accountInDb.AccountNumber);
        Assert.Equal(1000, accountInDb.Balance);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveSavingsAccount()
    {
        _context.SavingsAccounts.Add(_savingsAccount1);
        await _context.SaveChangesAsync();

        _savingsAccountRepository.Delete(_savingsAccount1);
        await _context.SaveChangesAsync();

        var accountInDb = await _context.SavingsAccounts.FindAsync(_savingsAccount1.Id);
        Assert.Null(accountInDb);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllSavingsAccounts()
    {
        var _savingsAccounts = new List<SavingsAccount>
        {
            _savingsAccount1,
            _savingsAccount2,
        };

        _context.SavingsAccounts.AddRange(_savingsAccounts);
        await _context.SaveChangesAsync();

        var result = _savingsAccountRepository.GetAll();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSavingsAccount()
    {
        _context.SavingsAccounts.Add(_savingsAccount1);
        await _context.SaveChangesAsync();

        var result = await _savingsAccountRepository.GetByIdAsync(_savingsAccount1.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(_savingsAccount1.AccountNumber, result.AccountNumber);
        Assert.Equal(1000, result.Balance);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateSavingsAccount()
    {
        _context.SavingsAccounts.Add(_savingsAccount1);
        await _context.SaveChangesAsync();

        _savingsAccount1.Balance = 2000;
        _savingsAccountRepository.Update(_savingsAccount1);
        await _context.SaveChangesAsync();

        var accountInDb = await _context.SavingsAccounts.FindAsync(_savingsAccount1.Id);
        Assert.NotNull(accountInDb);
        Assert.Equal(2000, accountInDb.Balance);
    }
}