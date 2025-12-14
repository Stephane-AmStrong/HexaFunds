using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.CheckingAccounts.Update;

public class UpdateCheckingAccountValidator : AbstractValidator<UpdateCheckingAccountCommand>
{
    public UpdateCheckingAccountValidator(ICheckingAccountsRepository checkingaccountsRepository)
    {
        
        RuleFor(command => command.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(string.Format(Validation.Messages.FieldRequired))
            .MustAsync(async (checkingaccountId, cancellationToken) =>
            {
                var checkingaccount = await checkingaccountsRepository.GetByIdAsync(checkingaccountId, cancellationToken);
                return checkingaccount is not null;
            }).WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.CheckingAccount));

        RuleFor(command => command.Payload.AccountNumber)
            .NotNull()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(UpdateCheckingAccountCommand.Payload.AccountNumber));

        RuleFor(command => command.Payload.OverdraftLimit)
            .NotNull()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(UpdateCheckingAccountCommand.Payload.OverdraftLimit));
    }
}
