using DataTransfertObjects;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Services.Abstractions;

using WebApplicationDocker.Controllers;

namespace WebApi.Test;

public class TransactionsControllerTests
{
    private readonly Mock<IServiceManager> _mockServiceManager;
    private readonly Mock<ITransactionService> _mockTransactionService;
    private readonly TransactionsController _controller;
    private readonly TransactionResponse _transactionResponse;
    private readonly BankAccountResponse _bankAccount;

    public TransactionsControllerTests()
    {
        _mockServiceManager = new Mock<IServiceManager>();
        _mockTransactionService = new Mock<ITransactionService>();
        _mockServiceManager.Setup(sm => sm.TransactionService).Returns(_mockTransactionService.Object);
        _controller = new TransactionsController(_mockServiceManager.Object);

        _bankAccount = new BankAccountResponse()
        {
            Id = Guid.NewGuid(),
            AccountNumber = "8487654321",
            Balance = 700,
        };

        _transactionResponse = new TransactionResponse { Id = Guid.NewGuid(), Amount = 100, Type = TransactionType.Credit, AccountId = _bankAccount.Id, /*BankAccount = _bankAccount*/ };
    }

    [Fact]
    public void Get_ShouldReturnOkWithTransactions()
    {
        // Arrange
        var transactions = new List<TransactionResponse> { _transactionResponse };
        _mockTransactionService.Setup(s => s.GetAll()).Returns(transactions);

        // Act
        var result = _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<TransactionResponse>>(okResult.Value);
        Assert.Equal(transactions.Count, returnValue.Count);
    }

    [Fact]
    public async Task GetAccountStatement_ShouldReturnOkWithAccountStatement()
    {
        // Arrange
        var accountStatementQuery = new AccountStatementQuery(Guid.NewGuid(), DateTime.UtcNow.AddDays(-30));
        var accountStatementResponse = new AccountStatementResponse("Checking", 500, new AccountStatementTransactionResponse[0]);
        _mockTransactionService.Setup(s => s.GetAccountStatementAsync(accountStatementQuery, It.IsAny<CancellationToken>())).ReturnsAsync(accountStatementResponse);

        // Act
        var result = await _controller.GetAccountStatement(accountStatementQuery, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<AccountStatementResponse>(okResult.Value);
        Assert.Equal(accountStatementResponse.AccountType, returnValue.AccountType);
    }

    [Fact]
    public async Task GetByAccountId_ShouldReturnOkWithCheckingAccountAndTransactions()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var accountTransactions = new AccountTransactionsResponse
        {
            CheckingAccount = new CheckingAccountResponse
            {
                Id = _bankAccount.Id,
                AccountNumber = _bankAccount.AccountNumber,
                Balance = _bankAccount.Balance,
                OverdraftLimit = 200,
            },
            Transactions = new List<TransactionResponse> { _transactionResponse },
        };

        _mockTransactionService.Setup(s => s.GetByAccountIdAsync(accountId, It.IsAny<CancellationToken>())).ReturnsAsync(accountTransactions);

        // Act
        var result = await _controller.GetByAccountId(accountId, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<AccountTransactionsResponse>(okResult.Value);

        Assert.Null(returnValue.SavingsAccount);
        Assert.Equal(accountTransactions.CheckingAccount, returnValue.CheckingAccount);
        Assert.Equal(accountTransactions.Transactions.Count(), returnValue.Transactions.Count());
    }

    [Fact]
    public async Task GetByAccountId_ShouldReturnOkWithSavingsAccountAndTransactions()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var accountTransactions = new AccountTransactionsResponse
        {
            SavingsAccount = new SavingsAccountResponse
            {
                Id = _bankAccount.Id,
                AccountNumber = _bankAccount.AccountNumber,
                Balance = _bankAccount.Balance,
                BalanceCeiling = 300,
            },
            Transactions = new List<TransactionResponse> { _transactionResponse },
        };

        _mockTransactionService.Setup(s => s.GetByAccountIdAsync(accountId, It.IsAny<CancellationToken>())).ReturnsAsync(accountTransactions);

        // Act
        var result = await _controller.GetByAccountId(accountId, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<AccountTransactionsResponse>(okResult.Value);

        Assert.Null(returnValue.CheckingAccount);
        Assert.Equal(accountTransactions.SavingsAccount, returnValue.SavingsAccount);
        Assert.Equal(accountTransactions.Transactions.Count(), returnValue.Transactions.Count());
    }

    [Fact]
    public async Task Get_ById_ShouldReturnOkWithTransaction()
    {
        // Arrange
        _mockTransactionService.Setup(s => s.GetByIdAsync(_transactionResponse.Id, It.IsAny<CancellationToken>())).ReturnsAsync(_transactionResponse);

        // Act
        var result = await _controller.Get(_transactionResponse.Id, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<TransactionResponse>(okResult.Value);
        Assert.Equal(_transactionResponse.Id, returnValue.Id);
    }

    [Fact]
    public async Task Post_ShouldReturnCreatedAtActionWithTransaction()
    {
        // Arrange
        var request = new TransactionRequest { Amount = 100, Type = TransactionType.Credit, AccountId = _bankAccount.Id };
        _mockTransactionService.Setup(s => s.CreateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(_transactionResponse);

        // Act
        var result = await _controller.Post(request, CancellationToken.None);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<TransactionResponse>(createdAtActionResult.Value);
        Assert.Equal(_transactionResponse.Id, returnValue.Id);
    }
}
