using DataTransfertObjects;

using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories.Abstractions;

using Moq;

using Services;
namespace Service.Test;

public class CheckingAccountServiceTest
{
    private readonly Mock<ICheckingAccountRepository> _mockCheckingAccountRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CheckingAccountService _checkingAccountService;
    private readonly CancellationToken _cancellationToken;
    private readonly List<CheckingAccount> _checkingAccounts;

    public CheckingAccountServiceTest()
    {
        _cancellationToken = new CancellationToken();
        _mockCheckingAccountRepository = new Mock<ICheckingAccountRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _checkingAccountService = new CheckingAccountService(_mockCheckingAccountRepository.Object, _mockUnitOfWork.Object);

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

        _mockCheckingAccountRepository.Setup(r => r.CreateAsync(It.IsAny<CheckingAccount>(), _cancellationToken))
                                      .Returns(Task.CompletedTask);

        _mockCheckingAccountRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), _cancellationToken))
                                      .ReturnsAsync(checkingAccount);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(_cancellationToken))
                       .ReturnsAsync(1);

        // Act
        var response = await _checkingAccountService.CreateAsync(request, _cancellationToken);

        // Assert
        _mockCheckingAccountRepository.Verify(r => r.CreateAsync(It.IsAny<CheckingAccount>(), _cancellationToken), Times.Once);
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

        _mockCheckingAccountRepository.Setup(r => r.GetByIdAsync(accountId, _cancellationToken))
                                      .ReturnsAsync(checkingAccount);

        _mockCheckingAccountRepository.Setup(r => r.Delete(checkingAccount));

        // Act
        await _checkingAccountService.DeleteAsync(accountId, _cancellationToken);

        // Assert
        _mockCheckingAccountRepository.Verify(r => r.GetByIdAsync(accountId, _cancellationToken), Times.Once);
        _mockCheckingAccountRepository.Verify(r => r.Delete(checkingAccount), Times.Once);
        _mockUnitOfWork.Verify(r => r.SaveChangesAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowAccountNotFoundException_WhenAccountDoesNotExist()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        _mockCheckingAccountRepository.Setup(r => r.GetByIdAsync(accountId, _cancellationToken))
                                      .ReturnsAsync((CheckingAccount?)null);

        // Act & Assert
        await Assert.ThrowsAsync<AccountNotFoundException>(() => _checkingAccountService.DeleteAsync(accountId, _cancellationToken));

        _mockCheckingAccountRepository.Verify(r => r.GetByIdAsync(accountId, _cancellationToken), Times.Once);
        _mockCheckingAccountRepository.Verify(r => r.Delete(It.IsAny<CheckingAccount>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(_cancellationToken), Times.Never);
    }

    ////
    ///
    [Fact]
    public void GetAll_ShouldReturnAllAccounts()
    {
        // Arrange
        _mockCheckingAccountRepository.Setup(r => r.GetAll())
                                        .Returns(_checkingAccounts);

        // Act
        var result = _checkingAccountService.GetAll();

        // Assert
        _mockCheckingAccountRepository.Verify(r => r.GetAll(), Times.Once);
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

        _mockCheckingAccountRepository.Setup(r => r.GetByIdAsync(accountId, _cancellationToken))
                                        .ReturnsAsync(checkingAccount);

        // Act
        var result = await _checkingAccountService.GetByIdAsync(accountId, _cancellationToken);

        // Assert
        _mockCheckingAccountRepository.Verify(r => r.GetByIdAsync(accountId, _cancellationToken), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(checkingAccount.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowAccountNotFoundException_WhenAccountDoesNotExist()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        _mockCheckingAccountRepository.Setup(r => r.GetByIdAsync(accountId, _cancellationToken))
                                        .ReturnsAsync((CheckingAccount?)null);

        // Act & Assert
        await Assert.ThrowsAsync<AccountNotFoundException>(() => _checkingAccountService.GetByIdAsync(accountId, _cancellationToken));

        _mockCheckingAccountRepository.Verify(r => r.GetByIdAsync(accountId, _cancellationToken), Times.Once);
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

        _mockCheckingAccountRepository.Setup(r => r.GetByIdAsync(accountId, _cancellationToken))
                                        .ReturnsAsync(existingAccount);

        _mockCheckingAccountRepository.Setup(r => r.Update(existingAccount));

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(_cancellationToken))
                        .ReturnsAsync(1);

        // Act
        await _checkingAccountService.UpdateAsync(accountId, request, _cancellationToken);

        // Assert
        _mockCheckingAccountRepository.Verify(r => r.GetByIdAsync(accountId, _cancellationToken), Times.Once);
        _mockCheckingAccountRepository.Verify(r => r.Update(existingAccount), Times.Once);
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

        _mockCheckingAccountRepository.Setup(r => r.GetByIdAsync(accountId, _cancellationToken))
                                        .ReturnsAsync((CheckingAccount?)null);

        // Act & Assert
        await Assert.ThrowsAsync<AccountNotFoundException>(() => _checkingAccountService.UpdateAsync(accountId, request, _cancellationToken));

        _mockCheckingAccountRepository.Verify(r => r.GetByIdAsync(accountId, _cancellationToken), Times.Once);
        _mockCheckingAccountRepository.Verify(r => r.Update(It.IsAny<CheckingAccount>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(_cancellationToken), Times.Never);
    }

}
