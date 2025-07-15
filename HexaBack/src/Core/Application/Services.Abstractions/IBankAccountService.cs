
using Application.DataTransfertObjects;

namespace Application.Services.Abstractions;

public interface IBankAccountService
{
    IList<BankAccountResponse> GetAll();
    Task<BankAccountResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<CheckingAccountResponse> CreateAsync(BankAccountRequest bankAccount, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, BankAccountRequest bankAccount, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
