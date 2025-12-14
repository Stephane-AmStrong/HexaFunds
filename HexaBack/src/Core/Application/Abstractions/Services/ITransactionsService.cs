using Application.DataTransfertObjects;

namespace Application.Abstractions.Services;

public interface ITransactionsService
{
    IList<TransactionResponse> Get(TransactionQuery transactionQuery);
    Task<TransactionResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<TransactionResponse> CreateAsync(TransactionRequest transaction, CancellationToken cancellationToken);
}
