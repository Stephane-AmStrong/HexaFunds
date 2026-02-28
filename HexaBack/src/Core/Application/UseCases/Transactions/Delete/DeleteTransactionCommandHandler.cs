using Application.Abstractions.Handlers;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Transactions.Delete;

public class DeleteTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, ILogger<DeleteTransactionCommandHandler> logger) : ICommandHandler<DeleteTransactionCommand>
{
    public async Task HandleAsync(DeleteTransactionCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting transaction with ID: {TransactionId}", command.Id);

        var transaction = await transactionRepository.GetByIdAsync(command.Id, cancellationToken) ?? throw new AccountNotFoundException(command.Id);

        transactionRepository.Delete(transaction);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully deleted transaction with AccountNumber: {AccountNumber}", transaction.BankAccount);
    }
}
