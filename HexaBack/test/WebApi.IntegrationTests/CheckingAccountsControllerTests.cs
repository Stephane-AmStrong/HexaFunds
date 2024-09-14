using System.Net;

using DataTransfertObjects;

using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Persistence;

using WebApi.IntegrationTests.Setup;

namespace WebApi.IntegrationTests;

[Collection(nameof(IntegrationTestWebFactory))]
public class CheckingAccountsControllerTests : IClassFixture<IntegrationTestWebFactory>, IAsyncLifetime
{
    private readonly HttpClientRepository _httpClientRepository;
    private readonly IntegrationTestWebFactory _factory;
    private IEnumerable<CheckingAccount> _checkingAccounts;

    public CheckingAccountsControllerTests(IntegrationTestWebFactory factory)
    {
        _factory = factory;
        _checkingAccounts = new List<CheckingAccount>();

        var httpClient = _factory.CreateClient();
        _httpClientRepository = new HttpClientRepository(httpClient, "api/checkingAccounts");
    }

    public async Task InitializeAsync()
    {
        await SeedDbWithCheckingAccountsAsync(_factory.Db);
        _checkingAccounts = await GetAllCheckingAccountsAsync(_factory.Db);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Get_ReturnsCheckingAccounts_WhenCall()
    {
        // Arrange

        // Act
        using var httpMessage = await _httpClientRepository.Get();

        // Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.OK, httpMessage.StatusCode);

        var checkingAccountsResponse = await _httpClientRepository.DeserializeAsync<List<CheckingAccountResponse>>(httpMessage);

        Assert.NotEmpty(checkingAccountsResponse);
        Assert.Equal(_checkingAccounts.Count(), checkingAccountsResponse.Count);
    }

    [Theory]
    [InlineData("valid-id", HttpStatusCode.OK)]
    [InlineData("non-existent-id", HttpStatusCode.NotFound)]
    public async Task GetById_ReturnsExpectedStatusCode(string accountId, HttpStatusCode expectedStatusCode)
    {
        // Arrange
        var id = accountId == "valid-id"
            ? _checkingAccounts.First().Id
            : Guid.NewGuid();

        // Act
        using var httpMessage = await _httpClientRepository.GetById<CheckingAccountResponse>(id);

        // Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(expectedStatusCode, httpMessage.StatusCode);
    }

    [Fact]
    public async Task Post_Succeeds_WhenCheckingAccountResquestIsValid()
    {
        //Arrange
        var checkingAccountRequest = new CheckingAccountRequest { AccountNumber = "IT3243543218465", OverdraftLimit = 100 };

        //Act
        using var httpMessage = await _httpClientRepository.Create(checkingAccountRequest);

        //Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.Created, httpMessage.StatusCode);

        var checkingAccountResponse = await _httpClientRepository.DeserializeAsync<CheckingAccountResponse>(httpMessage);

        Assert.NotNull(checkingAccountResponse);
        Assert.NotEqual(Guid.Empty, checkingAccountResponse.Id);
        Assert.Equal(checkingAccountRequest.AccountNumber, checkingAccountResponse.AccountNumber);
        Assert.Equal(checkingAccountRequest.Balance, checkingAccountResponse.Balance);
        Assert.Equal(checkingAccountRequest.OverdraftLimit, checkingAccountResponse.OverdraftLimit);
    }

    [Fact]
    public async Task Post_ReturnsBadRequest_WhenCheckingAccountOverdrawLimitResquestIsNegative()
    {
        //Arrange
        var checkingAccountRequest = new CheckingAccountRequest { AccountNumber = "IT3243543218465", OverdraftLimit = -10 };

        //Act
        using var httpMessage = await _httpClientRepository.Create(checkingAccountRequest);

        //Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.BadRequest, httpMessage.StatusCode);
    }

    [Fact]
    public async Task Put_ReturnsNoContent_WhenIdExistsAndCheckingAccountResquestIsValid()
    {
        //Arrange
        var idCheckingAccount = _checkingAccounts.First().Id;
        var checkingAccountRequest = new CheckingAccountRequest { AccountNumber = "Azerty", OverdraftLimit = 1024 };

        //Act
        using var httpMessage = await _httpClientRepository.Update(idCheckingAccount, checkingAccountRequest);

        //Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.NoContent, httpMessage.StatusCode);

        var checkingAccountFromTheDatabase = await GetCheckingAccountByIdAsync(idCheckingAccount);

        Assert.NotNull(checkingAccountFromTheDatabase);
        Assert.Equal(checkingAccountRequest.AccountNumber, checkingAccountFromTheDatabase.AccountNumber);
        Assert.Equal(checkingAccountRequest.Balance, checkingAccountFromTheDatabase.Balance);
        Assert.Equal(checkingAccountRequest.OverdraftLimit, checkingAccountFromTheDatabase.OverdraftLimit);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenIdIsNonExistent()
    {
        // Arrange        
        var checkingAccountRequest = new CheckingAccountRequest { AccountNumber = "Azerty", OverdraftLimit = 1024 };

        // Act
        using var httpMessage = await _httpClientRepository.Update(Guid.NewGuid(), checkingAccountRequest);

        // Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.NotFound, httpMessage.StatusCode);
    }

    [Fact]
    public async Task Put_ReturnsBadRequest_WhenIdExistsAndCheckingAccountResquestIsNegative()
    {
        //Arrange
        var checkingAccountBeforeUpdate = _checkingAccounts.First();
        var idCheckingAccount = checkingAccountBeforeUpdate.Id;
        var checkingAccountRequest = new CheckingAccountRequest { AccountNumber = "Azerty", OverdraftLimit = -1 };

        //Act
        using var httpMessage = await _httpClientRepository.Update(idCheckingAccount, checkingAccountRequest);

        //Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.BadRequest, httpMessage.StatusCode);

        var checkingAccountFromTheDatabase = await GetCheckingAccountByIdAsync(idCheckingAccount);

        Assert.NotNull(checkingAccountFromTheDatabase);
        Assert.Equal(checkingAccountBeforeUpdate.AccountNumber, checkingAccountFromTheDatabase.AccountNumber);
        Assert.Equal(checkingAccountBeforeUpdate.Balance, checkingAccountFromTheDatabase.Balance);
        Assert.Equal(checkingAccountBeforeUpdate.OverdraftLimit, checkingAccountFromTheDatabase.OverdraftLimit);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenIdExists()
    {
        //Arrange
        var idCheckingAccount = _checkingAccounts.First().Id;
        var dataSizeBeforeDeletion = _checkingAccounts.Count();
        //Act
        using var httpMessage = await _httpClientRepository.Delete(idCheckingAccount);

        //Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.NoContent, httpMessage.StatusCode);

        var checkingAccountFromTheDatabase = await GetCheckingAccountByIdAsync(idCheckingAccount);

        Assert.Null(checkingAccountFromTheDatabase);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenIdIsNonExists()
    {
        //Arrange
        var idCheckingAccount = Guid.NewGuid();
        var dataSizeBeforeDeletion = _checkingAccounts.Count();

        //Act
        using var httpMessage = await _httpClientRepository.Delete(idCheckingAccount);

        //Assert
        Assert.NotNull(httpMessage);
        Assert.Equal(HttpStatusCode.NotFound, httpMessage.StatusCode);

        var checkingAccountFromTheDatabase = await GetCheckingAccountByIdAsync(idCheckingAccount);

        Assert.Null(checkingAccountFromTheDatabase);
    }

    private async Task SeedDbWithCheckingAccountsAsync(BankingDbContext dbContext)
    {
        var checkingAccounts = new[]
        {
            new CheckingAccount { Id = Guid.NewGuid(), AccountNumber = "FR3243543218465", OverdraftLimit = 500 },
            new CheckingAccount { Id = Guid.NewGuid(), AccountNumber = "EN3546546353738", OverdraftLimit = 800 },
        };

        await dbContext.CheckingAccounts.AddRangeAsync(checkingAccounts);
        await dbContext.SaveChangesAsync();
    }

    private async Task<IEnumerable<CheckingAccount>> GetAllCheckingAccountsAsync(BankingDbContext dbContext)
    {
        return await dbContext.CheckingAccounts.ToListAsync();
    }

    private async Task<CheckingAccount?> GetCheckingAccountByIdAsync(Guid id)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BankingDbContext>();

        return await dbContext.CheckingAccounts.FirstOrDefaultAsync(account => account.Id == id);
    }
}
