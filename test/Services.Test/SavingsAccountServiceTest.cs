using DataTransfertObjects;

using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories.Abstractions;

using Moq;

using Services;
namespace Service.Test;

public class SavingsAccountServiceTest
{
    private readonly Mock<IRepositoryManager> _mockRepositoryManager;
    private readonly Mock<ISavingsAccountRepository> _mockSavingsAccountRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly SavingsAccountService _savingsAccountService;
    private readonly CancellationToken _cancellationToken;
    private readonly List<SavingsAccount> _savingsAccounts;

    public SavingsAccountServiceTest()
    {
        _cancellationToken = new CancellationToken();
        _mockRepositoryManager = new Mock<IRepositoryManager>();
        _mockSavingsAccountRepository = new Mock<ISavingsAccountRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockRepositoryManager.Setup(r => r.SavingsAccountRepository).Returns(_mockSavingsAccountRepository.Object);
        _mockRepositoryManager.Setup(r => r.UnitOfWork).Returns(_mockUnitOfWork.Object);

        _savingsAccountService = new SavingsAccountService(_mockRepositoryManager.Object);

        _savingsAccounts =
        [
            new() 
            {
                Id = Guid.NewGuid(),
                AccountNumber = "3623456789",
                Balance = 0,
                BalanceCeiling = 500
            },
            new() 
            {
                Id = Guid.NewGuid(),
                AccountNumber = "8487654321",
                Balance = 500.00f,
                BalanceCeiling = 1000
            }
        ];
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateAccountSuccessfully()
    {
        // Arrange
        var request = new SavingsAccountRequest
        {
            Balance = _savingsAccounts[0].Balance,
            AccountNumber = _savingsAccounts[0].AccountNumber,
            BalanceCeiling = _savingsAccounts[0].BalanceCeiling,
        };
        
        var savingsAccount = new SavingsAccount
        {
            Id = _savingsAccounts[0].Id,
            Balance = _savingsAccounts[0].Balance,
            BalanceCeiling = _savingsAccounts[0].BalanceCeiling,
            AccountNumber = _savingsAccounts[0].AccountNumber
        };

        _mockSavingsAccountRepository.Setup(r => r.CreateAsync(It.IsAny<SavingsAccount>(), _cancellationToken))
                                      .Returns(Task.CompletedTask);

        _mockSavingsAccountRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), _cancellationToken))
                                      .ReturnsAsync(savingsAccount);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(_cancellationToken))
                       .ReturnsAsync(1);

        // Act
        var response = await _savingsAccountService.CreateAsync(request);

        // Assert
        _mockSavingsAccountRepository.Verify(r => r.CreateAsync(It.IsAny<SavingsAccount>(), _cancellationToken), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(_cancellationToken), Times.Once);

        Assert.NotNull(response);
        Assert.Equal(savingsAccount.Balance, response.Balance);
        Assert.Equal(savingsAccount.BalanceCeiling, response.BalanceCeiling);
        Assert.Equal(savingsAccount.AccountNumber, response.AccountNumber);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteAccountSuccessfully()
    {
        // Arrange
        var accountId = _savingsAccounts[0].Id;
        var savingsAccount = new SavingsAccount
        {
            Id = accountId,
            AccountNumber = _savingsAccounts[0].AccountNumber,
            Balance = _savingsAccounts[0].Balance,
            BalanceCeiling = _savingsAccounts[0].BalanceCeiling,
        };

        _mockSavingsAccountRepository.Setup(r => r.GetByIdAsync(accountId, _cancellationToken))
                                      .ReturnsAsync(savingsAccount);

        _mockSavingsAccountRepository.Setup(r => r.DeleteAsync(savingsAccount, _cancellationToken))
                                      .Returns(Task.CompletedTask);

        // Act
        await _savingsAccountService.DeleteAsync(accountId);

        // Assert
        _mockSavingsAccountRepository.Verify(r => r.GetByIdAsync(accountId, _cancellationToken), Times.Once);
        _mockSavingsAccountRepository.Verify(r => r.DeleteAsync(savingsAccount, _cancellationToken), Times.Once);
        _mockUnitOfWork.Verify(r => r.SaveChangesAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowAccountNotFoundException_WhenAccountDoesNotExist()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        _mockSavingsAccountRepository.Setup(r => r.GetByIdAsync(accountId, _cancellationToken))
                                      .ReturnsAsync((SavingsAccount?)null);

        // Act & Assert
        await Assert.ThrowsAsync<AccountNotFoundException>(() => _savingsAccountService.DeleteAsync(accountId));

        _mockSavingsAccountRepository.Verify(r => r.GetByIdAsync(accountId, _cancellationToken), Times.Once);
        _mockSavingsAccountRepository.Verify(r => r.DeleteAsync(It.IsAny<SavingsAccount>(), _cancellationToken), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(_cancellationToken), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAccounts()
    {
        // Arrange
        _mockSavingsAccountRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                                        .ReturnsAsync(_savingsAccounts);

        // Act
        var result = await _savingsAccountService.GetAllAsync();

        // Assert
        _mockSavingsAccountRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(_savingsAccounts.Count, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAccount_WhenAccountExists()
    {
        // Arrange
        var accountId = _savingsAccounts[0].Id;
        var savingsAccount = new SavingsAccount
        {
            Id = accountId,
            AccountNumber = _savingsAccounts[0].AccountNumber,
            Balance = _savingsAccounts[0].Balance,
            BalanceCeiling = _savingsAccounts[0].BalanceCeiling,
        };

        _mockSavingsAccountRepository.Setup(r => r.GetByIdAsync(accountId, It.IsAny<CancellationToken>()))
                                        .ReturnsAsync(savingsAccount);

        // Act
        var result = await _savingsAccountService.GetByIdAsync(accountId);

        // Assert
        _mockSavingsAccountRepository.Verify(r => r.GetByIdAsync(accountId, It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(savingsAccount.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowAccountNotFoundException_WhenAccountDoesNotExist()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        _mockSavingsAccountRepository.Setup(r => r.GetByIdAsync(accountId, It.IsAny<CancellationToken>()))
                                        .ReturnsAsync((SavingsAccount?)null);

        // Act & Assert
        await Assert.ThrowsAsync<AccountNotFoundException>(() => _savingsAccountService.GetByIdAsync(accountId));

        _mockSavingsAccountRepository.Verify(r => r.GetByIdAsync(accountId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAccountSuccessfully()
    {
        // Arrange
        var accountId = _savingsAccounts[0].Id;
        var existingAccount = new SavingsAccount
        {
            Id = accountId,
            AccountNumber = _savingsAccounts[0].AccountNumber,
            Balance = _savingsAccounts[0].Balance,
            BalanceCeiling = _savingsAccounts[0].BalanceCeiling,
        };

        var request = new SavingsAccountRequest
        {
            AccountNumber = _savingsAccounts[0].AccountNumber,
            BalanceCeiling = _savingsAccounts[0].BalanceCeiling
        };

        _mockSavingsAccountRepository.Setup(r => r.GetByIdAsync(accountId, It.IsAny<CancellationToken>()))
                                        .ReturnsAsync(existingAccount);

        _mockSavingsAccountRepository.Setup(r => r.UpdateAsync(existingAccount, It.IsAny<CancellationToken>()))
                                        .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

        // Act
        await _savingsAccountService.UpdateAsync(accountId, request);

        // Assert
        _mockSavingsAccountRepository.Verify(r => r.GetByIdAsync(accountId, It.IsAny<CancellationToken>()), Times.Once);
        _mockSavingsAccountRepository.Verify(r => r.UpdateAsync(existingAccount, It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowAccountNotFoundException_WhenAccountDoesNotExist()
    {
        // Arrange
        var accountId = _savingsAccounts[0].Id;

        var request = new SavingsAccountRequest
        {
            AccountNumber = _savingsAccounts[0].AccountNumber,
            BalanceCeiling = _savingsAccounts[0].BalanceCeiling
        };

        _mockSavingsAccountRepository.Setup(r => r.GetByIdAsync(accountId, It.IsAny<CancellationToken>()))
                                        .ReturnsAsync((SavingsAccount?)null);

        // Act & Assert
        await Assert.ThrowsAsync<AccountNotFoundException>(() => _savingsAccountService.UpdateAsync(accountId, request));

        _mockSavingsAccountRepository.Verify(r => r.GetByIdAsync(accountId, It.IsAny<CancellationToken>()), Times.Once);
        _mockSavingsAccountRepository.Verify(r => r.UpdateAsync(It.IsAny<SavingsAccount>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

}
