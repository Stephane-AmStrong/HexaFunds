using Domain.Entities;

using Microsoft.EntityFrameworkCore;

using Persistence.Repository;

namespace Persistence.Test;

public class BankAccountRepositoryTests : IDisposable
{
    private readonly DbContextOptions<BankingDbContext> _dbContextOptions;
    private readonly BankAccountRepository _bankAccountRepository;
    private readonly BankingDbContext _context;
    private readonly BankAccount _checkingAccount;
    private readonly BankAccount _savingsAccount;

    public BankAccountRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<BankingDbContext>()
            .UseInMemoryDatabase(databaseName: $"BankingDb_{Guid.NewGuid()}")
            .Options;
        _context = new BankingDbContext(_dbContextOptions);

        _bankAccountRepository = new BankAccountRepository(_context);

        _checkingAccount = new BankAccount(Guid.NewGuid(), "6789065432", 2000, new CheckingBehavior(5000));
        _checkingAccount = new BankAccount(Guid.NewGuid(), "6789065432", 2000, new SavingsBehavior(5000));
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task CreateAsync_ShouldAddBankAccount()
    {
        await _bankAccountRepository.CreateAsync(_checkingAccount, CancellationToken.None);
        await _context.SaveChangesAsync();

        var accountInDb = await _context.BankAccounts.FindAsync(_checkingAccount.Id);

        Assert.NotNull(accountInDb);
        Assert.Equal(_checkingAccount.AccountNumber, accountInDb.AccountNumber);
        Assert.Equal(1000, accountInDb.Balance);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveBankAccount()
    {
        _context.BankAccounts.Add(_checkingAccount);
        await _context.SaveChangesAsync();

        _bankAccountRepository.Delete(_checkingAccount);
        await _context.SaveChangesAsync();

        var accountInDb = await _context.BankAccounts.FindAsync(_checkingAccount.Id);
        Assert.Null(accountInDb);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBankAccounts()
    {
        var accounts = new List<BankAccount> { _checkingAccount, _savingsAccount };

        _context.BankAccounts.AddRange(accounts);
        await _context.SaveChangesAsync();

        var result = _bankAccountRepository.GetAll();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBankAccount()
    {
        _context.BankAccounts.Add(_checkingAccount);
        await _context.SaveChangesAsync();

        var result = await _bankAccountRepository.GetByIdAsync(_checkingAccount.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(_checkingAccount.AccountNumber, result.AccountNumber);
        Assert.Equal(1000, result.Balance);
    }

    [Fact]
    public async Task Update_ShouldUpdateBankAccount()
    {
        _context.BankAccounts.Add(_checkingAccount);
        await _context.SaveChangesAsync();

        _checkingAccount.Balance = 2000;
        _bankAccountRepository.Update(_checkingAccount);
        await _context.SaveChangesAsync();

        var accountInDb = await _context.BankAccounts.FindAsync(_checkingAccount.Id);
        Assert.NotNull(accountInDb);
        Assert.Equal(2000, accountInDb.Balance);
    }

    [Fact]
    public void ApplyTransaction_ShouldUseStrategy()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = 100,
            Type = Domain.Enumerations.TransactionType.Debit,
            Date = DateTime.UtcNow,
            AccountId = _checkingAccount.Id,
            BankAccount = _checkingAccount
        };

        // Act
        _checkingAccount.ApplyTransaction(transaction);

        // Assert
        Assert.Equal(900, _checkingAccount.Balance);
    }
}