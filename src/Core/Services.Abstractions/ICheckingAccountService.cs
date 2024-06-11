
using DataTransfertObjects;

namespace Services.Abstractions;

public interface ICheckingAccountService
{
    Task<IEnumerable<CheckingAccountResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CheckingAccountResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CheckingAccountResponse> CreateAsync(CheckingAccountRequest checkingAccount, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, CheckingAccountRequest checkingAccount, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
