using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.SavingsAccounts.Create;

public class CreateSavingsAccountValidator : AbstractValidator<CreateSavingsAccountCommand>
{
    public CreateSavingsAccountValidator(ISavingsAccountRepository savingsAccountRepository)
    {
        RuleFor(command => command.Payload.AccountNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateSavingsAccountCommand.Payload.AccountNumber))
            .MustAsync(async (AccountNumber, cancellationToken) =>
            {
                var existingSavingsAccounts = await savingsAccountRepository.FindByConditionAsync(c => c.AccountNumber == AccountNumber, cancellationToken);
                return existingSavingsAccounts.Count == 0;
            })
            .WithMessage(string.Format(Validation.Messages.FieldAlreadyInUseByAnother, nameof(CreateSavingsAccountCommand.Payload.AccountNumber), Validation.Entities.SavingsAccount));

        RuleFor(command => command.Payload.BalanceCeiling)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .GreaterThan(100)
            .OverridePropertyName(nameof(CreateSavingsAccountCommand.Payload.BalanceCeiling));
    }
}
