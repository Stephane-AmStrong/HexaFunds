using DataTransfertObjects;

namespace Services.Abstractions;

public interface ITransactionService
{
    IList<TransactionResponse> GetAll();
    Task<AccountStatementResponse> GetAccountStatementAsync(AccountStatementQuery accountStatementQuery, CancellationToken cancellationToken);
    Task<TransactionResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<AccountTransactionsResponse> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    Task<TransactionResponse> CreateAsync(TransactionRequest transaction, CancellationToken cancellationToken);
}
