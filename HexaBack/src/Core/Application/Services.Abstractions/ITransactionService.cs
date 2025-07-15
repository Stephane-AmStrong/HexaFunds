using Application.DataTransfertObjects;

namespace Application.Services.Abstractions;

public interface ITransactionService
{
    IList<TransactionResponse> Get(TransactionQuery transactionQuery);
    Task<TransactionResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<TransactionResponse> CreateAsync(TransactionRequest transaction, CancellationToken cancellationToken);
}
