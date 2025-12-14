using Domain.Abstractions.Repositories;
using Domain.Entities;

using Microsoft.EntityFrameworkCore;
namespace Persistence.Repository;

public sealed class CheckingAccountsRepository(BankingDbContext dbContext) : RepositoryBase<CheckingAccount>(dbContext), ICheckingAccountsRepository
{
    public Task CreateAsync(CheckingAccount checkingAccount, CancellationToken cancellationToken)
    {
        return BaseCreateAsync(checkingAccount, cancellationToken);
    }

    public void Delete(CheckingAccount checkingAccount)
    {
        BaseDelete(checkingAccount);
    }

    public IList<CheckingAccount> GetAll()
    {
        return [.. BaseGetAll()];
    }

    public Task<CheckingAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return BaseFindByCondition(checkingAccount => checkingAccount.Id.Equals(id))
            .Include(x => x.Transactions)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void Update(CheckingAccount checkingAccount)
    {
        BaseUpdate(checkingAccount);
    }
}
