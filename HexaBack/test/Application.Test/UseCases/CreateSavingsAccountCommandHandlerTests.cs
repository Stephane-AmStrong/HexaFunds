using System.Linq.Expressions;
using Application.DataTransfertObjects.Requests;
using Application.UseCases.SavingsAccounts.Create;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Application.Tests.UseCases;

public class CreateSavingsAccountCommandHandlerTests
{
    private IFixture _fixture = null!;
    private ISavingsAccountRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private ILogger<CreateSavingsAccountCommandHandler> _logger = null!;
    private CreateSavingsAccountCommandHandler _handler = null!;
    private SavingsAccount _mockSavingsAccount = null!;
    private CreateSavingsAccountValidator _savingsAccountValidator = null!;

    [Before(Test)]
    public void BeforeTest(TestContext context)
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization{ConfigureMembers = true});

        _repository = _fixture.Freeze<ISavingsAccountRepository>();
        _unitOfWork = _fixture.Freeze<IUnitOfWork>();
        _logger = _fixture.Freeze<ILogger<CreateSavingsAccountCommandHandler>>();
        _handler = new CreateSavingsAccountCommandHandler(_repository, _unitOfWork, _logger);

        // Remove recursion crash
        _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));

        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        // Break Transaction -> BankAccount circular reference
        _fixture.Customize<Transaction>(c => c.Without(t => t.BankAccount));

        // Neutralize Transactions collection
        _fixture.Customize<SavingsAccount>(c => c.With(x => x.Transactions, []));

        _savingsAccountValidator = new CreateSavingsAccountValidator(_repository);



        _mockSavingsAccount = _fixture.Create<SavingsAccount>();

        _repository.FindByConditionAsync(Arg.Any<Expression<Func<SavingsAccount, bool>>>(), Arg.Any<CancellationToken>())
                  .Returns([_mockSavingsAccount]);
    }



    [Test]
    public async Task Create_Successfully_CreatesAccount()
    {
        // Arrange
        var payload = _fixture.Build<SavingsAccountRequest>()
                              .With(x => x.AccountNumber, "ACC123")
                              .With(x => x.BalanceCeiling, 200)
                              .Create();

        var command = new CreateSavingsAccountCommand(payload);

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
        var validator = new CreateSavingsAccountValidator(_repository);

        var payload = _fixture.Build<SavingsAccountRequest>()
                        .With(x => x.AccountNumber, _mockSavingsAccount.AccountNumber)
                        .Create();
        
        // Act
        var command = new CreateSavingsAccountCommand(payload);


        var result = await _savingsAccountValidator.TestValidateAsync(command);

        // Assert
        await Assert.That(result.IsValid).IsFalse();
        await Assert.That(result.Errors).Contains(e => e.PropertyName == "AccountNumber");
    }

    [Test]
    public async Task Create_Throws_When_BalanceCeilingTooLow()
    {
        // Arrange
        var payload = _fixture.Build<SavingsAccountRequest>()
                              .With(x => x.BalanceCeiling, 2)
                              .Create();

        // Act
        var command = new CreateSavingsAccountCommand(payload);

        // Assert
       
        var result = await _savingsAccountValidator.TestValidateAsync(command);

        await Assert.That(result.IsValid).IsFalse();
        await Assert.That(result.Errors).Contains(e => e.PropertyName == "BalanceCeiling");
    }
}