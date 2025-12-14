using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.CheckingAccounts.Delete;

public class DeleteCheckingAccountValidator : AbstractValidator<DeleteCheckingAccountCommand>
{
    public DeleteCheckingAccountValidator(ICheckingAccountsRepository checkingaccountsRepository)
    {
        RuleFor(command => command.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(string.Format(Validation.Messages.FieldRequired))
            .MustAsync(async (checkingaccountId, cancellationToken) =>
            {
                var checkingaccount = await checkingaccountsRepository.GetByIdAsync(checkingaccountId, cancellationToken);
                return checkingaccount != null;
            }).WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.CheckingAccount));

    }
}
