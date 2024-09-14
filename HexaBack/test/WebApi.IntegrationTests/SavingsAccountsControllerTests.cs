using System.Net;

using DataTransfertObjects;

using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Persistence;

using WebApi.IntegrationTests.Setup;

namespace WebApi.IntegrationTests;

[Collection(nameof(IntegrationTestWebFactory))]
public class SavingsAccountsControllerTests : IClassFixture<IntegrationTestWebFactory>, IAsyncLifetime
{
    private readonly HttpClientRepository _httpClientRepository;
    private readonly IntegrationTestWebFactory _factory;
    private IEnumerable<SavingsAccount> _savingsAccounts;

    public SavingsAccountsControllerTests(IntegrationTestWebFactory factory)
    {
        _factory = factory;
        _savingsAccounts = new List<SavingsAccount>();

        var httpClient = _factory.CreateClient();
        _httpClientRepository = new HttpClientRepository(httpClient, "api/savingsAccounts");
    }

    public async Task InitializeAsync()
    {
        await SeedDbWithSavingsAccountsAsync(_factory.Db);
        _savingsAccounts = await GetAllSavingsAccountsAsync(_factory.Db);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Get_ReturnsSavingsAccounts_WhenCall()
    {
        // Arrange

        // Act
        using var httpMessage = await _httpClientRepository.Get();

        // Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.OK, httpMessage.StatusCode);

        var savingsAccountsResponse = await _httpClientRepository.DeserializeAsync<List<SavingsAccountResponse>>(httpMessage);

        Assert.NotEmpty(savingsAccountsResponse);
        Assert.Equal(_savingsAccounts.Count(), savingsAccountsResponse.Count);
    }

    [Theory]
    [InlineData("valid-id", HttpStatusCode.OK)]
    [InlineData("non-existent-id", HttpStatusCode.NotFound)]
    public async Task GetById_ReturnsExpectedStatusCode(string accountId, HttpStatusCode expectedStatusCode)
    {
        // Arrange
        var id = accountId == "valid-id"
            ? _savingsAccounts.First().Id
            : Guid.NewGuid();

        // Act
        using var httpMessage = await _httpClientRepository.GetById<SavingsAccountResponse>(id);

        // Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(expectedStatusCode, httpMessage.StatusCode);
    }

    [Fact]
    public async Task Post_Succeeds_WhenSavingsAccountResquestIsValid()
    {
        //Arrange
        var savingsAccountRequest = new SavingsAccountRequest { AccountNumber = "IT3243543218465", BalanceCeiling = 100 };

        //Act
        using var httpMessage = await _httpClientRepository.Create(savingsAccountRequest);

        //Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.Created, httpMessage.StatusCode);

        var savingsAccountResponse = await _httpClientRepository.DeserializeAsync<SavingsAccountResponse>(httpMessage);

        Assert.NotNull(savingsAccountResponse);
        Assert.NotEqual(Guid.Empty, savingsAccountResponse.Id);
        Assert.Equal(savingsAccountRequest.AccountNumber, savingsAccountResponse.AccountNumber);
        Assert.Equal(savingsAccountRequest.Balance, savingsAccountResponse.Balance);
        Assert.Equal(savingsAccountRequest.BalanceCeiling, savingsAccountResponse.BalanceCeiling);
    }

    [Fact]
    public async Task Post_ReturnsBadRequest_WhenSavingsAccountOverdrawLimitResquestIsNegative()
    {
        //Arrange
        var savingsAccountRequest = new SavingsAccountRequest { AccountNumber = "IT3243543218465", BalanceCeiling = -10 };

        //Act
        using var httpMessage = await _httpClientRepository.Create(savingsAccountRequest);

        //Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.BadRequest, httpMessage.StatusCode);
    }

    [Fact]
    public async Task Put_ReturnsNoContent_WhenIdExistsAndSavingsAccountResquestIsValid()
    {
        //Arrange
        var idSavingsAccount = _savingsAccounts.First().Id;
        var savingsAccountRequest = new SavingsAccountRequest { AccountNumber = "Azerty", BalanceCeiling = 1024 };

        //Act
        using var httpMessage = await _httpClientRepository.Update(idSavingsAccount, savingsAccountRequest);

        //Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.NoContent, httpMessage.StatusCode);

        var savingsAccountFromTheDatabase = await GetSavingsAccountByIdAsync(idSavingsAccount);

        Assert.NotNull(savingsAccountFromTheDatabase);
        Assert.Equal(savingsAccountRequest.AccountNumber, savingsAccountFromTheDatabase.AccountNumber);
        Assert.Equal(savingsAccountRequest.Balance, savingsAccountFromTheDatabase.Balance);
        Assert.Equal(savingsAccountRequest.BalanceCeiling, savingsAccountFromTheDatabase.BalanceCeiling);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenIdIsNonExistent()
    {
        // Arrange        
        var savingsAccountRequest = new SavingsAccountRequest { AccountNumber = "Azerty", BalanceCeiling = 1024 };

        // Act
        using var httpMessage = await _httpClientRepository.Update(Guid.NewGuid(), savingsAccountRequest);

        // Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.NotFound, httpMessage.StatusCode);
    }

    [Fact]
    public async Task Put_ReturnsBadRequest_WhenIdExistsAndSavingsAccountResquestIsNegative()
    {
        //Arrange
        var savingsAccountBeforeUpdate = _savingsAccounts.First();
        var idSavingsAccount = savingsAccountBeforeUpdate.Id;
        var savingsAccountRequest = new SavingsAccountRequest { AccountNumber = "Azerty", BalanceCeiling = -1 };

        //Act
        using var httpMessage = await _httpClientRepository.Update(idSavingsAccount, savingsAccountRequest);

        //Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.BadRequest, httpMessage.StatusCode);

        var savingsAccountFromTheDatabase = await GetSavingsAccountByIdAsync(idSavingsAccount);

        Assert.NotNull(savingsAccountFromTheDatabase);
        Assert.Equal(savingsAccountBeforeUpdate.AccountNumber, savingsAccountFromTheDatabase.AccountNumber);
        Assert.Equal(savingsAccountBeforeUpdate.Balance, savingsAccountFromTheDatabase.Balance);
        Assert.Equal(savingsAccountBeforeUpdate.BalanceCeiling, savingsAccountFromTheDatabase.BalanceCeiling);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenIdExists()
    {
        //Arrange
        var idSavingsAccount = _savingsAccounts.First().Id;
        var dataSizeBeforeDeletion = _savingsAccounts.Count();
        //Act
        using var httpMessage = await _httpClientRepository.Delete(idSavingsAccount);

        //Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.NoContent, httpMessage.StatusCode);

        var savingsAccountFromTheDatabase = await GetSavingsAccountByIdAsync(idSavingsAccount);

        Assert.Null(savingsAccountFromTheDatabase);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenIdIsNonExists()
    {
        //Arrange
        var idSavingsAccount = Guid.NewGuid();
        var dataSizeBeforeDeletion = _savingsAccounts.Count();

        //Act
        using var httpMessage = await _httpClientRepository.Delete(idSavingsAccount);

        //Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.NotFound, httpMessage.StatusCode);

        var savingsAccountFromTheDatabase = await GetSavingsAccountByIdAsync(idSavingsAccount);

        Assert.Null(savingsAccountFromTheDatabase);
    }

    private async Task SeedDbWithSavingsAccountsAsync(BankingDbContext dbContext)
    {
        var savingsAccounts = new[]
        {
            new SavingsAccount { Id = Guid.NewGuid(), AccountNumber = "FR3243543218465", BalanceCeiling = 500 },
            new SavingsAccount { Id = Guid.NewGuid(), AccountNumber = "EN3546546353738", BalanceCeiling = 800 },
        };

        await dbContext.SavingsAccounts.AddRangeAsync(savingsAccounts);
        await dbContext.SaveChangesAsync();
    }

    private async Task<IEnumerable<SavingsAccount>> GetAllSavingsAccountsAsync(BankingDbContext dbContext)
    {
        return await dbContext.SavingsAccounts.ToListAsync();
    }

    private async Task<SavingsAccount?> GetSavingsAccountByIdAsync(Guid id)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BankingDbContext>();

        return await dbContext.SavingsAccounts.FirstOrDefaultAsync(account => account.Id == id);
    }
}