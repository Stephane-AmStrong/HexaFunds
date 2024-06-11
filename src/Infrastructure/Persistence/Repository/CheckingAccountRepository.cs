using Domain.Entities;
using Domain.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;
namespace Persistence.Repository;

public sealed class CheckingAccountRepository(BankingDbContext dbContext) : RepositoryBase<CheckingAccount>(dbContext), ICheckingAccountRepository
{
    public async Task CreateAsync(CheckingAccount checkingAccount, CancellationToken cancellationToken)
    {
        await BaseCreateAsync(checkingAccount, cancellationToken);
    }

    public async Task DeleteAsync(CheckingAccount checkingAccount, CancellationToken cancellationToken)
    {
        await Task.Run(() => BaseDelete(checkingAccount), cancellationToken);
    }

    public async Task<IEnumerable<CheckingAccount>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await BaseGetAll().ToListAsync(cancellationToken);
    }

    public async Task<CheckingAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await BaseFindByCondition(checkingAccount => checkingAccount.Id.Equals(id))
            .Include(x => x.Transactions)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(CheckingAccount checkingAccount, CancellationToken cancellationToken)
    {
        await Task.Run(() => BaseUpdate(checkingAccount), cancellationToken);
    }
}
