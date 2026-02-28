using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.Transactions.Delete;

public class DeleteTransactionValidator : AbstractValidator<DeleteTransactionCommand>
{
    public DeleteTransactionValidator(ITransactionRepository transactionRepository)
    {
        RuleFor(command => command.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(string.Format(Validation.Messages.FieldRequired))
            .MustAsync(async (transactionId, cancellationToken) =>
            {
                var transaction = await transactionRepository.GetByIdAsync(transactionId, cancellationToken);
                return transaction != null;
            }).WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.Transaction));

    }
}
