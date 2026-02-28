using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.CheckingAccounts.Delete;

public class DeleteCheckingAccountValidator : AbstractValidator<DeleteCheckingAccountCommand>
{
    public DeleteCheckingAccountValidator(ICheckingAccountRepository checkingAccountRepository)
    {
        RuleFor(command => command.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(string.Format(Validation.Messages.FieldRequired))
            .MustAsync(async (checkingAccountId, cancellationToken) =>
            {
                var checkingAccount = await checkingAccountRepository.GetByIdAsync(checkingAccountId, cancellationToken);
                return checkingAccount != null;
            }).WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.CheckingAccount));

    }
}
