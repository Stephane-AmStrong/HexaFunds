using Domain.Entities;
using Domain.Abstractions.Repositories;

using Microsoft.EntityFrameworkCore;
namespace Persistence.Repository;

public sealed class SavingsAccountsRepository(BankingDbContext dbContext) : RepositoryBase<SavingsAccount>(dbContext), ISavingsAccountsRepository
{
    public Task CreateAsync(SavingsAccount savingsAccount, CancellationToken cancellationToken)
    {
        return BaseCreateAsync(savingsAccount, cancellationToken);
    }

    public void Delete(SavingsAccount savingsAccount)
    {
        BaseDelete(savingsAccount);
    }

    public IList<SavingsAccount> GetAll()
    {
        return [.. BaseGetAll()];
    }

    public async Task<SavingsAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await BaseFindByCondition(checkingAccount => checkingAccount.Id.Equals(id))
            .Include(x => x.Transactions)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void Update(SavingsAccount savingsAccount)
    {
        BaseUpdate(savingsAccount);
    }
}
