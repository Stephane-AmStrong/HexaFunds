using Domain.Entities;
using Domain.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;
namespace Persistence.Repository;

public sealed class CheckingAccountRepository(BankingDbContext dbContext) : RepositoryBase<CheckingAccount>(dbContext), ICheckingAccountRepository
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
