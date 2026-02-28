using Application.Abstractions.Handlers;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Transactions.Update;

public class UpdateTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, ILogger<UpdateTransactionCommandHandler> logger) : ICommandHandler<UpdateTransactionCommand>
{
    public async Task HandleAsync(UpdateTransactionCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating transaction with ID: {TransactionId}", command.Id);

        var transaction = await transactionRepository.GetByIdAsync(command.Id, cancellationToken) ?? throw new AccountNotFoundException(command.Id);

        command.Payload.Adapt(transaction);

        transactionRepository.Update(transaction);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully updated transaction with AccountNumber: {AccountNumber}", transaction.BankAccount);
    }
}
