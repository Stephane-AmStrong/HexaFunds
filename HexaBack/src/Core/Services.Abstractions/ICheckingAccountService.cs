
using DataTransfertObjects;

namespace Services.Abstractions;

public interface ICheckingAccountService
{
    IList<CheckingAccountResponse> GetAll();
    Task<CheckingAccountResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<CheckingAccountResponse> CreateAsync(CheckingAccountRequest checkingAccount, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, CheckingAccountRequest checkingAccount, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
