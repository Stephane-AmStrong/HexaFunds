using System.Linq.Expressions;
using Application.DataTransfertObjects.Requests;
using Application.UseCases.CheckingAccounts.Create;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Application.Tests.UseCases;

public class CreateCheckingAccountCommandHandlerTests
{
    private IFixture _fixture = null!;
    private ICheckingAccountRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private ILogger<CreateCheckingAccountCommandHandler> _logger = null!;
    private CreateCheckingAccountCommandHandler _handler = null!;
    private CheckingAccount _mockCheckingAccount = null!;
    private CreateCheckingAccountValidator _checkingAccountValidator = null!;

    [Before(Test)]
    public void BeforeTest(TestContext context)
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization{ConfigureMembers = true});

        _repository = _fixture.Freeze<ICheckingAccountRepository>();
        _unitOfWork = _fixture.Freeze<IUnitOfWork>();
        _logger = _fixture.Freeze<ILogger<CreateCheckingAccountCommandHandler>>();
        _handler = new CreateCheckingAccountCommandHandler(_repository, _unitOfWork, _logger);

        // Remove recursion crash
        _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));

        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        // Break Transaction -> BankAccount circular reference
        _fixture.Customize<Transaction>(c => c.Without(t => t.BankAccount));

        // Neutralize Transactions collection
        _fixture.Customize<CheckingAccount>(c => c.With(x => x.Transactions, []));

        _checkingAccountValidator = new CreateCheckingAccountValidator(_repository);



        _mockCheckingAccount = _fixture.Create<CheckingAccount>();

        _repository.FindByConditionAsync(Arg.Any<Expression<Func<CheckingAccount, bool>>>(), Arg.Any<CancellationToken>())
                  .Returns([_mockCheckingAccount]);
    }



    [Test]
    public async Task Create_Successfully_CreatesAccount()
    {
        // Arrange
        var payload = _fixture.Build<CheckingAccountRequest>()
                              .With(x => x.AccountNumber, "ACC123")
                              .With(x => x.OverdraftLimit, 200)
                              .Create();

        var command = new CreateCheckingAccountCommand(payload);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await Assert.That(result.AccountNumber).IsEqualTo(payload.AccountNumber);
        await Assert.That(_repository.ReceivedCalls().Count()).IsEqualTo(1);
        await Assert.That(_unitOfWork.ReceivedCalls().Count()).IsEqualTo(1);
    }

    [Test]
    public async Task Create_Throws_When_AccountNumberAlreadyExists()
    {
        // Arrange
        var validator = new CreateCheckingAccountValidator(_repository);

        var payload = _fixture.Build<CheckingAccountRequest>()
                        .With(x => x.AccountNumber, _mockCheckingAccount.AccountNumber)
                        .Create();
        
        // Act
        var command = new CreateCheckingAccountCommand(payload);


        var result = await _checkingAccountValidator.TestValidateAsync(command);

        // Assert
        await Assert.That(result.IsValid).IsFalse();
        await Assert.That(result.Errors).Contains(e => e.PropertyName == "AccountNumber");
    }

    [Test]
    public async Task Create_Throws_When_OverdraftLimitTooLow()
    {
        // Arrange
        var payload = _fixture.Build<CheckingAccountRequest>()
                              .With(x => x.OverdraftLimit, 2)
                              .Create();

        // Act
        var command = new CreateCheckingAccountCommand(payload);

        // Assert
       
        var result = await _checkingAccountValidator.TestValidateAsync(command);

        await Assert.That(result.IsValid).IsFalse();
        await Assert.That(result.Errors).Contains(e => e.PropertyName == "OverdraftLimit");
    }
}