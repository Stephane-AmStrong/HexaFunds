using System.Net;

using DataTransfertObjects;

using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Persistence;

using WebApi.IntegrationTests.Setup;

namespace WebApi.IntegrationTests;

[Collection(nameof(IntegrationTestWebFactory))]
public class TransactionsControllerTests : IClassFixture<IntegrationTestWebFactory>, IAsyncLifetime
{
    private readonly HttpClientRepository _httpClientRepository;
    private readonly IntegrationTestWebFactory _factory;
    private IEnumerable<Transaction> _transactions;
    private CheckingAccount? _checkingAccount;
    private Guid _accountId;
    private string _requestUri;

    public TransactionsControllerTests(IntegrationTestWebFactory factory)
    {
        _accountId = Guid.NewGuid();
        _requestUri = "api/transactions";

        _transactions = [];

        _factory = factory;
        var httpClient = _factory.CreateClient();
        _httpClientRepository = new HttpClientRepository(httpClient, _requestUri);
    }

    public async Task InitializeAsync()
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BankingDbContext>();

        await SeedDbWithTransactionsAsync(dbContext);
        _checkingAccount = await GetBanckAccountByIdAsync(_accountId);
        _transactions = await GetAllTransactionsAsync(dbContext);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Get_ReturnsTransactions_WhenCall()
    {
        // Arrange

        // Act
        using var httpMessage = await _httpClientRepository.Get(new TransactionQuery());

        // Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.OK, httpMessage.StatusCode);

        var transactionsResponse = await _httpClientRepository.DeserializeAsync<List<TransactionResponse>>(httpMessage);

        Assert.NotEmpty(transactionsResponse);
        Assert.Equal(_transactions.Count(), transactionsResponse.Count);
    }

    [Fact]
    public async Task GetById_ReturnsTransaction_WhenCall()
    {
        // Arrange        
        var transaction = _transactions.First();

        // Act
        using var httpMessage = await _httpClientRepository.GetById<TransactionResponse>(transaction.Id);

        // Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.OK, httpMessage.StatusCode);

        var transactionResponse = await _httpClientRepository.DeserializeAsync<TransactionResponse>(httpMessage);

        Assert.NotNull(transactionResponse);
        Assert.Equal(transaction.Amount, transactionResponse.Amount);
        Assert.Equal(transaction.Type.ToString(), transactionResponse.Type.ToString());
        Assert.Equal(transaction.Date, transactionResponse.Date);
        Assert.Equal(transaction.AccountId, transactionResponse.AccountId);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenIdIsNonExistent()
    {
        // Arrange        

        // Act
        using var httpMessage = await _httpClientRepository.GetById<TransactionResponse>(Guid.NewGuid());

        // Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.NotFound, httpMessage.StatusCode);
    }

    [Fact]
    public async Task Post_Succeeds_WhenTransactionResquestIsValid()
    {
        //Arrange
        var transactionRequest = new TransactionRequest { Amount = 500, Type = TransactionType.Credit, AccountId = _accountId };

        //Act
        using var httpMessage = await _httpClientRepository.Create(transactionRequest);

        //Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.Created, httpMessage.StatusCode);

        var transactionResponse = await _httpClientRepository.DeserializeAsync<TransactionResponse>(httpMessage);

        Assert.NotNull(transactionResponse);
        Assert.NotEqual(Guid.Empty, transactionResponse.Id);
        Assert.Equal(transactionRequest.Amount, transactionResponse.Amount);
        Assert.Equal(transactionRequest.Type.ToString(), transactionResponse.Type.ToString());
        Assert.Equal(transactionRequest.Date.ToShortDateString(), transactionResponse.Date.ToShortDateString());
        Assert.Equal(transactionRequest.AccountId, transactionResponse.AccountId);
    }

    [Fact]
    public async Task Post_ReturnsBadRequest_WhenTransactionOverdrawLimitResquestIsNegative()
    {
        //Arrange
        var transactionRequest = new TransactionRequest { Amount = -5, Type = TransactionType.Credit, AccountId = _accountId };

        //Act
        using var httpMessage = await _httpClientRepository.Create(transactionRequest);

        //Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.BadRequest, httpMessage.StatusCode);
    }

    private async Task SeedDbWithTransactionsAsync(BankingDbContext dbContext)
    {
        var bankAccount = new CheckingAccount
        {
            Id = _accountId,
            AccountNumber = "FR8487654321",
            Balance = 700,
        };

        var transactions = new[]
        {
            new Transaction { Id = Guid.NewGuid(), Amount = 100, Date = DateTime.SpecifyKind(new DateTime(2024, 6, 4), DateTimeKind.Utc), Type = BankAccount.Core.Enumerations.TransactionType.Credit, AccountId = bankAccount.Id, BankAccount = bankAccount },
            new Transaction { Id = Guid.NewGuid(), Amount = 200, Date = DateTime.SpecifyKind(new DateTime(2024, 6, 9), DateTimeKind.Utc), Type = BankAccount.Core.Enumerations.TransactionType.Debit, AccountId = bankAccount.Id, BankAccount = bankAccount },
        };

        await dbContext.CheckingAccounts.AddAsync(bankAccount);
        await dbContext.Transactions.AddRangeAsync(transactions);
        await dbContext.SaveChangesAsync();
    }

    private async Task<IEnumerable<Transaction>> GetAllTransactionsAsync(BankingDbContext dbContext)
    {
        return await dbContext.Transactions.ToListAsync();
    }

    private async Task<Transaction?> GetTransactionByIdAsync(Guid id)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BankingDbContext>();

        return dbContext.Transactions.FirstOrDefault(account => account.Id == id);
    }

    private async Task<CheckingAccount?> GetBanckAccountByIdAsync(Guid id)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BankingDbContext>();

        return await dbContext.CheckingAccounts.Include(x => x.Transactions)
            .FirstOrDefaultAsync(account => account.Id == id);
    }
}