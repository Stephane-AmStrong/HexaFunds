using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface ISavingsAccountsRepository
{
    IList<SavingsAccount> GetAll();
    Task<SavingsAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(SavingsAccount savingsAccount, CancellationToken cancellationToken);
    void Update(SavingsAccount savingsAccount);
    void Delete(SavingsAccount savingsAccount);
}
