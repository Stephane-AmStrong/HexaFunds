using DataTransfertObjects;

namespace Services.Abstractions;

public interface ISavingsAccountService
{
    IList<SavingsAccountResponse> GetAll();
    Task<SavingsAccountResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SavingsAccountResponse> CreateAsync(SavingsAccountRequest savingsAccount, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, SavingsAccountRequest savingsAccount, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
