using BankAccount.Core.Enumerations;

using Domain.Entities;

using Microsoft.EntityFrameworkCore;

using Persistence.Repository;

namespace Persistence.Test;

public class TransactionRepositoryTests : IDisposable
{
    private readonly DbContextOptions<BankingDbContext> _dbContextOptions;
    private readonly TransactionRepository _transactionRepository;
    private readonly BankingDbContext _context;
    private readonly Transaction _creditTransaction;
    private readonly Transaction _debitTransaction;
    private readonly Domain.Entities.BankAccount _bankAccount;

    public TransactionRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<BankingDbContext>()
            .UseInMemoryDatabase(databaseName: $"BankingDb_{Guid.NewGuid()}")
            .Options;
        _context = new BankingDbContext(_dbContextOptions);

        _transactionRepository = new TransactionRepository(_context);

        _bankAccount = new CheckingAccount
        {
            Id = Guid.NewGuid(),
            AccountNumber = "8487654321",
            Balance = 700,
        };

        _creditTransaction = new Transaction { Id = Guid.NewGuid(), Amount = 100, Date = new DateTime(2024, 6, 9), Type = TransactionType.Credit, AccountId = _bankAccount.Id, BankAccount = _bankAccount };
        _debitTransaction = new Transaction { Id = Guid.NewGuid(), Amount = 200, Date = new DateTime(2024, 6, 18), Type = TransactionType.Debit, AccountId = _bankAccount.Id, BankAccount = _bankAccount };
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task CreateAsync_ShouldAddTransaction()
    {
        await _transactionRepository.CreateAsync(_creditTransaction, CancellationToken.None);
        await _context.SaveChangesAsync();

        var transactionInDb = await _context.Transactions.FindAsync(_creditTransaction.Id);
        Assert.NotNull(transactionInDb);
        Assert.Equal(100, transactionInDb.Amount);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveTransaction()
    {
        _context.Transactions.Add(_creditTransaction);
        await _context.SaveChangesAsync();

        _transactionRepository.Delete(_creditTransaction);
        await _context.SaveChangesAsync();

        var accountInDb = await _context.Transactions.FindAsync(_creditTransaction.Id);
        Assert.Null(accountInDb);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTransactions()
    {
        var transactions = new List<Transaction>
        {
            _creditTransaction,
            _debitTransaction
        };

        _context.Transactions.AddRange(transactions);
        await _context.SaveChangesAsync();

        var result = _transactionRepository.GetAll();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTransaction()
    {

        _context.Transactions.Add(_creditTransaction);
        await _context.SaveChangesAsync();

        var result = await _transactionRepository.GetByIdAsync(_creditTransaction.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(100, result.Amount);
    }

    [Fact]
    public void GetByCondition_ShouldReturnFilteredTransactions()
    {
        var transactions = new List<Transaction>
        {
            _creditTransaction,
            _debitTransaction
        };

        _context.Transactions.AddRange(transactions);
        _context.SaveChanges();

        var result = _transactionRepository.GetByCondition(t => t.AccountId == _bankAccount.Id).ToList();

        Assert.Equal(transactions.Count, result.Count);
        Assert.Equal(transactions.First().AccountId, result.First().AccountId);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTransaction()
    {

        _context.Transactions.Add(_creditTransaction);
        await _context.SaveChangesAsync();

        //_creditTransaction.Date = DateTime.UtcNow;
        _transactionRepository.Update(_creditTransaction);
        await _context.SaveChangesAsync();

        var transactionInDb = await _context.Transactions.FindAsync(_creditTransaction.Id);
        Assert.NotNull(transactionInDb);
        Assert.Equal(_creditTransaction.Amount, transactionInDb.Amount);
    }
}