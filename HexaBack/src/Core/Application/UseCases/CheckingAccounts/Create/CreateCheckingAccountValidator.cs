using Application.Common;
using FluentValidation;

namespace Application.UseCases.CheckingAccounts.Create;

public class CreateCheckingAccountValidator : AbstractValidator<CreateCheckingAccountCommand>
{
    public CreateCheckingAccountValidator()
    {
        RuleFor(command => command.Payload.AccountNumber)
            .NotNull()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateCheckingAccountCommand.Payload.AccountNumber));

        RuleFor(command => command.Payload.OverdraftLimit)
            .NotNull()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateCheckingAccountCommand.Payload.OverdraftLimit));
    }
}
