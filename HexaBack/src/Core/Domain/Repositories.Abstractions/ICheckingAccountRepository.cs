using Domain.Entities;

namespace Domain.Repositories.Abstractions;

public interface ICheckingAccountRepository
{
    IList<CheckingAccount> GetAll();
    Task<CheckingAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(CheckingAccount checkingAccount, CancellationToken cancellationToken);
    void Update(CheckingAccount checkingAccount);
    void Delete(CheckingAccount checkingAccount);
}
