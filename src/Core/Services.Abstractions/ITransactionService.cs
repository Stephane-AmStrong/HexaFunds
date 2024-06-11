using DataTransfertObjects;

namespace Services.Abstractions;

public interface ITransactionService
{
    Task<IEnumerable<TransactionResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AccountStatementResponse> GetAccountStatementAsync(AccountStatementQuery accountStatementQuery, CancellationToken cancellationToken = default);
    Task<TransactionResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TransactionResponse>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<TransactionResponse> CreateAsync(TransactionRequest transaction, CancellationToken cancellationToken = default);
}
