using Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Persistence.Repository;

public sealed class BankAccountsRepository(BankingDbContext dbContext) : RepositoryBase<Domain.Entities.BankAccount>(dbContext), IBankAccountsRepository
{
    public async Task<Domain.Entities.BankAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await BaseFindByCondition(checkingAccount => checkingAccount.Id.Equals(id))
            .Include(x => x.Transactions)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
