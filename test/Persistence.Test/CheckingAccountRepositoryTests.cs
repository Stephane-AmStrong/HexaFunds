using Domain.Entities;

using Microsoft.EntityFrameworkCore;

using Persistence.Repository;

namespace Persistence.Test;

public class CheckingAccountRepositoryTests : IDisposable
{
    private readonly DbContextOptions<BankingDbContext> _dbContextOptions;
    private readonly CheckingAccountRepository _checkingAccountRepository;
    private readonly BankingDbContext _context;
    private readonly CheckingAccount _checkingAccount1;
    private readonly CheckingAccount _checkingAccount2;

    public CheckingAccountRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<BankingDbContext>()
            .UseInMemoryDatabase(databaseName: $"BankingDb_{Guid.NewGuid()}")
            .Options;
        _context = new BankingDbContext(_dbContextOptions);

        _checkingAccountRepository = new CheckingAccountRepository(_context);

        _checkingAccount1 = new CheckingAccount { Id = Guid.NewGuid(), AccountNumber = "8487654321", Balance = 1000 };
        _checkingAccount2 = new CheckingAccount { Id = Guid.NewGuid(), AccountNumber = "6789065432", Balance = 2000 };
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task CreateAsync_ShouldAddCheckingAccount()
    {
        await _checkingAccountRepository.CreateAsync(_checkingAccount1, CancellationToken.None);
        await _context.SaveChangesAsync();

        var accountInDb = await _context.CheckingAccounts.FindAsync(_checkingAccount1.Id);

        Assert.NotNull(accountInDb);
        Assert.Equal(_checkingAccount1.AccountNumber, accountInDb.AccountNumber);
        Assert.Equal(1000, accountInDb.Balance);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveCheckingAccount()
    {
        _context.CheckingAccounts.Add(_checkingAccount1);
        await _context.SaveChangesAsync();

        _checkingAccountRepository.Delete(_checkingAccount1);
        await _context.SaveChangesAsync();

        var accountInDb = await _context.CheckingAccounts.FindAsync(_checkingAccount1.Id);
        Assert.Null(accountInDb);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCheckingAccounts()
    {
        var _checkingAccounts = new List<CheckingAccount>
        {
            _checkingAccount1,
            _checkingAccount2,
        };

        _context.CheckingAccounts.AddRange(_checkingAccounts);
        await _context.SaveChangesAsync();

        var result = _checkingAccountRepository.GetAll();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCheckingAccount()
    {
        _context.CheckingAccounts.Add(_checkingAccount1);
        await _context.SaveChangesAsync();

        var result = await _checkingAccountRepository.GetByIdAsync(_checkingAccount1.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(_checkingAccount1.AccountNumber, result.AccountNumber);
        Assert.Equal(1000, result.Balance);
    }

    [Fact]
    public async Task Update_ShouldUpdateCheckingAccount()
    {
        _context.CheckingAccounts.Add(_checkingAccount1);
        await _context.SaveChangesAsync();

        _checkingAccount1.Balance = 2000;
        _checkingAccountRepository.Update(_checkingAccount1);
        await _context.SaveChangesAsync();

        var accountInDb = await _context.CheckingAccounts.FindAsync(_checkingAccount1.Id);
        Assert.NotNull(accountInDb);
        Assert.Equal(2000, accountInDb.Balance);
    }
}