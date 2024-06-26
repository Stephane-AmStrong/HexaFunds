using System.Linq.Expressions;

using DataTransfertObjects;

using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories.Abstractions;

using Moq;

using Services;
namespace Service.Test;

public class TransactionServiceTest
{
    private readonly Mock<IRepositoryManager> _mockRepositoryManager;
    private readonly Mock<ITransactionRepository> _mockTransactionRepository;
    private readonly Mock<ICheckingAccountRepository> _mockCheckingAccountRepository;
    private readonly Mock<ISavingsAccountRepository> _mockSavingsAccountRepository;
    private readonly Mock<IBankAccountRepository> _mockBankAccountRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly TransactionService _transactionService;

    private readonly CancellationToken _cancellationToken;
    private readonly List<Domain.Entities.BankAccount> _accounts;
    private readonly List<Transaction> _transactions;

    public TransactionServiceTest()
    {
        _mockRepositoryManager = new Mock<IRepositoryManager>();
        _mockTransactionRepository = new Mock<ITransactionRepository>();
        _mockCheckingAccountRepository = new Mock<ICheckingAccountRepository>();
        _mockSavingsAccountRepository = new Mock<ISavingsAccountRepository>();
        _mockBankAccountRepository = new Mock<IBankAccountRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockRepositoryManager.Setup(r => r.TransactionRepository).Returns(_mockTransactionRepository.Object);
        _mockRepositoryManager.Setup(r => r.CheckingAccountRepository).Returns(_mockCheckingAccountRepository.Object);
        _mockRepositoryManager.Setup(r => r.SavingsAccountRepository).Returns(_mockSavingsAccountRepository.Object);
        _mockRepositoryManager.Setup(r => r.BankAccountRepository).Returns(_mockBankAccountRepository.Object);
        _mockRepositoryManager.Setup(r => r.UnitOfWork).Returns(_mockUnitOfWork.Object);

        _transactionService = new TransactionService(_mockRepositoryManager.Object);

        _accounts =
        [
            new CheckingAccount()
            {
                Id = Guid.NewGuid(),
                AccountNumber = "987654321",
                Balance = 500,
                OverdraftLimit = 200
            },
            new SavingsAccount()
            {
                Id = Guid.NewGuid(),
                AccountNumber = "8487654321",
                Balance = 700,
                BalanceCeiling = 1000
            }
        ];

        _transactions =
        [
            new()
            {
                Id = Guid.NewGuid(),
                Amount = 700,
                Date = new DateTime(2024, 6, 9),
                Type = BankAccount.Core.Enumerations.TransactionType.Credit,
                AccountId = _accounts[0].Id,
                BankAccount = _accounts[0],
            },
            new()
            {
                Id = Guid.NewGuid(),
                Amount = 500,
                Date = new DateTime(2024, 5, 18),
                Type = BankAccount.Core.Enumerations.TransactionType.Debit,
                AccountId = _accounts[0].Id,
                BankAccount = _accounts[0],
            },
            new()
            {
                Id = Guid.NewGuid(),
                Amount = 100,
                Date = new DateTime(2024, 6, 7),
                Type = BankAccount.Core.Enumerations.TransactionType.Credit,
                AccountId = _accounts[1].Id,
                BankAccount = _accounts[1],
            },
            new()
            {
                Id = Guid.NewGuid(),
                Amount = 300,
                Date = new DateTime(2024, 6, 15),
                Type = BankAccount.Core.Enumerations.TransactionType.Debit,
                AccountId = _accounts[1].Id,
                BankAccount = _accounts[1],
            },
            new()
            {
                Id = Guid.NewGuid(),
                Amount = 200,
                Date = new DateTime(2024, 6, 29),
                Type = BankAccount.Core.Enumerations.TransactionType.Debit,
                AccountId = _accounts[0].Id,
                BankAccount = _accounts[0],
            },
        ];
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTransactionForCheckingAccount_WithCredit()
    {
        // Arrange
        var request = new TransactionRequest
        {
            Amount = _transactions[0].Amount,
            Type = TransactionType.Credit,
            AccountId = _transactions[0].AccountId,
        };

        var transaction = _transactions[0];

        var initialBalance = transaction.BankAccount.Balance;


        _mockBankAccountRepository.Setup(r => r.GetByIdAsync(_accounts[0].Id, _cancellationToken))
                                  .ReturnsAsync(_accounts[0]);

        _mockTransactionRepository.Setup(r => r.CreateAsync(transaction, _cancellationToken))
                                      .Returns(Task.CompletedTask);

        _mockCheckingAccountRepository.Setup(r => r.UpdateAsync((CheckingAccount)_accounts[0], It.IsAny<CancellationToken>()))
                                  .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(_cancellationToken))
                       .ReturnsAsync(1);

        // Act
        var response = await _transactionService.CreateAsync(request);

        // Assert

        _mockBankAccountRepository.Verify(r => r.GetByIdAsync(request.AccountId, _cancellationToken), Times.Once);
        _mockTransactionRepository.Verify(r => r.CreateAsync(It.IsAny<Transaction>(), _cancellationToken), Times.Once);
        _mockCheckingAccountRepository.Verify(r => r.UpdateAsync(It.IsAny<CheckingAccount>(), _cancellationToken), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(_cancellationToken), Times.Once);

        Assert.NotNull(response);
        Assert.Equal(transaction.Amount, response.Amount);
        Assert.Equal(transaction.BankAccount.Balance, initialBalance + response.Amount);
        Assert.Equal(transaction.AccountId, response.AccountId);
        Assert.Equal(transaction.Type.ToString(), response.Type.ToString());
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTransactionForSavingsAccount_WithCredit()
    {
        // Arrange
        var request = new TransactionRequest
        {
            Amount = _transactions[2].Amount,
            Type = TransactionType.Credit,
            AccountId = _transactions[2].AccountId,
        };

        var transaction = _transactions[2];

        var initialBalance = transaction.BankAccount.Balance;


        _mockBankAccountRepository.Setup(r => r.GetByIdAsync(_accounts[1].Id, _cancellationToken))
                                  .ReturnsAsync(_accounts[1]);

        _mockTransactionRepository.Setup(r => r.CreateAsync(It.IsAny<Transaction>(), _cancellationToken))
                                      .Returns(Task.CompletedTask);

        _mockSavingsAccountRepository.Setup(r => r.UpdateAsync(It.IsAny<SavingsAccount>(), It.IsAny<CancellationToken>()))
                                  .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(_cancellationToken))
                       .ReturnsAsync(1);

        // Act
        var response = await _transactionService.CreateAsync(request);

        // Assert

        _mockBankAccountRepository.Verify(r => r.GetByIdAsync(request.AccountId, _cancellationToken), Times.Once);
        _mockTransactionRepository.Verify(r => r.CreateAsync(It.IsAny<Transaction>(), _cancellationToken), Times.Once);
        _mockSavingsAccountRepository.Verify(r => r.UpdateAsync(It.IsAny<SavingsAccount>(), _cancellationToken), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(_cancellationToken), Times.Once);

        Assert.NotNull(response);
        Assert.Equal(transaction.Amount, response.Amount);
        Assert.Equal(transaction.BankAccount.Balance, initialBalance + response.Amount);
        Assert.Equal(transaction.AccountId, response.AccountId);
        Assert.Equal(transaction.Type.ToString(), response.Type.ToString());
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowTransactionOverdraftLimitReachedException_ForCheckingAccount_WithDebitAndOverdraft()
    {
        // Arrange
        var transactionRequest = new TransactionRequest
        {
            Amount = 800,
            Type = TransactionType.Debit,
            AccountId = _accounts[0].Id,
        };

        var checkingAccount = (CheckingAccount)_accounts[0];

        _mockBankAccountRepository.Setup(r => r.GetByIdAsync(transactionRequest.AccountId, It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(checkingAccount);

        // Act & Assert
        await Assert.ThrowsAsync<TransactionOverdraftLimitReachedException>(() => _transactionService.CreateAsync(transactionRequest));
        _mockBankAccountRepository.Verify(r => r.GetByIdAsync(transactionRequest.AccountId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowTransactionDepositLimitReachedException_ForSavingsAccount_WithCreditAndExceedingDepositLimit()
    {
        // Arrange
        var transactionRequest = new TransactionRequest
        {
            Amount = 500,
            Type = TransactionType.Credit,
            AccountId = _accounts[1].Id
        };

        var savingsAccount = (SavingsAccount)_accounts[1];

        _mockBankAccountRepository.Setup(r => r.GetByIdAsync(transactionRequest.AccountId, It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(savingsAccount);

        // Act & Assert
        await Assert.ThrowsAsync<TransactionDepositLimitReachedException>(() => _transactionService.CreateAsync(transactionRequest));
        _mockBankAccountRepository.Verify(r => r.GetByIdAsync(transactionRequest.AccountId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowTransactionWithdrawalExceedException_ForSavingsAccount_WithNegativeBalance()
    {
        // Arrange
        var transactionRequest = new TransactionRequest
        {
            Amount = 1000,
            Type = TransactionType.Debit,
            AccountId = _accounts[1].Id
        };

        var savingsAccount = (SavingsAccount)_accounts[1];

        _mockBankAccountRepository.Setup(r => r.GetByIdAsync(transactionRequest.AccountId, It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(savingsAccount);

        // Act & Assert
        await Assert.ThrowsAsync<TransactionWithdrawalExceedException>(() => _transactionService.CreateAsync(transactionRequest));
        _mockBankAccountRepository.Verify(r => r.GetByIdAsync(transactionRequest.AccountId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowInvalidTransactionAmountException_ForNegativeAmount()
    {
        // Arrange
        var transactionRequest = new TransactionRequest
        {
            Amount = -100,
            Type = TransactionType.Credit,
            AccountId = Guid.NewGuid()
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidTransactionAmountException>(() => _transactionService.CreateAsync(transactionRequest));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTransactions()
    {
        // Arrange
        _mockTransactionRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(_transactions);

        // Act
        var result = await _transactionService.GetAllAsync();

        // Assert
        _mockTransactionRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(_transactions.Count, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTransaction_WhenTransactionExists()
    {
        // Arrange
        var transactionId = _transactions[0].Id;

        _mockTransactionRepository.Setup(r => r.GetByIdAsync(transactionId, It.IsAny<CancellationToken>()))
                                        .ReturnsAsync(_transactions[0]);

        // Act
        var result = await _transactionService.GetByIdAsync(transactionId);

        // Assert
        _mockTransactionRepository.Verify(r => r.GetByIdAsync(transactionId, It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(_transactions[0].Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowTransactionNotFoundException_WhenTransactionDoesNotExist()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        _mockTransactionRepository.Setup(r => r.GetByIdAsync(accountId, It.IsAny<CancellationToken>()))
                                        .ReturnsAsync((Transaction?)null);

        // Act & Assert
        await Assert.ThrowsAsync<TransactionNotFoundException>(() => _transactionService.GetByIdAsync(accountId));

        _mockTransactionRepository.Verify(r => r.GetByIdAsync(accountId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByAccountIdAsync_ShouldReturnAllTransactionsForGivenAccountId()
    {
        // Arrange
        var accountId = _accounts[0].Id;
        var transactions = _transactions.Where(t => t.AccountId == accountId).AsQueryable();


        var savingsAccount = (SavingsAccount)_accounts[1];

        _mockBankAccountRepository.Setup(r => r.GetByIdAsync(accountId, It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(savingsAccount);

        _mockTransactionRepository.Setup(r => r.GetByCondition(It.IsAny<Expression<Func<Transaction, bool>>>()))
                                  .Returns(transactions);

        // Act
        var result = await _transactionService.GetByAccountIdAsync(accountId);

        // Assert
        _mockTransactionRepository.Verify(r => r.GetByCondition(It.IsAny<Expression<Func<Transaction, bool>>>()), Times.Once);
        Assert.Equal(transactions.Count(), result.Count());
    }

    [Fact]
    public async Task GetAccountStatementAsync_ShouldReturnCorrectAccountStatementForGivenAccountId()
    {
        // Arrange
        var accountId = _accounts[0].Id;
        var accountStatementQuery = new AccountStatementQuery
        (
            accountId,
            new DateTime(2024, 6, 1)
        );

        var endOfSlidingMonth = accountStatementQuery.StartOfSlidingMonth.AddDays(30);

        var transactions = _transactions.Where(
            t => t.AccountId == accountId
            && t.Date >= accountStatementQuery.StartOfSlidingMonth
            && t.Date <= endOfSlidingMonth
        ).AsQueryable();

        _mockTransactionRepository.Setup(r => r.GetByCondition(It.IsAny<Expression<Func<Transaction, bool>>>()))
                                  .Returns(transactions);

        _mockBankAccountRepository.Setup(r => r.GetByIdAsync(accountStatementQuery.AccountId, It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(_accounts[0]);

        // Act
        var result = await _transactionService.GetAccountStatementAsync(accountStatementQuery);

        // Assert
        _mockTransactionRepository.Verify(r => r.GetByCondition(It.IsAny<Expression<Func<Transaction, bool>>>()), Times.Once);
        _mockBankAccountRepository.Verify(r => r.GetByIdAsync(accountStatementQuery.AccountId, It.IsAny<CancellationToken>()), Times.Once);

        Assert.Equal(_accounts[0].Balance, result.Balance);
        Assert.Equal(transactions.Count(), result.Transactions.Length);
        Assert.Equal(_transactions[4].Id, result.Transactions[0].Id);
        Assert.Equal(_transactions[0].Id, result.Transactions[1].Id);
    }


}
