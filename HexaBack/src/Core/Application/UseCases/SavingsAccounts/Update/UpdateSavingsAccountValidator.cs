using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.SavingsAccounts.Update;

public class UpdateSavingsAccountValidator : AbstractValidator<UpdateSavingsAccountCommand>
{
    public UpdateSavingsAccountValidator(ISavingsAccountRepository savingsAccountRepository)
    {
        RuleFor(command => command.Payload.AccountNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(UpdateSavingsAccountCommand.Payload.AccountNumber))
            .MustAsync(async (request, accountNumber, cancellationToken) =>
            {
                var conflictingSavingsAccounts = await savingsAccountRepository.FindByConditionAsync(savingsAccount => savingsAccount.AccountNumber == accountNumber && savingsAccount.Id != request.Id, cancellationToken);
                return conflictingSavingsAccounts.Count == 0;

            }).WithMessage(string.Format(Validation.Messages.FieldAlreadyInUseByAnother, nameof(UpdateSavingsAccountCommand.Payload.AccountNumber), Validation.Entities.SavingsAccount));

        RuleFor(command => command.Payload.BalanceCeiling)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .GreaterThan(100)
            .OverridePropertyName(nameof(UpdateSavingsAccountCommand.Payload.BalanceCeiling));
    }
}
