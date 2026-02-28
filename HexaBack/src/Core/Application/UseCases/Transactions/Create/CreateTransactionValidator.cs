using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.Transactions.Create;

public class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionValidator(ITransactionRepository transactionRepository, IBankAccountRepository bankAccountRepository)
    {
        RuleFor(command => command.Payload.AccountId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateTransactionCommand.Payload.AccountId))
            .MustAsync(async (accountId, cancellationToken) =>
            {
                var existingBankAccount = await bankAccountRepository.GetByIdAsync(accountId, cancellationToken);
                return existingBankAccount is not null;
            })
            .WithMessage(string.Format(Validation.Messages.FieldAlreadyInUseByAnother, nameof(CreateTransactionCommand.Payload.AccountId), Validation.Entities.Transaction));

        RuleFor(command => command.Payload.Amount)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .GreaterThan(100)
            .OverridePropertyName(nameof(CreateTransactionCommand.Payload.Amount));
    }
}
