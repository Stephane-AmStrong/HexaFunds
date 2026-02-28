using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.Transactions.Update;

public class UpdateTransactionValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionValidator(ITransactionRepository transactionRepository, IBankAccountRepository bankAccountRepository)
    {
        RuleFor(command => command.Payload.AccountId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(UpdateTransactionCommand.Payload.AccountId))
            .MustAsync(async (request, accountId, cancellationToken) =>
            {
                var existingBankAccount = await bankAccountRepository.GetByIdAsync(accountId, cancellationToken);
                return existingBankAccount is not null;

            }).WithMessage(string.Format(Validation.Messages.FieldAlreadyInUseByAnother, nameof(UpdateTransactionCommand.Payload.AccountId), Validation.Entities.Transaction));

        RuleFor(command => command.Payload.Amount)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .GreaterThan(100)
            .OverridePropertyName(nameof(UpdateTransactionCommand.Payload.Amount));
    }
}
