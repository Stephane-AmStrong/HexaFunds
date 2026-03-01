using System.Linq.Expressions;
using Application.DataTransfertObjects.Requests;
using Application.UseCases.Transactions.Create;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Application.Tests.UseCases;

public class CreateTransactionCommandHandlerTests
{
    private IFixture _fixture = null!;
    private ITransactionRepository _repository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private ILogger<CreateTransactionCommandHandler> _logger = null!;
    private CreateTransactionCommandHandler _handler = null!;
    private Transaction _mockTransaction = null!;
    private CreateTransactionValidator _transactionValidator = null!;

    [Before(Test)]
    public void BeforeTest(TestContext context)
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });

        _repository = _fixture.Freeze<ITransactionRepository>();
        _unitOfWork = _fixture.Freeze<IUnitOfWork>();
        _logger = _fixture.Freeze<ILogger<CreateTransactionCommandHandler>>();
        _handler = new CreateTransactionCommandHandler(_repository, _unitOfWork, _logger);

        // Remove recursion crash
        _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        // ✅ TypeRelay pour BankAccount abstrait → BankAccountStub
        _fixture.Customizations.Add(
            new AutoFixture.Kernel.TypeRelay(typeof(BankAccount), typeof(BankAccountStub))
        );

        // Break Transaction -> BankAccount circular reference
        _fixture.Customize<Transaction>(c => c.With(x => x.BankAccount, _fixture.Create<BankAccount>()));

        // Neutralize Transactions collection
        _fixture.Customize<BankAccount>(c => c.Without(x => x.Transactions));

        _transactionValidator = new CreateTransactionValidator(
            _repository,
            _fixture.Freeze<IBankAccountRepository>()
        );

        _mockTransaction = _fixture.Create<Transaction>();

        // Simule un transaction existant pour certains tests
        _repository.FindByConditionAsync(
            Arg.Any<Expression<Func<Transaction, bool>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new[] { _mockTransaction });
    }



    [Test]
    public async Task Create_Successfully_CreatesTransaction()
    {
        // Arrange
        var payload = _fixture.Build<TransactionRequest>()
                              .With(x => x.AccountId, _fixture.Create<Guid>())
                              .With(x => x.Amount, 200)
                              .Create();

        var command = new CreateTransactionCommand(payload);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await Assert.That(result.AccountId).IsEqualTo(payload.AccountId);
        await Assert.That(_repository.ReceivedCalls().Count()).IsEqualTo(1);
        await Assert.That(_unitOfWork.ReceivedCalls().Count()).IsEqualTo(1);
    }

    [Test]
    public async Task Create_Throws_When_AccountDoesNotExist()
    {
        // Arrange : repo vide → compte inexistant
        _repository.FindByConditionAsync(
            Arg.Any<Expression<Func<Transaction, bool>>>(),
            Arg.Any<CancellationToken>())
            .Returns([]);

        var payload = _fixture.Build<TransactionRequest>()
                              .With(x => x.AccountId, Guid.NewGuid())
                              .With(x => x.Amount, 200)
                              .Create();

        var command = new CreateTransactionCommand(payload);

        // Act
        var result = await _transactionValidator.TestValidateAsync(command);

        // Assert
        await Assert.That(result.IsValid).IsFalse();
        await Assert.That(result.Errors)
            .Contains(e => e.PropertyName == "AccountId");
    }

    [Test]
    public async Task Create_Throws_When_AmountTooLow()
    {
        // Arrange
        var payload = _fixture.Build<TransactionRequest>()
                              .With(x => x.Amount, 2)
                              .Create();

        // Act
        var command = new CreateTransactionCommand(payload);

        // Assert

        var result = await _transactionValidator.TestValidateAsync(command);

        await Assert.That(result.IsValid).IsFalse();
        await Assert.That(result.Errors).Contains(e => e.PropertyName == "Amount");
    }

    private class BankAccountStub : BankAccount
    {
        public BankAccountStub() : base() { }
    }
}