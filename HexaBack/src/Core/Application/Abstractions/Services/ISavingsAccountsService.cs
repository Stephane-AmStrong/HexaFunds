using Application.DataTransfertObjects;

namespace Application.Abstractions.Services;

public interface ISavingsAccountsService
{
    IList<SavingsAccountResponse> GetAll();
    Task<SavingsAccountResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SavingsAccountResponse> CreateAsync(SavingsAccountRequest savingsAccount, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, SavingsAccountRequest savingsAccount, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
