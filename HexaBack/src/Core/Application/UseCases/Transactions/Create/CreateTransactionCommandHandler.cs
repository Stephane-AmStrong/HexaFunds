using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Responses;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Transactions.Create;

public class CreateTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, ILogger<CreateTransactionCommandHandler> logger) : ICommandHandler<CreateTransactionCommand, TransactionResponse>
{
    public async Task<TransactionResponse> HandleAsync(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new transaction with AccountId: {AccountId} ", command.Payload.AccountId);

        var transaction = command.Payload.Adapt<Transaction>();

        await transactionRepository.CreateAsync(transaction, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully created transaction with ID: {TransactionId}", transaction.Id);

        return transaction.Adapt<TransactionResponse>();

    }
}
