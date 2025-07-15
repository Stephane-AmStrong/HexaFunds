using Domain.Entities;
namespace Domain.Repositories.Abstractions;

public interface IBankAccountRepository
{
    IList<BankAccount> GetAll();
    Task<BankAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(BankAccount bankAccount, CancellationToken cancellationToken);
    void Update(BankAccount bankAccount);
    void Delete(BankAccount bankAccount);
}
