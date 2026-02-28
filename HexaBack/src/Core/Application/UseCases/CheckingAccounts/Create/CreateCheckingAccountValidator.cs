using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.CheckingAccounts.Create;

public class CreateCheckingAccountValidator : AbstractValidator<CreateCheckingAccountCommand>
{
    public CreateCheckingAccountValidator(ICheckingAccountRepository checkingAccountRepository)
    {
        RuleFor(command => command.Payload.AccountNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateCheckingAccountCommand.Payload.AccountNumber))
            .MustAsync(async (AccountNumber, cancellationToken) =>
            {
                var existingCheckingAccounts = await checkingAccountRepository.FindByConditionAsync(c => c.AccountNumber == AccountNumber, cancellationToken);
                return existingCheckingAccounts.Count == 0;
            })
            .WithMessage(string.Format(Validation.Messages.FieldAlreadyInUseByAnother, nameof(CreateCheckingAccountCommand.Payload.AccountNumber), Validation.Entities.CheckingAccount));

        RuleFor(command => command.Payload.OverdraftLimit)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .GreaterThan(100)
            .OverridePropertyName(nameof(CreateCheckingAccountCommand.Payload.OverdraftLimit));
    }
}
