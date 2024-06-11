using Domain.Entities;
using Domain.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;
namespace Persistence.Repository;

public sealed class SavingsAccountRepository(BankingDbContext dbContext) : RepositoryBase<SavingsAccount>(dbContext), ISavingsAccountRepository
{
    public async Task CreateAsync(SavingsAccount savingsAccount, CancellationToken cancellationToken)
    {
        await BaseCreateAsync(savingsAccount, cancellationToken);
    }

    public async Task DeleteAsync(SavingsAccount savingsAccount, CancellationToken cancellationToken)
    {
        await Task.Run(()=> BaseDelete(savingsAccount), cancellationToken);
    }

    public async Task<IEnumerable<SavingsAccount>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await BaseGetAll().ToListAsync(cancellationToken);
    }

    public async Task<SavingsAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await BaseFindByCondition(checkingAccount => checkingAccount.Id.Equals(id))
            .Include(x => x.Transactions)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(SavingsAccount savingsAccount, CancellationToken cancellationToken)
    {
        await Task.Run(()=> BaseUpdate(savingsAccount), cancellationToken);
    }
}
