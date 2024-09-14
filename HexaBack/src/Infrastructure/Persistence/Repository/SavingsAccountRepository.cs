using Domain.Entities;
using Domain.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;
namespace Persistence.Repository;

public sealed class SavingsAccountRepository(BankingDbContext dbContext) : RepositoryBase<SavingsAccount>(dbContext), ISavingsAccountRepository
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
