using DataTransfertObjects;

namespace Services.Abstractions;

public interface ISavingsAccountService
{
    Task<IEnumerable<SavingsAccountResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SavingsAccountResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SavingsAccountResponse> CreateAsync(SavingsAccountRequest savingsAccount, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, SavingsAccountRequest savingsAccount, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
