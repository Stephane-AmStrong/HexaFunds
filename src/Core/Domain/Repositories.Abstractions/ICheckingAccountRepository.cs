using Domain.Entities;

namespace Domain.Repositories.Abstractions;

public interface ICheckingAccountRepository
{
    Task<IEnumerable<CheckingAccount>> GetAllAsync(CancellationToken cancellationToken);
    Task<CheckingAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(CheckingAccount checkingAccount, CancellationToken cancellationToken);
    Task UpdateAsync(CheckingAccount checkingAccount, CancellationToken cancellationToken);
    Task DeleteAsync(CheckingAccount checkingAccount, CancellationToken cancellationToken);
}
