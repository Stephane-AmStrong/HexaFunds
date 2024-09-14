using Domain.Entities;

namespace Domain.Repositories.Abstractions;

public interface ISavingsAccountRepository
{
    IList<SavingsAccount> GetAll();
    Task<SavingsAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(SavingsAccount savingsAccount, CancellationToken cancellationToken);
    void Update(SavingsAccount savingsAccount);
    void Delete(SavingsAccount savingsAccount);
}
