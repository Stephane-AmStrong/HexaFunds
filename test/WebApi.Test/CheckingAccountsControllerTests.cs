using System.Net;

using DataTransfertObjects;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Services.Abstractions;

using WebApplicationDocker.Controllers;

namespace WebApi.Test;

public class CheckingAccountsControllerTests
{
    private readonly Mock<IServiceManager> _mockServiceManager;
    private readonly Mock<ICheckingAccountService> _mockCheckingAccountService;
    private readonly CheckingAccountsController _controller;
    private readonly CancellationToken _cancellationToken;

    private readonly CheckingAccountRequest _checkingAccountRequest;
    private readonly CheckingAccountResponse _checkingAccountResponse;

    public CheckingAccountsControllerTests()
    {
        _cancellationToken = new CancellationToken();
        _mockServiceManager = new Mock<IServiceManager>();
        _mockCheckingAccountService = new Mock<ICheckingAccountService>();
        _mockServiceManager.Setup(sm => sm.CheckingAccountService).Returns(_mockCheckingAccountService.Object);
        _controller = new CheckingAccountsController(_mockServiceManager.Object);

        _checkingAccountRequest = new CheckingAccountRequest { AccountNumber = "12345", OverdraftLimit = 500 };
        _checkingAccountResponse = new CheckingAccountResponse { Id = Guid.NewGuid(), AccountNumber = "12345", OverdraftLimit = 500 };
    }

    [Fact]
    public void Get_ShouldReturnOkWithCheckingAccounts()
    {
        // Arrange
        var checkingAccounts = new List<CheckingAccountResponse> { _checkingAccountResponse };
        _mockCheckingAccountService.Setup(s => s.GetAll()).Returns(checkingAccounts);

        // Act
        var result = _controller.Get();

        // Assert
        _mockCheckingAccountService.Verify(r => r.GetAll(), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<CheckingAccountResponse>>(okResult.Value);
        Assert.Equal(checkingAccounts.Count, returnValue.Count);
    }

    [Fact]
    public async Task Get_ById_ShouldReturnOkWithCheckingAccount()
    {
        // Arrange        
        _mockCheckingAccountService.Setup(s => s.GetByIdAsync(_checkingAccountResponse.Id, _cancellationToken)).ReturnsAsync(_checkingAccountResponse);

        // Act
        var result = await _controller.Get(_checkingAccountResponse.Id, CancellationToken.None);

        // Assert
        _mockCheckingAccountService.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), _cancellationToken), Times.Once);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<CheckingAccountResponse>(okResult.Value);

        Assert.Equal(_checkingAccountResponse.Id, returnValue.Id);
        Assert.Equal(_checkingAccountResponse.AccountNumber, returnValue.AccountNumber);
    }

    [Fact]
    public async Task Post_ShouldReturnCreatedAtActionWithCheckingAccount()
    {
        // Arrange
        _mockCheckingAccountService
            .Setup(s => s.CreateAsync(_checkingAccountRequest, _cancellationToken))
            .ReturnsAsync(_checkingAccountResponse);

        // Act
        var result = await _controller.Post(_checkingAccountRequest, _cancellationToken);

        // Assert
        _mockCheckingAccountService.Verify(
            r => r.CreateAsync(It.IsAny<CheckingAccountRequest>(), _cancellationToken),
            Times.Once);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);

        Assert.NotNull(createdAtActionResult);

        Assert.Equal((int)HttpStatusCode.Created, createdAtActionResult.StatusCode);

        var returnValue = Assert.IsType<CheckingAccountResponse>(createdAtActionResult.Value);
        Assert.Equal(_checkingAccountResponse.Id, returnValue.Id);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockCheckingAccountService.Setup(s => s.DeleteAsync(id, _cancellationToken)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(id, CancellationToken.None);

        // Assert
        _mockCheckingAccountService.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), _cancellationToken), Times.Once);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Put_ShouldReturnNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mockCheckingAccountService.Setup(s => s.UpdateAsync(id, _checkingAccountRequest, _cancellationToken)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Put(id, _checkingAccountRequest, CancellationToken.None);

        // Assert
        _mockCheckingAccountService.Verify(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CheckingAccountRequest>(), _cancellationToken), Times.Once);
        Assert.IsType<NoContentResult>(result);
    }
}