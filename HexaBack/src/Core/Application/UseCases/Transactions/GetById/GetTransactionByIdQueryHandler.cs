using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Responses;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Transactions.GetById;

public class GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository, ILogger<GetTransactionByIdQueryHandler> logger) : IQueryHandler<GetTransactionByIdQuery, TransactionResponse?>
{
    public async Task<TransactionResponse?> HandleAsync(GetTransactionByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving transaction with ID: {TransactionId}", query.Id);
        var transaction = await transactionRepository.GetByIdAsync(query.Id, cancellationToken) ?? throw new AccountNotFoundException(query.Id);

        logger.LogInformation("Transaction of AccountNumber {AccountNumber} retrieved.", transaction.BankAccount);

        return transaction.Adapt<TransactionResponse>();
    }
}
