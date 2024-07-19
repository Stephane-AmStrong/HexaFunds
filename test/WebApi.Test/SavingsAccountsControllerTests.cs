using DataTransfertObjects;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Services.Abstractions;

using WebApplicationDocker.Controllers;

namespace WebApi.Test;

public class SavingsAccountsControllerTests
{
    private readonly Mock<IServiceManager> _mockServiceManager;
    private readonly Mock<ISavingsAccountService> _mockSavingsAccountService;
    private readonly SavingsAccountsController _controller;
    private readonly CancellationToken _cancellationToken;

    private readonly SavingsAccountRequest _savingsAccountRequest;
    private readonly SavingsAccountResponse _savingsAccountResponse;

    public SavingsAccountsControllerTests()
    {
        _cancellationToken = new CancellationToken();
        _mockServiceManager = new Mock<IServiceManager>();
        _mockSavingsAccountService = new Mock<ISavingsAccountService>();
        _mockServiceManager.Setup(sm => sm.SavingsAccountService).Returns(_mockSavingsAccountService.Object);
        _controller = new SavingsAccountsController(_mockServiceManager.Object);

        _savingsAccountRequest = new SavingsAccountRequest { AccountNumber = "12345", BalanceCeiling = 500 };
        _savingsAccountResponse = new SavingsAccountResponse { Id = Guid.NewGuid(), AccountNumber = "12345", BalanceCeiling = 500 };
    }

    [Fact]
    public void Get_ShouldReturnOkWithSavingsAccounts()
    {
        // Arrange
        var savingsAccounts = new List<SavingsAccountResponse> { _savingsAccountResponse };
        _mockSavingsAccountService.Setup(s => s.GetAll()).Returns(savingsAccounts);

        // Act
        var result = _controller.Get();

        // Assert
        _mockSavingsAccountService.Verify(r => r.GetAll(), Times.Once);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<SavingsAccountResponse>>(okResult.Value);

        Assert.Equal(savingsAccounts.Count, returnValue.Count);
    }

    [Fact]
    public async Task Get_ById_ShouldReturnOkWithSavingsAccount()
    {
        // Arrange        
        _mockSavingsAccountService.Setup(s => s.GetByIdAsync(_savingsAccountResponse.Id, _cancellationToken)).ReturnsAsync(_savingsAccountResponse);

        // Act
        var result = await _controller.Get(_savingsAccountResponse.Id, CancellationToken.None);

        // Assert
        _mockSavingsAccountService.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), _cancellationToken), Times.Once);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<SavingsAccountResponse>(okResult.Value);

        Assert.Equal(_savingsAccountResponse.Id, returnValue.Id);
        Assert.Equal(_savingsAccountResponse.AccountNumber, returnValue.AccountNumber);
    }

    [Fact]
    public async Task Post_ShouldReturnCreatedAtActionWithSavingsAccount()
    {
        // Arrange
        _mockSavingsAccountService.Setup(s => s.CreateAsync(_savingsAccountRequest, _cancellationToken)).ReturnsAsync(_savingsAccountResponse);

        // Act
        var result = await _controller.Post(_savingsAccountRequest, CancellationToken.None);

        // Assert
        _mockSavingsAccountService.Verify(r => r.CreateAsync(It.IsAny<SavingsAccountRequest>(), _cancellationToken), Times.Once);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<SavingsAccountResponse>(createdAtActionResult.Value);

        Assert.Equal(_savingsAccountResponse.Id, returnValue.Id);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockSavingsAccountService.Setup(s => s.DeleteAsync(id, _cancellationToken)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(id, CancellationToken.None);

        // Assert
        _mockSavingsAccountService.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), _cancellationToken), Times.Once);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Put_ShouldReturnNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mockSavingsAccountService.Setup(s => s.UpdateAsync(id, _savingsAccountRequest, _cancellationToken)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Put(id, _savingsAccountRequest, CancellationToken.None);

        // Assert
        _mockSavingsAccountService.Verify(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<SavingsAccountRequest>(), _cancellationToken), Times.Once);

        Assert.IsType<NoContentResult>(result);
    }
}