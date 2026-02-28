using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.SavingsAccounts.Delete;

public class DeleteSavingsAccountValidator : AbstractValidator<DeleteSavingsAccountCommand>
{
    public DeleteSavingsAccountValidator(ISavingsAccountRepository savingsAccountRepository)
    {
        RuleFor(command => command.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(string.Format(Validation.Messages.FieldRequired))
            .MustAsync(async (savingsAccountId, cancellationToken) =>
            {
                var savingsAccount = await savingsAccountRepository.GetByIdAsync(savingsAccountId, cancellationToken);
                return savingsAccount != null;
            }).WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.SavingsAccount));

    }
}
