using Application.DataTransfertObjects;
using Domain.Entities;
using Domain.Errors;
using Domain.Abstractions.Repositories;
using Moq;
using Services;
namespace Service.Test;

public class CheckingAccountsServiceTest
{
    private readonly Mock<ICheckingAccountsRepository> _mockCheckingAccountsRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CheckingAccountsService _checkingAccountsService;
    private readonly CancellationToken _cancellationToken;
    private readonly List<CheckingAccount> _checkingAccounts;

    public CheckingAccountsServiceTest()
    {
        _cancellationToken = new CancellationToken();
        _mockCheckingAccountsRepository = new Mock<ICheckingAccountsRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _checkingAccountsService = new CheckingAccountsService(_mockCheckingAccountsRepository.Object, _mockUnitOfWork.Object);

        _checkingAccounts =
        [
            new()
            {
                Id = Guid.NewGuid(),
                AccountNumber = "123456789",
                Balance = 1000.00f,
                OverdraftLimit = 500
            },
            new()
            {
                Id = Guid.NewGuid(),
                AccountNumber = "987654321",
                Balance = 2000.00f,
                OverdraftLimit = 1000
            }
        ];
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateAccountSuccessfully()
    {
        // Arrange
        var request = new CheckingAccountRequest
        {
            Balance = _checkingAccounts[0].Balance,
            AccountNumber = _checkingAccounts[0].AccountNumber,
            OverdraftLimit = _checkingAccounts[0].OverdraftLimit,
        };

        var checkingAccount = new CheckingAccount
        {
            Id = _checkingAccounts[0].Id,
            Balance = _checkingAccounts[0].Balance,
            OverdraftLimit = _checkingAccounts[0].OverdraftLimit,
            AccountNumber = _checkingAccounts[0].AccountNumber
        };

        _mockCheckingAccountsRepository.Setup(r => r.CreateAsync(It.IsAny<CheckingAccount>(), _cancellationToken))
                                      .Returns(Task.CompletedTask);

        _mockCheckingAccountsRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), _cancellationToken))
                                      .ReturnsAsync(checkingAccount);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(_cancellationToken))
                       .ReturnsAsync(1);

        // Act
        var response = await _checkingAccountsService.CreateAsync(request, _cancellationToken);

        // Assert
        _mockCheckingAccountsRepository.Verify(r => r.CreateAsync(It.IsAny<CheckingAccount>(), _cancellationToken), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(_cancellationToken), Times.Once);

        Assert.NotNull(response);
        Assert.Equal(checkingAccount.Balance, response.Balance);
        Assert.Equal(checkingAccount.OverdraftLimit, response.OverdraftLimit);
        Assert.Equal(checkingAccount.AccountNumber, response.AccountNumber);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteAccountSuccessfully()
    {
        // Arrange
        var accountId = _checkingAccounts[0].Id;
        var checkingAccount = new CheckingAccount
        {
            Id = accountId,
            AccountNumber = _checkingAccounts[0].AccountNumber,
            Balance = _checkingAccounts[0].Balance,
            OverdraftLimit = _checkingAccounts[0].OverdraftLimit,
        };

        _mockCheckingAccountsRepository.Setup(r => r.GetByIdAsync(accountId, _cancellationToken))
                                      .ReturnsAsync(checkingAccount);

        _mockCheckingAccountsRepository.Setup(r => r.Delete(checkingAccount));

        // Act
        await _checkingAccountsService.DeleteAsync(accountId, _cancellationToken);

        // Assert
        _mockCheckingAccountsRepository.Verify(r => r.GetByIdAsync(accountId, _cancellationToken), Times.Once);
        _mockCheckingAccountsRepository.Verify(r => r.Delete(checkingAccount), Times.Once);
        _mockUnitOfWork.Verify(r => r.SaveChangesAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowAccountNotFoundException_WhenAccountDoesNotExist()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        _mockCheckingAccountsRepository.Setup(r => r.GetByIdAsync(accountId, _cancellationToken))
                                      .ReturnsAsync((CheckingAccount?)null);

        // Act & Assert
        await Assert.ThrowsAsync<AccountNotFoundException>(() => _checkingAccountsService.DeleteAsync(accountId, _cancellationToken));

        _mockCheckingAccountsRepository.Verify(r => r.GetByIdAsync(accountId, _cancellationToken), Times.Once);
        _mockCheckingAccountsRepository.Verify(r => r.Delete(It.IsAny<CheckingAccount>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(_cancellationToken), Times.Never);
    }

    ////
    ///
    [Fact]
    public void GetAll_ShouldReturnAllAccounts()
    {
        // Arrange
        _mockCheckingAccountsRepository.Setup(r => r.GetAll())
                                        .Returns(_checkingAccounts);

        // Act
        var result = _checkingAccountsService.GetAll();

        // Assert
        _mockCheckingAccountsRepository.Verify(r => r.GetAll(), Times.Once);
        Assert.Equal(_checkingAccounts.Count, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAccount_WhenAccountExists()
    {
        // Arrange
        var accountId = _checkingAccounts[0].Id;
        var checkingAccount = new CheckingAccount
        {
            Id = accountId,
            AccountNumber = _checkingAccounts[0].AccountNumber,
            Balance = _checkingAccounts[0].Balance,
            OverdraftLimit = _checkingAccounts[0].OverdraftLimit,
        };

        _mockCheckingAccountsRepository.Setup(r => r.GetByIdAsync(accountId, _cancellationToken))
                                        .ReturnsAsync(checkingAccount);

        // Act
        var result = await _checkingAccountsService.GetByIdAsync(accountId, _cancellationToken);

        // Assert
        _mockCheckingAccountsRepository.Verify(r => r.GetByIdAsync(accountId, _cancellationToken), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(checkingAccount.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowAccountNotFoundException_WhenAccountDoesNotExist()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        _mockCheckingAccountsRepository.Setup(r => r.GetByIdAsync(accountId, _cancellationToken))
                                        .ReturnsAsync((CheckingAccount?)null);

        // Act & Assert
        await Assert.ThrowsAsync<AccountNotFoundException>(() => _checkingAccountsService.GetByIdAsync(accountId, _cancellationToken));

        _mockCheckingAccountsRepository.Verify(r => r.GetByIdAsync(accountId, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAccountSuccessfully()
    {
        // Arrange
        var accountId = _checkingAccounts[0].Id;
        var existingAccount = new CheckingAccount
        {
            Id = accountId,
            AccountNumber = _checkingAccounts[0].AccountNumber,
            Balance = _checkingAccounts[0].Balance,
            OverdraftLimit = _checkingAccounts[0].OverdraftLimit,
        };

        var request = new CheckingAccountRequest
        {
            AccountNumber = _checkingAccounts[0].AccountNumber,
            OverdraftLimit = _checkingAccounts[0].OverdraftLimit
        };

        _mockCheckingAccountsRepository.Setup(r => r.GetByIdAsync(accountId, _cancellationToken))
                                        .ReturnsAsync(existingAccount);

        _mockCheckingAccountsRepository.Setup(r => r.Update(existingAccount));

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(_cancellationToken))
                        .ReturnsAsync(1);

        // Act
        await _checkingAccountsService.UpdateAsync(accountId, request, _cancellationToken);

        // Assert
        _mockCheckingAccountsRepository.Verify(r => r.GetByIdAsync(accountId, _cancellationToken), Times.Once);
        _mockCheckingAccountsRepository.Verify(r => r.Update(existingAccount), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowAccountNotFoundException_WhenAccountDoesNotExist()
    {
        // Arrange
        var accountId = _checkingAccounts[0].Id;

        var request = new CheckingAccountRequest
        {
            AccountNumber = _checkingAccounts[0].AccountNumber,
            OverdraftLimit = _checkingAccounts[0].OverdraftLimit
        };

        _mockCheckingAccountsRepository.Setup(r => r.GetByIdAsync(accountId, _cancellationToken))
                                        .ReturnsAsync((CheckingAccount?)null);

        // Act & Assert
        await Assert.ThrowsAsync<AccountNotFoundException>(() => _checkingAccountsService.UpdateAsync(accountId, request, _cancellationToken));

        _mockCheckingAccountsRepository.Verify(r => r.GetByIdAsync(accountId, _cancellationToken), Times.Once);
        _mockCheckingAccountsRepository.Verify(r => r.Update(It.IsAny<CheckingAccount>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(_cancellationToken), Times.Never);
    }

}
