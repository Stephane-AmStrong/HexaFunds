using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.CheckingAccounts.Update;

public class UpdateCheckingAccountValidator : AbstractValidator<UpdateCheckingAccountCommand>
{
    public UpdateCheckingAccountValidator(ICheckingAccountRepository checkingAccountRepository)
    {
        RuleFor(command => command.Payload.AccountNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(UpdateCheckingAccountCommand.Payload.AccountNumber))
            .MustAsync(async (request, accountNumber, cancellationToken) =>
            {
                var conflictingCheckingAccounts = await checkingAccountRepository.FindByConditionAsync(checkingAccount => checkingAccount.AccountNumber == accountNumber && checkingAccount.Id != request.Id, cancellationToken);
                return conflictingCheckingAccounts.Count == 0;

            }).WithMessage(string.Format(Validation.Messages.FieldAlreadyInUseByAnother, nameof(UpdateCheckingAccountCommand.Payload.AccountNumber), Validation.Entities.CheckingAccount));

        RuleFor(command => command.Payload.OverdraftLimit)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .GreaterThan(100)
            .OverridePropertyName(nameof(UpdateCheckingAccountCommand.Payload.OverdraftLimit));
    }
}
