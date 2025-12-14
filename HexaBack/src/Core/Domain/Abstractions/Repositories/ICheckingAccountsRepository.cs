using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface ICheckingAccountsRepository
{
    IList<CheckingAccount> GetAll();
    Task<CheckingAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(CheckingAccount checkingAccount, CancellationToken cancellationToken);
    void Update(CheckingAccount checkingAccount);
    void Delete(CheckingAccount checkingAccount);
}
/*
PagedList<CheckingAccount> GetPagedList(GetCheckingAccountsQuery getCheckingAccountsQuery);
Task<CheckingAccount> GetByIdAsync(Guid id, CancellationToken cancellationToken);
Task CreateAsync(CheckingAccount checkingAccount, CancellationToken cancellationToken);
void Update(CheckingAccount checkingAccount);
void Delete(CheckingAccount checkingAccount);
*/