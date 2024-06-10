using Domain.Entities;

namespace Domain.Repositories.Abstractions;

public interface ISavingsAccountRepository
{
    Task<IEnumerable<SavingsAccount>> GetAllAsync(CancellationToken cancellationToken);
    Task<SavingsAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(SavingsAccount savingsAccount, CancellationToken cancellationToken);
    Task UpdateAsync(SavingsAccount savingsAccount, CancellationToken cancellationToken);
    Task DeleteAsync(SavingsAccount savingsAccount, CancellationToken cancellationToken);
}
